//Dustin Halsey - dhalsey
//vrHitboxFollowHMD.cs
//Functionality: 
//  have the hitbox of the player follow the headset's position and height
//
//How to Attach:
//  Attach this to the playerVR gameObject
//  Public Variables:
//      HMD - the gameobject for the head (camera)
//      playerCollider - the collider for the player

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vrHitboxFollowHMD : MonoBehaviour {
    public GameObject HMD;
    public CapsuleCollider playerCollider;
	// Use this for initialization
	void Start () {
		if (!HMD) {
            Debug.Log("MISSING HMD GAMEOBJECT FROM vrHitBoxFollowHMD");
        }
        if (!playerCollider) {
            Debug.Log("MISSING PLAYER COLLIDER FROM vrHitBoxFollowHMD");
        }
    }
	
	// Update is called once per frame
	void Update () {
        //set the center of the collider halfway between the headset height and the floor
        playerCollider.center = new Vector3(HMD.transform.localPosition.x, HMD.transform.localPosition.y/2, HMD.transform.localPosition.z); 
        playerCollider.height = HMD.transform.localPosition.y;

    }
}
