using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MatchingManager : MonoBehaviour {

    public GameObject dotCube;
    public GameObject starCube;
    public GameObject triCube;

    public GameObject dotSlot;
    public GameObject starSlot;
    public GameObject triSlot;

    public GameObject showDot;
    public GameObject showStar;
    public GameObject showTri;

    public GameObject dotLight;
    public GameObject starLight;
    public GameObject triLight;

    public Material redGlow;
    public Material greenGlow;

    public Material dot1;
    public Material dot2;
    public Material dot3;
    public Material dot4;
    public Material dot5;
    public Material dot6;
    public Material star1;
    public Material star2;
    public Material star3;
    public Material star4;
    public Material star5;
    public Material star6;
    public Material tri1;
    public Material tri2;
    public Material tri3;
    public Material tri4;
    public Material tri5;
    public Material tri6;

    public GameObject[] showSpawn;
    public GameObject[] slotSpawn;
    public int score = 0;

    public UnityEvent incrementScore;

    GameObject[] showList;
    GameObject[] slotList;

    int currentDotGoal;
    int currentStarGoal;
    int currentTriGoal;

    bool dotCorrect = false;
    bool starCorrect = false;
    bool triCorrect = false;

    bool reroll = true;

    int currentDotUp = 0;
    int currentStarUp = 0;
    int currentTriUp = 0;

    List<int> currentOrder;

	// Use this for initialization
	void Start () {
        showList = new GameObject[] { showDot, showStar, showTri };
        slotList = new GameObject[] { dotSlot, starSlot, triSlot };
    }
	
	// Update is called once per frame
	void Update () {
        currentDotUp = dotCube.GetComponent<Orientator>().GetNum();
        currentStarUp = starCube.GetComponent<Orientator>().GetNum();
        currentTriUp = triCube.GetComponent<Orientator>().GetNum();

        //Debug.Log("Dot: " + currentDotUp);

        if (reroll)
        {
            reroll = false;
            currentDotGoal = Random.Range(1, 6);
            currentStarGoal = Random.Range(1, 6);
            currentTriGoal = Random.Range(1, 6);

            currentOrder = NewOrder();

            showList[currentOrder[0]].transform.position = showSpawn[0].transform.position;
            slotList[currentOrder[0]].transform.position = slotSpawn[0].transform.position;

            showList[currentOrder[1]].transform.position = showSpawn[1].transform.position;
            slotList[currentOrder[1]].transform.position = slotSpawn[1].transform.position;

            showList[currentOrder[2]].transform.position = showSpawn[2].transform.position;
            slotList[currentOrder[2]].transform.position = slotSpawn[2].transform.position;

            showDot.GetComponent<Renderer>().material = IntToDotMaterial(currentDotGoal);
            showStar.GetComponent<Renderer>().material = IntToStarMaterial(currentStarGoal);
            showTri.GetComponent<Renderer>().material = IntToTriMaterial(currentTriGoal);
        }

        if (dotSlot.GetComponent<ContainsCube>().ContainsDotCube() && currentDotGoal == currentDotUp)
        {
            dotCorrect = true;
            dotLight.GetComponent<Renderer>().material = greenGlow;
        } else
        {
            dotCorrect = false;
            dotLight.GetComponent<Renderer>().material = redGlow;
        }

        if (starSlot.GetComponent<ContainsCube>().ContainsStarCube() && currentStarGoal == currentStarUp)
        {
            starCorrect = true;
            starLight.GetComponent<Renderer>().material = greenGlow;
        } else
        {
            starCorrect = false;
            starLight.GetComponent<Renderer>().material = redGlow;
        }

        if (triSlot.GetComponent<ContainsCube>().ContainsTriCube() && currentTriGoal == currentTriUp)
        {
            triCorrect = true;
            triLight.GetComponent<Renderer>().material = greenGlow;
        } else
        {
            triCorrect = false;
            triLight.GetComponent<Renderer>().material = redGlow;
        }

        if (dotCorrect && starCorrect && triCorrect)
        {
            reroll = true;
            score++;
            incrementScore.Invoke();
        }
    }

    private List<int> NewOrder()
    {
        List<int> list = new List<int> { 0, 1, 2};
        list = list.OrderBy(i => Random.value).ToList();

        return list;
    }

    Material IntToDotMaterial (int num)
    {
        switch (num)
        {
            case 1:
                return dot1;
            case 2:
                return dot2;
            case 3:
                return dot3;
            case 4:
                return dot4;
            case 5:
                return dot5;
            case 6:
                return dot6;
            default:
                return null;
        }
    }

    Material IntToStarMaterial(int num)
    {
        switch (num)
        {
            case 1:
                return star1;
            case 2:
                return star2;
            case 3:
                return star3;
            case 4:
                return star4;
            case 5:
                return star5;
            case 6:
                return star6;
            default:
                return null;
        }
    }

    Material IntToTriMaterial(int num)
    {
        switch (num)
        {
            case 1:
                return tri1;
            case 2:
                return tri2;
            case 3:
                return tri3;
            case 4:
                return tri4;
            case 5:
                return tri5;
            case 6:
                return tri6;
            default:
                return null;
        }
    }

    void PrintList(List<int> list)
    {
        string result = "";
        foreach (int num in list)
        {
            result += num + " ";
        }
        Debug.Log(result);
    }
}
