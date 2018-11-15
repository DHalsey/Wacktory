//Attach this to the vrPlayer GameObject

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
        playerCollider.center = new Vector3(HMD.transform.localPosition.x, HMD.transform.localPosition.y/2, HMD.transform.localPosition.z);
        playerCollider.height = HMD.transform.localPosition.y;

    }
}
