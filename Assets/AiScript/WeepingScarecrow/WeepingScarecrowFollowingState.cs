using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class WeepingScarecrowFollowingState : WeepingScarecrowBaseState
{
    private NavMeshAgent agent;
    private Animator anim;
    private monster_database md;
    private bool sfx = false;

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

            // Start moving toward the target and updating position
            weepingScarecrow.StartCoroutine(UpdateTargetPosition(weepingScarecrow, agent));

            // Play the persistent sound effect
            if (sfx == false)
            {
                SoundEffectManager.instance.PlayAndTrackSound(weepingScarecrow.GetFlwSoundClips(), weepingScarecrow.transform, 1f);
                sfx = true;
            }
            else
            {
                SoundEffectManager.instance.ResumePersistentSound();
            }
            

            //Debug.Log("Hi, I'm following");
        }
    }

    public override void UpdateState(WeepingScarecrowManager weepingScarecrow)
    {
        md = weepingScarecrow.GetMd();
        if (weepingScarecrow.GetTarget() != null)
        {
            if (weepingScarecrow.gameObject.GetComponent<monster_database>().canStop == true)
            {
                anim.SetBool("isShine", true);
                Debug.Log("Switching to idle");
                weepingScarecrow.SwitchState(weepingScarecrow.idleState);
            }
            else
            {
                // Update the persistent sound position to follow the scarecrow
                SoundEffectManager.instance.UpdatePersistentSoundPosition(weepingScarecrow.transform);
            }
        }
    }

    public override void ExitState(WeepingScarecrowManager weepingScarecrow)
    {
        // Stop the persistent sound when exiting the state
        SoundEffectManager.instance.PausePersistentSound();
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
}
