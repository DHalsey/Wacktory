using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controllerRig : MonoBehaviour {

    public ControllerMap TestController;
	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis(TestController.HorizontalLeft + "p1") > 0.0f)
            Debug.Log("L Right");
        if (Input.GetAxis(TestController.HorizontalLeft + "p1") < 0.0f)
            Debug.Log("L Left");
        if (Input.GetAxis(TestController.VerticalLeft + "p1") > 0.0f)
            Debug.Log("L Up");
        if (Input.GetAxis(TestController.VerticalLeft + "p1") < 0.0f)
            Debug.Log("L Down");
        if (Input.GetAxis(TestController.BottomButton + "p1") > 0.0f)
            Debug.Log("bottom");
        if (Input.GetAxis(TestController.LeftButton + "p1") > 0.0f)
            Debug.Log("left");
        if (Input.GetAxis(TestController.RightButton + "p1") > 0.0f)
            Debug.Log("right");
        if (Input.GetAxis(TestController.TopButton + "p1") > 0.0f)
            Debug.Log("top");
        if (Input.GetAxis(TestController.HorizontalDPad + "p1") > 0.0f)
            Debug.Log("DP Right");
        if (Input.GetAxis(TestController.HorizontalDPad + "p1") < 0.0f)
            Debug.Log("DP Left");
        if (Input.GetAxis(TestController.VerticalDPad + "p1") > 0.0f)
            Debug.Log("DP Up");
        if (Input.GetAxis(TestController.VerticalDPad + "p1") < 0.0f)
            Debug.Log("DP Down");
        if (Input.GetAxis(TestController.HorizontalRight + "p1") > 0.0f)
            Debug.Log("R Right");
        if (Input.GetAxis(TestController.HorizontalRight + "p1") < 0.0f)
            Debug.Log("R Left");
        if (Input.GetAxis(TestController.VerticalRight + "p1") > 0.0f)
            Debug.Log("R Up");
        if (Input.GetAxis(TestController.VerticalRight + "p1") < 0.0f)
            Debug.Log("R Down");
        if (Input.GetAxis(TestController.RightTrigger + "p1") > 0.0f)
            Debug.Log("Right Trigger");
        if (Input.GetAxis(TestController.RightBumper + "p1") > 0.0f)
            Debug.Log("Right Bumper");
        if (Input.GetAxis(TestController.LeftTrigger + "p1") > 0.0f)
            Debug.Log("Left Trigger");
        if (Input.GetAxis(TestController.LeftBumper + "p1") > 0.0f)
            Debug.Log("Left Bumper");
        if (Input.GetAxis(TestController.StartButton + "p1") > 0.0f)
            Debug.Log("Start");
        if (Input.GetAxis(TestController.SelectButton + "p1") > 0.0f)
            Debug.Log("Select");
        if (Input.GetAxis(TestController.LeftStick + "p1") > 0.0f)
            Debug.Log("LeftStick");
        if (Input.GetAxis(TestController.RightStick + "p1") > 0.0f)
            Debug.Log("RightStick");




    }
}
