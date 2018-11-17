using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class talon_buttonTrigger : MonoBehaviour {

    public GameObject btn;
    public float pressAmount;
    public bool isPressed;
    public Color btnColor;

    // Use this for initialization
    void Start ()
    {
        pressAmount = 0.17f;
        isPressed = false;
    }

    // When triggered, button will press in towards the floor
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.tag + " ENTERED");
        
        btn.transform.position = new Vector3(
            btn.transform.position.x,
            btn.transform.position.y - pressAmount,
            btn.transform.position.z
        );

        isPressed = true;
    }

    // Will run as long as object is in button trigger
    private void OnTriggerStay(Collider other)
    {

    }

    // When triggered, button will release; reutrning to original height
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log(other.tag + " EXITED");
        btn.transform.position = new Vector3(
            btn.transform.position.x,
            btn.transform.position.y + pressAmount,
            btn.transform.position.z
        );
        isPressed = false;
    }

    // Update is called once per frame
    void Update () {

	}
}
