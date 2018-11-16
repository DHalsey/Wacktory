using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class animateHand : MonoBehaviour {
    //used to get which controller this script is using. Initialized to any hand
    //in case we do not find the hand properly
    private SteamVR_Input_Sources hand = SteamVR_Input_Sources.Any;
    private Animator anim;
    public bool isLeftHand = false;

    void Start () {
        anim = GetComponent<Animator>();
        
        //Gets the input source for the hand we are using.  This allows the script to be easily thrown on either controller
        if (isLeftHand == true) {
            hand = SteamVR_Input_Sources.LeftHand;
        } else if (isLeftHand == false) {
            hand = SteamVR_Input_Sources.RightHand;
        }
        if (hand == SteamVR_Input_Sources.Any) {
            Debug.Log("Warning: Hand not found for animateHand script.  Defaulting to any controller input");
        }
    }
    // Update is called once per frame
    void Update () {
        AnimatorClipInfo[] currentClipInfo = anim.GetCurrentAnimatorClipInfo(0); //get the first clip on layer 0 of animations
        float triggerPullAmount = SteamVR_Input._default.inActions.GrabTrigger.GetAxis(hand);
        Debug.Log(triggerPullAmount);
        anim.Play(currentClipInfo[0].clip.name.ToString(), 0, triggerPullAmount);
    }
}
