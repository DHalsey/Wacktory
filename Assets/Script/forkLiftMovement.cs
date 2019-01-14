using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forkLiftMovement : MonoBehaviour {

    // How fast the right joy stick rotates the forklift's back wheel
    public float turnSpeed = 1.0f;
    // Current angle of back wheel's rotation
    int wheelRotationAngle = 0; // Rotation angle between 90 and -90 degrees where 0 degrees is pointing forward
    // How fast the forklift can go at max speed
    public float maxSpeed = 1.0f;
    // How fast the forklift is currently going
    public float currentSpeed = 0.0f;

    // Holding controller inputs
    private Vector3 leftStick;
    private Vector3 rightStick;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        // If L3 (left joystick) is pressed, honk the horn

		
	}

    // Honks the forklift's horn
    void honk() {

    }
}
