using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forkliftMovement : MonoBehaviour {

    private float rearWheelRotation = 0.0f; // Wheel rotation between [85 deg, -85 deg]
    private float maxWheelRotation = 85.0f; // +-85 degrees from 0 is max rear wheel rotation
    public float wheelRotationAmt = 0.5f;

    private float currentSpeed = 0.0f; // How fast the forklift is currently moving in either direction
    public float IncSpeedForwardAmt = 0.1f; // How fast the forklift can speed up going forward
    public float IncSpeedBackwardsAmt = 0.05f; // How fast the forklift can speed up going backwards

    public float maxForwardSpeed = 15.0f;
    public float maxBackwardSpeed = -5.0f;
    public float slowDownAmt = 0.01f; // How fast the forklift will return to a resting states

    private Transform rearWheel;
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rearWheel = transform.Find("rearWheel");
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        // Moving forklift forward
        if (Input.GetKey("w")){
            if (currentSpeed >= maxForwardSpeed) {
                currentSpeed = maxForwardSpeed;
            }
            else {
                currentSpeed += IncSpeedForwardAmt;

                // Moving forklift forward
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
                rearWheel.transform.eulerAngles = new Vector3(0.0f, rearWheelRotation, 90.0f);
            }

            // Debug.Log("rearWheelRotation: " + rearWheelRotation);
        }
        // Moving forklift backwards
        if (Input.GetKey("s")) {
            if (currentSpeed <= maxBackwardSpeed) {
                currentSpeed = maxBackwardSpeed;
            }
            else {
                currentSpeed -= IncSpeedBackwardsAmt;

                
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
                rearWheel.transform.eulerAngles = new Vector3(0.0f, rearWheelRotation, 90.0f);
            }

            // Debug.Log("rearWheelRotation: " + rearWheelRotation);
        }

        if (currentSpeed > 0.0f) {
            currentSpeed -= slowDownAmt;
        }
        else if(currentSpeed < 0.0f) {
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

    // Forward movement is calculated through a combination of current wheel angle and movement speed
    // If wheel angle is 0 (completely forward), movement speed is 100% forward
    // If wheel angle is an angle other than 0, movement speed will be reduced by
    void moveForklift() {
        transform.position += transform.forward * Time.deltaTime * currentSpeed;
    }

} // End - forkLiftMovement
