using UnityEngine;
using UnityEngine.Video;

public class SpecialForLogo : MonoBehaviour
{
    public GameObject next;
    public VideoPlayer videoPlayer;

    void Start()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer component is not assigned!");
            return;
        }

        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        // Switch to the next scene
        StartCoroutine(ScreenLoader.Instance.LoadLevel("-", false, next, this.gameObject));
    }
}
