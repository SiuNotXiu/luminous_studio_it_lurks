using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAnimation : MonoBehaviour
{
    public Animator animator; // Reference to the Animator component
    public string animationStateName; // Name of the animation state to check

    private bool isPlayingAudio = false;
    private bool AudioRunning = false;
    public float adjustTimingForWalk = 0.15f;

    //this script is for player walking audio, should assign to player
    void Update()
    {
        // Check if the specified animation is playing
        bool isAnimationPlaying = animator.GetCurrentAnimatorStateInfo(0).IsName(animationStateName);
/*        if(isAnimationPlaying)
        {
            Debug.Log("it true");
        }
        else
        {
            Debug.Log("it false");
        }*/

        
        if (isAnimationPlaying && !isPlayingAudio)
        {
            isPlayingAudio = true;
            if(Audio.Instance != null)
            {
                StartCoroutine(WalkingAudioLoop());
            }
        }
        else if (!isAnimationPlaying && isPlayingAudio)
        {
            StopWalking(); // Stop audio
            isPlayingAudio = false;
        }
    }

    private IEnumerator WalkingAudioLoop()
    {
        while (isPlayingAudio) // Loop while walking
        {
            if (!AudioRunning)
            {
                AudioClip clip = AudioSFXPlayerBehave.Instance.RandomNoiseForGrassFootstep();
                Audio.Instance.playerWalking.clip = clip;
                Audio.Instance.playerWalking.Play();
                AudioRunning = true;

                yield return new WaitForSeconds(clip.length + adjustTimingForWalk); 
                AudioRunning = false; 
            }
            else
            {
                yield break;
            }
        }
    }

    private void StopWalking()
    {
        if (Audio.Instance != null && Audio.Instance.playerWalking != null)
        {
            Audio.Instance.playerWalking.Stop(); 
        }
    }
}
