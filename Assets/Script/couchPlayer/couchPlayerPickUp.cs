using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class couchPlayerPickUp : MonoBehaviour {
    public float throwForce = 10.0f;
    public float pickupCooldown = 1.0f;

    public string controllerType;
    ControlScheme control;

    private float timestamp;

    private Transform holdPosition; // Position at which the item will be held
    private bool pickup = false;
    private bool interPressed = false; //boolean check that is true when pressed and set to false once released


    private GameObject heldItem;
    private couchPlayerMovement parentScript;
    private bool ragdolling = false;

    private string holdButtonName;
    private float holdButtonInput;

    private string throwButtonName;
    private float throwButtonInput;

    private SphereCollider holdPositionCollider;

	// Use this for initialization
	void Start () {

        holdPosition = gameObject.transform.parent.Find("CouchPlayerHoldPosition");
        parentScript = gameObject.transform.parent.GetComponent<couchPlayerMovement>();
        control = transform.parent.GetComponent<couchPlayerMovement>().control;

        holdButtonName = control.Interact + transform.parent.GetComponent<couchPlayerMovement>().playerNumber;
        throwButtonName = control.Throw + transform.parent.GetComponent<couchPlayerMovement>().playerNumber;

        holdPositionCollider = holdPosition.GetComponent<SphereCollider>(); // Get a reference to the holdPosition's collider in order to disable/enable it accordingly
	}
	
	// Update is called once per frame
	void Update () {
        holdButtonInput = Input.GetAxis(holdButtonName);
        throwButtonInput = Input.GetAxis(throwButtonName);

        ragdolling = parentScript.ragdolling;

        if (holdButtonInput == 0)
            interPressed = false;
        if (holdButtonInput > 0.0f && pickup == false && timestamp <= Time.time && !ragdolling && interPressed == false)
        {
            pickup = true;
            interPressed = true;
        }

        // If we start to ragdoll while holding an item, let go of the item
        if (ragdolling && heldItem != null)
        {
            Release();
        }

        if (holdButtonInput > 0 && heldItem != null && interPressed == false)
        {
            Release();
            interPressed = true;
        }

        // If the throw button is pressed
        if (throwButtonInput > 0.0f && heldItem != null)
        {
            Throw();
        }

        // If we are not holding any item, disable the holdPosition collider (so we we don't have an invisible collider in front of us)
        if (heldItem == null && holdPositionCollider.enabled)
        {
            holdPositionCollider.enabled = false;
        }

        // If we we are holding an item and the holdPositio collider is disabled, enable it to ensure that it is enabled (so the item does not go through walls)
        if (heldItem != null && !holdPositionCollider.enabled)
        {
            holdPositionCollider.enabled = true;
        }
	}

    // Check if there is a grabbable object in front of us.
    private void OnTriggerStay(Collider other)
    {
        // If we're pressing the hold button, we're not already holding an item and this item is grabbable, pick it up.
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

            holdPosition.transform.position = transform.parent.position + transform.parent.forward * (0.4f + (heldItem.GetComponent<Collider>().bounds.size.z / 2));
            heldItem.transform.rotation = holdPosition.rotation;
            heldItem.transform.position = holdPosition.position; // Place item in the holdPosition
            heldItem.transform.parent = holdPosition; // Change the item's parent to holdPosition so we can move it around properly.

            // Freeze all the item's rigidbody constraints so we can freely move the item as a child of the player.
            rbItem.constraints = RigidbodyConstraints.FreezeAll;

            // Change the held vairable in item's grabbableCollision script for it to properly react to collisions with other things (like walls and such)
            heldItem.GetComponent<grabbableCollision>().held = true;
        }
    }

    private void Release()
    {
        pickup = false;
        Rigidbody rbItem = heldItem.GetComponent<Rigidbody>();
        heldItem.transform.parent = null; // Reset the item's parent to stop it from moving with the player
        
        if (rbItem != null)
        {
            rbItem.constraints = RigidbodyConstraints.None; // Disable item's rigidbody constraints so its physics are back to normal
        }
        heldItem.GetComponent<grabbableCollision>().held = false; // Stop the item from reacting in held mode in its grabbableCollision script
        heldItem = null; // Set heldItem back to null
        holdPosition.transform.position = transform.parent.position + transform.parent.forward * 0.5f;
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
