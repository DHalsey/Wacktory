using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabbableCollision : MonoBehaviour {

    [HideInInspector] public bool held; // Accessed by couchPlayerPickUp script

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        held = false;
	}

    private void OnCollisionStay(Collision collision)
    {
        // If the object is colliding with something and it's being held by a player
        if (held)
        {
            // Unfreeze all constraints except for Y position 
            // (so the item doesn't freak out when hitting another object, i.e it doesn't try to force its way through another physics object)
            rb.constraints = RigidbodyConstraints.FreezePositionY;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // When we stop colliding with object, if item is still held, re-enable all constraints and reset its position to player's holdPosition
        if (held)
        {
            transform.position = transform.parent.position;
            transform.rotation = transform.parent.rotation;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
