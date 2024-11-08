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

    #endregion

    #region<Variables>
    /*[SerializeField] private Sprite atkSprite;
    [SerializeField] private Sprite shineSprite;
    [SerializeField] private Sprite idleSprite;
    private SpriteRenderer sr;*/

    [SerializeField] private bool shine = false;
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
    private void Awake()
    {
        atkTrigger.EnteredTrigger += OnAtkTriggerEntered;
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
            SwitchState(atkState);
        }
    }

    #region<Setter/Getter>
    public bool GetShine()
    {
        return shine;
    }

   /* public Sprite GetAtkSprite()
    {
        return atkSprite;
    }

    public Sprite GetShineSprite()
    {
        return shineSprite;
    }

    public Sprite GetIdleSprite()
    {
        return idleSprite;
    }*/

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
