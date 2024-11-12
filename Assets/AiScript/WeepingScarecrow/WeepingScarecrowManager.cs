using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
    public CustomTrigger followTrigger;

    #endregion

    #region<Variables>
    [SerializeField] private bool flahsed;
    private Transform target;
    private NavMeshAgent agent;
    private float speed = 6f;
    [SerializeField] private float timer = 0f;
    private float time = 5f;
    private bool flw = false;
    private bool inAtkArea = false;
    private Animator anim;

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
        idleState.EnterState(this);
    }

    private void Awake()
    {
        idleTrigger.EnteredTrigger += OnIdleTriggerEntered;
        idleTrigger.ExitedTrigger += OnIdleTriggerExited;
        attackTrigger.EnteredTrigger += OnAtkTriggerEntered;
        attackTrigger.ExitedTrigger += OnAtkTriggerExited;
        followTrigger.EnteredTrigger += OnFollowTriggerEntered;

        agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = speed;
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        anim = gameObject.GetComponent<Animator>();
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
            
        }
    }

    private void OnIdleTriggerExited(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = null;
            flw = false;
            SwitchState(idleState);
            
        }
    }

    private void OnAtkTriggerEntered(Collider2D collision)
    {
        if (collision.CompareTag("Player") && target != null)
        {
            inAtkArea = true;
            SwitchState(attackState);
            
        }
    }

    private void OnAtkTriggerExited(Collider2D collision)
    {
        if (collision.CompareTag("Player") && target != null)
        { 
            inAtkArea = false;
            
        }
    }

    private void OnFollowTriggerEntered(Collider2D collision)
    {
        if (flw == false && target != null) 
        {
            anim.SetBool("isActivate", true);
            SoundEffectManager.instance.PlayRandomSoundFxClip(enterSoundClips, transform, 1f);
            StartCoroutine(FollowStateDelay());
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

    public bool GetFlw()
    {
        return flw;
    }

    public bool GetInAtkArea()
    {
        return inAtkArea;
    }

    public void SetInAtkArea(bool _pInAtkArea)
    {
        inAtkArea = _pInAtkArea;
    }

    public Animator GetAnimator()
    {
        return anim;
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

    private IEnumerator FollowStateDelay()
    {
        yield return new WaitForSeconds(4f);
        SwitchState(followState);
        flw = true;
    }
}
