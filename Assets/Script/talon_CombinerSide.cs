using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class talon_CombinerSide : MonoBehaviour {

    // What is the item this side is looking for?
    public GameObject neededItem;
    // How many of that item is needed
    public int howMany;
    // The button that needs to be pressed to combine the item
    public GameObject buttonForCombining;
    public GameObject textDisplay;

    // Current number of neededItems in combiner trigger
    int neededItemCount;
    public bool hasBeenCombined;

    bool isGitWorking = false;

	// Use this for initialization
	void Start () {

        neededItemCount = 0;
        hasBeenCombined = false;
        textDisplay.GetComponent<TextMesh>().text = 0 + " / " + howMany;
        textDisplay.GetComponent<TextMesh>().color = new Color(0.90f, 0.1f, 0.1f);

	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == neededItem.tag) {
            neededItemCount++;

            textDisplay.GetComponent<TextMesh>().text = neededItemCount + " / " + howMany;;

            if(neededItemCount >= howMany) {
                textDisplay.GetComponent<TextMesh>().color = new Color(0.1f, 0.9f, 0.1f);
            }

            Debug.Log("Items: " + neededItemCount);
        }
    }

    // Will run as long as object is in button trigger
    private void OnTriggerStay(Collider other)
    {

    }

    // When triggered, button will release; reutrning to original height
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == neededItem.tag) {
            neededItemCount--;

            textDisplay.GetComponent<TextMesh>().text = 0 + " / " + howMany;

            if (neededItemCount >= howMany)
            {
                textDisplay.GetComponent<TextMesh>().color = new Color(0.9f, 0.1f, 0.1f);
            }

            Debug.Log("Items: " + neededItemCount);
        }
    }


    // Update is called once per frame
    void Update () {
        if(neededItemCount >= howMany && buttonForCombining.GetComponent<talon_buttonTrigger>().isPressed && !hasBeenCombined){
            // Setting hasBeenCombined to true so this if statement is only true once. Otherwise, the combiner will be true for as long as there is a trigger on the button
            hasBeenCombined = true;
            Debug.Log("I'M COMBINEDDDDDDD!!!!");
        }
    }
}