using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthEffects : MonoBehaviour
{
    //this script is used by "HealthControll" not "player_dont_change_name"

    [SerializeField] private GameObject object_player;
    [SerializeField] private GameObject object_sprite_sheet_mask;
    [SerializeField] private GameObject object_sprite_sheet_normal;
    [SerializeField] private Animator animator_mask;
    [SerializeField] private Animator animator_normal;

    public float currentHp = 100f;
    private float maxHp = 100f;

    public Image redSplatterImage = null;
    public Image hurtImage = null;
    private float hurtTimer = 0.3f;

    private Coroutine coroutine_ready_to_go_next_scene;
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
                object_sprite_sheet_mask = object_player.transform.Find("animation").Find("sprite_sheet_mask").gameObject;
                animator_mask = object_sprite_sheet_mask.GetComponent<Animator>();
            }
        }
        if (object_sprite_sheet_normal == null)
        {
            if (object_player != null)
            {
                object_sprite_sheet_normal = object_player.transform.Find("animation").Find("sprite_sheet_normal").gameObject;
                animator_normal = object_sprite_sheet_normal.GetComponent<Animator>();
            }
        }
        #endregion
    }



    void UpdateHealth()
    {
        Color splatterAlpha = redSplatterImage.color;
        splatterAlpha.a = 1 - (currentHp / maxHp);

        if (currentHp >= maxHp)
        {
            splatterAlpha.a = 0;
        }

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
            animator_mask.Play("death");
            animator_normal.Play("death");
            ResetBGM();
            coroutine_ready_to_go_next_scene = StartCoroutine(ready_to_go_next_scene());
            //SceneManager.LoadScene("1st Scene");//need to make it start corountine
        }
    }

    public void Heal()
    {
        currentHp += 25f;
        UpdateHealth();
    }

    public void FullHeal()
    {
        currentHp = maxHp;
        UpdateHealth();
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

    #region Sound
    private void ResetBGM()
    {
        if (Audio.Instance != null)
        {
            Audio.Instance.SetBackgroundMusic(null);
        }
    }


    #endregion

    IEnumerator ready_to_go_next_scene()
    {
        Debug.Log("ready_to_go_next_scene");
        while (true)
        {
            //Debug.Log("force off");
            player_database.is_flashlight_on = false;
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("next scene");
                SceneManager.LoadScene("1st Scene");//need to make it start corountine
                yield break;
            }
            yield return null;
        }
    }
}


