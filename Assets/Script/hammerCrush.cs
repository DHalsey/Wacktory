using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hammerCrush : MonoBehaviour {

    public GameObject hammer;
    public GameObject button; //the gameobject of the button that activates the hammer
    private buttonTrigger buttonScript;

    private bool hammerDown = false; //true if the hammer is currently moving down
    private bool hammerUp = false; //true if the hammer is currently moving up
    private bool hammerReset = true; //true if the hammer is ready for another swing
    [HideInInspector] //we need public access to isBroken, but we dont want to see it in the inspector
    public bool isBroken = false;

    public Vector3 downAngle = new Vector3(0.0f, 0.0f, 90f);
    public Vector3 upAngle = new Vector3(0.0f, 0.0f, 0.0f);
    public float hammerSpeed = 3.0f;
    public float raiseSpeed = 1.5f;
    public float coolDownSpeed = 1.0f;

    private int currentSwings = 0;
    private int swingsToBreak = 3;

    private float moveTime = 0f;
    private Vector3 currentAngle;

	// Use this for initialization
	void Start () {
        currentAngle = transform.eulerAngles;
        buttonScript = button.GetComponent<buttonTrigger>(); //gets the button script to handle if the hammer should activate
        if (!buttonScript) { //error to catch changes to the buttonTrigger script
            Debug.LogError("Error!: hammerCrush did not find the buttonScript");
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (buttonScript.isPressed && hammerReset && !hammerDown && !hammerUp){ //pulls isPressed from buttonScript to see if the  button is pressed
            hammerReset = false;
            hammerDown = true;
            hammerUp = false;
        }

        if (hammerDown || isBroken) SwingDown();
        if (hammerUp) SwingUp();

        if (currentSwings>= swingsToBreak) {
            Break();
            currentSwings = 0;
        }
    }

    void SwingDown() {
        //keep moving if we havent reached the end
        if (hammer.transform.eulerAngles.z < downAngle.z - 0.1f) {
            currentAngle = Vector3.Lerp(upAngle, downAngle, moveTime);
            hammer.transform.localEulerAngles = currentAngle; //using localEulerAngles to make it work with a rotated hammer
            moveTime += Time.deltaTime * hammerSpeed;
            hammerReset = false;

        } else {  
            if (isBroken) return; //Keep the hammer down if it is broken
            currentSwings += 1;
            hammer.transform.eulerAngles = downAngle;
            hammerUp = true;
            hammerDown = false;
            moveTime = 1f;
        }
    }

    void SwingUp() {        
        //keep moving if we havent reached the end
        if (hammer.transform.eulerAngles.z > upAngle.z + 0.1f) {
            currentAngle = Vector3.Lerp(upAngle, downAngle, moveTime);
            hammer.transform.localEulerAngles = currentAngle;
            moveTime -= Time.deltaTime * raiseSpeed;
        } else {
            hammer.transform.eulerAngles = upAngle;
            hammerUp = false;
            hammerReset = true;
            moveTime = 0f;
        }
    }

    //sets the hammer to a broken state.  Returns true if successful
    public bool Break() {
        isBroken = true;
        hammerUp = false;
        hammerDown = true;
        return true;
    }

    //sets the hammer to a repaired state.  Returns true if successful
    public bool Repair() {
        isBroken = false;
        hammerUp = true;
        hammerDown = false;
        return true;
    }
}
