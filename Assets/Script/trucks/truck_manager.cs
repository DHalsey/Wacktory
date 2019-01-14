using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class truck_manager : MonoBehaviour {

    //variables used to hold timers to determine when to send a new truck
    private float time1;
    private float time2;
    //the total amount of trucks we want to have on the map at a time
    private int maxTruckCount;
    private int currentTruckCount;
    //a listcontaining all possible items
    //private List<string> items = new List<string>();
    public GameObject truck;
    //queue for holding the trucks
    private Queue<GameObject> truckQueue = new Queue<GameObject>();
    private GameObject[] truckQueueCopy;
    //truck stops
    public GameObject first_stop;
    public GameObject second_stop;
    private bool changeInTruck;

	// Use this for initialization
	void Start () {
        //set the current time as well as the max truck count
        time1 = Time.time;
        maxTruckCount = 3;
        currentTruckCount = 0;

        //makes an list of strings for all the possible items this can be changed later to something else for now just hard
        //coding it in
        //items.Add("banana");
        //items.Add("bomb");
        //Debug.Log(items.Count);
        //we need an array that can store a copy of the truck queue so that we can reference the trucks that aren't at the top
        //of the queue
        //truckQueue = new Queue<GameObject>();
        truckQueueCopy = new GameObject[maxTruckCount];
	}
	
	// Update is called once per frame
	void Update () {
        //have a variable that checks to see if we manipulated the queue in any way (truck added or deleted)
        changeInTruck = false;
        //update time2
        time2 = Time.time;
        //if 10 seconds have passed in the game and we have less than the max trucks spawn a truck in
        if(time2- time1 >= 8 && currentTruckCount < maxTruckCount)
        {
            //reset time1 to be current time
            time1 = Time.time;
            GameObject newTruck = Instantiate(truck, this.transform.position, Quaternion.identity) as GameObject;
            //once the new truck has been created we add it to the truck queue and add 1 to truck count
            truckQueue.Enqueue(newTruck);
            currentTruckCount++;
            changeInTruck = true;
        }
        //put something here for when trucks leave

        //if a truck was added to the queue or popped off we need to recreate the array that holds a copy of the queue
        //this is so we don't copy the array every single frame and will help optimization
        if(changeInTruck == true)
        {
            truckQueueCopy = truckQueue.ToArray();
        }
        
        //Debug.Log(currentTruckCount);
        //just check if the first truck needs to be moved and if so run moveTrucks otherwise it's pointless
        if(truckQueue.Count > 0)
        {
            Debug.Log(truckQueue.Count);
            string status = truckQueueCopy[0].GetComponent<truckController>().getStatus();
            //Debug.Log(status);
            if (status == "start" || status == "waitForLoad" || status == "complete")
            {
                moveTrucks();
            }
        }
        //Debug.Log(currentTruckCount);
        
	}

    void moveTrucks()
    {
        //dealing with edge cases first, if this is the first truck
        for(int i = 0; i < truckQueue.Count; i++)
        {
   
            if (truckQueueCopy[i].GetComponent<truckController>().getStatus() == "start")
            {
                truckQueueCopy[i].GetComponent<truckController>().setMoveToWindow();
            }
            else if(truckQueueCopy[i].GetComponent<truckController>().getStatus() == "waitForLoad")
            {
                truckQueueCopy[i].GetComponent<truckController>().setMoveToLoading();
            }
            else if(truckQueueCopy[i].GetComponent<truckController>().getStatus() == "complete")
            {
                truckQueueCopy[i].GetComponent<truckController>().setMoveToEnd();
            }
        }
    }

    public void deleteTruck()
    {
        truckQueue.Dequeue();
        changeInTruck = true;
    }
    //public List<string> getItemList()
    //{
        //Debug.Log(items.Count);
        //return items;
    //}
}
