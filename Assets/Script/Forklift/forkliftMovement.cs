using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Forklift movement
    Movement is modeled after a single, rear-wheel drive forklift.
    The pivot point is in the center of the forklift body.
*/
public class forkliftMovement : MonoBehaviour {

    // Wheel
    public float rearWheelRotation = 0.0f; // Wheel rotation between [85 deg, -85 deg]
    private float maxWheelRotation = 85.0f; // +-85 degrees from 0 is max rear wheel rotation
    public float wheelRotationAmt = 0.5f;
    public float turningDeadZone = 35.0f; // Maximum speed output between +-turning dead zone. Meaning speed will remain at full output while the wheel is between +-35 deg.
    private Transform rearWheel;

    // Speed
    public float currentSpeed = 0.0f; // How fast the forklift is currently moving in either direction
    public float incrementSpeedAmt = 0.05f; // How fast the forklift can speed up going forward
    public float maxForwardSpeed = 1.0f;
    public float maxBackwardSpeed = -1.0f;
    public float slowDownAmt = 0.005f; // How fast the forklift will return to a resting states
    
    // Fork
    public float maxForkHeight = 2.0f; // Passing this point will over shoot the top of the fork backing
    public float currentForkHeight = 0.0f;
    public float forkRaiseAmt = 0.1f;
    private Transform rightFork;
    private Transform leftFork;
    
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rearWheel = transform.Find("rearWheel");
        rightFork = transform.Find("forkBacking").Find("rightFork");
        leftFork = transform.Find("forkBacking").Find("leftFork");
        rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update () {
        // Moving forklift forward
        if (Input.GetKey("w")) {
            if (currentSpeed >= maxForwardSpeed) {
                currentSpeed = maxForwardSpeed;
            }
            else {
                currentSpeed += incrementSpeedAmt;
            }
        }
        // Turning rear wheel counter clockwise (left, towards +90deg) by wheel rotation amount 
        if (Input.GetKey("a")) {
            if(rearWheelRotation >= maxWheelRotation) {
                rearWheelRotation = maxWheelRotation;
            }
            else {
                rearWheelRotation += wheelRotationAmt;

                // Updating rear wheel from new wheel rotation
                rearWheel.transform.eulerAngles += new Vector3(0.0f, wheelRotationAmt, 0.0f);
            }

            // Debug.Log("rearWheelRotation: " + rearWheelRotation);
        }
        // Moving forklift backwards
        if (Input.GetKey("s")) {
            if (currentSpeed <= maxBackwardSpeed) {
                currentSpeed = maxBackwardSpeed;
            }
            else {
                currentSpeed -= incrementSpeedAmt;
            }
        }
        // Turning rear wheel clockwise (right, towards -90deg) by wheel rotation amount 
        if (Input.GetKey("d")) {
            if (rearWheelRotation <= -maxWheelRotation) {
                rearWheelRotation = -maxWheelRotation;
            }
            else {
                rearWheelRotation -= wheelRotationAmt;

                // Updating rear wheel from new wheel rotation
                rearWheel.transform.eulerAngles += new Vector3(0.0f, -wheelRotationAmt, 0.0f);
            }

            // Debug.Log("rearWheelRotation: " + rearWheelRotation);
        }
        // Raising forks
        if (Input.GetKey("q")) {
            if (currentForkHeight >= maxForkHeight) {
                currentForkHeight = maxForkHeight;
            }
            else {
                currentForkHeight += forkRaiseAmt;

                // Updating rear wheel from new wheel rotation
                rightFork.transform.position += new Vector3(0.0f, currentForkHeight * Time.deltaTime, 0.0f);
                leftFork.transform.position += new Vector3(0.0f, currentForkHeight * Time.deltaTime, 0.0f);
            }

        }
        // Raising forks
        if (Input.GetKey("e")) {
            if (currentForkHeight <= 0.0f)
            {
                currentForkHeight = 0.0f;
            }
            else
            {
                currentForkHeight -= forkRaiseAmt;

                // Updating rear wheel from new wheel rotation
                rightFork.transform.position -= new Vector3(0.0f, currentForkHeight * Time.deltaTime, 0.0f);
                leftFork.transform.position -= new Vector3(0.0f, currentForkHeight * Time.deltaTime, 0.0f);
            }

        }

        if (currentSpeed >= 0.01f) {
            currentSpeed -= slowDownAmt;
        }
        else if(currentSpeed <= -0.01f) {
            currentSpeed += slowDownAmt;
        }
        else {
            currentSpeed = 0.0f;
        }

        // Always moving forklift speed towards 0, towards standing still.
        if(currentSpeed != 0.0f) {
            moveForklift();
        }

	} // End - update

    /* Forward movement is calculated through a combination of current wheel angle and movement speed.
        If wheel angle is 0 degrees (completely forward), movement speed is 100% forward.
        However, if wheel angle is an angle other than 0 degrees, movement speed will be reduced by
        that ratio and translated into rotation, instead.
    */
    void moveForklift() {
        // Applying the forklift's calculated forward movement
        if(rearWheelRotation <= turningDeadZone && rearWheelRotation >= -turningDeadZone) {
            this.transform.position += transform.forward * Time.deltaTime * currentSpeed * 2.5f;
        }
        else {
            this.transform.position += transform.forward * Time.deltaTime * currentSpeed * (1 / (Mathf.Abs(rearWheelRotation / maxWheelRotation) / 1));
        }

        // Applying the forklift's calculated rotations
        this.transform.Rotate(0.0f, (-rearWheelRotation/maxWheelRotation), 0.0f);
    }

} // End - forkLiftMovement
