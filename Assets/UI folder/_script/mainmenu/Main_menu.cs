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

    [Header("Main Menu")]
    public GameObject play;
    public GameObject tutorial;
    public GameObject option;
    public GameObject credit;
    public GameObject quit;

    public GameObject main_menu;

    private bool gameOnCalled = false; // Flag to track if GameOn has been called

    private void Start()
    {
        button_g.onClick.AddListener(GameOn);  // Start game
        button_t.onClick.AddListener(Tutorial); // Tutorial
        button_o.onClick.AddListener(Option); // Option
        button_c.onClick.AddListener(Credit); // Credit
        button_q.onClick.AddListener(Quit); // Quit
        gameOnCalled = false;
    }

    private void Update()
    {
        audioMainMenu.volume = Audio.Instance.bgmVolume * Audio.Instance.mainVolume;
        if(!audioMainMenu.isActiveAndEnabled )
        {
            audioMainMenu.clip = AudioSFXPlayerBehave.Instance.RandomNoiseForFlashlightFlicker();
        }
    }
    #region Main Menu
    public void GameOn()
    {
        if (gameOnCalled) return; // Prevent multiple calls
        gameOnCalled = true; // Set the flag to true
        disableAnimator();
        playClick();
        StartCoroutine(ScreenLoader.Instance.LoadLevel("Premise", false, play, main_menu)); //here got set play active to true,also audio include

    }

    public void Tutorial()
    {
        disableAnimator();
        playClick();
        tutorial.SetActive(true);
    }

    public void Option()
    {
        disableAnimator();
        playClick();
        option.SetActive(true);
    }

    public void Credit()
    {
        disableAnimator();
        playClick();
        credit.SetActive(true);
    }

    public void Quit()
    {
        disableAnimator();
        playClick();
        quit.SetActive(true);
    }
    #endregion

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
