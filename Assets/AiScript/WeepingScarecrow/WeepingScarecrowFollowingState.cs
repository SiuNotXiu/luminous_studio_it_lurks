using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class WeepingScarecrowFollowingState : WeepingScarecrowBaseState
{
    private NavMeshAgent agent;
    private Coroutine playSoundCoroutine;
    private Animator anim;
    private monster_database md;

    public override void EnterState(WeepingScarecrowManager weepingScarecrow)
    {
        if (weepingScarecrow.GetTarget() != null)
        {
            agent = weepingScarecrow.GetAgent();
            anim = weepingScarecrow.GetAnimator();

            anim.SetBool("isRunning", true);
            if (agent.isStopped)
            {
                agent.isStopped = false;
            }

            weepingScarecrow.StartCoroutine(UpdateTargetPosition(weepingScarecrow, agent));
            playSoundCoroutine = weepingScarecrow.StartCoroutine(PlaySoundEffect(weepingScarecrow));
            SoundEffectManager.instance.PlayRandomSoundFxClip(weepingScarecrow.GetFlwSoundClips(), weepingScarecrow.transform, 1f);
            Debug.Log("Hi, I'm following");
        }
    }

    public override void UpdateState(WeepingScarecrowManager weepingScarecrow)
    {
        md = weepingScarecrow.GetMd();
        if (weepingScarecrow.GetTarget() != null)
        {
            if (md.GetShine() == true) 
            {
                //weepingScarecrow.StartCoroutine(SwitchStateDelay(weepingScarecrow));
                anim.SetBool("isShine", true);
                Debug.Log("switching to idle");
                weepingScarecrow.SwitchState(weepingScarecrow.idleState);
                
                
            }
        }
      
    }

    public override void ExitState(WeepingScarecrowManager weepingScarecrow)
    {
        if (playSoundCoroutine != null)
        {
            weepingScarecrow.StopCoroutine(playSoundCoroutine);
        }
        anim.SetBool("isRunning", false);
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
            float rand = Random.Range(2f, 4f);
            yield return new WaitForSeconds(rand);
            SoundEffectManager.instance.PlayRandomSoundFxClip(weepingScarecrow.GetFlwSoundClips(), weepingScarecrow.transform, 1f);
        }
    }
}
