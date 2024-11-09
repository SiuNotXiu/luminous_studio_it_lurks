using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button button_g;
    public Button button_t;
    public Button button_o;
    public Button button_c;
    public Button button_q;

    public GameObject play;
    public GameObject tutorial;
    public GameObject option;
    public GameObject credit;




    public GameObject gamePopup; // Assign your popup GameObject here

    private void Start()
    {
        button_g.onClick.AddListener(GameOn);  // Start game
        button_t.onClick.AddListener(Tutorial); // Tutorial
        button_o.onClick.AddListener(Option); // Option
        button_c.onClick.AddListener(Credit); // Credit
        button_q.onClick.AddListener(Quit); // Quit

    }

    public void GameOn()
    {
        
        play.SetActive(true);
        
    }

    public void Tutorial()
    {
        // Implement tutorial logic here
        tutorial.SetActive(true);
    }

    public void Option()
    {
        // Implement option logic here
        option.SetActive(true);
    }

    public void Credit()
    {
        // Implement credit logic here
        credit.SetActive(true);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
