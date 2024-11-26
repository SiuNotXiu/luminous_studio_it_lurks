using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Play : MonoBehaviour
{
    public Image nextImage;
    public Image skipImage;

    public Button next;
    public Button skip;

    public TextMeshProUGUI smallWord;
    public GameObject skipPage;

    public GameObject[] StoryPage;

    private int currentPage = 0;

    void Start()
    {
        //deactivate all story pages initially
        foreach (var page in StoryPage)
        {
            page.SetActive(false);
        }

        currentPage = 0;
        StoryPage[currentPage].SetActive(true);

        next.onClick.AddListener(NextStory);
        skip.onClick.AddListener(SkipStory);

        // Disable next button and fade in after delay
        skip.interactable = false;
        //StartCoroutine(ButtonPrompt(10f, next, nextImage));

        // Initialize skip button's opacity
        SetImageAlpha(skipImage, 0f);
    }

    private void NextStory()
    {
        playClick();
        StoryPage[currentPage].SetActive(false);
        currentPage++;

        if (currentPage < StoryPage.Length)
        {
            StoryPage[currentPage].SetActive(true);
        }

        next.interactable = false;
        skip.interactable = true;
        SetImageAlpha(nextImage, 0f);
        SetImageAlpha(skipImage, 1f);
        StartCoroutine(TextPrompt(10.14f, smallWord)); //unity 10sec = designer 7 sec, d8s = u10.14f
        //StartCoroutine(ButtonPrompt(20f, skip, skipImage));
    }

    private void SkipStory()
    {
        continueSFX();
        SetImageAlpha(skipImage, 0f);
        StartCoroutine(ScreenLoader.Instance.LoadLevel("Main", false, skipPage)); //here got set skip active to true

        // Deactivate all story pages
        foreach (var page in StoryPage)
        {
            page.SetActive(false);
        }
    }
    #region Delected code
    /*    private IEnumerator ButtonPrompt(float delay, Button button, Image image)
        {
            yield return new WaitForSeconds(delay);

            button.interactable = true;
            yield return StartCoroutine(FadeInImage(image, 1f));
        }*/

   /* private IEnumerator FadeInImage(Image image, float duration)
    {
        float elapsedTime = 0f;
        Color color = image.color;

        while (elapsedTime < duration)
        {
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            image.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetImageAlpha(image, 1f);
    }*/

    #endregion

    private IEnumerator TextPrompt(float delay, TextMeshProUGUI text)
    {
        yield return new WaitForSeconds(delay);

        yield return StartCoroutine(FadeInText(text, 1f));
    }


    private IEnumerator FadeInText(TextMeshProUGUI text, float duration)
    {
        float elapsedTime = 0f;
        Color color = text.color;

        while (elapsedTime < duration)
        {
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            text.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final opacity is 1f
        SetTextAlpha(text, 1f);
    }

    private void SetImageAlpha(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    private void SetTextAlpha(TextMeshProUGUI text, float alpha)
    {
        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }

    #region Sound
    private void playClick()
    {
        Audio.Instance.PlayClipWithSource(AudioSFXUI.Instance.StoryBriefNext, Audio.Instance.SFXSource);
    }
    private void continueSFX()
    {
        Audio.Instance.PlayClipWithSource(AudioSFXUI.Instance.StoryBriefEnd, Audio.Instance.SFXSource);
    }

    #endregion
}
