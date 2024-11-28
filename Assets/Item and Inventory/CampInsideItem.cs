using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampInsideItem : MonoBehaviour
{
    // Logic for this is to assign random items (4-5) into the campsite inventory journal
    // This acts as data storage for the items

    [SerializeField] private ItemData[] itemData;
    [SerializeField] ChestInventory chestIn;
    public bool brokeCampsite;
    public bool MakeshiftCampsite;
    private int luckynum;
    public ItemData[] itemDataStroing = new ItemData[9];


    //sequence
    //1: battery, 2: first aid kit, 3:bandage, 4: adrenaline
    private void Start()
    {
        if(MakeshiftCampsite == false)
        {
            AssignRandomItems();
        }
    }


    public void AsssignToPlayerJournal()//active this at inventoryController
    {
        for (int i = 0; i < 9; i++) 
        {
            if (itemDataStroing[i] == null)
            {
                continue;
            }
            else
            {
                // Attempt to add the random item to the camp slot
                //Debug.Log("chestIn > " + chestIn);
                chestIn.StoreItemFromChest(itemDataStroing[i]);
            }

        }

        for (int j = 0; j < 9; j++)
        {
            itemDataStroing[j] = null;

        }

    }

    public void TakeBackFromPlayerJournal(ItemData item)
    {
        for (int i = 0; i < 9; i++)
        {
            if(itemDataStroing[i] == null)
            {
                itemDataStroing[i] =item;
                break;
            }
        }
    }
    private void AssignRandomItems()
    {

        // Determine the number of items to assign (randomly between 4 and 5)
        if(brokeCampsite)
        {
            luckynum = Random.Range(2, 4);//2~3
            ItemData randomItem = itemData[1];//bandage
            itemDataStroing[0] = randomItem;
            
        }
        else
        {
            luckynum = Random.Range(4, 6);//4~5
            ItemData randomItem = itemData[2];//medkit
            ItemData randomItem2 = itemData[0];//battery
            itemDataStroing[0] = randomItem;
            itemDataStroing[1] = randomItem2;

        }

        for (int i = 1; i < luckynum; i++) //-1 is to let the campsite got fixed specific item
        {
            int randomIndex;

            if (brokeCampsite)
            {
                // Ensure valid range for the random index
                randomIndex = Random.Range(0, Mathf.Max(0, itemData.Length - 2));
            }
            else
            {
                randomIndex = Random.Range(0, itemData.Length);
            }

            ItemData randomItem = itemData[randomIndex];

            if (!brokeCampsite && i == 1)
            {
                continue;
            }

            itemDataStroing[i] = randomItem;

        }

    }
}
