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

    private bool EnterCalled = false;

    void Start()
    {
        previousScene.SetActive(false);
        gameScene.onClick.AddListener(() => changeScene(sceneToLoad));
        EnterCalled = false;
    }

    private void changeScene(string scene)
    {
        if (EnterCalled) return;
        EnterCalled = true;
        playClick();
        AudioSFXEnvironment.Instance.EnterGame();
        StartCoroutine(ScreenLoader.Instance.LoadLevel(scene,true));
    }
    private void playClick()
    {
        Audio.Instance.PlayClipWithSource(AudioSFXUI.Instance.UIHoverAndClick, Audio.Instance.SFXSource);
    }

}
