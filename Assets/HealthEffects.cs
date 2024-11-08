    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class HealthEffects : MonoBehaviour
    {
        public float currentHp = 100f;
        private float maxHp = 100f;

        public Image redSplatterImage = null;
        public Image hurtImage = null;
        private float hurtTimer = 0.3f;

        // Start is called before the first frame update
       

    
        // Update is called once per frame
        void UpdateHealth()
        {
            Color splatterAlpha = redSplatterImage.color;
            splatterAlpha.a = 1 - (currentHp / maxHp);
            redSplatterImage.color = splatterAlpha;
        }

    IEnumerator HurtFlash()
    {
        // Calculate initial opacity based on health
        Color hurtImageAlpha = hurtImage.color;
        hurtImageAlpha.a = 1 - (currentHp / maxHp); // Lower health = Higher opacity

        // Apply the calculated opacity
        hurtImage.enabled = true;
        hurtImage.color = hurtImageAlpha;

        // Keep the image visible for the duration of the flash
        yield return new WaitForSeconds(hurtTimer);

        // Start fading out the hurt image
        float fadeDuration = 5f; // Time it takes to fully fade out
        float fadeOutTime = 0f;

        while (fadeOutTime < fadeDuration)
        {
            fadeOutTime += Time.deltaTime;
            // Gradually reduce the alpha to zero over time
            hurtImageAlpha.a = Mathf.Lerp(hurtImageAlpha.a, 0f, fadeOutTime / fadeDuration);
            hurtImage.color = hurtImageAlpha;
            yield return null; // Wait for the next frame
        }

        // Disable the image after it fully fades out
        hurtImage.enabled = false;
        // Reset the alpha for the next hurt event
        hurtImageAlpha.a = 3f;
        hurtImage.color = hurtImageAlpha;
    }

    public void TakeDamage()
        {
            if (currentHp >= 0) 
            {
                StartCoroutine(HurtFlash());
                UpdateHealth();
            }
        }

    public void Heal()
    {
        currentHp += 10f;
    }

    public void FullHeal()
    {
        currentHp = maxHp;
    }

    public bool GetFullHealth()
    {
        if (currentHp == maxHp)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    }


