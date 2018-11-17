//Dustin Halsey - dhalsey
//vrPickupObject.cs
//Functionality: 
//  Gives the player the ability to pick up and throw objects by pulling the trigger on the VR controllers

//How to Attach:
//  Attach this to the controller object
//  this relies on the steamVR controls having actions for "grabTrigger"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
public class vrPickupObject : MonoBehaviour {
    private SteamVR_Action_Single triggerPull; //if the touch pad is being touched
    private GameObject grabbedObject;
    private float pullAmount;
    private bool pulled = false;
    private SteamVR_Input_Sources hand; //used to get which controller this script is using
    private Collider grabCollider; //the collider box for grabbing (what the object must be touching to be grabbed)
    private Collider playerCollider; //the collider of the player
    private Vector3 lastPosition; //the position of the controller on the previous frame
    private Quaternion lastRotation; //the rotation of the controller on the previous frame

    void Start () {
        triggerPull = SteamVR_Input._default.inActions.GrabTrigger;
        grabCollider = GetComponent<Collider>();
        playerCollider = gameObject.transform.parent.GetComponent<Collider>();

        //Gets the input source for the hand we are using
        if (gameObject.name.Contains("left")) {
            hand = SteamVR_Input_Sources.LeftHand;
        } else if (gameObject.name.Contains("right")) {
            hand = SteamVR_Input_Sources.RightHand;
        }
        
	}
	
	void Update () {
        GetInput(); 
        MoveObject(); 
        checkGrab(); 

        //updates lastPosition and lastRotation so we can have this information for calculating velocities on the next frame
        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }

    //gets all controller input that we need
    private void GetInput() {
        pullAmount = triggerPull.GetAxis(hand);
        if (pullAmount < 0.5) {
            ReleaseObject();
        }
        //Debug.Log(pullAmount);
    }

    //moves the object when grabbed
    private void MoveObject() {
        if (grabbedObject) { //if we are holding an object
            Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
            rb.MovePosition(this.transform.position);
            rb.MoveRotation(this.transform.rotation);       
        }
    }

    //checks to see if we should release the object
    private void checkGrab() {
        if (pullAmount >= 0.5) {
            pulled = true;
        } else {
            ReleaseObject();
        }
    }

    private void ReleaseObject() {
        pulled = false;
        if (grabbedObject && grabbedObject.transform.childCount >= 1) {
            GameObject childObject = grabbedObject.transform.GetChild(0).gameObject;
            Rigidbody childRB = childObject.GetComponentInChildren<Rigidbody>(); //get the rigidbody of the object we are holding

            //Gives control of the rigidbody back to the original object instead of the controller
            childRB.freezeRotation = false;
            childRB.useGravity = true;
            childRB.isKinematic = false;
            childRB.velocity = CalulateVelocity(transform.position, lastPosition)*1.5f; //sets the speed of the object 
            childRB.angularVelocity = CalulateAngularVelocity(transform.eulerAngles, lastRotation.eulerAngles);
            childObject.GetComponentInChildren<Transform>().parent = null; //releases the object
            Destroy(grabbedObject); //remove the parent object that was used to preserve the position offset of the grabbed object
            if (childObject.GetComponent<Collider>()) { //to prevent errors
                Physics.IgnoreCollision(playerCollider, childObject.GetComponent<Collider>(), false); //reenables collision with the player when thrown
            }        
        }
        grabbedObject = null;
    }

    //calculates the velocity between 2 frames of gameplay
    //currentPos = the newer position; lastPos = the older position
    private Vector3 CalulateVelocity(Vector3 currentPos, Vector3 lastPos) {
        Vector3 velocity = ((currentPos - lastPos) / Time.deltaTime); // distance/time
        return velocity;
    }

    //FIX THIS DAMN THING. WHY IS THE DIRECTIONS STILL MESSED UP??????????
    private Vector3 CalulateAngularVelocity(Vector3 currentPos, Vector3 lastPos) {
        
        Vector3 angularVelocity = (currentPos - lastPos);
        //adjusts the rotation to compensate rotation always normalizing from -180 to 180
        if (angularVelocity.x > 180) angularVelocity.x -= 360;
        else if (angularVelocity.x < -180) angularVelocity.x += 360;
        if (angularVelocity.y > 180) angularVelocity.y -= 360;
        else if (angularVelocity.y < -180) angularVelocity.y += 360;
        if (angularVelocity.z > 180) angularVelocity.z -= 360;
        else if (angularVelocity.z < -180) angularVelocity.z += 360;
        return angularVelocity;
    }

    private void OnTriggerEnter(Collider other) {
        //OnTriggerStay(other); //Does anyone know if you can do this? -Dustin
    }
    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "grabbable") {
            //only grab if we do not already have an object
            if (pulled == true && !grabbedObject) {

                //creates an object that we child the picked up object to.  This will handle the actual movement of the object
                // and allows us to preserve the offset of the grab since a child object will keep its offset when the parent rotates
                grabbedObject = new GameObject();
                grabbedObject.AddComponent<Rigidbody>();
                grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                grabbedObject.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
                
                grabbedObject.transform.position = this.transform.position; //set the position of this dummy object to the controller's position
                grabbedObject.transform.rotation = this.transform.rotation; //set the rotation of this dummy object to the controller's rotation
                other.gameObject.transform.parent = grabbedObject.transform; //parents the picked up object to this dummy object

                //disables physics for this object so it behaves corrently when grabbed
                other.gameObject.GetComponent<Rigidbody>().freezeRotation = true;
                other.gameObject.GetComponent<Rigidbody>().useGravity = false;
                other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                
                Physics.IgnoreCollision(playerCollider, other); //prevents the held object from pushing the player
            }

        }
    }
    private void OnTriggerExit(Collider other) {
        
    }
}
