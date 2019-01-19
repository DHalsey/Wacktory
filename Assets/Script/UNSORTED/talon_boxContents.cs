using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class talon_boxContents : MonoBehaviour
{

    public GameObject chickenObj;
    public GameObject bombObj;
    public GameObject walnutObj;
    public int numItems;

    List<GameObject> boxObjsArray = new List<GameObject>();

    [HideInInspector] //allows for the variable to be accessed by other scripts, but not show up in the inspector - dustin
    public GameObject boxContains;

    GameObject contentsCopy;

    // Use this for initialization
    void Start()
    {
        // Adding all the items to the list
        boxObjsArray.Add(chickenObj);
        boxObjsArray.Add(bombObj);
        boxObjsArray.Add(walnutObj);

        // Putting random object in a box
        boxContains = boxObjsArray[Random.Range(0, numItems)];
    }

    // Displays the contints 
    public void showContents() {
        contentsCopy = Instantiate(boxContains, transform.position, transform.rotation);
        contentsCopy.name = boxContains.name; //remove "(Clone)" from the name
    }

    public void updateContentsPos(Vector3 newPos) {
        float yOffset = newPos.y + 0.25f;

        newPos = new Vector3(newPos.x, yOffset, newPos.z);

        contentsCopy.transform.position = newPos;
    }

    public void hideContents() {
        Destroy(contentsCopy);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
