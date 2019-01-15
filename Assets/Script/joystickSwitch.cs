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




    // Start is called before the first frame update
    void Start(){

         colorInactive = joystickGlow.color;

    }

    // Update is called once per frame
    void Update(){
        
    }

    public void interact() {
        print("Interacting with Joystick");
    }
}
