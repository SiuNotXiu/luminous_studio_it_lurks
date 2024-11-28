using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingSetup : MonoBehaviour
{
    public Button Back;
    public GameObject[] EndingPic; // Panels for scenes
    public GameObject RightArrowButton; // Button for panel navigation
    public CanvasGroup ScreenFade; // For fade-in/out effects
    public Text CreditsText; // "Thank you for playing" text
    public string MainMenuScene = "1st Scene";

    public AudioClip DoorOpenSFX, DoorCloseSFX, WoodFootstepSFX, AngryGrowlSFX, EatingSFX, SuspenseSFX;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        ClearBGM();
        StartCoroutine(PlayEndingSequence());
    }

    private void Start()
    {
        Back.onClick.AddListener(BackToMenu);
        Back.gameObject.SetActive(false);
    }

    private IEnumerator PlayEndingSequence()
    {
        // Fade to black and play DoorOpen SFX
        yield return StartCoroutine(FadeScreen(true));
        PlaySound(DoorOpenSFX);
        yield return new WaitForSeconds(1f);

        // Play DoorClose SFX
        PlaySound(DoorCloseSFX);
        yield return new WaitForSeconds(DoorCloseSFX.length);

        // Fade in first panel and simulate walking
        yield return StartCoroutine(FadeScreen(false));
        yield return ShowPanelWithWalkingSFX(0, 4);

        // Show arrow, wait for click, then transition to next panels
        yield return HandlePanelTransition(1, 2);
        yield return HandlePanelTransition(2, 2, true);
        yield return HandlePanelTransition(3, 2, false, AngryGrowlSFX);

        // Handle eating loop and final panel
        PlayLoopingSound(EatingSFX);
        yield return new WaitForSeconds(5f);
        StopLoopingSound();

        yield return HandleFinalPanel();
    }

    private IEnumerator HandlePanelTransition(int panelIndex, int footstepCount, bool longInterval = false, AudioClip extraSFX = null)
    {
        ShowPanel(panelIndex);
        if (extraSFX != null)
        {
            PlaySound(extraSFX);
            yield return new WaitForSeconds(extraSFX.length);
        }
        yield return PlayFootsteps(footstepCount, longInterval);
        yield return ShowArrowAndWait();
    }

    private IEnumerator ShowPanelWithWalkingSFX(int panelIndex, int footstepCount)
    {
        ShowPanel(panelIndex);
        yield return PlayFootsteps(footstepCount);
        yield return ShowArrowAndWait();
    }

    private IEnumerator HandleFinalPanel()
    {
        ShowPanel(EndingPic.Length - 1);
        PlaySound(SuspenseSFX);
        yield return new WaitForSeconds(SuspenseSFX.length);

        // Fade to black, show credits, and return to menu
        yield return StartCoroutine(FadeScreen(true));
        ShowCredits();
        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene(MainMenuScene);
    }

    private IEnumerator PlayFootsteps(int count, bool longInterval = false)
    {
        for (int i = 0; i < count; i++)
        {
            PlaySound(WoodFootstepSFX);
            yield return new WaitForSeconds(longInterval ? 1.5f : 0.5f);
        }
    }

    private IEnumerator ShowArrowAndWait()
    {
        RightArrowButton.SetActive(true);
        bool clicked = false;
        RightArrowButton.GetComponent<Button>().onClick.AddListener(() => clicked = true);
        while (!clicked) yield return null;
        RightArrowButton.SetActive(false);
    }

    private IEnumerator FadeScreen(bool fadeOut)
    {
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            ScreenFade.alpha = fadeOut ? Mathf.Lerp(0, 1, elapsed / duration) : Mathf.Lerp(1, 0, elapsed / duration);
            yield return null;
        }

        ScreenFade.alpha = fadeOut ? 1 : 0;
    }

    private void ShowPanel(int index)
    {
        foreach (var pic in EndingPic) pic.SetActive(false);
        EndingPic[index].SetActive(true);
    }

    private void ShowCredits()
    {
        CreditsText.text = "Thank you for playing";
        CreditsText.gameObject.SetActive(true);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource && clip)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void PlayLoopingSound(AudioClip clip)
    {
        if (audioSource && clip)
        {
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void StopLoopingSound()
    {
        if (audioSource && audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.loop = false;
        }
    }

    private void BackToMenu()
    {
        SceneManager.LoadScene(MainMenuScene);
    }

    private void ClearBGM()
    {
        if (Audio.Instance != null)
            Audio.Instance.SetBackgroundMusic(null);
    }
}
