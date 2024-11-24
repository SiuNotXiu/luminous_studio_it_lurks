using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public Animator animator;//for the main menu
    public AudioSource audioMainMenu;

    public Button button_g;
    public Button button_t;
    public Button button_o;
    public Button button_c;
    public Button button_q;

    public GameObject play;
    public GameObject tutorial;
    public GameObject option;
    public GameObject credit;
    public GameObject main_menu;




    public GameObject gamePopup; // Assign your popup GameObject here

    private void Start()
    {
        button_g.onClick.AddListener(GameOn);  // Start game
        button_t.onClick.AddListener(Tutorial); // Tutorial
        button_o.onClick.AddListener(Option); // Option
        button_c.onClick.AddListener(Credit); // Credit
        button_q.onClick.AddListener(Quit); // Quit

    }

    private void Update()
    {
        audioMainMenu.volume = Audio.Instance.bgmVolume * Audio.Instance.mainVolume;
        if(!audioMainMenu.isActiveAndEnabled )
        {
            audioMainMenu.clip = AudioSFXPlayerBehave.Instance.RandomNoiseForFlashlightFlicker();
        }
    }

    public void GameOn()
    {
        disableAnimator();
        playClick();
        StartCoroutine(ScreenLoader.Instance.LoadLevel("Premise", false, play, main_menu)); //here got set play active to true,also audio include

    }

    public void Tutorial()
    {
        disableAnimator();
        playClick();
        // Implement tutorial logic here
        tutorial.SetActive(true);
    }

    public void Option()
    {
        disableAnimator();
        playClick();
        // Implement option logic here
        option.SetActive(true);
    }

    public void Credit()
    {
        disableAnimator();
        playClick();
        // Implement credit logic here
        credit.SetActive(true);
    }

    public void Quit()
    {
        disableAnimator();
        playClick();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    private void disableAnimator()
    {
        animator.enabled = false;
        audioMainMenu.enabled = false;
    }

    #region Sound
    private void playClick()
    {
        Audio.Instance.PlayClipWithSource(AudioSFXUI.Instance.UIHoverAndClick, Audio.Instance.SFXSource);
    }
    private void playStoryBGM()
    {
        //Audio.Instance.SetBackgroundMusic(AudioSFXEnvironment.Instance.StoryBriefAmbience);
    }

    #endregion

}
