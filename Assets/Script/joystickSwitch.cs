//Reed Scriven - SolSearcher
//joystickSwitch.cs
//
//Functionality: 
//  Currently allows the player to toggle the rotation of a set object
//  Still need to add a varaible joystick mode so the player can be locked down
//
//How to Attach:
//  IMPORTANT : Choose the type of the joystick, currently only the switch type
//  1. Attach the object you want to rotate in the editor under "Output Transform"
//  2. Under "Output Rotation 0" and 1, add the rotations you want your object to switch between 
//
//Public Variables:
//  Type Switch : if it's an ON OFF Switch
//  Output Transform : The object you want to rotate
//  Output Rotation 0: The rotation for setting 0
//  Output Rotation 1: The rotation for setting 1


using UnityEngine;

public class joystickSwitch : MonoBehaviour {
    //Initialize Universal Vars
    private Material joystickGlow; // the accent color on the joystick
    private Color colorActive; // what color the accent will turn when in use
    private Color colorInactive; // what color the accent will be at rest
    private float speedAnimation = 0.2f;

    //Switch Type Vars
    public bool typeSwitch = true; // On / OFF switch. Switches between 2 rotation angles for object
    private bool toggle = true; // The boolean var associated with the switch
    public Transform outputTransform; // The output objects transform. Will rotate this
    private Vector3 outputInitialRotation; // The rotation state the object starts in
    // public Vector3 outputInitialRotation; //  Lets you see what the current rotation val is if you add this to update :  outputInitialRotation = outputTransform.rotation.eulerAngles;
    public Vector3 outputRotation0; // Rotation option 0
    public Vector3 outputRotation1; // Rotation option 1

    //Variable Type Vars

    // Start is called before the first frame update
    void Start() {
        colorInactive = joystickGlow.color;
        outputInitialRotation = outputTransform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update() {
    //    outputInitialRotation = outputTransform.rotation.eulerAngles;
    }

    //This will need to active the switch and freeze the player
    public bool interact() {
        if (typeSwitch) { // if it's an on / off switch
            return switchInteract();
        } else {
            return false;
        }
    }

    // Switch Type Interaction. Toggle ON / OFF
    private bool switchInteract() {
        if (toggle) { // Turn OFF
            toggle = false;
            outputTransform.rotation = Quaternion.Euler(outputRotation0);
            print("Interacting : 0");
        } else if (!toggle) { // Turn ON
            toggle = true;
            outputTransform.rotation = Quaternion.Euler(outputRotation1);
            print("Interacting : 1");
        }
        return true;
    }
}
