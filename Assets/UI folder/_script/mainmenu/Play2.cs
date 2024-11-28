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
    }

    private void Story()
    {
        continueSFX();

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

    private void continueSFX()
    {
        Audio.Instance.PlayClipWithSource(AudioSFXUI.Instance.StoryBriefEnd, Audio.Instance.SFXSource);
    }

    #endregion
}
