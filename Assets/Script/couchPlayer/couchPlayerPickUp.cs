using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class couchPlayerPickUp : MonoBehaviour {

    public float throwForce = 10.0f;
    public float pickupCooldown = 1.0f;

    private float timestamp;

    private Transform holdPosition;
    private bool pickup = false;

    private GameObject heldItem;
    private couchPlayerMovement parentScript;
    private bool ragdolling = false;

    private string holdButtonName;
    private float holdButtonInput;

    private string throwButtonName;

    private SphereCollider holdPositionCollider;

	// Use this for initialization
	void Start () {
        holdPosition = gameObject.transform.parent.Find("CouchPlayerHoldPosition");
        parentScript = gameObject.transform.parent.GetComponent<couchPlayerMovement>();

        holdButtonName = "Hold" + transform.parent.GetComponent<couchPlayerMovement>().playerNumber;
        throwButtonName = "Throw" + transform.parent.GetComponent<couchPlayerMovement>().playerNumber;

        holdPositionCollider = holdPosition.GetComponent<SphereCollider>();
	}
	
	// Update is called once per frame
	void Update () {

        holdButtonInput = Input.GetAxis(holdButtonName);
        ragdolling = parentScript.ragdolling;

        if (holdButtonInput > 0.0f && pickup == false && timestamp <= Time.time && !ragdolling)
        {
            pickup = true;
        }

        if (ragdolling && heldItem != null)
        {
            Release();
        }

        if (holdButtonInput == 0 && heldItem != null)
        {
            Release();
        }

        if (Input.GetButtonDown(throwButtonName) && heldItem != null)
        {
            Throw();
        }

        if (heldItem == null && holdPositionCollider.enabled)
        {
            holdPositionCollider.enabled = false;
        }

        if (heldItem != null && !holdPositionCollider.enabled)
        {
            holdPositionCollider.enabled = true;
        }
	}

    private void OnTriggerStay(Collider other)
    {
        if (pickup && heldItem == null && other.gameObject.tag == "grabbable")
        {
            PickUp(other.gameObject);
        }
    }

    private void PickUp(GameObject item)
    {
        Rigidbody rbItem = item.GetComponent<Rigidbody>();

        if (rbItem != null)
        {
            heldItem = item;
            heldItem.transform.position = holdPosition.position;
            heldItem.transform.rotation = holdPosition.rotation;
            heldItem.transform.parent = holdPosition;

            rbItem.constraints = RigidbodyConstraints.FreezeAll;
            heldItem.GetComponent<grabbableCollision>().held = true;
        }
    }

    private void Release()
    {
        pickup = false;
        heldItem.transform.parent = null;
        Rigidbody rbItem = heldItem.GetComponent<Rigidbody>();
        if (rbItem != null)
        {
            rbItem.constraints = RigidbodyConstraints.None;
        }
        heldItem.GetComponent<grabbableCollision>().held = false;
        heldItem = null;

        timestamp = Time.time + pickupCooldown;
    }

    private void Throw()
    { 
        Rigidbody rbItem = heldItem.GetComponent<Rigidbody>();
        Release();
        if (rbItem != null)
        {
            rbItem.AddForce(transform.parent.forward * throwForce, ForceMode.Impulse);
        }
    }
}
