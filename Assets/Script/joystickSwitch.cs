//Reed Scriven - SolSearcher
//joystickSwitch.cs
//
//Functionality: 
//  Allows the player to control and use the joystick to control an outcome in the game
//
//How to Attach:
//  tbd
//
//Public Variables:
//  tbd

using UnityEngine;

public class joystickSwitch : MonoBehaviour{
    //Initialize
    public Material joystickGlow; // the accent color on the joystick
    public Color colorActive; // what color the accent will turn when in use
    private Color colorInactive; // what color the accent will be at rest
    private bool toggle = true;
    public bool typeSwitch = true;
    public float speedAnimation = 0.2f;

    public Transform outputTransform;
    public Quaternion outputInitialRotation;
    public Quaternion outputRotation1;
    public Quaternion outputRotation2;



    // Start is called before the first frame update
    void Start(){

        colorInactive = joystickGlow.color;
        outputInitialRotation = outputTransform.rotation;

}

    // Update is called once per frame
    void Update(){
        outputInitialRotation = outputTransform.rotation;
    }

    //This will need to active the switch and freeze the player
    public bool interact() {
        if (typeSwitch) { // if it's an on / off switch
            if (toggle) {
                toggle = false;
                outputTransform.rotation = outputRotation1;
                print("Interacting : 1");
            } else if (!toggle) {
                toggle = true;
                outputTransform.rotation = outputRotation2;
                print("Interacting : 0");
            }
            return true;
        } else {
            return false;
        }

    }
}
