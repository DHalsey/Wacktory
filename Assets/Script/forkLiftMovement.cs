using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forkLiftMovement : MonoBehaviour {

    // How fast the right joy stick rotates the forklift's back wheel
    public float turnSpeed = 0.4f;
    // Current angle of back wheel's rotation
    int wheelAngle = 0; // Rotation angle between 90 and -90 degrees where 0 degrees is pointing forward
    // How fast the forklift can go at max speed
    public float forwardSpeed = 15.0f;
    // How fast the forklift is currently going
    public float backwardSpeed = 15.0f;
    // How fast the forklift can gain speed
    public float accelRate = 0.05f;
    public float decelRate = 0.01f;

    // Holding controller inputs
    private Vector3 leftStick;
    private Vector3 rightStick;
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
		
	}

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

	// Update is called once per frame
	void Update () {

        // test forklift inputs
        if (Input.GetKey(KeyCode.W)) {
            rb.AddForce(transform.forward * forwardSpeed);
        }
        if (Input.GetKey(KeyCode.S)){
            rb.AddForce(-(transform.forward) * backwardSpeed);
        }
        if (Input.GetKey(KeyCode.D)){
            transform.Rotate(0, 1.5f, 0);
        }
        if (Input.GetKey(KeyCode.A)){
            transform.Rotate(0, -1.5f, 0);
        }
	}

    // Honks the forklift's horn
    void honk() {

    }
}
