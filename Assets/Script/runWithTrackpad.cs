using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class runWithTrackpad : MonoBehaviour {
    private Vector2 trackPadLeftPos;
    public float acceleration = 1000.0f;
    private float maxSpeed = 2.0f;
    private Rigidbody rb; // the rigidbody of the player
    public SteamVR_Action_Vector2 touchPadAction;
	// Use this for initialization
	void Start () {
        rb = this.gameObject.GetComponent<Rigidbody>();
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
        trackPadLeftPos = touchPadAction.GetAxis(SteamVR_Input_Sources.Any);
        Debug.Log(trackPadLeftPos);

    }

    void MovePlayer() {
        Vector3 direction = new Vector3(trackPadLeftPos.x, 0, trackPadLeftPos.y);
        rb.AddForce(direction*acceleration*Time.deltaTime);
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
