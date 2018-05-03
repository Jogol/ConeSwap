using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity;
using OpenCVForUnityExample;
using System.IO;
using System.Xml.Serialization;
using System;
using System.Linq;

[RequireComponent(typeof(WebCamTextureToMatHelper))]
public class Tracking : MonoBehaviour
{

    /// <summary>
    /// Determines if restores the camera parameters when the file exists.
    /// </summary>
    public bool useStoredCameraParameters = true;

    /// <summary>
    /// The dictionary identifier.
    /// </summary>
    public ArUcoDictionary dictionaryId = ArUcoDictionary.DICT_6X6_250;

    /// <summary>
    /// Determines if shows rejected corners.
    /// </summary>
    public bool showRejectedCorners = false;

    /// <summary>
    /// Determines if applied the pose estimation.
    /// </summary>
    public bool applyEstimationPose = true;

    /// <summary>
    /// Determines if refine marker detection. (only valid for ArUco boards)
    /// </summary>
    public bool refineMarkerDetection = true;

    /// <summary>
    /// The length of the markers' side. Normally, unit is meters.
    /// </summary>
    public float markerLength = 0.1f;

    /// <summary>
    /// The AR game object.
    /// </summary>
    public GameObject arGameObject;

    /// <summary>
    /// The list of AR game objects.
    /// </summary>
    public GameObject[] arGameObjectList;

    public int objPerGroup;

    GameObject[][] arGameObjectGroupedList;

    /// <summary>
    /// The AR camera.
    /// </summary>
    public Camera arCamera;

    /// <summary>
    /// Determines if request the AR camera moving.
    /// </summary>
    public bool shouldMoveARCamera = false;

    int numberOfGroups = 1;

    //TODO Correct groups
    //new int[]{2,3}, new int[]{4,5}
    int[][] groupList = new int[][] {
        new int[]{0,1,2,3,4,5}
    };

    int[][] groupedGroupList;

    //TODO Implement in RemoveDuplicates
    /// <see cref="RemoveDuplicates(ref List{Mat}, ref Mat)"/>
    int[] lastKeptID;
    int[] lastKeptIDCurrentArea;

    /// <summary>
    /// The texture.
    /// </summary>
    Texture2D texture;

    /// <summary>
    /// The webcam texture to mat helper.
    /// </summary>
    WebCamTextureToMatHelper webCamTextureToMatHelper;

    /// <summary>
    /// The rgb mat.
    /// </summary>
    Mat rgbMat;

    /// <summary>
    /// The cameraparam matrix.
    /// </summary>
    Mat camMatrix;

    /// <summary>
    /// The distortion coeffs.
    /// </summary>
    MatOfDouble distCoeffs;

    /// <summary>
    /// The matrix that inverts the Y axis.
    /// </summary>
    Matrix4x4 invertYM;

    /// <summary>
    /// The matrix that inverts the Z axis.
    /// </summary>
    Matrix4x4 invertZM;

    /// <summary>
    /// The transformation matrix.
    /// </summary>
    Matrix4x4 transformationM;

    /// <summary>
    /// The transformation matrix for AR.
    /// </summary>
    Matrix4x4 ARM;

    /// <summary>
    /// The identifiers.
    /// </summary>
    Mat ids;

    /// <summary>
    /// The corners.
    /// </summary>
    List<Mat> corners;

    /// <summary>
    /// The rejected corners.
    /// </summary>
    List<Mat> rejectedCorners;

    /// <summary>
    /// The rvecs.
    /// </summary>
    Mat rvecs;

    /// <summary>
    /// The tvecs.
    /// </summary>
    Mat tvecs;

    /// <summary>
    /// The rot mat.
    /// </summary>
    Mat rotMat;

    /// <summary>
    /// The detector parameters.
    /// </summary>
    DetectorParameters detectorParams;

    /// <summary>
    /// The dictionary.
    /// </summary>
    Dictionary dictionary;

    /// <summary>
    /// The FPS monitor.
    /// </summary>
    FpsMonitor fpsMonitor;

    Mat rvec;
    Mat tvec;
    Mat recoveredIdxs;

