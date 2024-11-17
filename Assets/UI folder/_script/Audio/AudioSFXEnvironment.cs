using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSFXEnvironment : MonoBehaviour
{
    public static AudioSFXEnvironment Instance;
    [Header("SFX Environment")]
    public AudioClip BasementAmbience;
    public AudioClip CampfireFireplace;
    public AudioClip CornfieldAmbience;
    public AudioClip ForestAmbience;
    public AudioClip MainMenuAmbience;
    public AudioClip VillageAmbience;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


}
