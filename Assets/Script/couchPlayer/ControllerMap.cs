using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Methodology largely based on TaleOf4Gamers' post on the reddit thread https://www.reddit.com/r/Unity3D/comments/8t585d/how_to_detect_if_xbox_or_playstation_controller 

[CreateAssetMenu(fileName = "Controller Map", menuName = "Controller/Create Controller Map")]
public class ControllerMap : ScriptableObject {

    [Header("Type")]
    public string ControllerType = "PS4";

    [HideInInspector] public string HorizontalMovementAxis = "Horizontal";
    [HideInInspector] public string VerticalMovementAxis = "Vertical";
    [HideInInspector] public bool InvertHorizontalMovement = false;
    [HideInInspector] public bool InvertVerticalMovement = false;

    [HideInInspector] public string JumpButton;
    [HideInInspector] public string InteractButton;
    [HideInInspector] public string ThrowButton;

    public void Awake()
    {
        JumpButton = ControllerType + "Jump";
        InteractButton = ControllerType + "Interact";
        ThrowButton = ControllerType + "Throw";
    }
}
