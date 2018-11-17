//Dustin Halsey - dhalsey
//animateHand.cs
//Functionality: 
//  This script is intended to control the animation of the VR person's hands whenever the trigger on the controller is pulled
//  The boolean "isLeftHand" is used to swap functionality of this script between hands
//  The hand object MUST have an animator on it
//How to Attach:
//  Attach this to the hand model that is a child of the controller objects.
//  Public Variables:
//      isLeftHand - set this to be accurate to the hand you are using

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class animateHand : MonoBehaviour {
    //used to get which controller this script is using. Initialized to any hand
    //in case we do not find the hand properly
    private SteamVR_Input_Sources hand = SteamVR_Input_Sources.Any; //keeps track of which controller this script should use.  Initialized to "any" to ensure that it should work if even if the input is funky
    private Animator anim; //the animator that controls the hand animation
    public bool isLeftHand = false; //decides what controller "hand" will be

    void Start () {
        anim = GetComponent<Animator>();
        
        //Gets the input source for the hand we are using.  This allows the script to be easily thrown on either controller
        if (isLeftHand == true) {
            hand = SteamVR_Input_Sources.LeftHand;
        } else if (isLeftHand == false) {
            hand = SteamVR_Input_Sources.RightHand;
        }
    }
    
    //updates the animation of the hand.  This may be expanded on in the future if we need other hand animations
    void Update () {
        //gets an array of all clips.  In this case, there will only be 1 clip to animate so that is the only one we care about
        //the zero is to specify the animation layer to pull from.  This is a basic animation so only 1 animation should be present atm
        AnimatorClipInfo[] currentClipInfo = anim.GetCurrentAnimatorClipInfo(0); 
        float triggerPullAmount = SteamVR_Input._default.inActions.GrabTrigger.GetAxis(hand); //the amount from 0-1 that the trigger is currently pulled

        string clipToPlay = currentClipInfo[0].clip.name.ToString(); //get the name of the clip we want to play

        //always update current time in the animation to match the controller's trigger pull amount
        //note that the time of an animation is normalized from 0-1 so the triggerPullAmount works perfectly
        anim.Play(clipToPlay, 0, triggerPullAmount); 
    }
}
