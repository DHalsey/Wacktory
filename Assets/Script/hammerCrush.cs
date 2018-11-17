using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hammerCrush : MonoBehaviour {

    public GameObject hammer;
    public GameObject button; //the gameobject of the button that activates the hammer
    private talon_buttonTrigger buttonScript;

    private bool hammerDown;
    private bool hammerUp;

    public Vector3 downAngle = new Vector3(0.0f, 0.0f, 90f);
    public Vector3 upAngle = new Vector3(0.0f, 0.0f, 0.0f);
    public float hammerSpeed = 3.0f;
    public float raiseSpeed = 1.5f;
    public float coolDownSpeed = 1.0f;

    private float moveTime = 0f;
    private Vector3 currentAngle;

	// Use this for initialization
	void Start () {
        hammerDown = false;
        currentAngle = transform.eulerAngles;
        buttonScript = button.GetComponent<talon_buttonTrigger>(); //gets the button script to handle if the hammer should activate
        if (!buttonScript) { //error to catch changes to the buttonTrigger script
            Debug.LogError("Error!: hammerCrush did not find the buttonScript");
        }
        //Debug.Log(buttonScript);
	}
	
	// Update is called once per frame
	void Update () {

        if (/*Input.GetKeyDown("space")*/ buttonScript.isPressed && !hammerDown && !hammerUp) //pulls isPressed from buttonScript to see if the  button is pressed
        {
            hammerDown = true;
        }

        // Lower hammer
        if (hammerDown)
        {
            currentAngle = Vector3.Lerp(upAngle, downAngle, moveTime);

            hammer.transform.localEulerAngles = currentAngle; //changed this to localEulerAngles to make it work with a rotated hammer - dustin
            moveTime += Time.deltaTime * hammerSpeed;

            // Once hammer is fully lowered
            //changed this from an == to a >= due to computer rounding errors making it slightly off when the hammer is rotated - dustin
            // the 0.1 is to accomodate a small variance
            if (hammer.transform.eulerAngles.z >= downAngle.z - 0.1f) 
            {
                hammerUp = true;
                hammerDown = false;
                moveTime = 0f;
            }
        }

        // Raise hammer
        if (hammerUp)
        {
            currentAngle = Vector3.Lerp(downAngle, upAngle, moveTime);

            hammer.transform.localEulerAngles = currentAngle;
            moveTime += Time.deltaTime * raiseSpeed;

            // Once hammer is fully raised
            //changed this from an == to a >= due to computer rounding errors making it slightly off when the hammer is rotated - dustin
            // the 0.1 is to accomodate a small variance
            if (hammer.transform.eulerAngles.z <= upAngle.z + 0.1f)
            {
                hammerUp = false;
                moveTime = 0f;
            }

        }
	}
}
