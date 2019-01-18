using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This file's methodology is largely based on TaleOf4Gamers' post on the reddit thread https://www.reddit.com/r/Unity3D/comments/8t585d/how_to_detect_if_xbox_or_playstation_controller 

/*-------------------------------------------------------------*//*
    CONTROLLERMAP
        To create a new ControllerMap go to Assets > Controller > Create Controller Map

        A ControllerMap as I've defined it is just a proper mapping of all the relevant 
        inputs a controller can give. All of the other programmers should stay away from
        mapping new contollers for the most part unless they'd like to run through the
        process with me, it's pretty easy from this point actually.

        The strings mapped to each button here are eventually passed to the input manager
        as I've set it up with the proper representation of the current player concatenated 
        onto it, i.e. "p1", "p2", "p3" or "p4". 

        Not every input here is immediately available to the game programmer if it is not 
        mapped to a ControlScheme. These inputs are ran through a ControlScheme for the sake 
        of making higher level programming easier with button names such as "Jump" or
        "Interact". If you'd like to map new buttons to a ControlSheme run over 
        ControlScheme.cs for more information.
        
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


//button0 through button11 Axes, Vertical Axis, Horizontal Axis, 3rdAxis through 8thAxis, and N/A currently available
//defaults mapped out to XBox360

[CreateAssetMenu(fileName = "Controller Map", menuName = "Controller/Create Controller Map")]
public class ControllerMap : ScriptableObject {

    [Header("Axis Mapping")]
    //Left Joystick
    public string HorizontalLeft = "Horizontal";
    public string VerticalLeft = "Vertical"; 
    public bool InvertHL = false;
    public bool InvertVL = true;
    //Right Joystick
    public string HorizontalRight = "4thAxis";
    public string VerticalRight = "5thAxis";
    public bool InvertHR = false;
    public bool InvertVR = true;
    //D-Pad
    public string HorizontalDPad = "6thAxis";
    public string VerticalDPad = "7thAxis";
    public bool InvertHD = false;
    public bool InvertVD = false;


    [Header("Button Mapping")]
    //Face Buttons
    public string BottomButton = "button0"; 
    public string RightButton = "button1";  
    public string LeftButton = "button2"; 
    public string TopButton = "button3"; 
    //Bumpers and Triggers
    public string RightBumper = "button5";
    public string RightTrigger = "3rdAxisInvert";
    public string LeftBumper = "button4";
    public string LeftTrigger = "3rdAxis";
    //Clicking Joysticks
    public string RightStick = "button9";
    public string LeftStick = "button8";
    //Start and Select
    public string StartButton = "button7";
    public string SelectButton = "button6";



    private string[] Axes;
    public void OnEnable()
    {
        //Switch back to uninverted axis where applicable
        Axes = new string[]{HorizontalLeft, VerticalLeft, HorizontalRight, VerticalRight, HorizontalDPad, VerticalDPad};
        for(int i = 0; i < Axes.Length; i++)
        {
            while (true)
            {
                if (Axes[i].Substring(Axes[i].Length - Mathf.Min(Axes[i].Length, 6)) == "Invert")
                    Axes[i] = Axes[i].Substring(0, Axes[i].Length - Mathf.Min(Axes[i].Length, 6));
                else break;
            }
        }
        HorizontalLeft = Axes[0]; VerticalLeft = Axes[1]; HorizontalRight = Axes[2]; VerticalRight = Axes[3]; HorizontalDPad = Axes[4]; VerticalDPad = Axes[5];

        //Use inverted axis where applicable
        if (InvertHL)
            HorizontalLeft += "Invert";
        if (InvertVL)
            VerticalLeft += "Invert";
        if (InvertHR)
            HorizontalRight += "Invert";
        if (InvertVR)
            VerticalRight += "Invert";
        if (InvertHD)
            HorizontalDPad += "Invert";
        if (InvertVD)
            VerticalDPad += "Invert";
    }
}
