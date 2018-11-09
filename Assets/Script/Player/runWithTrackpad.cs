using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class runWithTrackpad : MonoBehaviour {
    private Vector2 trackPadLeftPos;
    private bool trackPadLeftTap = false;
    public float acceleration = 1000.0f;
    public Transform headTransform;
    private Quaternion headForward; //used to handle the direction of the movement to be in relation of direction that the player is looking
    private float maxSpeed = 2.0f;
    private Rigidbody rb; // the rigidbody of the player

    public int test = 1; //A variable ONLY for testing (for toggles)

    private SteamVR_Action_Vector2 touchPadAction; //the position of the touched touch pad
    private SteamVR_Action_Boolean touchPadTap; //if the touch pad is being touched
	// Use this for initialization
	void Start () {
        rb = this.gameObject.GetComponent<Rigidbody>();
        touchPadAction = SteamVR_Input._default.inActions.TouchpadTouch;
        touchPadTap = SteamVR_Input._default.inActions.TouchpadTap;
	}
	
	// Update is called once per frame
	void Update () {
        GetControls();
	}

    private void FixedUpdate() {
        ApplyFriction();
        MovePlayer();
        ClampSpeed();
        
    }

    void GetControls() {
        trackPadLeftPos = touchPadAction.GetAxis(SteamVR_Input_Sources.LeftHand);
        if(test == 1) Test();
        if(test == 2) Test2();

    }

    void Test() {
        if (touchPadTap.GetStateDown(SteamVR_Input_Sources.LeftHand)) {
            headForward = headTransform.rotation;
            Debug.Log("Touched");
        }
    }

    void Test2() {
        headForward = headTransform.rotation;
    }

    void MovePlayer() {
        Vector3 direction = new Vector3(trackPadLeftPos.x, 0, trackPadLeftPos.y);
        rb.AddForce(headForward*direction*acceleration*Time.deltaTime);
    }

    //prevents the player from moving faster than the maximum speed
    void ClampSpeed() {
        if (rb.velocity.magnitude > maxSpeed) {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    //Simulates slowing down the player so decelleration is always the same
    void ApplyFriction() {

        Vector3 changedVelocity = rb.velocity - rb.velocity.normalized * 0.05f;

        rb.velocity = changedVelocity;
    }
}
