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
    public float mainVolume = 0.5f;

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
            AudioListener.volume = mainVolume;//Apply

        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetVolume(float volume)
    {
        mainVolume = Mathf.Clamp01(volume);
        AudioListener.volume = mainVolume;
        PlayerPrefs.SetFloat("MainVolume", mainVolume); //Save
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
