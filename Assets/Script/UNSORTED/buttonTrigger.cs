using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonTrigger : MonoBehaviour {

    public GameObject btn; //the piece that gets moved downwards (the colored part)
    private float pressAmount = .2f;
    private Vector3 defaultLocalPosition; //keeps track of the initial position of the button to keep the button always moving correctly
    public bool isPressed = false;
    [HideInInspector] //we need public access to isBroken, but we dont want to see it in the inspector
    public bool isBroken = false;
    private float repairProgress = 0.0f;
    public Color btnColor;
    public Color btnBrokenColor;
    private Slider repairSlider;



    private string interactButtonName = ""; //DEBUG
    // Use this for initialization
    void Start () {
        interactButtonName = control.Interact + "1"; //DEBUG
        defaultLocalPosition = btn.transform.localPosition;
        btn.GetComponent<Renderer>().material.SetColor("_Color", btnColor);
        repairSlider = transform.Find("RepairSliderCanvas").gameObject.transform.Find("RepairSlider").gameObject.GetComponent<Slider>();
    }

    private void Update() {
        DrawDebug();
        
    }

    //temporary control to allow the repair by the player
    private void DrawDebug() {

    }

    // When triggered, button will press in towards the floor
    private void OnTriggerEnter(Collider other) {
        if (isPressed == true) return; //prevent the chance of another double click
        if (isBroken == true) return; //don't do anything if the button is broken
        
        btn.transform.localPosition = new Vector3(
            defaultLocalPosition.x,
            defaultLocalPosition.y - pressAmount,
            defaultLocalPosition.z
        );

        isPressed = true;
    }

    public ControlScheme control;
    
    // Will run as long as object is in button trigger
    private void OnTriggerStay(Collider other) {
        
        if (other.gameObject.tag == "CouchPlayers") {
            if (Input.GetButtonDown(interactButtonName)) {
                repairProgress += 0.25f;
                repairSlider.value = repairProgress;
                if (repairSlider.value >= 1) {
                    isBroken = false;
                }
            }
        }
        
    }

    // When triggered, button will release; reutrning to original height
    private void OnTriggerExit(Collider other) {
        if (isPressed == false) return; //prevent the chance of another double click
        if (isBroken == true) return; //don't do anything if the button is broken
        btn.transform.localPosition = new Vector3(
            defaultLocalPosition.x,
            defaultLocalPosition.y,
            defaultLocalPosition.z
        );
        isPressed = false;
    }

    //call this to move the button into an inoperable broken state
    public void Break() {
        isBroken = true;
        btn.GetComponent<Renderer>().material.SetColor("_Color", btnBrokenColor);
        GetComponent<ParticleSystem>().Play();
        repairProgress = 0f;
        repairSlider.value = repairProgress;
        repairSlider.transform.parent.GetComponent<Canvas>().enabled = true;
    }

    //call this to move the button into an operable repaired state
    public void Repair() {
        isBroken = false;
        btn.GetComponent<Renderer>().material.SetColor("_Color", btnColor);
        GetComponent<ParticleSystem>().Stop();
        GetComponent<ParticleSystem>().Clear(); //quickly remove the old particles to make the repair more satisfying
        repairSlider.transform.parent.GetComponent<Canvas>().enabled = false;
        repairProgress = 0;
        repairSlider.value = repairProgress;
    }

}
