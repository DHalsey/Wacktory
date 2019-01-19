using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class couchPlayerPickUp : MonoBehaviour {

    public float minThrowForce = 3.0f; // Minimum force with which an item would be thrown
    public float maxThrowForce = 10.0f; // Maximum force with which an item would be thrown
    public float chargeRate = 0.1f; // Rate at which a throw is charged
    private float throwForce; // How hard the player would throw if the throw button was released
    private bool charging = false; // If a throw is being charged or not

    public float pickupCooldown = 1.0f; // Cooldown for picking up items (to avoid weird collision glitches)
    ControlScheme control;

    private float timestamp; // Timestamp for calculating pick up cooldown

    private Transform holdPosition; // Position at which the item will be held
    private bool pickup = false; // Whether or not an item is picked up
    private bool joined = false; // Whether or not a joint between the hold position and item is made
    private bool interPressed = false; //boolean check that is true when pressed and set to false once released

    private GameObject heldItem;
    private couchPlayerMovement parentScript;
    private bool ragdolling = false;

    private string holdButtonName;
    private float holdButtonInput;

    private string throwButtonName;
    private float throwButtonInput;

    public PhysicMaterial heldItemPhysicMaterial; // Physics material used to disable held item's friction and bounciness to reduce collision anomalies

	void Start () {

        holdPosition = gameObject.transform.parent.Find("CouchPlayerHoldPosition");
        parentScript = gameObject.transform.parent.GetComponent<couchPlayerMovement>();
        control = transform.parent.GetComponent<couchPlayerMovement>().control;

        holdButtonName = control.Interact + transform.parent.GetComponent<couchPlayerMovement>().playerNumber;
        throwButtonName = control.Throw + transform.parent.GetComponent<couchPlayerMovement>().playerNumber;

        throwForce = minThrowForce;
	}
	
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
        
        // If we are holding an item, check for release/throw conditions
        if (heldItem != null)
        {
            // If the hold button is released or we start ragdolling, let go of the held item
            if ((holdButtonInput > 0f && !interPressed) || ragdolling)
            {
                Release();
                interPressed = true;
            }
            // If we press the throw button, start charging the throw. 
            // Else, if we are not pressing the throw button but charging is still true (AKA we let go of the throw button), throw item.
            if (Input.GetButton(throwButtonName))
            {
                charging = true;
                ChargeThrow();
            }
            else if (charging == true)
            {
                Throw();
            }
        }
	}

    // Check if there is a grabbable object in front of us.
    private void OnTriggerStay(Collider other)
    {
        // If we're pressing the hold button, we're not already holding an item, and this item is grabbable, pick it up.
        if (pickup && heldItem == null && other.gameObject.tag == "grabbable")
        {
            PickUp(other.gameObject);
        }
    }

    /*----PICK UP AND RELEASE----*/

    private void PickUp(GameObject item)
    {
        Rigidbody rbItem = item.GetComponent<Rigidbody>();

        if (rbItem != null && !joined)
        {
            heldItem = item;
            
            // Change the holdPosition's transform to fit the item's size, plus a little bit of padding
            holdPosition.transform.position = transform.parent.position + transform.parent.forward * ((transform.parent.GetComponent<Collider>().bounds.size.z / 2)
                + (heldItem.GetComponent<Collider>().bounds.size.z / 2) + 0.05f);
            heldItem.transform.rotation = holdPosition.rotation; // Set itme's rotation to holdPosition's
            heldItem.transform.position = holdPosition.position; // Place item in the holdPosition
            heldItem.GetComponent<Collider>().material = heldItemPhysicMaterial; // Set item's physics material to one without friction and bounciness

            // Item ignores collision with player so that there aren't any crazy game-breaking rigidbody bugs like being launched at the speed of light
            Physics.IgnoreCollision(holdPosition.transform.parent.GetComponent<Collider>(), heldItem.GetComponent<Collider>(), true);

            // To actually hold the object and use still use rigidbody physics, the item is held in place (holdPosition) with a FixedJoint. This tells Unity to keep calculating
            // heldItem's rigidbody physics and to not force it through other objects like walls (something that would happen if we set it as a chiled of holdPosition instead)
            heldItem.AddComponent<FixedJoint>();
            FixedJoint fixJoint = heldItem.GetComponent<FixedJoint>();
            fixJoint.GetComponent<FixedJoint>().connectedBody = holdPosition.GetComponent<Rigidbody>();
            joined = true;
        }
    }

    private void Release()
    {
        // Destroy the FixedJoint that held the item in place
        pickup = false;
        if (joined)
        {
            Destroy(heldItem.GetComponent<FixedJoint>());
            joined = false;
        }

        // Reset the item's physics material to null (resets its friction and bounciness)
        heldItem.GetComponent<Collider>().material = null;

        // Re-enable collision with player
        Physics.IgnoreCollision(holdPosition.transform.parent.GetComponent<Collider>(), heldItem.GetComponent<Collider>(), false);
        
        // Revert holdPosition back to where it was
        holdPosition.transform.position = transform.parent.position + transform.parent.forward * (transform.parent.GetComponent<Collider>().bounds.size.z / 2);
        Rigidbody rbItem = heldItem.GetComponent<Rigidbody>();

        heldItem = null; // Set heldItem back to null

        timestamp = Time.time + pickupCooldown; // Take a timestamp of when the item was released in order to check the pickup cooldown
    }

    /*----THROWING ITEMS ----*/

    private void Throw()
    { 
        Rigidbody rbItem = heldItem.GetComponent<Rigidbody>();
        Release(); // Release the item right before applying a force to it
        if (rbItem != null)
        {
            // Throw the item in the player's forward direction at throwForce strength
            rbItem.AddForce(transform.parent.forward * throwForce, ForceMode.Impulse);
        }
        // Reset throwForce to minimum throw force value
        throwForce = minThrowForce;
        charging = false;

        // Clear the charging line renderer
        ClearThrowLine();
    }
    
    // If throwForce is >= maxThrowForce, throw the object (keeps players from charging button forever)
    // Else, increase throwForce by chargeRate
    private void ChargeThrow()
    {
        DrawThrowLine();
        if (throwForce >= maxThrowForce)
        {
            Throw();
        }
        else
        {
            throwForce += chargeRate;
        }
    }

    /*----DRAWING CHARGE LINE FOR THROWING----*/

    // Draw line that indicates how much force a throw will have
    private void DrawThrowLine()
    {
        // Check if there is already a LineRenderer component in the holdPosition object
        // If not, create a new one and set its attributes.
        LineRenderer line = holdPosition.gameObject.GetComponent<LineRenderer>();
        if (!line)
        { 
            line = holdPosition.gameObject.AddComponent<LineRenderer>(); // Add LineRenderer component to holdPosition
            line.positionCount = 2; // How many points in the line
            line.useWorldSpace = true; // If the line's positions are treated as world coordinates or relative to the game object its a part of
            line.startWidth = 0.1f; // Width of start of the line
            line.endWidth = 0.15f; // Width of end of the line
            line.numCapVertices = 10; // Number of vertices at the ends (affects how round the ends are)
            line.SetPosition(0, holdPosition.transform.position); // Position of first point in line
            line.material = new Material(Shader.Find("Sprites/Default")); // Sets line's material to default material (so we can add colors to it, otherwise it defaults to gross pink)
            line.startColor = new Color(255, 212, 0); // Color for the start of the line (not working)
            line.endColor = new Color(255, 0, 0); // Color for the end of the line (not working)
        }
        else
        {
            line.SetPosition(0, holdPosition.transform.position); // Set position of the start of the line every frame so it moves with the player
            line.SetPosition(1, line.GetPosition(0) + transform.parent.forward * (throwForce / 15)); // Set position of end of the line (simply add a vector relative to the throwForce so it grows every frame)
            line.endWidth += 0.0025f; // Increase the width of the end to make it thicker the longer it charges.
        }
    }

    // Destroy LineRenderer component that shows the charge line
    private void ClearThrowLine()
    {
        LineRenderer line = holdPosition.gameObject.GetComponent<LineRenderer>();
        if (line != null)
        {
            Destroy(line);
        }
    }
}
