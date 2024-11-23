using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnterGame : MonoBehaviour
{
    public Button gameScene;
    public GameObject previousScene;
    public string sceneToLoad;

    void Start()
    {
        previousScene.SetActive(false);
        gameScene.onClick.AddListener(() => changeScene(sceneToLoad));
    }

    private void changeScene(string scene)
    {
        playClick();
        AudioSFXEnvironment.Instance.EnterGame();
        StartCoroutine(ScreenLoader.Instance.LoadLevel(scene,true));
    }
    private void playClick()
    {
        Audio.Instance.PlayClipWithSource(AudioSFXUI.Instance.UIHoverAndClick, Audio.Instance.SFXSource);
    }

}
