using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public Button dev1;
    public Button dev2;

    // Start is called before the first frame update
    void Start()
    {
        dev1.onClick.AddListener(ChangeScene1);
        dev2.onClick.AddListener(ChangeScene2);
    }

    private void ChangeScene1()
    {
        SceneManager.LoadScene("Main");
    }
    private void ChangeScene2()
    {
        SceneManager.LoadScene("Ending");
    }
}
