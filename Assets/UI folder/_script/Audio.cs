using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Audio : MonoBehaviour
{

    [Header("-----Audio Source-----")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("-----Audio Clip-----")]
    public AudioClip BGM;
    public AudioClip InGameMusic;
    [Header("SFX Monster")]
    public AudioClip NightWalker;//below didnt include idle got damage or what
    public AudioClip WeepingScarecrow;
    public AudioClip Shapeshifter;

    public AudioClip DropItem;

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

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);

    }
}
