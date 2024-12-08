using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class campfireSfx : MonoBehaviour
{
    [SerializeField] private AudioClip campfireSFX;
    // Start is called before the first frame update
    void Start()
    {
        SoundEffectManager.instance.PlayAndTrackSound(campfireSFX, transform, Volume());
    }

    public float Volume()
    {
        float volumeControl;
        if (Audio.Instance != null)
        {
            volumeControl = Audio.Instance.SFXSource.volume;

        }
        else
        {
            volumeControl = 1f;
        }
        return volumeControl;
    }
}
