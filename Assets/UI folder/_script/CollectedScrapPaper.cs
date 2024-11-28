using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Unity.Collections.LowLevel.Unsafe;
using System.Linq;

public class CollectedScrapPaper : MonoBehaviour
{
    [SerializeField] private GameObject object_canvas_big_map;
    [SerializeField] private big_map_icon_reveal_manager script_big_map_icon_reveal_manager;
    [SerializeField] private InventoryController journal;

    private float transparencyZero = 0f;
    private float transparencyMax = 1f;
    public Image leftImage;
    public Image rightImage;
    public Image clipImage;
    public Sprite[] leftSprite;//journal sprite
    public Sprite[] rightSprite;
    public Sprite emptySprite; //blank

    public Button nextPageButton;                // Button next page
    public Button previousPageButton;            // Button previous page

    private List<int> collectedScrapIDs = new List<int>(); // IDs of collected scraps
    private int currentPageIndex = 0;            // Tracks the current page index
    private int currentMaxPage = 1;
    public bool[] unlockJournal;

    private void OnValidate()
    {
        if (object_canvas_big_map == null)
            object_canvas_big_map = GameObject.Find("canvas_big_map");
        if (script_big_map_icon_reveal_manager == null && object_canvas_big_map != null)
            script_big_map_icon_reveal_manager = object_canvas_big_map.GetComponent<big_map_icon_reveal_manager>();
    }
    private void Start()
    {
        SetTransparency(leftImage, rightImage, clipImage, transparencyZero);

        nextPageButton.onClick.AddListener(FlipToNextPage);
        previousPageButton.onClick.AddListener(FlipToPreviousPage);//shouldn't assign it on the button cuz it already assign it form here
        UpdateJournal();
    }

    public void CollectScrapPaper(int id)
    {
        if (!collectedScrapIDs.Contains(id))
        {
            collectedScrapIDs.Add(id);
            currentPageIndex = id - 1;
            UpdateJournal(id);
            Debug.Log("Id added: " + id);
            unlockJournal[id -1] = true;
            journal.OpenJournal(id);
            StartCoroutine(WaitingForJournalClose());
            
        }
    }

    public void UpdateJournal(int page = -1)
    {
        if (collectedScrapIDs.Count > 0)
        {
            currentMaxPage = GetMaxValue(collectedScrapIDs); 
        }

        
        if (page != -1)
        {
            currentPageIndex = Mathf.Clamp(page - 1, 0, currentMaxPage - 1);
        }
        else
        {
            currentPageIndex = Mathf.Clamp(currentPageIndex, 0, currentMaxPage - 1);
        }

        
        if (collectedScrapIDs.Contains(currentPageIndex + 1))
        {
            
            SetTransparency(leftImage, rightImage, clipImage, transparencyMax);
            leftImage.sprite = leftSprite[currentPageIndex];
            rightImage.sprite = rightSprite[currentPageIndex];
        }
        else
        {
            
            SetTransparency(leftImage, rightImage, clipImage, transparencyZero);
            leftImage.sprite = emptySprite;
            rightImage.sprite = emptySprite;
        }

        previousPageButton.interactable = currentPageIndex > 0;
        nextPageButton.interactable = currentPageIndex < currentMaxPage - 1;

        //Debug.Log($"Updated Journal - Current Page: {currentPageIndex + 1}, Max Page: {currentMaxPage}, Collected IDs: {string.Join(", ", collectedScrapIDs)}");
    }


    private int GetMaxValue(List<int> numbers)
    {
        return numbers.Max();
    }


    public void FlipToNextPage()
    {
        Debug.Log("Flipping");
        if (currentPageIndex < currentMaxPage -1)
        {
            flipjournal();
            currentPageIndex++;
            UpdateJournal();
        }
    }

    public void FlipToPreviousPage()
    {
        if (currentPageIndex > 0)
        {
            flipjournal();
            currentPageIndex--;
            UpdateJournal();
        }
    }

    private void SetTransparency(Image image1, Image image2,Image clip, float alpha)
    {
        Color imgColor = image1.color;
        Color imgColor2 = image2.color;
        Color clipColor = clip.color;
        imgColor.a = alpha;
        imgColor2.a = alpha;
        clipColor.a = alpha;
        image1.color = imgColor;
        image2.color = imgColor2;
        clip.color = clipColor;
    }

    private IEnumerator WaitingForJournalClose()
    {
        while (!InventoryController.JournalOpen)
        {
            //Debug.Log("!InventoryController.JournalOpen");
            yield return null;
        }
        if (script_big_map_icon_reveal_manager != null)
        {
            script_big_map_icon_reveal_manager.call_this_after_scrap_paper_taken(unlockJournal);
        }
        else
        {
            Debug.LogWarning("Big map icon reveal manager is missing!");
        }
    }




    #region Sound effect
    private void flipjournal()
    {
        if (Audio.Instance != null)
        {
            Audio.Instance.PlayClipWithSource(AudioSFXUI.Instance.RandomNoiseForPageTurn(), Audio.Instance.SFXSource, 0.2f, 1.0f);
        }
    }



    #endregion
}
