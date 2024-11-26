using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class Audio : MonoBehaviour
{

    [Header("-----Audio Source-----")]
    [SerializeField] AudioSource musicSource; //for playing bgm
    [SerializeField] AudioSource RandomSFXForBGM; //for playing random spooky bgm
    [SerializeField] public AudioSource SFXSource; //for playing sfx
    [SerializeField] public AudioSource playerFlashlight; //for playing player flashlight
    [SerializeField] public AudioSource playerWalking; //for playing player walking

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
            //something like void start
            mainVolume = 1.0f;
            bgmVolume = 0.5f;
            sfxVolume = 0.5f;


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
        ApplyVolumeSettings();
    }
    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        ApplyVolumeSettings();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        ApplyVolumeSettings();
    }

    public void SetBackgroundMusic(AudioClip newBackgroundClip)//set BGM
    {
        musicSource.clip = newBackgroundClip;
        musicSource.Play();
    }

    private void ApplyVolumeSettings()
    {
        musicSource.volume = bgmVolume * mainVolume;
        RandomSFXForBGM.volume = bgmVolume * mainVolume;
        SFXSource.volume = sfxVolume * mainVolume;
        playerFlashlight.volume = sfxVolume * mainVolume;
        playerWalking.volume = sfxVolume * mainVolume;
    }

    public void SpecialForWalking(AudioClip clip)
    {
        Debug.Log("Music in");
        playerFlashlight.clip = clip;
        playerWalking.Play();
    }


    //for sfx
    public void PlayClipWithSource(AudioClip clip, AudioSource source, float startTime = 0f, float endTime = 0f)
    {
        if (clip == null)
        {
            //Debug.LogWarning("Audio clip is null. Cannot play SFX.");
            return;
        }
        else if (clip == AudioSFXPlayerBehave.Instance.Flashlight)//setting for specific audio
        {
            startTime = 0.2f;
            endTime = 0.6f;
        }


        if (startTime < 0f || startTime >= clip.length)
        {
            //Debug.LogWarning("Start time is out of bounds for the provided clip.");
            return;
        }

        if (endTime > 0f && endTime <= clip.length && endTime > startTime)
        {
            StartCoroutine(PlayClipWithEndTime(clip, source, startTime, endTime));
        }
        else
        {
            source.PlayOneShot(clip);
        }
    }

    private IEnumerator PlayClipWithEndTime(AudioClip clip, AudioSource source, float startTime, float endTime)
    {
        source.clip = clip;
        source.time = startTime;
        source.Play();

        yield return new WaitForSeconds(endTime - startTime);

        source.Stop();
        source.clip = null;
    }

    private IEnumerator ForBGMSFX(AudioClip clip)
    {
        RandomSFXForBGM.clip = clip;

        yield return new WaitForSeconds(120f); //wait for 2 minute

        //cuz the environment audio is at specific place so it should have a condition
        //need consider if game end still will promt the sound or not
        //need to use main to set this
    }




    public void JustForOnce(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
