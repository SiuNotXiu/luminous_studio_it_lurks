using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Play : MonoBehaviour
{
    public Button back;
    public Button next;
    public Button skip;

    public GameObject skipPage;

    public Image Story;
    public Sprite[] StoryPage;

    private int currentPage = 0;

    // Start is called before the first frame update
    void Start()
    {
        //setting
        currentPage = 0;
        Story.sprite = StoryPage[currentPage];

        back.onClick.AddListener(previousStory);
        next.onClick.AddListener(nextStory);
        skip.onClick.AddListener(skipStory);
        back.interactable = currentPage > 0;
        next.interactable = currentPage < StoryPage.Length -1;
    }

    // Update is called once per frame
    private void previousStory()
    {
        currentPage--;
        Story.sprite = StoryPage[currentPage];

        back.interactable = currentPage > 0;
        next.interactable = currentPage < StoryPage.Length - 1;
    }
    private void nextStory()
    {
        currentPage++;
        Story.sprite = StoryPage[currentPage];

        back.interactable = currentPage > 0;
        next.interactable = currentPage < StoryPage.Length - 1;
    }
    private void skipStory()
    {
        
        skipPage.SetActive(true);
    }
}