    // for GridBoard.
    // number of markers in X direction
    const int gridBoradMarkersX = 5;
    // number of markers in Y direction
    const int gridBoradMarkersY = 7;
    // marker side length (normally in meters)
    const float gridBoradMarkerLength = 0.04f;
    // separation between two markers (same unit as markerLength)
    const float gridBoradMarkerSeparation = 0.01f;
    // id of first marker in dictionary to use on board.
    const int gridBoradMarkerFirstMarker = 0;
    GridBoard gridBoard;

    // for ChArUcoBoard.
    //  number of chessboard squares in X direction
    const int chArUcoBoradSquaresX = 5;
    //  number of chessboard squares in Y direction
    const int chArUcoBoradSquaresY = 7;
    // chessboard square side length (normally in meters)
    const float chArUcoBoradSquareLength = 0.04f;
    // marker side length (same unit than squareLength)
    const float chArUcoBoradMarkerLength = 0.02f;
    const int charucoMinMarkers = 2;
    Mat charucoCorners;
    Mat charucoIds;
    CharucoBoard charucoBoard;

    // for ChArUcoDiamondMarker.
    // size of the chessboard squares in pixels
    const float diamondSquareLength = 0.1f;
    // size of the markers in pixels.
    const float diamondMarkerLength = 0.06f;
    // identifiers for diamonds in diamond corners.
    const int diamondId1 = 45;
    const int diamondId2 = 68;
    const int diamondId3 = 28;
    const int diamondId4 = 74;
    List<Mat> diamondCorners;
    Mat diamondIds;

    WebCamTexture webCamTexture;
    new Renderer renderer;
    // Use this for initialization
    void Start()
    {
        if (Modulo(arGameObjectList.Length, objPerGroup) != 0)
            objPerGroup = arGameObjectList.Length;

        int numOfGroups = arGameObjectList.Length / objPerGroup;

        arGameObjectGroupedList = new GameObject[numOfGroups][];
        groupedGroupList = new int[numOfGroups][];

        for (int i = 0; i < numOfGroups; i++)
        {
            int[] idSubGroup = new int[objPerGroup];
            GameObject[] objectSubGroup = new GameObject[objPerGroup];
            for (int j = 0; j < objPerGroup; j++)
            {
                idSubGroup[j] = (i * objPerGroup) + j;
                objectSubGroup[j] = arGameObjectList[(i * objPerGroup) + j];
            }
            arGameObjectGroupedList[i] = objectSubGroup;
            groupedGroupList[i] = idSubGroup;
        }
        lastKeptID = new int[numberOfGroups];
        lastKeptIDCurrentArea = new int[numberOfGroups];
        webCamTextureToMatHelper = gameObject.GetComponent<WebCamTextureToMatHelper>();
        //webCamTexture = new WebCamTexture();
        //renderer = GetComponent<Renderer>();
        //texture = new Texture2D(640, 480, TextureFormat.RGBA32, false);
        ////renderer.material.mainTexture = webCamTexture;
        //webCamTexture.Play();
        ////Debug.Log("Webcam:" + webCamTexture.height + " " + webCamTexture.width);
        //rgbaMat = new Mat(webCamTexture.height, webCamTexture.width, CvType.CV_8UC4);
        //texture = new Texture2D (webCamTexture.width, webCamTexture.height, TextureFormat.RGBA32, false);
        //renderer.material.mainTexture = texture;
        webCamTextureToMatHelper.Initialize();
    }

    private int Modulo(int a, int b)
    {
        return a - (int)((double)a / b) * b;
    }

