using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class truckController : MonoBehaviour
{

    public Truck thisTruck;
    public GameObject truckManager;
    public GameObject windowStop;
    public GameObject loadingStop;
    public GameObject truck_end;
    public GameObject onScreenText;
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
        itemList.Add("Banana");
        itemList.Add("Bomb");
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

        onScreenText = GameObject.Find("Canvas/truckText");
        
    }

    // Update is called once per frame
    void Update()
    {
        //if the truck has the OK to move to the window move it to the window and set status to moving
        if (window == true)
        {
            truckStatus = "moving";
            //Debug.Log("window is true and start");
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(this.transform.position, windowStop.transform.position, fracJourney);
            //once the truck has reached the window set status to ordering and wait
            if (Vector3.Distance(transform.position, windowStop.transform.position) < .5f)
            {
                window = false;
                truckStatus = "ordering";
                startTime = Time.time;
            }
        }
        else if (truckStatus == "ordering")
        {
            //while the truck is ordering wait 5 seconds and then set status to waitForLoad
            if (Time.time - startTime > 5f)
            {
                truckStatus = "waitForLoad";
            }
        }
        //one truck gets the OK to start loading set status to moving and move it to the loading bay
        else if(loading == true)
        {
            truckStatus = "moving";
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(this.transform.position, loadingStop.transform.position, fracJourney);
            //once the truck arrives at loading bay set status to loading
            if (Vector3.Distance(transform.position, loadingStop.transform.position) < .5f)
            {
                loading = false;
                truckStatus = "loading";
                onScreenText.GetComponent<Text>().text += thisTruck.getItemList();
                startTime = Time.time;
            }
        }
        else if(truckStatus == "loading")
        {
            //wait until the timer expires at the loading bay (Later if we have received enough items)
            if(Time.time - startTime > waitTime)
            {
                truckStatus = "complete";
                onScreenText.GetComponent<Text>().text = "";
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
                //Debug.Log(thisTruck.compareLists());
            }
        }
        //else if(truckStatus == "delete")
        //{
        //    truckManager.GetComponent<truck_manager>().deleteTruck();
            //Destroy(this.gameObject);
        //}
    }

    //sets the move variable to true and sets timers for the lerp (called from truck_manager)
    public void setMoveToWindow()
    {
        //Debug.Log("setting window to true");
        window = true;
        startTime = Time.time;
        journeyLength = Vector3.Distance(this.transform.position, windowStop.transform.position);
        //Debug.Log(journeyLength);
    }
    //sets the loading variable to true and sets timers for the lerp
    public void setMoveToLoading()
    {
        loading = true;
        startTime = Time.time;
        journeyLength = Vector3.Distance(this.transform.position, loadingStop.transform.position);
    }
    //sets the end variable to true and sets timers for the lerp
    public void setMoveToEnd()
    {
        end = true;
        startTime = Time.time;
        journeyLength = Vector3.Distance(this.transform.position, truck_end.transform.position);
    }
    //returns the string truckStatus
    public string getStatus()
    {
        return truckStatus;
    }
}
