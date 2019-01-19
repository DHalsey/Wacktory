using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class couchPlayerIsGrounded : MonoBehaviour {

    private couchPlayerMovement playerScript;

	// Use this for initialization
	void Start () {
        playerScript = transform.parent.GetComponent<couchPlayerMovement>();
	}

    // Checks if player is grounded to let them jump.
    // Courtesy of Dustin Halsey
    private void OnCollisionStay(Collision collision)
    {
        //Debug.Log("COLLIDING");
        Vector3 normalCol = collision.contacts[0].normal; //gets the normal of the newest contacted surface
        Vector3 vecFlat = new Vector3(normalCol.x, 0, normalCol.z); //the adjacent vertex
        //check the degree of the angle of the collided normal using "Cosine=Adjacent/Horizontal" to calculate the angle
        if (vecFlat.magnitude / normalCol.magnitude * Mathf.Rad2Deg <= 45)
        { //if we are walking on a slope <=45 degrees
            playerScript.isGrounded = true;
            playerScript.jumped = false;
        }
    }
}
