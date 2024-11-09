using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterGame : MonoBehaviour
{
    public Button gameScene;
    public GameObject previousScene;

    // Start is called before the first frame update
    void Start()
    {
        previousScene.SetActive(false);
        gameScene.onClick.AddListener(changeScene);
    }

    // Update is called once per frame
    private void changeScene()
    {
        //switch scene script
    }
}
