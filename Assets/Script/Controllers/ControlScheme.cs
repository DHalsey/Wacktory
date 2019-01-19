using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-------------------------------------------------------------*//*
    CONTROLSCHEME
        To create a new ControlScheme go to Assets > Controller > Create Control Scheme

        A ControlScheme is basically a bridge to make mapping controls easier for the 
        game programming side of things as well as to allow largely different control 
        schemes between controllers.
        
        A ControlScheme is fed a ControllerMap in the editor and it will convert the 
        specified buttons to names like "Jump", "Interact" or "Throw". 
        
        To map a button to a name, specify one of the AVAILABLE INPUTS to that name 
        in the editor.
        
        To create a new input altogether, there's a small process:
            1. Declare a serialized private string as shown below and instantiate it
               to one of the AVAILABLE INPUTS, this will be the default.
            2. Declare a hidden public string as with the desired name as shown below.
            3. Copy and paste one of the lines in OnEnable(), replace the public 
               variable on the left with your new one, and the private variable
               found in GetField() toward the center-right with your new private 
               variable.
               
        The defaults here should all be set to that of XBox/PlayStation if possible, 
        if they are ever not, let me(Zack) know so I can fix them. 

    AVAILABLE INPUTS
        BottomButton - Bottom of the general 4 face buttons, think "A" on an XBox controller
        RightButton - Right of the general 4 face buttons, think "B" on an XBox controller
        LeftButton - Left of the general 4 face buttons, think "X" on an XBox controller
        TopButton - Top of the general 4 face buttons, think "Y" on an XBox controller

        RightBumper - Right Bumper
        LeftBumper - Left Bumper
        RightTrigger - Right Trigger
        LeftTrigger - Left Trigger

        RightStick - Clicking down on the right stick i.e. R3 on PlayStation
        LeftStick - Clicking down on the left stick i.e. L3 on PlayStation

        StartButton - Generally pause button
        SelectButton - Button very similar to pause button often found to left of controller

        HorizontalLeft - Horizontal Axis of left joystick
        VerticalLeft - Vertical Axis of left joystick

        HorizontalRight - Horizontal Axis of right joystick
        VerticalRight - Vertical Axis of right joystick

        HorizontalDPad - Horizontal Axis of d-pad
        VerticalDPad - Vertical Axis of d-pad

*//*-------------------------------------------------------------*/


[CreateAssetMenu(fileName = "ControlScheme", menuName = "Controller/Create Control Scheme")]
public class ControlScheme : ScriptableObject
{

    public ControllerMap map;

    /*----SERIALIZED PRIVATE STRINGS----*/
    [Header("Directional Inputs")]
    [SerializeField] private string HorizontalMovementAxis = "HorizontalLeft";
    [SerializeField] private string VerticalMovementAxis = "VerticalLeft";
    [SerializeField] private string HorizontalRightStickAxis = "HorizontalRight";
    [SerializeField] private string VerticalRightStickAxis = "VerticalRight";

    [SerializeField] private string HorizontalLookAxis = "HorizontalRight";
    [SerializeField] private string VerticalLookAxis = "VerticalRight";

    [Header("Button Inputs")]
    [SerializeField] private string JumpButton = "BottomButton";
    [SerializeField] private string InteractButton = "RightButton";
    [SerializeField] private string ThrowButton = "LeftButton";
    [SerializeField] private string TauntButton = "TopButton";

    /*----HIDDEN PUBLIC STRINGS----*/
    //Directional Inputs
    [HideInInspector] public string HorizontalMovement;
    [HideInInspector] public string VerticalMovement;
    [HideInInspector] public string HorizontalLook;
    [HideInInspector] public string VerticalLook;
    //Button Inputs
    [HideInInspector] public string Jump;
    [HideInInspector] public string Interact;
    [HideInInspector] public string Throw;
    [HideInInspector] public string Taunt;

    /*----ON ENABLE----*/
    //For explanation, this is setting your public strings equal to the strings available buttons in a ControllerMap.
    //Choice of available buttons can be specified in the editor.
    void OnEnable()
    {
        Jump = (string)map.GetType().GetField(JumpButton).GetValue(map) + "p";
        Interact = (string)map.GetType().GetField(InteractButton).GetValue(map) + "p";
        Throw = (string)map.GetType().GetField(ThrowButton).GetValue(map) + "p";
        HorizontalMovement = (string)map.GetType().GetField(HorizontalMovementAxis).GetValue(map) + "p";
        VerticalMovement = (string)map.GetType().GetField(VerticalMovementAxis).GetValue(map) + "p";
        HorizontalLook = (string)map.GetType().GetField(HorizontalLookAxis).GetValue(map) + "p";
        VerticalLook = (string)map.GetType().GetField(VerticalLookAxis).GetValue(map) + "p";
        Taunt = (string)map.GetType().GetField(TauntButton).GetValue(map) + "p";
    }



}
