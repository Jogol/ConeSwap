using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderCheck : MonoBehaviour {

    Support support;
	// Use this for initialization
	void Start () {
        support = new Support();
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        Vector3 direction = new Vector3(0, 0, -1);
        int[] list = new int[5];
        float posX = 1.3f;
        float posY = 0.1f;
        float posZ = 0.5f;

        //Shoots rays at the cones, checking if they are in the right order
        //i should never become 5, but just in case
        for (int i = 0; (posX > -1.3); posX = posX - 0.1f) // && (i < 5)
        {
            Vector3 origin = new Vector3(posX, posY, posZ);
            //If the the ray hits a cone
            if (Physics.Raycast(origin, direction, out hit, 20))
            {
                if (support.tagIsCone(hit.collider.tag))
                {
                    Debug.DrawLine(origin, hit.point, Color.green, 2f, false);
                    //If we just started list is empty and we always add
                    if (i == 0)
                    {
                        list[i] = support.getConeNumberFromTag(hit.collider.tag);
                        i++;
                    } else if (support.getConeNumberFromTag(hit.collider.tag) != list[i - 1]) //If the current hit object is not the same as the last
                    {
                        list[i] = support.getConeNumberFromTag(hit.collider.tag);
                        i++;
                    }
                }
            }
            
        }

        support.DebugLogArray(list);

        if (support.IsSortedAscending(list) || support.IsSortedDescending(list))
        {
            Debug.Log("Correct!");
        } else
        {
            Debug.Log("Incorrect...");
        }


    }
}
