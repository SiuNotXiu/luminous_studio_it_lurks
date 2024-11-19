using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Audio : MonoBehaviour
{

    [Header("-----Audio Source-----")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource playerFootstepSource;

    [Header("-----Audio Clip-----")]
    public AudioClip BGM;
    public AudioClip InGameMusic;


    public static Audio Instance;
    public float mainVolume = 1.0f;
    public float bgmVolume = 0.5f; 
    public float sfxVolume = 0.5f;    

    private void Awake()
    {
        //PlayerPrefs.DeleteAll(); //for developers
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (!musicSource.isPlaying)
            {
                musicSource.clip = BGM;
                musicSource.Play();
            }
            // Load saved volume
            mainVolume = PlayerPrefs.GetFloat("MainVolume", mainVolume);
            bgmVolume = PlayerPrefs.GetFloat("BGMVolume", bgmVolume);
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume", sfxVolume);

            ApplyVolumeSettings();

        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMasterVolume(float volume)
    {
        mainVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("MasterVolume", mainVolume);
        ApplyVolumeSettings();
    }
    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        ApplyVolumeSettings();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        ApplyVolumeSettings();
    }

    public void SetBackgroundMusic(AudioClip newBackgroundClip)//set BGM
    {
        // Only switch if it's a new clip
        if (musicSource.clip != newBackgroundClip)
        {
            musicSource.clip = newBackgroundClip;
            musicSource.Play();
        }
    }

    private void ApplyVolumeSettings()
    {
        musicSource.volume = bgmVolume * mainVolume;
        SFXSource.volume = sfxVolume * mainVolume;
        playerFootstepSource.volume = sfxVolume * mainVolume;
    }

    public void playWalking(AudioClip clip)
    {
        playerFootstepSource.PlayOneShot(clip);
    }


    public void PlaySFX(AudioClip clip, float startTime = 0f, float endTime = 0f)
    {
        if (clip == null)
        {
            Debug.LogWarning("Audio clip is null. Cannot play SFX.");
            return;
        }
        else if (clip == AudioSFXUI.Instance.UIHoverAndClick)//setting for specific audio
        {
            startTime = 0.075f;
            endTime = 0.21f;
        }

        if (startTime < 0f || startTime >= clip.length)
        {
            Debug.LogWarning("Start time is out of bounds for the provided clip.");
            return;
        }

        if (endTime > 0f && endTime <= clip.length && endTime > startTime)
        {
            StartCoroutine(PlayClipWithEndTime(clip, startTime, endTime));
        }
        else
        {
            // Default behavior: Play the whole clip
            SFXSource.PlayOneShot(clip);
        }
    }

    private IEnumerator PlayClipWithEndTime(AudioClip clip, float startTime, float endTime)
    {
        SFXSource.clip = clip;
        SFXSource.time = startTime; // Set start time
        SFXSource.Play();

        float duration = endTime - startTime;
        yield return new WaitForSeconds(duration);

        SFXSource.Stop(); // Stop playback
        SFXSource.clip = null; // Clear the clip
    }
    public void JustForOnce(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
