using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Unity.Collections.LowLevel.Unsafe;

public class CollectedScrapPaper : MonoBehaviour
{
    [SerializeField] private GameObject object_canvas_big_map;
    [SerializeField] private big_map_icon_reveal_manager script_big_map_icon_reveal_manager;

    private float transparencyZero = 0f;
    private float transparencyMax = 1f;
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
    private Color colorL;
    private Color colorR;

    private void OnValidate()
    {
        if (object_canvas_big_map == null)
            object_canvas_big_map = GameObject.Find("canvas_big_map");
        if (script_big_map_icon_reveal_manager == null && object_canvas_big_map != null)
            script_big_map_icon_reveal_manager = object_canvas_big_map.GetComponent<big_map_icon_reveal_manager>();
    }
    private void Start()
    {

        colorL = leftImage.color;
        colorR = rightImage.color;
        colorL.a = transparencyZero;
        colorR.a = transparencyZero;
        leftImage.color = colorL;
        rightImage.color = colorR;

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
            if(script_big_map_icon_reveal_manager != null)
            {
                script_big_map_icon_reveal_manager.call_this_after_scrap_paper_taken(unlockJournal);
            }
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
        

        if (collectedScrapIDs.Contains(currentPageIndex +1))
        {
            //display
            colorL.a = transparencyMax;
            colorR.a = transparencyMax;
            leftImage.color = colorL;
            rightImage.color = colorR;
            leftImage.sprite = leftSprite[currentPageIndex];
            rightImage.sprite = rightSprite[currentPageIndex];
        }
        else
        {
            //display blank
            colorL.a = transparencyZero;
            colorR.a = transparencyZero;
            leftImage.color = colorL;
            rightImage.color = colorR;
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
