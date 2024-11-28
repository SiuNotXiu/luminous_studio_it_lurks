using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Play2 : MonoBehaviour
{
    public TextMeshProUGUI smallWord;

    public GameObject Next;
    public Button nextBTN;
    public Image nextImage;

    void Start()
    {
        nextBTN.onClick.AddListener(Story);
        nextBTN.interactable = false;
        SetImageAlpha(nextImage, 0f);
        StartCoroutine(TextPrompt(4.2f, smallWord));
        StartCoroutine(ButtonPrompt(8f, nextBTN, nextImage));
    }

    private void Story()
    {
        continueSFX();
        StartCoroutine(ScreenLoader.Instance.LoadLevel("Main", false, Next)); //here got set skip active to true

    }
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

        //ensure final opacity is 1f
        SetTextAlpha(text, 1f);
    }
    private IEnumerator ButtonPrompt(float delay, Button button, Image image)
    {
        yield return new WaitForSeconds(delay);

        button.interactable = true;
        yield return StartCoroutine(FadeInImage(image, 1f));
    }

    private IEnumerator FadeInImage(Image image, float duration)
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

    private void continueSFX()
    {
        Audio.Instance.PlayClipWithSource(AudioSFXUI.Instance.StoryBriefEnd, Audio.Instance.SFXSource);
    }

    #endregion
}
