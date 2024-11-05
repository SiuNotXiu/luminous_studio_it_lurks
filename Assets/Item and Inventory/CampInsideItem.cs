using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampInsideItem : MonoBehaviour
{
    //logic for this is to assign random item 1~5 into the campsite inventory journal
    //is like a data storage in the item

    [SerializeField] private CampSlot slot;
    [SerializeField] private ItemData[] itemData;




    //if player inventory all full, cant assign
    //if player inventory not full, add item to player inventory

    private void Start()
    {
        if(slot.isFull)
        {
            return;
        }
        //random assign first
        //slot.AddItem();
    }


}
