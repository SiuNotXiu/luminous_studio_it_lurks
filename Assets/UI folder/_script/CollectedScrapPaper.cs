using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Unity.Collections.LowLevel.Unsafe;

public class CollectedScrapPaper : MonoBehaviour
{


    public Image leftImage;
    public Image rightImage;
    public Sprite[] leftSprite;//journal sprite
    public Sprite[] rightSprite;
    public Sprite emptySprite; //blank

    public Button nextPageButton;                // Button next page
    public Button previousPageButton;            // Button previous page

    private List<int> collectedScrapIDs = new List<int>(); // IDs of collected scraps
    private int currentPageIndex = 0;            // Tracks the current page index
    private int currentMaxPage = 1;
    public bool[] unlockJournal;


    private void Start()
    {
        nextPageButton.onClick.AddListener(FlipToNextPage);
        previousPageButton.onClick.AddListener(FlipToPreviousPage);//shouldn't assign it on the button cuz it already assign it form here
        UpdateJournal();
    }

    public void CollectScrapPaper(int id)
    {
        if (!collectedScrapIDs.Contains(id))
        {
            Debug.Log("Id added: " + id);
            unlockJournal[id -1] = true;
            collectedScrapIDs.Add(id);
            UpdateJournal();
        }
    }

    public void UpdateJournal()
    {
        Debug.Log("Scrap in");

        if (collectedScrapIDs.Count >= 1)
        {
            currentMaxPage = GetMaxValue(collectedScrapIDs);
        }
        else
        {
            //the sprite for theleft right should be blank//i think no need?
        }

        currentPageIndex = Mathf.Clamp(currentPageIndex, 0, currentMaxPage);
        
        // Show the images based on the current page index
        //Debug.Log("current?:     " + currentPageIndex);
        //Debug.Log("max?:     " + currentMaxPage);

        if (collectedScrapIDs.Contains(currentPageIndex +1))
        {
            //display
            leftImage.sprite = leftSprite[currentPageIndex];
            rightImage.sprite = rightSprite[currentPageIndex];
        }
        else
        {
            //display blank
            leftImage.sprite = emptySprite;
            rightImage.sprite = emptySprite;
        }

        previousPageButton.interactable = currentPageIndex > 0;
        nextPageButton.interactable = currentPageIndex < currentMaxPage - 1 ;
    }

    private int GetMaxValue(List<int> numbers)
    {
        int maxValue = numbers[0]; 
        for (int i = 1; i < numbers.Count; i++)
        {
            if (numbers[i] > maxValue)
            {
                maxValue = numbers[i]; // Update maxValue if current number is greater
            }
        }

        return maxValue;
    }


    public void FlipToNextPage()
    {
        Debug.Log("Flipping");
        if (currentPageIndex < currentMaxPage -1)
        {
            currentPageIndex++;
            UpdateJournal();
        }
    }

    public void FlipToPreviousPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            UpdateJournal();
        }
    }
}
