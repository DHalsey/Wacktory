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

    GameObject heldContent;

    // Use this for initialization
    void Start()
    {
        // Adding all the items to the list
        boxObjsArray.Add(chickenObj);
        boxObjsArray.Add(bombObj);
        boxObjsArray.Add(walnutObj);

        // Putting random object in a box
        heldContent = boxObjsArray[Random.Range(0, numItems)];
    }

    // Displays the contints 
    public void showContents() {
        heldContent = Instantiate(heldContent, transform.position, transform.rotation);
    }

    public void updateContentsPos(Vector3 newPos) {
        heldContent.transform.position = newPos;
    }

    public void hideContents() {
        Destroy(heldContent);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
