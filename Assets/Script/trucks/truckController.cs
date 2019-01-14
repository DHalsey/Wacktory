using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class truckController : MonoBehaviour
{

    private Truck thisTruck;
    public GameObject truckManager;
    public GameObject windowStop;
    public GameObject loadingStop;
    public GameObject truck_end;
    //list of all the possible items
    private List<string> itemList = new List<string>();
    //total # of items the truck wants
    private int numItems;
    //a string of items the truck wants
    private string[] groceryList;
    //how long the truck is willing to wait
    private int waitTime;
    //start time for lerping purposes and distance
    private float startTime;
    float journeyLength;
    private float speed = .05f;
    //if the object is moving towards the window/loading bay
    private bool window = false;
    private bool loading = false;
    private bool end = false;
    private string truckStatus = "start";
    // Use this for initialization

    void Start()
    {
        //on creation we want to create a random item list for the truck to need
        //first we make a list of all possible items
        itemList.Add("banana");
        itemList.Add("bomb");
        //now we can choose how many items we want and then randomly select items from the list for testing it will be set to 5
        numItems = 5;
        int rand;
        groceryList = new string[numItems];
        for (int i = 0; i < numItems; i++)
        {
            //picks a random index from the total items list and puts that item on the grocery list
            //Debug.Log(items.Count);
            rand = Random.Range(0, itemList.Count);
            groceryList[i] = itemList[rand];
        }

        //we can assign this a random value but for testing we will make it a standard 10 second wait time
        waitTime = 10;
        //at this point we have all the components for the construction of the truck
        thisTruck = new Truck(waitTime, groceryList, numItems);
    }

    // Update is called once per frame
    void Update()
    {
        //in here will put some code to detect what items have been given to the truck and what not
        if (window == true)
        {
            truckStatus = "moving";
            //Debug.Log("window is true and start");
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(this.transform.position, windowStop.transform.position, fracJourney);
            //Debug.Log(Vector3.Distance(transform.position, windowStop.transform.position));
            if (Vector3.Distance(transform.position, windowStop.transform.position) < .5f)
            {
                window = false;
                //truckManager.GetComponent<truck_manager>().changeMovingStatus();
                truckStatus = "ordering";
                startTime = Time.time;
            }
        }
        else if (truckStatus == "ordering")
        {
            //Debug.Log("status is ordering");
            if (Time.time - startTime > 5f)
            {
                truckStatus = "waitForLoad";
            }
        }
        else if(loading == true)
        {
            truckStatus = "moving";
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(this.transform.position, loadingStop.transform.position, fracJourney);
            if (Vector3.Distance(transform.position, loadingStop.transform.position) < .5f)
            {
                loading = false;
                //truckManager.GetComponent<truck_manager>().changeMovingStatus();
                truckStatus = "loading";
                startTime = Time.time;
            }
        }
        else if(truckStatus == "loading")
        {
            if(Time.time - startTime > waitTime)
            {
                truckStatus = "complete";
            }
        }
        else if(end == true)
        {
            truckStatus = "moving";
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(this.transform.position, truck_end.transform.position, fracJourney);
            if (Vector3.Distance(transform.position, truck_end.transform.position) < .5f)
            {
                end = false;
                //truckManager.GetComponent<truck_manager>().changeMovingStatus();
                truckStatus = "delete";
                Debug.Log(thisTruck.compareLists());
            }
        }
        //else if(truckStatus == "delete")
        //{
        //    truckManager.GetComponent<truck_manager>().deleteTruck();
            //Destroy(this.gameObject);
        //}
    }

    public void setMoveToWindow()
    {
        //Debug.Log("setting window to true");
        window = true;
        startTime = Time.time;
        journeyLength = Vector3.Distance(this.transform.position, windowStop.transform.position);
        //Debug.Log(journeyLength);
    }
    public void setMoveToLoading()
    {
        loading = true;
        startTime = Time.time;
        journeyLength = Vector3.Distance(this.transform.position, loadingStop.transform.position);
    }
    public void setMoveToEnd()
    {
        end = true;
        startTime = Time.time;
        journeyLength = Vector3.Distance(this.transform.position, truck_end.transform.position);
    }
    public string getStatus()
    {
        return truckStatus;
    }
}