    public void OnWebCamTextureToMatHelperInitialized()
    {
        Debug.Log("OnWebCamTextureToMatHelperInitialized");

        Mat webCamTextureMat = webCamTextureToMatHelper.GetMat();


        //Debug.Log(markerImage.width() + " rast " + markerImage.height());
        //Utils.fastMatToTexture2D(markerImage, markerTexture);
        //SaveTextureAsPNG(markerTexture, Application.dataPath + "marker.png");

        texture = new Texture2D(webCamTextureMat.cols(), webCamTextureMat.rows(), TextureFormat.RGBA32, false);

        gameObject.GetComponent<Renderer>().material.mainTexture = texture;

        gameObject.transform.localScale = new Vector3(webCamTextureMat.cols(), webCamTextureMat.rows(), 1);
        Debug.Log("Screen.width " + Screen.width + " Screen.height " + Screen.height + " Screen.orientation " + Screen.orientation);

        if (fpsMonitor != null)
        {
            fpsMonitor.Add("width", webCamTextureMat.width().ToString());
            fpsMonitor.Add("height", webCamTextureMat.height().ToString());
            fpsMonitor.Add("orientation", Screen.orientation.ToString());
        }


        float width = webCamTextureMat.width();
        float height = webCamTextureMat.height();

        float imageSizeScale = 1.0f;
        float widthScale = (float)Screen.width / width;
        float heightScale = (float)Screen.height / height;
        if (widthScale < heightScale)
        {
            Camera.main.orthographicSize = (width * (float)Screen.height / (float)Screen.width) / 2;
            imageSizeScale = (float)Screen.height / (float)Screen.width;
        }
        else
        {
            Camera.main.orthographicSize = height / 2;
        }


        // set camera parameters.
        double fx;
        double fy;
        double cx;
        double cy;

        string loadDirectoryPath = Path.Combine(Application.persistentDataPath, "ArUcoCameraCalibrationExample");
        string calibratonDirectoryName = "camera_parameters" + width + "x" + height;
        string loadCalibratonFileDirectoryPath = Path.Combine(loadDirectoryPath, calibratonDirectoryName);
        string loadPath = Path.Combine(loadCalibratonFileDirectoryPath, calibratonDirectoryName + ".xml");
        if (useStoredCameraParameters && File.Exists(loadPath))
        {
            CameraParameters param;
            XmlSerializer serializer = new XmlSerializer(typeof(CameraParameters));
            using (var stream = new FileStream(loadPath, FileMode.Open))
            {
                param = (CameraParameters)serializer.Deserialize(stream);
            }

            camMatrix = param.GetCameraMatrix();
            distCoeffs = new MatOfDouble(param.GetDistortionCoefficients());

            fx = param.camera_matrix[0];
            fy = param.camera_matrix[4];
            cx = param.camera_matrix[2];
            cy = param.camera_matrix[5];

            Debug.Log("Loaded CameraParameters from a stored XML file.");
            Debug.Log("loadPath: " + loadPath);

        }
        else
        {
            int max_d = (int)Mathf.Max(width, height);
            fx = max_d;
            fy = max_d;
            cx = width / 2.0f;
            cy = height / 2.0f;

            camMatrix = new Mat(3, 3, CvType.CV_64FC1);
            camMatrix.put(0, 0, fx);
            camMatrix.put(0, 1, 0);
            camMatrix.put(0, 2, cx);
            camMatrix.put(1, 0, 0);
            camMatrix.put(1, 1, fy);
            camMatrix.put(1, 2, cy);
            camMatrix.put(2, 0, 0);
            camMatrix.put(2, 1, 0);
            camMatrix.put(2, 2, 1.0f);

            // fx   0   cx
            // 0    fy  cy
            // 0    0   1

            distCoeffs = new MatOfDouble(0, 0, 0, 0);

            Debug.Log("Created a dummy CameraParameters.");
        }

        Debug.Log("camMatrix " + camMatrix.dump());
        Debug.Log("distCoeffs " + distCoeffs.dump());


        // calibration camera matrix values.
        Size imageSize = new Size(width * imageSizeScale, height * imageSizeScale);
        double apertureWidth = 0;
        double apertureHeight = 0;
        double[] fovx = new double[1];
        double[] fovy = new double[1];
        double[] focalLength = new double[1];
        Point principalPoint = new Point(0, 0);
        double[] aspectratio = new double[1];

        Calib3d.calibrationMatrixValues(camMatrix, imageSize, apertureWidth, apertureHeight, fovx, fovy, focalLength, principalPoint, aspectratio);

        Debug.Log("imageSize " + imageSize.ToString());
        Debug.Log("apertureWidth " + apertureWidth);
        Debug.Log("apertureHeight " + apertureHeight);
        Debug.Log("fovx " + fovx[0]);
        Debug.Log("fovy " + fovy[0]);
        Debug.Log("focalLength " + focalLength[0]);
        Debug.Log("principalPoint " + principalPoint.ToString());
        Debug.Log("aspectratio " + aspectratio[0]);


        // To convert the difference of the FOV value of the OpenCV and Unity. 
        double fovXScale = (2.0 * Mathf.Atan((float)(imageSize.width / (2.0 * fx)))) / (Mathf.Atan2((float)cx, (float)fx) + Mathf.Atan2((float)(imageSize.width - cx), (float)fx));
        double fovYScale = (2.0 * Mathf.Atan((float)(imageSize.height / (2.0 * fy)))) / (Mathf.Atan2((float)cy, (float)fy) + Mathf.Atan2((float)(imageSize.height - cy), (float)fy));

        Debug.Log("fovXScale " + fovXScale);
        Debug.Log("fovYScale " + fovYScale);


        // Adjust Unity Camera FOV https://github.com/opencv/opencv/commit/8ed1945ccd52501f5ab22bdec6aa1f91f1e2cfd4
        if (widthScale < heightScale)
        {
            arCamera.fieldOfView = (float)(fovx[0] * fovXScale);
        }
        else
        {
            arCamera.fieldOfView = (float)(fovy[0] * fovYScale);
        }
        // Display objects near the camera.
        arCamera.nearClipPlane = 0.01f;


        rgbMat = new Mat(webCamTextureMat.rows(), webCamTextureMat.cols(), CvType.CV_8UC3);
        ids = new Mat();
        corners = new List<Mat>();
        rejectedCorners = new List<Mat>();
        rvecs = new Mat();
        tvecs = new Mat();
        rotMat = new Mat(3, 3, CvType.CV_64FC1);


        transformationM = new Matrix4x4();

        invertYM = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, -1, 1));
        Debug.Log("invertYM " + invertYM.ToString());

        invertZM = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, 1, -1));
        Debug.Log("invertZM " + invertZM.ToString());

        detectorParams = DetectorParameters.create();
        dictionary = Aruco.getPredefinedDictionary((int)dictionaryId);

        rvec = new Mat();
        tvec = new Mat();
        recoveredIdxs = new Mat();

        gridBoard = GridBoard.create(gridBoradMarkersX, gridBoradMarkersY, gridBoradMarkerLength, gridBoradMarkerSeparation, dictionary, gridBoradMarkerFirstMarker);

        charucoCorners = new Mat();
        charucoIds = new Mat();
        charucoBoard = CharucoBoard.create(chArUcoBoradSquaresX, chArUcoBoradSquaresY, chArUcoBoradSquareLength, chArUcoBoradMarkerLength, dictionary);

        diamondCorners = new List<Mat>();
        diamondIds = new Mat(1, 1, CvType.CV_32SC4);
        diamondIds.put(0, 0, new int[] { diamondId1, diamondId2, diamondId3, diamondId4 });


        // if WebCamera is frontFaceing, flip Mat.
        if (webCamTextureToMatHelper.GetWebCamDevice().isFrontFacing)
        {
            webCamTextureToMatHelper.flipHorizontal = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (webCamTextureToMatHelper.IsPlaying() && webCamTextureToMatHelper.DidUpdateThisFrame())
        {

            Mat rgbaMat = webCamTextureToMatHelper.GetMat();

            Imgproc.cvtColor(rgbaMat, rgbMat, Imgproc.COLOR_RGBA2RGB);

            // detect markers.
            Aruco.detectMarkers(rgbMat, dictionary, corners, ids, detectorParams, rejectedCorners, camMatrix, distCoeffs);
            //Debug.Log("Before: " + ids.dump());
            RemoveDuplicates(ref corners, ref ids);
            //Debug.Log("After: " + ids.dump());

            // if at least one marker detected
            if (ids.total() > 0)
            {
                Aruco.drawDetectedMarkers(rgbMat, corners, ids, new Scalar(0, 255, 0));


                // estimate pose.
                if (applyEstimationPose)
                {
                    EstimatePoseCanonicalMarker(rgbMat);
                }

            }

            if (showRejectedCorners && rejectedCorners.Count > 0)
                Aruco.drawDetectedMarkers(rgbMat, rejectedCorners, new Mat(), new Scalar(255, 0, 0));


            Imgproc.putText(rgbaMat, "W:" + rgbaMat.width() + " H:" + rgbaMat.height() + " SO:" + Screen.orientation, new Point(5, rgbaMat.rows() - 10), Core.FONT_HERSHEY_SIMPLEX, 1.0, new Scalar(255, 255, 255, 255), 2, Imgproc.LINE_AA, false);

            Utils.matToTexture2D(rgbMat, texture, webCamTextureToMatHelper.GetBufferColors());
        }

    }

    //TODO UNTESTED
    /// <summary>
    /// Takes the dump() from the mat and reconstructs the same mat
    /// Useful for broken mats that output null when accessed with get()
    /// </summary>
    /// <param name="mat"></param>
    /// <returns></returns>
    private Mat RepairMat(Mat mat)
    {
        //TODO not working :/
        Mat copy = new Mat();
        String dump = mat.dump();
        dump = dump.Replace("[", "");
        dump = dump.Replace("]", "");
        int[] matContents = Array.ConvertAll(dump.Split(','), s => int.Parse(s));
        int arrayIndex = 0;
        for (int i = 0; i < mat.width(); i++)
        {
            Debug.Log(i);
            double[] point = new double[] { matContents[arrayIndex], matContents[arrayIndex + 1] };
            copy.put(0, i, point);
            arrayIndex += 2;
        }
        Debug.Log("UNTESTED " + copy.dump());

        mat = copy;

        return mat;
    }



    private void RemoveDuplicates(ref List<Mat> corners, ref Mat ids)
    {
        if (ids.total() == 0 || ids.total() == 1)
            return;
        //Values are only added to the list if they exist in ids, hence removalList.len <= ids.len
        List<int> keepList = new List<int>();

        foreach (int[] arr in groupedGroupList)
        {

            if (arr.Length == 0 || arr.Length == 1)
                continue;

            double largestArea = 0;
            int markerIndex = -1;
            //Loop over a subgroup of the grouplist. The subgroup contains id's that are grouped
            for (int i = 0; i < arr.Length; i++)
            {
                //If an id from that group was found in the image, we check if it it has the biggest area out of the found members of its group
                //I have made the simplification that larger area equals higher marker tracking quality. Should be decent
                //TODO Try making a boundingRect and checking the difference in size
                //i = index in the subgroup
                //arr[i] = marker id
                //indices[0] = index in the list
                int[] indices = new int[2];
                if (MatContains(ids, arr[i], ref indices))
                {
                    //Debug.Log("i: " + i + " indices[0]: " + indices[0] + " arr[i]: " + arr[i] + " arr.len: " + arr.Length);
                    double currentArea = Imgproc.contourArea(corners[indices[0]]);
                    if (currentArea > largestArea)
                    {
                        largestArea = currentArea;
                        markerIndex = arr[i];
                    }
                    //Debug.Log("Index: " + arr[i] + " , Area: " + currentArea);

                    //TODO properly remake corners and ids, maybe not necessary
                }
            }

            if (largestArea != 0 && markerIndex != -1)
            {
                keepList.Add(markerIndex);
            }
        }

        //If we are going to keep everything
        if (keepList.Count == ids.total())
            return;

        List<Mat> newCorners = new List<Mat>();
        Mat copy = new Mat(keepList.Count, ids.cols(), ids.type());
        //for (int i = 0; i < ids.total(); i++)
        //{
        //    double dub = ids.get(i, 0)[0];

        //    //If it's on the keepList, copy it over
        //    if (keepList.Contains((int)ids.get(i, 0)[0]))
        //    {
        //        copy.put(i, 0, dub); //THIS CAUSES WRONG NUMBERS FOR SOME REASON
        //        Debug.Log("Kept: " + dub);
        //    }
        //}

        for (int i = 0; i < keepList.Count; i++)
        {
            copy.put(i, 0, new double[] { keepList[i] });
            newCorners.Add(corners.ElementAt(IndexInIdsMat(ids, keepList[i])));
        }

        ids = copy;
        corners = newCorners;

        return;
    }

    private bool IntArrayContains(int[] arr, int val)
    {
        foreach (int num in arr)
        {
            if (num == val)
                return true;
        }

        return false;
    }

    //Only for use with Ids, which is vertical
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="val">Value (Marker ID) to find index for</param>
    /// <returns>Index if found, else -1</returns>
    private int IndexInIdsMat(Mat ids, int val)
    {
        for (int i = 0; i < ids.height(); i++)
        {
            if (ids.get(i, 0)[0] == val)
                return i;
        }

        return -1;
    }

    private bool MatContains(Mat mat, int val)
    {
        for (int i = 0; i < mat.height(); i++)
        {
            for (int j = 0; j < mat.width(); j++)
            {
                if (mat.get(i, j)[0] == val)
                    return true;
            }
        }

        return false;
    }

    private bool MatContains(Mat mat, int val, ref int[] indices)
    {
        for (int i = 0; i < mat.height(); i++)
        {
            for (int j = 0; j < mat.width(); j++)
            {
                if (mat.get(i, j)[0] == val)
                {
                    indices[0] = i;
                    indices[1] = j;
                    return true;
                }

            }
        }

        return false;
    }

    private string IntArrayToString(int[] arr)
    {
        String printout = "";
        foreach (int num in arr)
        {
            printout += num + " ";
        }
        printout = printout.TrimEnd();

        return printout;
    }

    public static void SaveTextureAsPNG(Texture2D _texture, string _fullPath)
    {
        byte[] _bytes = _texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(_fullPath, _bytes);
        Debug.Log(_bytes.Length / 1024 + "Kb was saved as: " + _fullPath);
    }

    private void EstimatePoseCanonicalMarker(Mat rgbMat)
    {
        Aruco.estimatePoseSingleMarkers(corners, markerLength, camMatrix, distCoeffs, rvecs, tvecs);

        for (int i = 0; i < ids.total(); i++)
        {
            using (Mat rvec = new Mat(rvecs, new OpenCVForUnity.Rect(0, i, 1, 1)))
            using (Mat tvec = new Mat(tvecs, new OpenCVForUnity.Rect(0, i, 1, 1)))
            {
                // In this example we are processing with RGB color image, so Axis-color correspondences are X: blue, Y: green, Z: red. (Usually X: red, Y: green, Z: blue)
                Aruco.drawAxis(rgbMat, camMatrix, distCoeffs, rvec, tvec, markerLength * 0.5f);

                UpdateARObjectTransform(rvec, tvec, i);
            }
        }
    }

    private void UpdateARObjectTransform(Mat rvec, Mat tvec, int index)
    {
        //If there are not enough arGameObjects, just return
        if (index >= arGameObjectList.Length)
        {
            Debug.Log("Not enough arGameObjects, i: " + index);
            return;
        }

        // Position
        double[] tvecArr = new double[3];
        tvec.get(0, 0, tvecArr);

        // Rotation
        Calib3d.Rodrigues(rvec, rotMat);

        double[] rotMatArr = new double[rotMat.total()];
        rotMat.get(0, 0, rotMatArr);

        transformationM.SetRow(0, new Vector4((float)rotMatArr[0], (float)rotMatArr[1], (float)rotMatArr[2], (float)tvecArr[0]));
        transformationM.SetRow(1, new Vector4((float)rotMatArr[3], (float)rotMatArr[4], (float)rotMatArr[5], (float)tvecArr[1]));
        transformationM.SetRow(2, new Vector4((float)rotMatArr[6], (float)rotMatArr[7], (float)rotMatArr[8], (float)tvecArr[2]));
        transformationM.SetRow(3, new Vector4(0, 0, 0, 1));

        // right-handed coordinates system (OpenCV) to left-handed one (Unity)
        ARM = invertYM * transformationM;

        // Apply Z axis inverted matrix.
        ARM = ARM * invertZM;

        if (shouldMoveARCamera)
        {

            ARM = arGameObjectList[index].transform.localToWorldMatrix * ARM.inverse; //TODO MAYBE NOT [0]

            ARUtils.SetTransformFromMatrix(arCamera.transform, ref ARM);

        }
        else
        {

            ARM = arCamera.transform.localToWorldMatrix * ARM;

            ARUtils.SetTransformFromMatrix(arGameObjectList[index].transform, ref ARM);//TODO MAYBE NOT [0]
        }
    }

    void LoopMatList(List<Mat> matList)
    {
        foreach (Mat mat in matList)
        {
            for (int i = 0; i < mat.height(); i++)
            {
                for (int j = 0; j < mat.width(); j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        Debug.Log(i + " " + j + " " + k);
                        Debug.Log(mat.get(i, j)[k]);
                    }
                }
            }
        }
    }

    void PrintCorners(List<Mat> matList)
    {
        Debug.Log("Print: ");
        foreach (Mat mat in matList)
        {
            Debug.Log("Mat: ");
            Debug.Log(mat.dump());
        }
    }

    public enum ArUcoDictionary
    {
        DICT_4X4_50 = Aruco.DICT_4X4_50,
        DICT_4X4_100 = Aruco.DICT_4X4_100,
        DICT_4X4_250 = Aruco.DICT_4X4_250,
        DICT_4X4_1000 = Aruco.DICT_4X4_1000,
        DICT_5X5_50 = Aruco.DICT_5X5_50,
        DICT_5X5_100 = Aruco.DICT_5X5_100,
        DICT_5X5_250 = Aruco.DICT_5X5_250,
        DICT_5X5_1000 = Aruco.DICT_5X5_1000,
        DICT_6X6_50 = Aruco.DICT_6X6_50,
        DICT_6X6_100 = Aruco.DICT_6X6_100,
        DICT_6X6_250 = Aruco.DICT_6X6_250,
        DICT_6X6_1000 = Aruco.DICT_6X6_1000,
        DICT_7X7_50 = Aruco.DICT_7X7_50,
        DICT_7X7_100 = Aruco.DICT_7X7_100,
        DICT_7X7_250 = Aruco.DICT_7X7_250,
        DICT_7X7_1000 = Aruco.DICT_7X7_1000,
        DICT_ARUCO_ORIGINAL = Aruco.DICT_ARUCO_ORIGINAL,
    }

    private void RemoveDuplicatesRefactored(ref List<Mat> corners, ref Mat ids)
    {
        if (ids.total() == 0 || ids.total() == 1)
            return;
        //Values are only added to the list if they exist in ids, hence removalList.len <= ids.len
        List<int> removalList = new List<int>();

        foreach (int[] arr in groupList)
        {
            if (arr.Length == 0 || arr.Length == 1)
                continue;
            //Only goes to length - 1, because if we are at the last element, there is nothing to remove after it
            for (int i = 0; i < arr.Length - 1; i++)
            {
                int[] indices = new int[2];
                if (MatContains(ids, arr[i], ref indices))
                {
                    Debug.Log("i: " + indices[0] + " j: " + indices[1]);
                    //TODO properly remake corners and ids
                    for (int j = i + 1; j < arr.Length; j++)
                    {
                        if (MatContains(ids, arr[j]))
                        {
                            removalList.Add(arr[j]);
                        }

                    }
                }
            }
        }

        if (removalList.Count == 0)
            return;

        //TODO Test if it works
        Mat copy = new Mat(ids.rows() - removalList.Count, ids.cols(), ids.type());
        for (int i = 0; i < ids.total(); i++)
        {
            double dub = ids.get(i, 0)[0];

            //If it's not on the removalList, copy it over
            if (!removalList.Contains((int)ids.get(i, 0)[0]))
            {
                copy.put(i, 0, dub);
            }
        }

        ids = copy;

        return;
    }
}