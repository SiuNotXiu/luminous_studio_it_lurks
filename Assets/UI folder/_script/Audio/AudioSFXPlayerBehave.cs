using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSFXPlayerBehave : MonoBehaviour
{
    public static AudioSFXPlayerBehave Instance;
    [Header("SFX Player Behave")]
    [Header("--NightWalker")]

    public AudioClip CornfieldFootstep;
    public AudioClip CornfieldFootstep2;
    public AudioClip CornfieldFootstep3;
    public AudioClip CornfieldFootstep4;

    public AudioClip Flashlight;
    public AudioClip FlashlightFlicker;
    public AudioClip FlashlightFlicker2;

    public AudioClip GrassFootstep;
    public AudioClip GrassFootstep2;
    public AudioClip GrassFootstep3;
    public AudioClip GrassFootstep4;

    public AudioClip HeartBeat;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public AudioClip RandomNoiseForCornfieldFootstep()
    {
        AudioClip[] randomNoises = { CornfieldFootstep, CornfieldFootstep2, CornfieldFootstep3, CornfieldFootstep4 };
        AudioClip randomClip = randomNoises[Random.Range(0, randomNoises.Length)];
        return randomClip;
       
    }
    public AudioClip RandomNoiseForGrassFootstep()
    {
        AudioClip[] randomNoises = { GrassFootstep, GrassFootstep2, GrassFootstep3, GrassFootstep4 };
        AudioClip randomClip = randomNoises[Random.Range(0, randomNoises.Length)];
        return randomClip;
        //Audio.Instance.SetBackgroundMusic(randomClip); (this should be diff)
    }

    public AudioClip RandomNoiseForFlashlightFlicker()
    {
        AudioClip[] randomNoises = { FlashlightFlicker, FlashlightFlicker2 };
        AudioClip randomClip = randomNoises[Random.Range(0, randomNoises.Length)];
        return randomClip;

        //Audio.Instance.SetBackgroundMusic(randomClip); (this should be diff)
    }
}
