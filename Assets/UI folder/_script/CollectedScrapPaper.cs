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
    public Transform leftPageParent;             // Left page UI parent for scrap papers
    public Transform rightPageParent;            // Right page UI parent for comments
    public GameObject blankPagePrefab;           // Blank page prefab for missed scrap pages
    public GameObject scrapPagePrefab;           // Scrap page prefab for collected papers
    public GameObject commentPrefab;             // Comment prefab for displaying comments
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
            collectedScrapIDs.Add(id);
            UpdateJournal();
        }
    }

    private void UpdateJournal()
    {
        // Clear the current journal pages
        foreach (Transform child in leftPageParent) Destroy(child.gameObject);
        foreach (Transform child in rightPageParent) Destroy(child.gameObject);

        // Ensure the current page index is within bounds of the journal pages
        int maxPageIndex = Mathf.Max(0, Mathf.CeilToInt(journalScrapPapers.Count / (float)scrapsPerPage) - 1);
        currentPageIndex = Mathf.Clamp(currentPageIndex, 0, maxPageIndex);

        // Display scraps based on current page
        int startIndex = currentPageIndex * scrapsPerPage;
        for (int i = startIndex; i < startIndex + scrapsPerPage && i < journalScrapPapers.Count; i++)
        {
            ScrapPaper scrap = journalScrapPapers[i];
            GameObject leftPage;
            GameObject rightPage;

            if (collectedScrapIDs.Contains(scrap.id))
            {
                // Display collected scrap
                leftPage = Instantiate(scrapPagePrefab, leftPageParent);

                // Display corresponding comment
                rightPage = Instantiate(commentPrefab, rightPageParent);
            }
            else
            {
                // Display blank page for missed scrap
                leftPage = Instantiate(blankPagePrefab, leftPageParent);
                rightPage = Instantiate(blankPagePrefab, rightPageParent);
            }
        }

        // Update button interactability based on the current page index
        previousPageButton.interactable = currentPageIndex > 0;
        nextPageButton.interactable = currentPageIndex < maxPageIndex;
    }

    private void FlipToNextPage()
    {
        int maxPageIndex = Mathf.CeilToInt(journalScrapPapers.Count / (float)scrapsPerPage) - 1;
        if (currentPageIndex < maxPageIndex)
        {
            currentPageIndex++;
            UpdateJournal();
        }
    }

    private void FlipToPreviousPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            UpdateJournal();
        }
    }
}
