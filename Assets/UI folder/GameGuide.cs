using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

        // Start the blinking effect
        StartCoroutine(BlinkAndHideGuide(index));

        // Mark as shown so it won't show again
        guide[index].hasBeenShown = true;
    }

    // Coroutine to blink the UI for 2.5 seconds
    private IEnumerator BlinkAndHideGuide(int index)
    {
        float duration = 2.5f;               // Total duration for blinking
        float blinkInterval = 0.25f;         // Interval between alpha toggles
        var guideElement = guide[index];
        float elapsed = 0f;

        guideElement.imageGuide.SetActive(true); // Make the guide visible initially
        Color color = guideElement.Guide.color;  // Get the current color of the guide image
        color.a = 1f; // Set the alpha to fully visible

        // Loop to blink the guide's image and collider
        while (elapsed < duration)
        {
            elapsed += blinkInterval;

            // Calculate alpha using Mathf.PingPong to toggle between 0 and 1
            float alpha = Mathf.PingPong(elapsed / blinkInterval, 1f);

            // Update the image's alpha
            color.a = alpha;
            guideElement.Guide.color = color;

            // Show/hide the GuideBoxCollider based on alpha value
            guideElement.GuideBoxCollider.SetActive(alpha > 0);

            yield return new WaitForSeconds(blinkInterval);
        }

        // Hide the guide UI elements after blinking
        guideElement.imageGuide.SetActive(false);
        guideElement.GuideBoxCollider.SetActive(false);

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered Guide!");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Triggered Guide!");
            // Example: Trigger the first guide when player enters
            TriggerGuide(0);
        }
    }
}
