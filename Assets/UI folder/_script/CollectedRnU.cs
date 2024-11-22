using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectedRnU : MonoBehaviour
{
    public Image leftImage;
    public Image rightImage;
    public Image leftClip;
    public Image rightClip;

    [Header("Recipe Sprites")]
    public Sprite[] leftRecipe;
    public Sprite[] rightRecipe;

    [Header("Upgrade Sprites")]
    public Sprite[] upgrades;

    public Sprite emptySprite; // blank page

    public Button nextPageButton;
    public Button previousPageButton;

    private int currentRecipePage = 0;
    private int currentUpgradePage = 0;
    private bool showingUpgrades = false;
    private List<int> collectedUpgrade = new List<int>(); // collected scraps

    private float transparencyZero = 0f;
    private float transparencyMax = 1f;


    //small record
    //bulb = 1
    //casing = 2

    private void Start()
    {

        SetTransparency(leftImage, leftClip, transparencyZero);
        SetTransparency(rightImage, rightClip, transparencyZero);

        nextPageButton.onClick.AddListener(FlipToNextPage);
        previousPageButton.onClick.AddListener(FlipToPreviousPage);

        UpdatePage();
    }

    public void CollectedUpgrade(int id)
    {
        if (!collectedUpgrade.Contains(id))
        {
            collectedUpgrade.Add(id);
            UpdatePage();
        }
    }

    private void UpdatePage()
    {
        if (!showingUpgrades && currentRecipePage < leftRecipe.Length)
        {
            // Display recipe pages
            SetTransparency(leftImage, leftClip, transparencyMax);
            SetTransparency(rightImage, rightClip, transparencyMax);
            leftImage.sprite = leftRecipe[currentRecipePage];
            rightImage.sprite = rightRecipe[currentRecipePage];
        }
        else if (showingUpgrades)
        {
            // Display upgrade pages in pairs
            int leftIndex = currentUpgradePage * 2;
            int rightIndex = leftIndex + 1;

            // Assign left image
            SetTransparency(leftImage, leftClip, transparencyMax);
            leftImage.sprite = upgrades[collectedUpgrade[leftIndex] - 1];

            // Assign right image, or empty if there’s no corresponding right-side upgrade
            if (rightIndex < collectedUpgrade.Count)
            {
                SetTransparency(rightImage, rightClip, transparencyMax);
                rightImage.sprite = upgrades[collectedUpgrade[rightIndex] - 1];
            }
            else
            {
                SetTransparency(rightImage, rightClip, transparencyZero);
                rightImage.sprite = emptySprite;
            }
        }

        previousPageButton.interactable = currentRecipePage > 0 ; 
        nextPageButton.interactable = (!showingUpgrades && currentRecipePage < leftRecipe.Length - 1)
                                      || (showingUpgrades && currentUpgradePage < (collectedUpgrade.Count + 1) / 2 - 1)
                                      || (!showingUpgrades && currentRecipePage == leftRecipe.Length - 1 && collectedUpgrade.Count > 0);
    }

    public void FlipToNextPage()
    {
        if (!showingUpgrades)
        {
            if (currentRecipePage < leftRecipe.Length - 1)
            {
                currentRecipePage++;
            }
            else
            {
                showingUpgrades = true;
                currentUpgradePage = 0;
            }
        }
        else if (currentUpgradePage < (collectedUpgrade.Count - 1) / 2) // left right as one page
        {
            currentUpgradePage++;
        }
        flipjournal();
        UpdatePage();
    }

    public void FlipToPreviousPage()
    {
        if (showingUpgrades)
        {
            if (currentUpgradePage > 0)
            {
                currentUpgradePage--;
            }
            else
            {
                // Go back to the last recipe page
                showingUpgrades = false;
                currentRecipePage = leftRecipe.Length - 1;
            }
        }
        else if (currentRecipePage > 0)
        {
            currentRecipePage--;
        }
        flipjournal();
        UpdatePage();
    }

    private void SetTransparency(Image image, Image clip, float alpha)
    {
        Color imgColor = image.color;
        Color clipColor = clip.color;
        imgColor.a = alpha;
        clipColor.a = alpha;
        image.color = imgColor;
        clip.color = clipColor;
    }

    #region Sound effect
    private void flipjournal()
    {
        if (Audio.Instance != null)
        {
            Audio.Instance.PlayClipWithSource(AudioSFXUI.Instance.RandomNoiseForPageTurn(), Audio.Instance.SFXSource ,0.2f, 1.0f);
        }
    }



    #endregion
}
