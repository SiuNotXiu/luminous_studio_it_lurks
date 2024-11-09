using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSFXMonster : MonoBehaviour
{
    [Header("SFX GettingHurt")]
    public AudioClip Hit;
    public AudioClip MissAttack;

    [Header("SFX Monster")]
    [Header("--NightWalker")]
    public AudioClip Running;
    public AudioClip Walking;
    public AudioClip Anger1;
    public AudioClip Anger2;
    public AudioClip Growl1;
    public AudioClip Growl2;
    public AudioClip Whimper1;
    public AudioClip Whimper2;
    public AudioClip Whimper3;

    [Header("--WeepingScarecrow")]
    public AudioClip Rustling1;
    public AudioClip Rustling2;
    public AudioClip Rustling3;
    public AudioClip Activation1;
    public AudioClip Activation2;

    [Header("--Shapeshifter")]
    public AudioClip Breathing1;
    public AudioClip Breathing2;
    public AudioClip Breathing3;
    public AudioClip Laugh1;
    public AudioClip Laugh2;
    public AudioClip Laugh3;
}
