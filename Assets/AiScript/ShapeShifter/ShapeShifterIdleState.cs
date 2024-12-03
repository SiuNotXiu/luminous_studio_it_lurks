using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShapeShifterIdleState : ShapeShifterBaseState
{
    private SpriteRenderer sr;
    private bool isPlayingSound = false;
    private bool isShineDurationCheck = false;
    public override void EnterState(ShapeShifterManager shapeShifter)
    {
        sr = shapeShifter.GetComponent<SpriteRenderer>();
        Debug.Log("Hi Im IDle");
    }

    public override void UpdateState(ShapeShifterManager shapeShifter)
    {
        Debug.Log("shapeshifter is flashed>" + shapeShifter.gameObject.GetComponent<monster_database>().flashed);
        if (shapeShifter.gameObject.GetComponent<monster_database>().flashed == true)
        {
            //sr.sprite = shapeShifter.GetShineSprite();
            if (!isPlayingSound) // Only start coroutine if it’s not already running
            {
                isPlayingSound = true;
                shapeShifter.StartCoroutine(PlaySoundEffect(shapeShifter));
                SoundEffectManager.instance.PlayRandomSoundFxClip(shapeShifter.GetShineAudio(), shapeShifter.transform, shapeShifter.Volume());
                shapeShifter.anim.SetBool("Flash", true); shapeShifter.anim_monochrome.SetBool("Flash", true);
            }

            if (isShineDurationCheck == false)
            {
                shapeShifter.StartCoroutine(ShineDuration(shapeShifter));
            }
        }
        else
        {
            
            if (isPlayingSound)
            {
                shapeShifter.StopCoroutine(PlaySoundEffect(shapeShifter));
                shapeShifter.StopAllCoroutines();
                isPlayingSound = false;
            }
            shapeShifter.anim.SetBool("KeepFlashing", false); shapeShifter.anim_monochrome.SetBool("KeepFlashing", false);
            shapeShifter.anim.SetBool("Flash", false); shapeShifter.anim_monochrome.SetBool("Flash", false);
        }
        
    }

    public override void ExitState(ShapeShifterManager shapeShifter)
    {
        shapeShifter.StopAllCoroutines();
        isPlayingSound = false;
        shapeShifter.anim.SetBool("KeepFlashing", false); shapeShifter.anim_monochrome.SetBool("KeepFlashing", false);
        shapeShifter.anim.SetBool("Flash", false); shapeShifter.anim_monochrome.SetBool("Flash", false);
    }

    private IEnumerator PlaySoundEffect(ShapeShifterManager shapeShifter)
    {
        while (true)
        {
            float rand = Random.Range(3f,5f);
            yield return new WaitForSeconds(rand);
            SoundEffectManager.instance.PlayRandomSoundFxClip(shapeShifter.GetShineAudio(), shapeShifter.transform, shapeShifter.Volume());
        }
    }

    private IEnumerator ShineDuration(ShapeShifterManager shapeShifter)
    {
        yield return new WaitForSeconds(2f);
        shapeShifter.anim.SetBool("KeepFlashing", true); shapeShifter.anim_monochrome.SetBool("KeepFlashing", true);
    }

}
