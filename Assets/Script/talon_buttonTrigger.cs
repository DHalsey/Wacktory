using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class talon_buttonTrigger : MonoBehaviour {

    GameObject btn;

    float btnX, btnY, btnZ;
    float btnR, btnG, btnB;
    float btnBaseR, btnBaseG, btnBaseB;

    public float btnPressAmt = 0.17f;

    public Color btnColor, btnBaseColor = new Color();

    // Use this for initialization
    void Start ()
    {
        // Needed for accessing the currnet button
        btn = gameObject;

        // Setting button presser and button base color
        btn.transform.parent.gameObject.GetComponent<Renderer>().material.color = btnBaseColor;
        btn.GetComponent<Renderer>().material.color = btnColor;

        btnX = btn.transform.position.x;
        btnY = btn.transform.position.y;
        btnZ = btn.transform.position.z;
    }

    // When triggered, button will press in towards the floor
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " entered " + gameObject.name + " trigger");

        // Slightly changing color to show button is pressed
        btn.GetComponent<Renderer>().material.color = new Color(btnColor.r * 0.7f, btnColor.g * 0.7f, btnColor.b * 0.7f);

        // Translating button down, because it's being pressed
        transform.position = new Vector3(btnX, btnY - btnPressAmt, btnZ);
    }

    // Will run as long as object is in button trigger
    private void OnTriggerStay(Collider other)
    {

    }

    // When triggered, button will release; reutrning to original height
    private void OnTriggerExit(Collider other)
    {
        btn.GetComponent<Renderer>().material.color = new Color(btnColor.r, btnColor.g, btnColor.b);

        Debug.Log(other.name + " exited " + gameObject.name + " trigger");
        
        transform.position = new Vector3(btnX, btnY, btnZ);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
