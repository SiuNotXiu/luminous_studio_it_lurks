using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using static System.Net.Mime.MediaTypeNames;

public class EndingSetup : MonoBehaviour
{
    public Button Next;
    [Header("Ending")]
    public GameObject[] EndingPic;
    [Header("Video")]
    public VideoPlayer[] videoPlayer;
    public GameObject[] videoPlayerGO;
    public GameObject creditAppear;

    private bool isPlayingAudio = false;
    private bool AudioRunning = false;
    private int currentVideoIndex = 0;

    public TextMeshProUGUI Thanks;

    private void Start()
    {
        Next.onClick.AddListener(OnNextButtonClicked);
        Next.gameObject.SetActive(false);
        creditAppear.SetActive(false);
        foreach (var player in videoPlayer)
        {
            if(Audio.Instance != null)
            {
                player.SetDirectAudioVolume(0, Audio.Instance.bgmVolume);
            }
            else
            {
                player.SetDirectAudioVolume(0, 1f);
            }
        }
        //bgm
        BasementBGM();

        PlayVideo(currentVideoIndex);
    }

    private void PlayVideo(int index)
    {
        if (index < videoPlayer.Length)
        {
            if (currentVideoIndex != index)
            {
                videoPlayer[currentVideoIndex].Stop();
                EndingPic[currentVideoIndex].SetActive(false);
            }


            videoPlayerGO[index].SetActive(true);
            EndingPic[index].SetActive(true);

            if (!videoPlayer[index].isPrepared)
            {
                videoPlayer[index].Prepare();
            }

            StartCoroutine(PlayWhenReady(videoPlayer[index]));

            if (index == 5) //monster munching loop
            {
                if (Audio.Instance != null)
                {
                    isPlayingAudio = true;
                    StartCoroutine(EatingAudioLoop());
                }
            }

            StartCoroutine(WaitForVideoToFinish(videoPlayer[index]));
        }
    }
    #region Credit
    public IEnumerator CreditPromt()
    {
        NoBGM();
        yield return new WaitForSeconds(2.5f);

        yield return StartCoroutine(ScreenLoader.Instance.LoadLevel("End", false, creditAppear));
        StartCoroutine(TextPrompt(4.2f, Thanks));

        yield return new WaitForSeconds(9f);

        ScreenLoader.skipAlert = true;
        StartCoroutine(ScreenLoader.Instance.LoadLevel("1st Scene", true));
        ResetBGM();
        OnAllVideosFinished();
    }
    private IEnumerator TextPrompt(float delay, TextMeshProUGUI text)
    {
        yield return new WaitForSeconds(delay);

        yield return StartCoroutine(FadeInText(text, 1f));
    }
    private IEnumerator FadeInText(TextMeshProUGUI text, float duration)
    {
        float elapsedTime = 0f;
        Color color = text.color;

        while (elapsedTime < duration)
        {
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            text.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //ensure final opacity is 1f
        SetTextAlpha(text, 1f);
    }
    private void SetTextAlpha(TextMeshProUGUI text, float alpha)
    {
        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }
    #endregion

    private IEnumerator PlayWhenReady(VideoPlayer player)
    {
        yield return new WaitUntil(() => player.isPrepared);
        player.Play();
    }

    private IEnumerator WaitForVideoToFinish(VideoPlayer player)
    {
        bool videoFinished = false;
        player.loopPointReached += vp => videoFinished = true;

        yield return new WaitUntil(() => videoFinished);
        if(currentVideoIndex == 6)
        {
            StopEating();
            isPlayingAudio = false;
            StartCoroutine(CreditPromt());
        }
        else
        {
            Next.gameObject.SetActive(true);
        }

        if (currentVideoIndex + 1 < videoPlayer.Length)
        {
            videoPlayer[currentVideoIndex + 1].Prepare();
        }
    }

    private void OnNextButtonClicked()
    {
        EndingPic[currentVideoIndex].SetActive(false);
        videoPlayerGO[currentVideoIndex].SetActive(false);
        Next.gameObject.SetActive(false);

        currentVideoIndex++;
        PlayVideo(currentVideoIndex);
    }

    private void OnAllVideosFinished()
    {      
        //Debug.Log("All videos finished!");
    }

    #region SoundEffect
    private void BasementBGM()
    {
        if (Audio.Instance != null)
        {
            Audio.Instance.SetBackgroundMusic(AudioSFXEnvironment.Instance.BasementAmbience);
        }
    }

    private void ResetBGM()
    {
        if (Audio.Instance != null)
        {
            Audio.Instance.SetBackgroundMusic(AudioSFXEnvironment.Instance.Ambience);
        }
    }

    private void NoBGM()
    {
        if (Audio.Instance != null)
        {
            Audio.Instance.SetBackgroundMusic(null);
        }
    }

    private IEnumerator EatingAudioLoop()
    {
        while (isPlayingAudio) // Loop while eating
        {
            if (!AudioRunning)
            {
                AudioClip clip = AudioSFXComic.Instance.Eating;
                Audio.Instance.SFXSource.clip = clip;
                Audio.Instance.SFXSource.Play();
                AudioRunning = true;

                yield return new WaitForSeconds(clip.length);
                AudioRunning = false;
            }
            else
            {
                yield break;
            }
        }
    }
    private void StopEating()
    {
        if (Audio.Instance != null && Audio.Instance.playerWalking != null)
        {
            Audio.Instance.SFXSource.Stop();
        }
    }

    #endregion

}
