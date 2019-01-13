using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck {
    //PUBLIC VARIABLES========================================
    //the time the truck has waited in seconds
    public int timeWaited = 0;
    //the total number of items the truck is expecting
    public int numItems = 0;

    //PRIVATE VARIABLES=======================================
    //The time that the truck is willing to wait in seconds
    private int waitTime;
    //dictionary to hold the number of each item the truck needs
    private Dictionary<string, int> itemList;
    //dictionary to hold the number of each item the truck has received thus far
    private Dictionary<string, int> itemsReceived;
    //the total number of items the truck has received
    private int numItemsReq;


    //Constructer takes in 3 parameters------------------------------------------------------------------------------------------
    //@param waitTime: the amount of time the truck is willing to wait
    //@param itemsNeeded: a string array which holds the items that the truck wants
    //@param numItems: the total number of items the truck should receive
    public Truck(int waitTime, string[] itemsNeeded, int numItems)
    {
        //initializes both dictionaries
        itemList = new Dictionary<string, int>();
        itemsReceived =  new Dictionary<string, int>();
        //initializes waitTime to parameter
        this.waitTime = waitTime;

        //loops through the provided arry from parameter
        for(int i = 0; i < numItems; i++)
        {
            //if the item in the array already exists in our dictionary add 1 to the value
            if (itemList.ContainsKey(itemsNeeded[i]))
            {
                itemList[itemsNeeded[i]]++;
            }
            //else if the item in the array doesn't already exist add it and set its value to 1
            else
            {
                itemList.Add(itemsNeeded[i], 1);
            }
        }
        //initalizes numItemsRew to the given parameter
        this.numItemsReq = numItems;
    }



    //public helper functions---------------------------------------------------------------------------------------------

    //adds item to the itemsReceived dictionary
    public void addItemToTruck(string item)
    {
        //if the entry already exists add 1 to the value
        if (itemsReceived.ContainsKey(item))
        {
            itemsReceived[item]++;
        }
        //if there is no entry for the item add it to the itemsReceived dictionary with value 1
        else
        {
            itemsReceived.Add(item, 1);
        }
    }

    //removes item from itemsReceived dictionary should only be called if dictionary has something in it otherwise will probably
    //throw an error
    public void removeItemFromTruck(string item)
    {
        itemsReceived[item]--;
    }

    //this method should be called once the required number of items have been given to the truck at that point this will check
    //and see how many missing items there are between the required items and the ones the truck actually received
    public int compareLists()
    {
        //if we have not received the required number of items return -1
        if(numItems != numItemsReq)
        {
            return -1;
        }
        int missingItems = 0;
        string itemName;
        int itemQuantity;
        //for all the items in the itemList we check them against the items we received
        foreach (var item in itemList)
        {
            itemName = item.Key;
            itemQuantity = item.Value;
            //if there is no entry of one of the required items add the value of the required item to missingItems
            if (!itemsReceived.ContainsKey(itemName)) {
                missingItems += itemQuantity;
            }
            //if we have the item but we don't have enough
            else if(itemsReceived[itemName] < itemQuantity)
            {
                missingItems = itemQuantity - itemsReceived[itemName];
            }
            //at this point we have covered if we don't have the item or if we don't have enough therefore all that is left is
            //if we have the item  in the required quantity or a surplus amount in that case we don't need to do anything
        }
        return missingItems;
    }
}
