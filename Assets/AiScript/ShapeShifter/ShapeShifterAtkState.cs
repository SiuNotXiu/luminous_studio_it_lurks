using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeShifterAtkState : ShapeShifterBaseState
{
    private EnemyAttack atk;
    public override void EnterState(ShapeShifterManager shapeShifter)
    {
        //atk = shapeShifter.GetComponent<EnemyAttack>();
        //atk.Attack();
        SoundEffectManager.instance.PlayRandomSoundFxClip(shapeShifter.GetAtkAudio(),shapeShifter.transform, 1f);
    }

    public override void UpdateState(ShapeShifterManager shapeShifter)
    {

    }

    public override void ExitState(ShapeShifterManager shapeShifter)
    {

    }

    IEnumerator AtkDelay(ShapeShifterManager shapeShifter)
    {
        yield return new WaitForSeconds(2f);
        shapeShifter.SwitchState(shapeShifter.idleState);
    }
}
