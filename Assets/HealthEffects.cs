using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthEffects : MonoBehaviour
{
    [HideInInspector] private GameObject object_player;
    [HideInInspector] private GameObject object_sprite_sheet_mask;
    [HideInInspector] private GameObject object_sprite_sheet_normal;
    [HideInInspector] private Animator animator_mask;
    [HideInInspector] private Animator animator_normal;

    public float currentHp = 100f;
    private float maxHp = 100f;

    public Image redSplatterImage = null;
    public Image hurtImage = null;
    private float hurtTimer = 0.3f;

    private void OnValidate()
    {
        #region initialization
        if (object_player == null)
        {
            object_player = GameObject.Find("player_dont_change_name");
        }
        if (object_sprite_sheet_mask == null)
        {
            if (object_player != null)
            {
                object_sprite_sheet_mask = object_player.transform.Find("sprite_sheet_mask").gameObject;
                animator_mask = object_sprite_sheet_mask.GetComponent<Animator>();
            }
        }
        if (object_sprite_sheet_normal == null)
        {
            if (object_player != null)
            {
                //object_sprite_sheet_normal = object_player.transform.Find("sprite_sheet_normal").gameObject;
                //animator_normal = object_sprite_sheet_normal.GetComponent<Animator>();
            }
        }
        #endregion
    }


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
        if (currentHp > 0) 
        {
            StartCoroutine(HurtFlash());
            UpdateHealth();
        }
        if (currentHp <= 0)
        {
            currentHp = 0;
            //animator_mask.Play("death");
            //animator_normal.Play("death");
            SceneManager.LoadScene("EndOfDemo");
           

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


