using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager instance;

    [SerializeField] private AudioSource soundFXObject;

    private AudioSource persistentAudioSource; 
    private bool isPersistentAudioPlaying = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlayRandomSoundFxClip(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        int rand = Random.Range(0, audioClip.Length);

        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip[rand];

        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayAndTrackSound(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        if (persistentAudioSource == null)
        {
            // Instantiate and set up the persistent audio source
            persistentAudioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
            persistentAudioSource.gameObject.name = "PersistentSoundFXObject"; // Optional naming
        }

        // Update its position, clip, and volume
        persistentAudioSource.transform.position = spawnTransform.position;
        persistentAudioSource.clip = audioClip;
        persistentAudioSource.volume = volume;
        persistentAudioSource.loop = true; // Enable looping
        persistentAudioSource.Play();

        isPersistentAudioPlaying = true;
    }

    // Update the position of the persistent audio source
    public void UpdatePersistentSoundPosition(Transform newTransform)
    {
        if (persistentAudioSource != null && isPersistentAudioPlaying)
        {
            persistentAudioSource.transform.position = newTransform.position;
        }
    }

    // Pause the persistent audio
    public void PausePersistentSound()
    {
        if (persistentAudioSource != null && persistentAudioSource.isPlaying)
        {
            persistentAudioSource.Pause();
        }
    }

    // Resume the persistent audio
    public void ResumePersistentSound()
    {
        if (persistentAudioSource != null && !persistentAudioSource.isPlaying)
        {
            persistentAudioSource.UnPause();
        }
    }

    public void StopPersistentSound()
    {
        if (persistentAudioSource != null && isPersistentAudioPlaying)
        {
            persistentAudioSource.Stop();
            isPersistentAudioPlaying = false;
        }
    }

}
