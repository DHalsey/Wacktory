using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class talon_playSound : MonoBehaviour {
    public AudioClip soundToPlay;
    public AudioSource audio;
    public GameObject triggerCombined;

    bool beenPlayed;

	// Use this for initialization
	void Start () {
        beenPlayed = false;
        audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(triggerCombined.GetComponent<talon_CombinerSide>().hasBeenCombined && !beenPlayed){
            beenPlayed = true;
            audio.PlayOneShot(soundToPlay);
        }
	}
}
