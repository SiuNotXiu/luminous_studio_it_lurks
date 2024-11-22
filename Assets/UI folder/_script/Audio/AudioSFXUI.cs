using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSFXUI : MonoBehaviour
{
    public static AudioSFXUI Instance;

    [Header("SFX Journal")]
    public AudioClip JournalOpenClose;
    public AudioClip PageTurn;
    public AudioClip PageTurn2;
    public AudioClip PageTurn3;
    public AudioClip PageTurn4;
    public AudioClip PencilWriting;
    public AudioClip StoryBriefEnd;
    public AudioClip StoryBriefNext;
    public AudioClip StoryBriefPlay;
    public AudioClip UIHoverAndClick;


    [Header("SFX Item")]
    public AudioClip Adrenaline;
    public AudioClip BatteryRefill;
    public AudioClip BatteryRefill2;
    public AudioClip CampsiteCraft;
    public AudioClip DoorClose;
    public AudioClip DoorLocked;
    public AudioClip DoorOpen;
    public AudioClip GateOpen;
    public AudioClip Healing;
    public AudioClip Herb_Stick_Pickup;
    public AudioClip Item_Drop;
    public AudioClip Item_Pickup;
    public AudioClip Key_Pickup;
    public AudioClip MakeshiftCampsite_Place;
    public AudioClip MedicineCraft;
    public AudioClip OptionalScrapPaperPickup;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
