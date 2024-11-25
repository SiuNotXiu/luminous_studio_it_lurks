using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class EasterEgg : MonoBehaviour
{
    public static EasterEgg Instance;
    public static bool closingEgg = true;
    [Header("Easter Egg")]
    public GameObject[] Egg;
    private bool spacebarPressed = false;
    private Coroutine spacebarCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OpenEasterEgg(int egg)
    {
        if (player_database.is_flashlight_on)
        {
            player_database.is_flashlight_on = false;
            flashlightSFX();
        }

        if (spacebarCoroutine != null)
        {
            StopCoroutine(spacebarCoroutine);  
        }

        if (egg == 520)
        {
            Debug.Log("already active");
            Egg[0].SetActive(true);
        }
        else if (egg == 521)
        {
            Egg[1].SetActive(true);
        }

        closingEgg = false;
        spacebarPressed = false; 
        spacebarCoroutine = StartCoroutine(WaitForSpaceBar()); 
    }

    private void CloseEasterEgg()
    {
        foreach (var egg in Egg)
        {
            egg.SetActive(false);
        }
        closingEgg = true;
        spacebarPressed = false;
    }

    private IEnumerator WaitForSpaceBar()
    {
        while (!spacebarPressed)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                spacebarPressed = true;
                CloseEasterEgg();
                yield break;
            }

            yield return null;
        }
    }

    #region Sound
    private void flashlightSFX()
    {
        if (Audio.Instance != null)
        {
            Audio.Instance.PlayClipWithSource(AudioSFXPlayerBehave.Instance.Flashlight, Audio.Instance.playerFlashlight);
        }
    }
    #endregion
}
