using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerController : MonoBehaviour
{


    //Public variables
    public GameObject leftButtonLeft;
    public GameObject leftButtonRight;

    public GameObject rightButtonLeft;
    public GameObject rightButtonRight;

    //Private variables
    private GameObject leftController;
    private GameObject rightController;

    private int leftDeviceIndex;
    private int rightDeviceIndex;



    void Start()
    {
        
        leftDeviceIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);
        rightDeviceIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);
    }

    void Update()
    {

        if (leftDeviceIndex != -1 && SteamVR_Controller.Input(leftDeviceIndex).GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {

            SteamVR_Controller.Input(leftDeviceIndex).TriggerHapticPulse(500);
            
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