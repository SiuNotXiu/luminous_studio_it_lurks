using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AI;

public class WeepingScarecrowIdleState : WeepingScarecrowBaseState
{
    private NavMeshAgent agent;
    private monster_database md;

    public override void EnterState(WeepingScarecrowManager weepingScarecrow)
    {
        agent = weepingScarecrow.GetAgent();
        if (agent.isStopped == false) 
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }

        //Debug.Log("Hi im idle");
        
    }

    public override void UpdateState(WeepingScarecrowManager weepingScarecrow)
    {
        md = weepingScarecrow.GetMd();
        if (md.GetShine()==false && weepingScarecrow.GetFlw() == true)  
        {
            weepingScarecrow.SwitchState(weepingScarecrow.followState);
        }

    }

    public override void ExitState(WeepingScarecrowManager weepingScarecrow)
    {

    }
}
