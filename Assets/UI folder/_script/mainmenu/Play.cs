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

    public GameObject[] StoryPage;

    private int currentPage = 0;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < StoryPage.Length; i++)
        {
            StoryPage[i].SetActive(false);
        }
        //setting
        currentPage = 0;
        StoryPage[currentPage].SetActive(true);

        back.onClick.AddListener(PreviousStory);
        next.onClick.AddListener(NextStory);
        skip.onClick.AddListener(SkipStory);
        back.interactable = currentPage > 0;
        next.interactable = currentPage < StoryPage.Length -1;

    }

    // Update is called once per frame
    private void PreviousStory()
    {
        if (currentPage <= 0) return;

        playClick();
        StoryPage[currentPage].SetActive(false);
        currentPage--;
        StoryPage[currentPage].SetActive(true);

        UpdateButtonInteractability();
    }

    private void NextStory()
    {
        if (currentPage >= StoryPage.Length - 1) return;

        playClick();
        StoryPage[currentPage].SetActive(false);
        currentPage++;
        StoryPage[currentPage].SetActive(true);

        UpdateButtonInteractability();
    }

    private void SkipStory()
    {
        playClick();
        skipPage.SetActive(true);

        // Optionally deactivate all story pages if skip ends the sequence
        foreach (var page in StoryPage)
        {
            page.SetActive(false);
        }
    }

    private void UpdateButtonInteractability()
    {
        back.interactable = currentPage > 0;
        next.interactable = currentPage < StoryPage.Length - 1;
    }


    private void playClick()
    {
        Audio.Instance.PlaySFX(AudioSFXUI.Instance.UIHoverAndClick);
    }
}
