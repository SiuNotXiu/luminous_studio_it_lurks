using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectedRnU : MonoBehaviour
{
    public Image leftImage;
    public Image rightImage;

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

    //the script doest finish
    //got bug where the player got the item it should assign the upgrade photo(using collected scrap paper as reference)
    //the max value isnt right for this, the upgrade should got the sprite where assign it to this script

    private void Start()
    {
        nextPageButton.onClick.AddListener(FlipToNextPage);
        previousPageButton.onClick.AddListener(FlipToPreviousPage);

        UpdatePage();
    }

    private void UpdatePage()
    {
        if (!showingUpgrades && currentRecipePage < leftRecipe.Length)
        {
            // Display recipe pages
            leftImage.sprite = leftRecipe[currentRecipePage];
            rightImage.sprite = rightRecipe[currentRecipePage];
        }
        else if (showingUpgrades && currentUpgradePage < upgrades.Length)
        {
            // Display upgrade pages once recipes are finished
            leftImage.sprite = upgrades[currentUpgradePage];
            rightImage.sprite = emptySprite; // Assuming right side is empty for upgrades
        }
        else
        {
            // Out of bounds, set empty sprites
            leftImage.sprite = emptySprite;
            rightImage.sprite = emptySprite;
        }
        previousPageButton.interactable = currentRecipePage > 0;
        nextPageButton.interactable = (showingUpgrades && currentUpgradePage < upgrades.Length - 1) ;
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
                currentUpgradePage = 0; // Start upgrades after all recipes
            }
        }
        else if (currentUpgradePage < upgrades.Length - 1)
        {
            currentUpgradePage++;
        }

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
                showingUpgrades = false;
                currentRecipePage = leftRecipe.Length - 1; // Go back to last recipe
            }
        }
        else if (currentRecipePage > 0)
        {
            currentRecipePage--;
        }

        UpdatePage();
    }
}
