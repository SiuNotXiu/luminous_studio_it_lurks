using UnityEngine;
using UnityEngine.UI;
public class AudioSlider : MonoBehaviour
{
    public Slider MainSlider;
    public Slider BGMSlider;
    public Slider SFXSlider;

    // Start is called before the first frame update
    void Start()
    {
        MainSlider.onValueChanged.AddListener(ChangeValueMain);
        BGMSlider.onValueChanged.AddListener(ChangeValueBGM);
        SFXSlider.onValueChanged.AddListener(ChangeValueSFX);
        MainSlider.value = Audio.Instance.mainVolume;
        BGMSlider.value = Audio.Instance.bgmVolume;
        SFXSlider.value = Audio.Instance.sfxVolume;
    }

    void ChangeValueMain(float value)
    {
        Audio.Instance.SetMasterVolume(value);
    }
    void ChangeValueBGM(float value)
    {
        Audio.Instance.SetBGMVolume(value);
    }
    void ChangeValueSFX(float value)
    {
        Audio.Instance.SetSFXVolume(value);
    }
}
