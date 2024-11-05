using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShapeShifterIdleState : ShapeShifterBaseState
{
    private SpriteRenderer sr;
    public override void EnterState(ShapeShifterManager shapeShifter)
    {
        //sr = shapeShifter.GetComponent<SpriteRenderer>();
    }

    public override void UpdateState(ShapeShifterManager shapeShifter)
    {
        if (shapeShifter.GetShine() == true)
        {
            //sr.sprite = shapeShifter.GetShineSprite();
            SoundEffectManager.instance.PlayRandomSoundFxClip(shapeShifter.GetShineAudio(),shapeShifter.transform,1f);
        }
        else
        {
           // sr.sprite = shapeShifter.GetIdleSprite();
        }
      
    }

    public override void ExitState(ShapeShifterManager shapeShifter)
    {
        
    }

}
