using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class couchPlayerInteract : MonoBehaviour {

    public string controllerType;
    ControlScheme control;

    private float timestamp;

    private GameObject interactObject;
    public bool interactingWithObject = false;
    public bool interact = false; // If the player is currently interacting with something
    private bool interPressed = false;

    //Input name for interacting 
    private string interactButtonName;
    private float interactButtonInput;

    // Start is called before the first frame update
    void Start() {
        control = transform.parent.GetComponent<couchPlayerMovement>().control;

        // Input names for holding and throwing objects
        interactButtonName = control.Interact + transform.parent.GetComponent<couchPlayerMovement>().playerNumber;
    }

    // Update is called once per frame
    void Update() {

        interactButtonInput = Input.GetAxis(interactButtonName);

        //Checks if there's no input
        if(interactButtonInput == 0) {
            interPressed = false;
        }

        if (interactButtonInput > 0.0f && !interact && !interPressed) {
            interact = true;
            interPressed = true;
        }
        //if (interactButtonInput > 0.0f && interact && !interPressed) {
        //    interact = false;
        //    print("Interact = false");
        //    interPressed = true;

        //}
    }

    //Check if the interactable object is in front of us
    private void OnTriggerStay(Collider other) {
        // If we're pressing the hold button, we're not already holding an item and this item is grabbable, pick it up.
        //if (pickup && heldItem == null && other.gameObject.tag == "grabbable") {
        //    PickUp(other.gameObject);
        //}
        if (other.gameObject.tag == "Interactable" && interact && !interactingWithObject) {
            interactingWithObject = true;
            if (other.GetComponent<joystickSwitch>().interact() == true) { // want to pop out of interacting and not lock down
                interact = false;
                interactingWithObject = false;
            } 
            //    PickUp(other.gameObject);
            //interact = true;
           // print("Interact with Object!");
        }

    }
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