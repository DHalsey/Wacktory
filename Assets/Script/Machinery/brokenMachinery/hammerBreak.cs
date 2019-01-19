using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hammerBreak : MonoBehaviour {
    public GameObject hammer; //the hammer object
    public GameObject button; //the button object that controls the hammer

    private hammerCrush hammerScript;
    private buttonTrigger buttonScript;
    private BoxCollider buttonRepairCollider;

    private bool wasHammerBroken; //holds the last state of the hammer so we can check for a change
    private bool isHammerBroken;  //the current status of the hammer
    private bool wasButtonBroken; //holds the last state of the button so we can check for a change
    private bool isButtonBroken;  //the current status of the button
	// Use this for initialization
	void Start () {
        hammerScript = hammer.GetComponent<hammerCrush>();
        buttonScript = button.GetComponent<buttonTrigger>();
        isHammerBroken = hammerScript.isBroken;
        isButtonBroken = buttonScript.isBroken;
	}
	
	// Update is called once per frame
	void Update () {
        DrawDebug();
        isHammerBroken = hammerScript.isBroken;
        isButtonBroken = buttonScript.isBroken;
        //if we moved from a repaired state to a broken, Break()
        if (isHammerBroken && !wasHammerBroken) {
            Break();
        } else if (isButtonBroken && !wasButtonBroken) {
            Break();
        }

        //if we moved from a broken state to a repaired, Repair()
        if(!isHammerBroken && wasHammerBroken) {
            Repair();
        } else if (!isButtonBroken && wasButtonBroken) {
            Repair();
        }


        wasHammerBroken = isHammerBroken;
        wasButtonBroken = isButtonBroken;
	}

    public bool debugBreak = false;
    public bool debugRepair = false;
    //used to quickly toggle a repair or break
    void DrawDebug() {
        if (debugBreak) {
            debugBreak = false;
            Break();
        }
        if (debugRepair) {
            debugRepair = false;
            Repair();
        }
    }

    void Break() {
        buttonScript.Break();
        hammerScript.Break();
    }
    void Repair() {
        buttonScript.Repair();
        hammerScript.Repair();
    }

    
}
