using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSFXComic : MonoBehaviour
{
    public static AudioSFXComic Instance;
    [Header("SFX Environment")]
    public AudioClip Eating;
    public AudioClip Riser;
    public AudioClip WoodFootstep;
    public AudioClip WoodFootstep2;
    public AudioClip WoodFootstep3;
    public AudioClip WoodFootstep4;
    public AudioClip WoodRunning;
    public AudioClip WoodRunning2;
    public AudioClip WoodRunning3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
