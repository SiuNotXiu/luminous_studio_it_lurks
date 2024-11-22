using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSFXEnvironment : MonoBehaviour
{
    public static AudioSFXEnvironment Instance;
    [Header("SFX Environment")]
    public AudioClip Ambience;
    public AudioClip BasementAmbience;
    public AudioClip CampfireFireplace;
    public AudioClip CornfieldAmbience;
    public AudioClip EnvironmentRandomNoise;
    public AudioClip EnvironmentRandomNoise2;
    public AudioClip StoryBriefAmbience;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void RandomNoiseForEnvironment()
    {
        AudioClip[] randomNoises = { EnvironmentRandomNoise, EnvironmentRandomNoise2 };
        AudioClip randomClip = randomNoises[Random.Range(0, randomNoises.Length)];

        Audio.Instance.SetBackgroundMusic(randomClip);
    }

    public void EnterGame()
    {
        Audio.Instance.SetBackgroundMusic(Ambience);
    }




}
