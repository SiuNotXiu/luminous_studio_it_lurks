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
        //Debug.Log("Scrap in");

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
            SetTransparency(leftImage, rightImage, clipImage, transparencyMax);
            leftImage.sprite = leftSprite[currentPageIndex];
            rightImage.sprite = rightSprite[currentPageIndex];
        }
        else
        {
            //display blank
            SetTransparency(leftImage, rightImage, clipImage, transparencyZero);
            leftImage.sprite = emptySprite;
            rightImage.sprite = emptySprite;
        }

        previousPageButton.interactable = currentPageIndex > 0;
        nextPageButton.interactable = currentPageIndex < currentMaxPage - 1 ;
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
}
