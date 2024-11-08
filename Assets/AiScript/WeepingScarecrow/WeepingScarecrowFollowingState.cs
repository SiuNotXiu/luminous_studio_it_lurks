using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class WeepingScarecrowFollowingState : WeepingScarecrowBaseState
{
    private NavMeshAgent agent;
    private Coroutine playSoundCoroutine;

    public override void EnterState(WeepingScarecrowManager weepingScarecrow)
    {
        if (weepingScarecrow.GetTarget() != null)
        {
            agent = weepingScarecrow.GetAgent();
            if (agent.isStopped)
            {
                agent.isStopped = false;
            }

            weepingScarecrow.StartCoroutine(UpdateTargetPosition(weepingScarecrow, agent));
            playSoundCoroutine = weepingScarecrow.StartCoroutine(PlaySoundEffect(weepingScarecrow));
            Debug.Log("Hi, I'm following");
        }
    }

    public override void UpdateState(WeepingScarecrowManager weepingScarecrow)
    {
        if (weepingScarecrow.GetTarget() != null)
        {
            if (weepingScarecrow.GetFlashed())
            {
                weepingScarecrow.StartCoroutine(SwitchStateDelay(weepingScarecrow));
            }
        }
      
    }

    public override void ExitState(WeepingScarecrowManager weepingScarecrow)
    {
        if (playSoundCoroutine != null)
        {
            weepingScarecrow.StopCoroutine(playSoundCoroutine);
        }
    }

    private IEnumerator UpdateTargetPosition(WeepingScarecrowManager weepingScarecrow, NavMeshAgent agent)
    {
        while (true)
        {
            if (weepingScarecrow.GetTarget() != null)
            {
                agent.SetDestination(weepingScarecrow.GetTarget().position);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator SwitchStateDelay(WeepingScarecrowManager weepingScarecrow)
    {
        yield return new WaitForSeconds(0.6f);
        weepingScarecrow.SwitchState(weepingScarecrow.idleState);
    }

    private IEnumerator PlaySoundEffect(WeepingScarecrowManager weepingScarecrow)
    {
        while (true)
        {
            float rand = Random.Range(5f, 7f);
            yield return new WaitForSeconds(rand);
            SoundEffectManager.instance.PlayRandomSoundFxClip(weepingScarecrow.GetFlwSoundClips(), weepingScarecrow.transform, 1f);
        }
    }
}
