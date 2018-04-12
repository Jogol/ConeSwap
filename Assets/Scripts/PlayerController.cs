using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerController : MonoBehaviour
{


    //Public variables
    public ControllerCollider leftControllerCollider;
    public ControllerCollider rightControllerCollider;
    //Private variables

    private int leftDeviceIndex;
    private int rightDeviceIndex;

    private int heldFrames = 0;



    void Start()
    {
        
        leftDeviceIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);
        rightDeviceIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);
    }

    void Update()
    {

        //Debug.DrawLine(new Vector3(0, 0, 0),new Vector3(2, 100, 0), Color.green, 2f, false);
        //If holding both right and left trigger
        if (leftDeviceIndex != -1 && SteamVR_Controller.Input(leftDeviceIndex).GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && SteamVR_Controller.Input(rightDeviceIndex).GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            heldFrames++;
            if (heldFrames > 200)
            {
                GameObject leftCC = leftControllerCollider.GetColliding();
                GameObject rightCC = rightControllerCollider.GetColliding();

                if (leftCC != null && rightCC != null)
                {
                    Vector3 leftPos = leftCC.transform.position;
                    Vector3 rightPos = rightCC.transform.position;

                    leftCC.transform.position = rightPos;
                    rightCC.transform.position = leftPos;

                    SteamVR_Controller.Input(leftDeviceIndex).TriggerHapticPulse(500);
                }
                
                heldFrames = 0;
            }
            
            
            
        } else
        {
            heldFrames = 0;
        }

        if (rightDeviceIndex != -1 && SteamVR_Controller.Input(rightDeviceIndex).GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            SteamVR_Controller.Input(rightDeviceIndex).TriggerHapticPulse(500);
        }

        if (((rightDeviceIndex != -1 && SteamVR_Controller.Input(rightDeviceIndex).GetPressDown(SteamVR_Controller.ButtonMask.Grip)) || (leftDeviceIndex != -1 && SteamVR_Controller.Input(leftDeviceIndex).GetPressDown(SteamVR_Controller.ButtonMask.Grip))))
        {
            if (rightDeviceIndex != -1 && SteamVR_Controller.Input(rightDeviceIndex).GetPressDown(SteamVR_Controller.ButtonMask.Grip))
            {
                
            }

        }

    }
}