using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSFXMonster : MonoBehaviour
{
    public static AudioSFXMonster Instance;
    [Header("SFX GettingHurt")]
    public AudioClip Hit;
    public AudioClip Hit2;
    public AudioClip Hit3;

    [Header("SFX Monster")]
    [Header("--NightWalker")]
    public AudioClip Running;
    public AudioClip Walking;
    public AudioClip Anger;
    public AudioClip Growl;
    public AudioClip Whimper1;
    public AudioClip Whimper2;
    public AudioClip Whimper3;
    public AudioClip Whimper4;

    [Header("--WeepingScarecrow")]
    public AudioClip Rustling;
    public AudioClip Activation1;
    public AudioClip Activation2;

    [Header("--Shapeshifter")]
    public AudioClip Breathing1;
    public AudioClip Breathing2;
    public AudioClip Breathing3;
    public AudioClip Laugh1;
    public AudioClip Laugh2;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
