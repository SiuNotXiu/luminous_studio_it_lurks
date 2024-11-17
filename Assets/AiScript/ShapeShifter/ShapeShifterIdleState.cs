using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShapeShifterIdleState : ShapeShifterBaseState
{
    private SpriteRenderer sr;
    private bool isPlayingSound = false;
    public override void EnterState(ShapeShifterManager shapeShifter)
    {
        sr = shapeShifter.GetComponent<SpriteRenderer>();
        Debug.Log("Hi Im IDle");
    }

    public override void UpdateState(ShapeShifterManager shapeShifter)
    {
        if (shapeShifter.GetShine() == true)
        {
            //sr.sprite = shapeShifter.GetShineSprite();
            if (!isPlayingSound) // Only start coroutine if it’s not already running
            {
                isPlayingSound = true;
                shapeShifter.StartCoroutine(PlaySoundEffect(shapeShifter));
                SoundEffectManager.instance.PlayRandomSoundFxClip(shapeShifter.GetShineAudio(), shapeShifter.transform, 1f);
            }
            sr.color = Color.red;
        }
        else
        {
            // sr.sprite = shapeShifter.GetIdleSprite();
            sr.color = Color.white;
            if (isPlayingSound)
            {
                shapeShifter.StopCoroutine(PlaySoundEffect(shapeShifter));
                shapeShifter.StopAllCoroutines();
                isPlayingSound = false;
            }
        }
        //Debug.Log("Shine" + shapeShifter.GetShine());
    }

    public override void ExitState(ShapeShifterManager shapeShifter)
    {
        
    }

    private IEnumerator PlaySoundEffect(ShapeShifterManager shapeShifter)
    {
        while (true)
        {
            float rand = Random.Range(3f,5f);
            yield return new WaitForSeconds(rand);
            SoundEffectManager.instance.PlayRandomSoundFxClip(shapeShifter.GetShineAudio(), shapeShifter.transform, 1f);
        }
    }

}
