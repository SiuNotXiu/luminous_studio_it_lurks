using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WeepingScarecrowManager : MonoBehaviour
{
    #region<StateMachine>
    public WeepingScarecrowBaseState currentState;
    public WeepingScarecrowFollowingState followState = new WeepingScarecrowFollowingState();
    public WeepingScarecrowIdleState idleState = new WeepingScarecrowIdleState();
    public WeepingScarecrowAttackState attackState = new WeepingScarecrowAttackState();
    #endregion

    #region<CustomTrigger>
    public CustomTrigger idleTrigger;
    public CustomTrigger attackTrigger;
    #endregion

    #region<Variables>
    [SerializeField] private bool flahsed;
    private Transform target;
    private NavMeshAgent agent;
    private float speed = 6f;
    private float timer = 0f;
    private float time = 5f;

    #endregion

    #region<SFX>
    [SerializeField] private AudioClip[] enterSoundClips;
    [SerializeField] private AudioClip[] flwSoundClips;
    [SerializeField] private AudioClip[] atkSoundClips;
    #endregion

    #region<MainFunction>
    private void Start()
    {
        currentState = idleState;
        currentState.EnterState(this);
    }

    private void Awake()
    {
        idleTrigger.EnteredTrigger += OnIdleTriggerEntered;
        idleTrigger.ExitedTrigger += OnIdleTriggerExited;
        agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = speed;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(WeepingScarecrowBaseState state)
    {
        currentState.ExitState(this);
        currentState = state;
        state.EnterState(this);
    }
    #endregion

    #region<OnTriggerEnter/Exits>
    private void OnIdleTriggerEntered(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = collision.transform;
            SoundEffectManager.instance.PlayRandomSoundFxClip(enterSoundClips, transform, 1f);
            timer += Time.deltaTime;
            if (timer > time) 
            {
                SwitchState(followState);
                timer = 0;
            }
            
                    
        }
    }

    private void OnIdleTriggerExited(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = null;
            SwitchState(idleState);
            Debug.Log("thiswork");
        }
    }

    private void OnAttackTriggerEntered(Collider2D collision)
    {
        if (collision.CompareTag("Player") && target != null)
        {
            SwitchState(attackState);
        }
    }
    #endregion

    #region<Setters and Getters>
    public Transform GetTarget()
    {
        return target;
    }

    public NavMeshAgent GetAgent()
    {
        return agent;
    }

    public bool GetFlashed()
    {
        return flahsed;
    }

    public AudioClip[] GetFlwSoundClips()
    {
        return flwSoundClips;
    }

    public AudioClip[] GetAtkSoundClips()
    {
        return atkSoundClips;
    }
    #endregion
}
