using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Orientator : MonoBehaviour {

    public string cubeName = "default";
    int num = 0;
    Vector3 upmost;

	// Use this for initialization
	void Start () {
        upmost = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
        float up = Vector3.Angle(transform.up, Vector3.up);
        float down = Vector3.Angle(-transform.up, Vector3.up);
        float forward = Vector3.Angle(transform.forward, Vector3.up);
        float backward = Vector3.Angle(-transform.forward, Vector3.up);
        float right = Vector3.Angle(transform.right, Vector3.up);
        float left = Vector3.Angle(-transform.right, Vector3.up);
        float min = Mathf.Min(up, Mathf.Min(down, Mathf.Min(forward, Mathf.Min(backward, Mathf.Min(right, left)))));

        if (min == up)
        {
            upmost = Vector3.up;
            num = 4;
        } else if (min == down)
        {
            upmost = -Vector3.up;
            num = 3;
        } else if (min == forward)
        {
            upmost = Vector3.forward;
            num = 5;
        } else if (min == backward)
        {
            upmost = -Vector3.forward;
            num = 2;
        } else if (min == right)
        {
            upmost = Vector3.right;
            num = 1;
        } else if (min == left)
        {
            upmost = -Vector3.right;
            num = 6;
        } else
        {
            Debug.LogError("WTF");
        }
    }

    public int GetNum()
    {
        return num;
    }
}
