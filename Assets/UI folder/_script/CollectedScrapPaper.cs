using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CollectedScrapPaper : MonoBehaviour
{
    [System.Serializable]
    public class ScrapPaper
    {
        public int id;                 // Unique identifier for each scrap
    }

    public List<ScrapPaper> journalScrapPapers;  // Complete list of scrap papers in chronological order

    public Image leftImage;
    public Image rightImage;
    public Sprite[] leftSprite;//journal sprite
    public Sprite[] rightSprite;
    public Sprite emptySprite; //blank

    public Button nextPageButton;                // Button next page
    public Button previousPageButton;            // Button previous page

    private List<int> collectedScrapIDs = new List<int>(); // IDs of collected scraps
    private int currentPageIndex = 0;            // Tracks the current page index
    private int scrapsPerPage = 1;               // Number of scraps per page (can adjust if needed)

    private void Start()
    {
        nextPageButton.onClick.AddListener(FlipToNextPage);
        previousPageButton.onClick.AddListener(FlipToPreviousPage);
        UpdateJournal();
    }

    public void CollectScrapPaper(int id)
    {
        if (!collectedScrapIDs.Contains(id))
        {
            Debug.Log("Id added: " + id);
            collectedScrapIDs.Add(id);

            //debug process
            if (id == 1)
            {
                leftImage.sprite = leftSprite[0];

                // Display corresponding comment
                rightImage.sprite = rightSprite[0];
            }
            UpdateJournal();
        }
    }

    private void UpdateJournal()
    {
        Debug.Log("SCcrap in");
        // Ensure the current page index is within bounds of the journal pages
        int maxPageIndex = Mathf.Max(0, Mathf.CeilToInt(journalScrapPapers.Count / (float)scrapsPerPage) - 1);
        currentPageIndex = Mathf.Clamp(currentPageIndex, 0, maxPageIndex);

        // Display scraps based on current page
        int startIndex = currentPageIndex * scrapsPerPage;
        for (int i = startIndex; i < startIndex + scrapsPerPage && i < journalScrapPapers.Count; i++) //looping is to ensure got blank page
        {
            ScrapPaper scrap = journalScrapPapers[i];
 
            if (collectedScrapIDs.Contains(scrap.id))
            {
                // Display collected scrap
                Debug.Log("Assign True tot");
                leftImage.sprite = leftSprite[i];

                // Display corresponding comment
                rightImage.sprite = rightSprite[i];
            }
            else
            {
                // Display blank page for missed scrap
                leftImage.sprite = emptySprite;
                rightImage.sprite = emptySprite;
            }
        }

        // Update button interactability based on the current page index
        //here's maybe the problem causing the button can't interact
        previousPageButton.interactable = currentPageIndex > 0;
        nextPageButton.interactable = currentPageIndex < maxPageIndex;
    }

    public void FlipToNextPage()
    {
        Debug.Log("Flipping");
        int maxPageIndex = Mathf.CeilToInt(journalScrapPapers.Count / (float)scrapsPerPage) - 1;
        if (currentPageIndex < maxPageIndex)
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
