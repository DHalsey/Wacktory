using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class couchPlayerInteract : MonoBehaviour {

    public bool interact = false; // If the player is currently interacting with something

    //Input name for interacting 
    private string interactButtonName;
    private string interactButtonInput;

    // Start is called before the first frame update
    void Start() {
        // Input names for holding and throwing objects
        interactButtonName = "Interact" + gameObject.transform.parent.GetComponent<couchPlayerMovement>().playerNumber;

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetAxis(interactButtonName) > 0.0f && !interact) {
            interact = true;
        }
    }

    //Check if the interactable object is in front of us
    private void OnTriggerStay(Collider other) {
        // If we're pressing the hold button, we're not already holding an item and this item is grabbable, pick it up.
        //if (pickup && heldItem == null && other.gameObject.tag == "grabbable") {
        //    PickUp(other.gameObject);
        //}
        if (other.gameObject.tag == "Interactable" && interact) {
            //    PickUp(other.gameObject);
            //interact = true;
            print("Interact with Object!");
        }


        //Goals,
        // trying to have the player interact with the object
        // the players hand hitbox will check the type of the item in question and if it's 
        // interactable, it will activate code in it
        // the code will pass in a reference to the robot that grabbed it
        // this can lock the player in place
        // the player might have to pass in more info for the controller
        // maybe I could copy the  string from the beginning and send that over <--- investigate
        // i need to use this input to do things with the switch
        // this could have a few settings
        // a toggle where you switch the switch between types
        // types include
        //
        //  1. Boolean switch ON/OFF
        //  2. Multi Settings Switch : Switch between different options
        //      Switches between set values or outputs
        //  3. Variable Switch : Use the joystick to control the full range of the joystick
        //  4. Omnidirectional joystick. mirrors the joystick in your hands
        //
        // Need to make sure players can't interact while holding an item


    }
}
