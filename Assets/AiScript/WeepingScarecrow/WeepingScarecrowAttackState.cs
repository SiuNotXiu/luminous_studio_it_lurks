using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class WeepingScarecrowAttackState : WeepingScarecrowBaseState
{
    private EnemyAttack atk;
    private bool atkAnim = false;
    private bool attacking = false;
    private NavMeshAgent agent;
    private Animator anim;

    public override void EnterState(WeepingScarecrowManager weepingScarecrow)
    {
        Debug.Log("InAtkArea" + weepingScarecrow.GetInAtkArea());
        atk = weepingScarecrow.GetComponent<EnemyAttack>();
        anim = weepingScarecrow.GetAnimator();
        agent = weepingScarecrow.GetAgent();
        if (agent.isStopped == false) 
        {
           
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
           

        }
  
        Debug.Log("Hi im Atk State");
    }

    public override void UpdateState(WeepingScarecrowManager weepingScarecrow)
    {

        if (weepingScarecrow.GetInAtkArea() == true && attacking == false) 
        {
            atk.Attack();
            attacking = true;

            Debug.Log("before atk animation" + anim.GetBool("isAtking"));
            anim.SetBool("isAtking", true);
            weepingScarecrow.StartCoroutine(ResetAttackAnimationCoroutine());
            anim.SetBool("isRunning", false);

           
            weepingScarecrow.StartCoroutine(AttackDelay());
        }
        else
        {
            weepingScarecrow.StopCoroutine(AttackDelay());
        }

        if (weepingScarecrow.GetInAtkArea() == false)
        {
            weepingScarecrow.StartCoroutine(SwitchStateDelay(weepingScarecrow));
        }

    }

    public override void ExitState(WeepingScarecrowManager weepingScarecrow)
    {
        weepingScarecrow.StopAllCoroutines();
        attacking = false;
    }

    IEnumerator SwitchStateDelay(WeepingScarecrowManager weepingScarecrow)
    {
        //Debug.Log("Coroutine Started");
        yield return new WaitForSeconds(3f);
        weepingScarecrow.SwitchState(weepingScarecrow.followState);
        weepingScarecrow.SetInAtkArea(false);
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(3f);
        attacking = false;
    }

    private IEnumerator ResetAttackAnimationCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("isAtking", false);
        Debug.Log("should be false" + anim.GetBool("isAtking"));
    }

}
