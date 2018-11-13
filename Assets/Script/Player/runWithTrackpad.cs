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
    private Vector3 collisionNormal = new Vector3(0,0,0);
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
        headForward = Quaternion.identity; //to zero our the other angles
        headForward.y = headTransform.rotation.y; //we only care about the y rotation of the headset (for direction). we dont want to launch up

    }

    void MovePlayer() {
        Vector3 direction = new Vector3(trackPadLeftPos.x, 0, trackPadLeftPos.y); //the direction of input from the player
        Vector3 movementDirection = headForward * direction * acceleration * Time.deltaTime; //adjusts the direction of movement to be relative to the rotation of the player's head
        if (collisionNormal.y == 0) collisionNormal.y = 0.0000001f; //prevent divide by zero
        float newY = (-movementDirection.x * collisionNormal.x - movementDirection.z * collisionNormal.z) / collisionNormal.y; //gets the y value for the vector perpendicular to the ground collision
        Vector3 newMovement = new Vector3(movementDirection.x, newY, movementDirection.z); //the new direction of movement that is perpendicular to the normal we are walking on.  This ensures we can move up ramps properly
        rb.AddForce(newMovement);
        DrawDebugMovement(newMovement);
    }

    //prevents the player from moving faster than the maximum speed
    //we care only about horizontal speed as to not limit the fall speed of the player
    void ClampSpeed() {
        float horizontalMagnitude = Mathf.Sqrt(Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z,2)); //the speed of the rigidbody without consideration for the y
        if (horizontalMagnitude > maxSpeed) {           
            rb.velocity = new Vector3(rb.velocity.x * maxSpeed/horizontalMagnitude, rb.velocity.y, rb.velocity.z * maxSpeed / horizontalMagnitude);
        }
    }

    //Simulates slowing down the player so decelleration is always the same
    void ApplyFriction() { 

        Vector3 changedVelocity = rb.velocity * 0.95f;
        //if(Mathf.Sign(rb.velocity.magnitude) != changedVelocity.magnitude) {
        //    changedVelocity = Vector3.zero;
        //}
        rb.velocity = changedVelocity;
    }

    private void OnCollisionEnter(Collision collision) {
        Vector3 normalCol = collision.contacts[0].normal;
        Vector3 vecFlat = new Vector3(normalCol.x, 0, normalCol.z);
        Debug.Log(vecFlat.magnitude/normalCol.magnitude* Mathf.Rad2Deg);
        if (vecFlat.magnitude / normalCol.magnitude * Mathf.Rad2Deg <= 30) { //if we are walking on a slipe <=30 degrees
            collisionNormal = normalCol;
        }
        
    }
    private void OnCollisionStay(Collision collision) {
        Vector3 normalCol = collision.contacts[0].normal;
        Vector3 vecFlat = new Vector3(normalCol.x, 0, normalCol.z);
        if (vecFlat.magnitude / normalCol.magnitude * Mathf.Rad2Deg <= 30) { //if we are walking on a slipe <=30 degrees
            collisionNormal = normalCol;
        }
    }
    private void OnCollisionExit(Collision collision) {
        collisionNormal = Vector3.up;
    }


    //draws a debug line to show the direction of movement
    private void DrawDebugMovement(Vector3 newMovement) {
        LineRenderer line = this.gameObject.GetComponent<LineRenderer>();
        if (!line) { //if we dont have a linecomponent
            //add a line component and initialize it
            line = this.gameObject.AddComponent<LineRenderer>();
            line.positionCount = 2;
            line.useWorldSpace = false;
            line.startWidth = 0.2f;
            line.endWidth = 0.1f;
            line.numCapVertices = 10;
            line.SetPosition(0, new Vector3(0, 0.2f, 0));
        } else {
            line.SetPosition(1, line.GetPosition(0) + newMovement/500);
        }
        
    }
}
