using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Play : MonoBehaviour
{
    public Button back;
    public Button next;
    public Button skip;

    public Image Story;
    public Sprite[] StoryPage;

    private int currentPage = 0;

    // Start is called before the first frame update
    void Start()
    {
        back.onClick.AddListener(previousStory);
        next.onClick.AddListener(nextStory);
        skip.onClick.AddListener(skipStory);
    }

    // Update is called once per frame
    private void previousStory()
    {
        back.interactable = currentPage > 0;
    }
    private void nextStory()
    {
        next.interactable= currentPage < StoryPage.Length;
    }
    private void skipStory()
    {

    }
}
