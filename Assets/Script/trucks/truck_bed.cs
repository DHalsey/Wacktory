using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class truck_bed : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    	
	}
    private void OnTriggerEnter(Collider item)
    {
        if(item.gameObject.layer == 8)
        {
            //first we need to get the name of the object
            string itemName = item.gameObject.name;
            //once we have the name it is very likely the name has "nameOfObject(CLONE)" in it. To get rid of clone we split
            //on '(' so we get an array of two strings nameOfObject at index 0 and CLONE) at index 1
            //then we just take index 0
            string[] itemID = itemName.Split('(');
            GetComponentInParent<truckController>().addItem(itemID[0]);
            Destroy(item.gameObject);
        }
        
    }
}
