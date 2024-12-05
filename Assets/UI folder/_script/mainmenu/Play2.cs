using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Play2 : MonoBehaviour
{
    public TextMeshProUGUI mainText;
    public TextMeshProUGUI smallWord;

    public GameObject Next;
    public Button nextBTN;

    public Image nextImage;

    private bool play2Called = false;

    void Start()
    {
        nextBTN.onClick.AddListener(Story);
        nextBTN.interactable = false;
        SetImageAlpha(nextImage, 0f);
        StartCoroutine(ButtonPrompt(8f, nextBTN, nextImage,"in"));
        play2Called = false;
    }
    
    //sequence
    private void Story()
    {
        if (play2Called) return;
        play2Called = true; 
        continueSFX();
        //fade out
        StartCoroutine(ButtonPrompt(0.5f, nextBTN, nextImage, "out"));//image
        StartCoroutine(TextPrompt(0.5f, mainText, "out"));//text

        //fade in
        //StartCoroutine(TextPrompt(4.2f, smallWord,"in"));     (this one is inside the text prompt)
        //StartCoroutine(ScreenLoader.Instance.LoadLevel("Main", false, Next)); //here got set skip active to true

    }
    #region Fade Effect
    private IEnumerator TextPrompt(float delay, TextMeshProUGUI text, string situation)
    {
        yield return new WaitForSeconds(delay);
        yield return StartCoroutine(FadeText(text, 1f, situation));

        if (situation == "out")
        {
            //fade in the second phase
            yield return new WaitForSeconds(2f);
            yield return StartCoroutine(FadeText(smallWord, 1f, "in"));
            yield return new WaitForSeconds(4f);
            StartCoroutine(ScreenLoader.Instance.LoadLevel("Main", false, Next));
        }
    }
    private IEnumerator ButtonPrompt(float delay, Button button, Image image, string situation)
    {
        yield return new WaitForSeconds(delay);

        button.interactable = situation == "in";
        yield return StartCoroutine(FadeImage(image, 1f, situation));

    }

    private IEnumerator FadeText(TextMeshProUGUI text, float duration, string situation)
    {
        float elapsedTime = 0f;
        Color color = text.color;
        float startAlpha = situation == "in" ? 0f : 1f;
        float targetAlpha = situation == "in" ? 1f : 0f;

        while (elapsedTime < duration)
        {
            color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            text.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetTextAlpha(text, targetAlpha);
    }

    private IEnumerator FadeImage(Image image, float duration, string situation)
    {
        float elapsedTime = 0f;
        Color color = image.color;
        float startAlpha = situation == "in" ? 0f : 1f;
        float targetAlpha = situation == "in" ? 1f : 0f;

        while (elapsedTime < duration)
        {
            color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            image.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetImageAlpha(image, targetAlpha);
    }
    #endregion

    #region Set opacity
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
    #endregion

    #region Sound

    private void continueSFX()
    {
        Audio.Instance.PlayClipWithSource(AudioSFXUI.Instance.StoryBriefEnd, Audio.Instance.SFXSource);
    }
    private void clearBGM()
    {
        Audio.Instance.PlayClipWithSource(AudioSFXUI.Instance.StoryBriefEnd, Audio.Instance.SFXSource);
    }

    #endregion
}
