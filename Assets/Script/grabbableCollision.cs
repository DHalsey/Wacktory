using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabbableCollision : MonoBehaviour {

    [HideInInspector] public bool held;

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        held = false;
	}

    private void OnCollisionStay(Collision collision)
    {
        if (held)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionY;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (held)
        {
            transform.position = transform.parent.position;
            transform.rotation = transform.parent.rotation;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
