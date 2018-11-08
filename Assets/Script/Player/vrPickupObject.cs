﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
public class vrPickupObject : MonoBehaviour {
    private SteamVR_Action_Single triggerPull; //if the touch pad is being touched
    private GameObject grabbedObject;
    private float pullAmount;
    private bool pulled = false;
    private Vector3 grabOffsetPos; //the offset position when grabbed. makes sure the object stays in the grabbed position
    private Quaternion grabOffsetRot;
    private SteamVR_Input_Sources hand; //used to get which controller this script is using
    private Collider grabCollider; //the collider box for grabbing
    private Collider playerCollider; //the collider of the player
    private Vector3 lastPosition;
    private Vector3 lastRotation;

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
        lastPosition = transform.position;
        lastRotation = transform.eulerAngles;
	}

    private void GetInput() {
        pullAmount = triggerPull.GetAxis(hand);
        if (pullAmount < 0.5) {
            ReleaseObject();
        }
        //Debug.Log(pullAmount);
    }

    private void MoveObject() {
        if (grabbedObject) { //if we are holding an object
            Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
            rb.MovePosition(this.transform.position);
            rb.MoveRotation(this.transform.rotation);


        }
    }

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
            Debug.Log(childObject.name);
            childObject.GetComponentInChildren<Rigidbody>().freezeRotation = false;
            childObject.GetComponentInChildren<Rigidbody>().useGravity = true;
            childObject.GetComponentInChildren<Rigidbody>().isKinematic = false;
            childObject.GetComponentInChildren<Rigidbody>().velocity = CalulateVelocity();
            childObject.GetComponentInChildren<Rigidbody>().AddTorque((transform.eulerAngles - lastRotation) / Time.deltaTime);
            childObject.GetComponentInChildren<Transform>().parent = null;
            Destroy(grabbedObject);
        }
        grabOffsetPos = Vector3.zero;
        grabOffsetRot = Quaternion.identity;
        grabbedObject = null;
    }

    //calculates the velocity 
    private Vector3 CalulateVelocity() {
        Vector3 velocity = ((transform.position - lastPosition) / Time.deltaTime);
        return velocity;
    }

    private void OnTriggerEnter(Collider other) {

    }
    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "grabbable") {
            //only grab if we do not already have an object
            checkGrab();
            if (pulled == true && !grabbedObject) {
                grabbedObject = new GameObject();
                grabbedObject.AddComponent<Rigidbody>();
                grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                grabbedObject.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
                grabbedObject.transform.position = this.transform.position;
                grabbedObject.transform.rotation = this.transform.rotation;
                other.gameObject.transform.parent = grabbedObject.transform;

                //hold the offsets so we can grab the objects in different locations
                grabOffsetPos = other.gameObject.transform.position - this.transform.position;
                grabOffsetRot = this.transform.rotation;

                other.gameObject.GetComponent<Rigidbody>().freezeRotation = true;
                other.gameObject.GetComponent<Rigidbody>().useGravity = false;
                other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                Physics.IgnoreCollision(playerCollider, other);
            }

        }
    }
    private void OnTriggerExit(Collider other) {
        
    }
}
