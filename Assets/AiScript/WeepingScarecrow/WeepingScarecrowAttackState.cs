using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WeepingScarecrowAttackState : WeepingScarecrowBaseState
{
    private EnemyAttack atk;
    private NavMeshAgent agent;
    public override void EnterState(WeepingScarecrowManager weepingScarecrow)
    {
        agent = weepingScarecrow.GetAgent();
        if (agent.isStopped == false) 
        {
            atk = weepingScarecrow.GetComponent<EnemyAttack>();
            atk.Attack();
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            weepingScarecrow.StartCoroutine(AttackDelay(weepingScarecrow));
        }
      
    }

    public override void UpdateState(WeepingScarecrowManager weepingScarecrow)
    {
                
    }

    public override void ExitState(WeepingScarecrowManager weepingScarecrow)
    {
        weepingScarecrow.StopAllCoroutines();
    }

    IEnumerator AttackDelay(WeepingScarecrowManager weepingScarecrow)
    {
        yield return new WaitForSeconds(2f);
        weepingScarecrow.SwitchState(weepingScarecrow.followState);
    }
}
