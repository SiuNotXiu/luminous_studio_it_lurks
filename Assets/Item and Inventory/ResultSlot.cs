using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultSlot : MonoBehaviour
{
    [SerializeField] private Image resultImage;
    private string itemName;
    private Sprite itemSprite;
    private bool isFull = false;

    public void ShowResult(string name, Sprite sprite)
    {
        itemName = name;
        itemSprite = sprite;
        resultImage.sprite = sprite;
        resultImage.enabled = true;
        isFull = true;
    }

    public void ClearSlot()
    {
        itemName = "";
        itemSprite = null;
        resultImage.sprite = null;
        resultImage.enabled = false;
        isFull = false;
    }

    public string GetItemName()
    {
        return itemName;
    }

    public Sprite GetItemSprite()
    {
        return itemSprite;
    }

    public bool IsFull()
    {
        return isFull;
    }

    private void OnMouseUpAsButton()
    {
        if (isFull)
        {
            // Add logic to transfer item to inventory or interact with it
        }
    }
}
