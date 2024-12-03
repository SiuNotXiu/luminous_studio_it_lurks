using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class WeepingScarecrowIdleState : WeepingScarecrowBaseState
{
    private NavMeshAgent agent;
    private monster_database md;
    private float followTriggerRadius = 8f;
    
    private Animator anim;
    private Animator anim_monochrome;
    


    public override void EnterState(WeepingScarecrowManager weepingScarecrow)
    {
        agent = weepingScarecrow.GetAgent();
        anim = weepingScarecrow.GetAnimator();
        anim_monochrome = weepingScarecrow.GetAnimatorMonochrome();
        if (agent.isStopped == false) 
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }

        Debug.Log("Hi im idle");
        
    }

    public override void UpdateState(WeepingScarecrowManager weepingScarecrow)
    {
        BoxCollider2D collider =weepingScarecrow.GetComponent<BoxCollider2D>();
        Vector2 centerPosition = collider.bounds.center;

        if (weepingScarecrow.GetTarget() != null && Vector2.Distance(centerPosition, weepingScarecrow.GetTarget().position) <= followTriggerRadius && weepingScarecrow.flw == false)   
        {
            anim.SetBool("isActivate", true); anim_monochrome.SetBool("isActivate", true);
            if (weepingScarecrow.soundPlayed == false)
            {
                SoundEffectManager.instance.PlayRandomSoundFxClip(weepingScarecrow.GetEnterSoundClips(), weepingScarecrow.transform, weepingScarecrow.Volume());
                weepingScarecrow.soundPlayed = true;
            }
            weepingScarecrow.StartCoroutine(FollowStateDelay(weepingScarecrow));
            
        }

        md = weepingScarecrow.GetMd();
        // Debug.Log("Hi shine is:" + md.GetShine());
        if (weepingScarecrow.gameObject.GetComponent<monster_database>().canStop == false && weepingScarecrow.flw == true)   
        {
            Debug.Log("Hi shine is:" + md.GetShine());
            Debug.Log("hi switching to follow");
            weepingScarecrow.SwitchState(weepingScarecrow.followState);

        }



    }

    public override void ExitState(WeepingScarecrowManager weepingScarecrow)
    {

    }

    private IEnumerator FollowStateDelay(WeepingScarecrowManager weepingScarecrow)
    {
        yield return new WaitForSeconds(4f);
        weepingScarecrow.SwitchState(weepingScarecrow.followState);
        weepingScarecrow.flw = true;


    }
}
