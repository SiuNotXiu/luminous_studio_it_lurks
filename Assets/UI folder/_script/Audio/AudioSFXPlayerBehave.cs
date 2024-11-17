using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSFXPlayerBehave : MonoBehaviour
{
    public static AudioSFXPlayerBehave Instance;
    [Header("SFX Player Behave")]
    [Header("--NightWalker")]
    public AudioClip ConcreteFootstep;

    public AudioClip CornfieldFootstep;
    public AudioClip CornfieldFootstep2;
    public AudioClip CornfieldFootstep3;
    public AudioClip CornfieldFootstep4;

    public AudioClip DirtFootstep;

    public AudioClip Flashlight;
    public AudioClip FlashlightFlicker;

    public AudioClip GrassFootstep;
    public AudioClip GrassFootstep2;

    public AudioClip HeartBeat;

    public AudioClip WoodFootstep1;
    public AudioClip WoodFootstep2;
    public AudioClip WoodFootstep3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

}
