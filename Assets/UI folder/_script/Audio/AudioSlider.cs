using UnityEngine;
using UnityEngine.UI;
public class AudioSlider : MonoBehaviour
{
    public Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        volumeSlider.onValueChanged.AddListener(valueChangebySlider);
        volumeSlider.value = Audio.Instance.mainVolume;
    }

    void valueChangebySlider(float value)
    {
        Audio.Instance.SetVolume(value);
    }

}
