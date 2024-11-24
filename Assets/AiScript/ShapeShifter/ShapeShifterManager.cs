using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeShifterManager : MonoBehaviour
{
    #region<StateMachine>
    public ShapeShifterBaseState currentState;
    public ShapeShifterIdleState idleState = new ShapeShifterIdleState();
    public ShapeShifterAtkState atkState = new ShapeShifterAtkState();
    #endregion

    #region<CustomTrigger>
    public CustomTrigger atkTrigger;

    private void Awake()
    {
        atkTrigger.EnteredTrigger += OnAtkTriggerEntered;
        atkTrigger.ExitedTrigger += OnAtkTriggerExited;
    }

    #endregion

    #region<Variables>
    [SerializeField] private bool shine = false;
    public bool inAtkArea { get; private set; } = false;
    public Transform target { get; private set; }
    public Rigidbody2D rb { get; private set; }

    public Animator anim;
   
    #endregion

    #region<SFX>
    [SerializeField] private AudioClip[] shineAudio;
    [SerializeField] private AudioClip[] atkAudio;

    #endregion

    #region<MainFunctions>
    private void Start()
    {
        currentState = idleState;
        currentState.EnterState(this);
    }


    private void Update()
    {
        
        currentState.UpdateState(this);
    }

    public void SwitchState(ShapeShifterBaseState state)
    {
        currentState.ExitState(this);
        currentState = state;
        state.EnterState(this);
    }

    

    #endregion

    private void OnAtkTriggerEntered(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            inAtkArea = true;
            target = collision.transform;
            rb = collision.GetComponent<Rigidbody2D>();
            Debug.Log("Assigned Rigidbody2D: " + rb);
            SwitchState(atkState);
            
            
        }
    }

    private void OnAtkTriggerExited(Collider2D collision)
    {
        inAtkArea = false;
        target = null;
        
    }


    #region<Setter/Getter>
    public bool GetShine()
    {
        return shine;
    }

    public AudioClip[] GetAtkAudio()
    {
        return atkAudio;
    }

    public AudioClip[] GetShineAudio()
    {
        return shineAudio;
    }

    #endregion

}
