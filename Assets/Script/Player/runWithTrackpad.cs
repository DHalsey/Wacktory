//Dustin Halsey - dhalsey
//runWithTrackpad.cs
//Functionality: 
//  Allows the player to move by touching the touch pad on the left controller
//
//How to Attach:
//  Attach this to the controller object
//  assign headTransform to be the transform of the camera for the headset
//  this relies on the steamVR controls having actions for:
//      touchpadTouch
//      touchpadTap
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class runWithTrackpad : MonoBehaviour {
    private Vector2 trackPadLeftPos;
    private bool trackPadLeftTap = false;
    public float acceleration = 100000f; 
    public Transform headTransform; //the transform of the VR headset (needed to decide what is forward)
    private Vector3 headForward; //used to handle the direction of the movement to be in relation of direction that the player is looking
    private Vector3 collisionNormal = new Vector3(0,0,0); //will hold the normal vector of the collided object. used to move up hills
    private float maxSpeed = 2.0f; //the max horizontal velocity of the player
    private Rigidbody rb; // the rigidbody of the player


    private SteamVR_Action_Vector2 touchPadAction; //the position of the touch on the touch pad
    private SteamVR_Action_Boolean touchPadTap; //if the touch pad is being touched

	void Start () {
        rb = this.gameObject.GetComponent<Rigidbody>();
        touchPadAction = SteamVR_Input._default.inActions.TouchpadTouch;
        touchPadTap = SteamVR_Input._default.inActions.TouchpadTap;
	}
	

	void Update () {
        GetControls();
	}

    private void FixedUpdate() {
        ApplyFriction();
        MovePlayer(); //kept in fixed update to make sure movement doesnt look funky (to see this, moving MovePlayer to update makes the drawDebugMovement freak out)
        ClampSpeed(); 
    }

    //gets all the neccessary input from the player
    void GetControls() {
        trackPadLeftPos = touchPadAction.GetAxis(SteamVR_Input_Sources.LeftHand); //use only the left hand input
        //trackPadLeftPos = touchPadAction.GetAxis(SteamVR_Input_Sources.Any); //debug to allow movement for any controller to make testing easier
        headForward = Vector3.zero; //zero out the vector
        headForward.y = headTransform.eulerAngles.y; //we only care about the y rotation of the headset (for direction). we dont want to launch up

    }

    //physically moves the player's vr playspace (and thus the player as well)
    void MovePlayer() {
        Vector3 direction = new Vector3(trackPadLeftPos.x, 0, trackPadLeftPos.y); //the direction of input from the player
        Vector3 movementDirection = Quaternion.Euler(headForward) * direction * acceleration * Time.deltaTime; //adjusts the direction of movement to be relative to the rotation of the player's head
        if (collisionNormal.y == 0) collisionNormal.y = 0.0000001f; //prevent divide by zero
        float newY = (-movementDirection.x * collisionNormal.x - movementDirection.z * collisionNormal.z) / collisionNormal.y; //gets the y value for the vector perpendicular to the ground collision
        Vector3 newMovement = new Vector3(movementDirection.x, newY, movementDirection.z); //the new direction of movement that is perpendicular to the normal we are walking on.  This ensures we can move up ramps properly
        rb.AddForce(newMovement); //adds the force in a direction that mathces the slops of the incline we are on
        //DrawDebugMovement(newMovement); //comment this line out to hide the debug draw line
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
        Vector3 changedVelocity = Vector3.zero;
        changedVelocity.x = rb.velocity.x * 0.95f; //scales the velocity down
        changedVelocity.y = rb.velocity.y; //scales the velocity down
        changedVelocity.z = rb.velocity.z * 0.95f; //scales the velocity down
        rb.velocity = changedVelocity;
    }

    //Updates the collision normal 
    private void OnCollisionEnter(Collision collision) {  
        Vector3 normalCol = collision.contacts[0].normal; //gets the normal of the newest contacted surface
        Vector3 vecFlat = new Vector3(normalCol.x, 0, normalCol.z); //the adjacent vertex
        //check the degree of the angle of the collided normal using "Cosine=Adjacent/Horizontal" to calculate the angle
        if (vecFlat.magnitude / normalCol.magnitude * Mathf.Rad2Deg <= 30) { //if we are walking on a slope <=30 degrees
            collisionNormal = normalCol;
        }
        
    }

    //A direct copy of onCollisionEnter (see onCollisionEnter)
    private void OnCollisionStay(Collision collision) {
        Vector3 normalCol = collision.contacts[0].normal;
        Vector3 vecFlat = new Vector3(normalCol.x, 0, normalCol.z);
        if (vecFlat.magnitude / normalCol.magnitude * Mathf.Rad2Deg <= 30) {
            collisionNormal = normalCol;
        }
    }
    private void OnCollisionExit(Collision collision) {
        collisionNormal = Vector3.up; //reset movement back to horizontal if we are in the air
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
        } else { //update the line to show direction of movement
            line.SetPosition(1, line.GetPosition(0) + newMovement/500);
        }
        
    }
}
