using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class couchPlayerPickUp : MonoBehaviour {

    public float throwForce = 10.0f;
    public float pickupCooldown = 1.0f;

    private float timestamp; // Timestamp for calculating pick up cooldown

    private Transform holdPosition; // Position at which the item will be held
    private bool pickup = false; // Whether or not an item is picked up
    private bool joined = false;

    private GameObject heldItem;
    private couchPlayerMovement parentScript;
    private bool ragdolling = false;

    private string holdButtonName;
    private float holdButtonInput;

    private string throwButtonName;

    //private SphereCollider holdPositionCollider;

	// Use this for initialization
	void Start () {
        holdPosition = gameObject.transform.parent.Find("CouchPlayerHoldPosition"); // Set reference to CouchPlayerHoldPosition object in the player's children
        //holdPosition = gameObject.transform.parent;
        parentScript = gameObject.transform.parent.GetComponent<couchPlayerMovement>(); // Get the player's couchPlayerMovement script

        // Input names for holding and throwing objects
        holdButtonName = "Hold" + transform.parent.GetComponent<couchPlayerMovement>().playerNumber;
        throwButtonName = "Throw" + transform.parent.GetComponent<couchPlayerMovement>().playerNumber;

        //holdPositionCollider = holdPosition.GetComponent<SphereCollider>(); // Get a reference to the holdPosition's collider in order to disable/enable it accordingly
	}
	
	// Update is called once per frame
	void Update () {

        holdButtonInput = Input.GetAxis(holdButtonName);
        ragdolling = parentScript.ragdolling;

        // Check input for pickup. Only true if something isn't already picked up, pickup cooldown is over, and we're not ragdolling
        if (holdButtonInput > 0.0f && pickup == false && timestamp <= Time.time && !ragdolling)
        {
            pickup = true;
        }

        // If we start to ragdoll while holding an item, let go of the item
        if (ragdolling && heldItem != null)
        {
            Release();
        }

        // If we let go of the hold button and we are holding an item, let go of it.
        if (holdButtonInput == 0 && heldItem != null)
        {
            Release();
        }

        // If the throw button is pressed
        if (Input.GetButtonDown(throwButtonName) && heldItem != null)
        {
            Throw();
        }

        // If we are not holding any item, disable the holdPosition collider (so we we don't have an invisible collider in front of us)
        //if (heldItem == null && holdPositionCollider.enabled)
        //{
        //    holdPositionCollider.enabled = false;
        //}

        //// If we we are holding an item and the holdPositio collider is disabled, enable it (so the item does not go through walls)
        //if (heldItem != null && !holdPositionCollider.enabled)
        //{
        //    holdPositionCollider.enabled = true;
        //}
	}

    // Check if there is a grabbable object in front of us.
    private void OnTriggerStay(Collider other)
    {
        // If we're pressing the hold button, we're not already holding an item and this item is grabbable, pick it up.
        if (pickup && heldItem == null && other.gameObject.tag == "grabbable")
        {
            PickUp(other.gameObject);
            Debug.Log("Picked up: " + other.gameObject.name);
        }
    }

    private void PickUp(GameObject item)
    {
        Rigidbody rbItem = item.GetComponent<Rigidbody>();

        if (rbItem != null && !joined)
        {
            heldItem = item;

            //heldItem.transform.parent = holdPosition; // Change the item's parent to holdPosition so we can move it around properly.

            // Change the holdPosition's transform to fit the item's size.
            holdPosition.transform.position = transform.parent.position + transform.parent.forward * (0.4f + (heldItem.GetComponent<Collider>().bounds.size.z / 2));
            heldItem.transform.rotation = holdPosition.rotation;
            heldItem.transform.position = holdPosition.position; // Place item in the holdPosition

            Physics.IgnoreCollision(holdPosition.transform.parent.GetComponent<Collider>(), heldItem.GetComponent<Collider>(), true);
            heldItem.AddComponent<FixedJoint>();
            FixedJoint fixJoint = heldItem.GetComponent<FixedJoint>();
            fixJoint.GetComponent<FixedJoint>().connectedBody = holdPosition.GetComponent<Rigidbody>();
            joined = true;

            // Freeze all the item's rigidbody constraints so we can freely move the item as a child of the player.
            //rbItem.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        }
    }

    private void Release()
    {
        pickup = false;
        if (joined)
        {
            Destroy(heldItem.GetComponent<FixedJoint>());
            joined = false;
        }
        Physics.IgnoreCollision(holdPosition.transform.parent.GetComponent<Collider>(), heldItem.GetComponent<Collider>(), false);
        // Revert holdPosition back to where it was before picking up the item
        holdPosition.transform.position = transform.parent.position + transform.parent.forward * 0.5f;
        //Vector3 oldHoldPosition = new Vector3(holdPosition.position.x - transform.forward.x * heldItem.GetComponent<Collider>().bounds.size.z / 2,
        //holdPosition.position.y, holdPosition.position.z - transform.forward.z * heldItem.GetComponent<Collider>().bounds.size.z / 2);
        Rigidbody rbItem = heldItem.GetComponent<Rigidbody>();
        rbItem.constraints = RigidbodyConstraints.None; // Disable item's rigidbody constraints so its physics are back to normal
        heldItem = null; // Set heldItem back to null
        //holdPosition.position = oldHoldPosition; // Move holdPosition back to its old position.
        timestamp = Time.time + pickupCooldown; // Take a timestamp of when the item was released in order to check the pickup cooldown
    }

    private void Throw()
    { 
        Rigidbody rbItem = heldItem.GetComponent<Rigidbody>();
        Release(); // Release the item right before applying a force to it
        if (rbItem != null)
        {
            // Throw the item in the player's forward direction at throwForce strength
            rbItem.AddForce(transform.parent.forward * throwForce, ForceMode.Impulse);
        }
    }
    
}
