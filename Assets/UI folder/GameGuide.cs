using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameGuide : MonoBehaviour
{
    [System.Serializable]
    public class GuideOut
    {
        public Image Guide;                // The image to fade in/out
        public GameObject imageGuide;      // The guide UI element to show/hide
        public GameObject GuideBoxCollider; // The collider related to the guide
        public bool hasBeenShown = false;  // Track if this guide has already been shown
    }

    [SerializeField] private List<GuideOut> guide = new List<GuideOut>();

    public void TriggerGuide(int index)
    {
        if (index < 0 || index >= guide.Count) return;

        // Check if the guide has already been shown
        if (guide[index].hasBeenShown) return;

        //blinking effect
        StartCoroutine(BlinkAndHideGuide(index));

        //won't show again
        guide[index].hasBeenShown = true;
    }

    // Coroutine to blink the UI for 2.5 seconds
    private IEnumerator BlinkAndHideGuide(int index)
    {
        float duration = 5f;                 // Total duration for blinking
        float fadeSpeed = 2f;                 // Speed of the alpha change
        float minAlpha = 0f;                  // Minimum alpha value
        float maxAlpha = 1f;                  // Maximum alpha value
        bool fadingOut = false;              // Controls if the fade is in or out
        float elapsed = 0f;                  // Track how long the effect has been active

        var guideElement = guide[index];
        guideElement.imageGuide.SetActive(true);
        Color color = guideElement.Guide.color; 

        while (elapsed < duration)
        {

            elapsed += Time.deltaTime;

            if (fadingOut)
            {
                color.a -= fadeSpeed * Time.deltaTime;
                if (color.a <= minAlpha)
                {
                    color.a = minAlpha;
                    fadingOut = false; 
                }
            }
            else
            {
                color.a += fadeSpeed * Time.deltaTime;
                if (color.a >= maxAlpha)
                {
                    color.a = maxAlpha;
                    fadingOut = true; 
                }
            }

            guideElement.Guide.color = color;

            yield return null; // Wait for the next frame
        }

        guideElement.imageGuide.SetActive(false);
        guideElement.GuideBoxCollider.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Triggered Guide!");

        if (collision.CompareTag("Player"))
        {
            Debug.Log("Triggered Guide!");
            TriggerGuide(0);
        }
    }
}
