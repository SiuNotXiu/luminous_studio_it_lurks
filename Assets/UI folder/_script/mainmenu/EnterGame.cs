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

    // Start is called before the first frame update
    void Start()
    {
        previousScene.SetActive(false);
        gameScene.onClick.AddListener(() => changeScene(sceneToLoad));
    }

    // Update is called once per frame
    private void changeScene(string scene)
    {
        playClick();
        AudioSFXEnvironment.Instance.EnterGame();
        SceneManager.LoadScene(scene);
    }
    private void playClick()
    {
        Audio.Instance.PlayClipWithSource(AudioSFXUI.Instance.UIHoverAndClick, Audio.Instance.SFXSource);
    }

}
