using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class EndingSetup : MonoBehaviour
{
    public Button Next;
    [Header("Ending")]
    public GameObject[] EndingPic;
    [Header("Video")]
    public VideoPlayer[] videoPlayer;
    public GameObject[] videoPlayerGO;

    private int currentVideoIndex = 0; 

    private void Start()
    {
        Next.onClick.AddListener(OnNextButtonClicked);
        Next.gameObject.SetActive(false);
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


        PlayVideo(currentVideoIndex);
    }

    private void PlayVideo(int index)
    {
        if (index < videoPlayer.Length)
        {
 
            EndingPic[index].SetActive(true);
            videoPlayerGO[index].SetActive(true);
            videoPlayer[index].Play();

            StartCoroutine(WaitForVideoToFinish(videoPlayer[index]));
        }
        else
        {
            SceneManager.LoadScene("1st Scene");
            OnAllVideosFinished();
        }
    }

    private IEnumerator WaitForVideoToFinish(VideoPlayer player)
    {
        yield return new WaitUntil(() => !player.isPlaying);

        Next.gameObject.SetActive(true);
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
        Debug.Log("All videos finished!");
    }
}
