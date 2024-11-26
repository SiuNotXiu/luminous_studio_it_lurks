using System.Collections;
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
    [SerializeField] private bool shine;
    [SerializeField] private bool flahsed;
    private Transform target;
    private NavMeshAgent agent;
    private float speed = 6f;
    [SerializeField] private float timer = 0f;
    private float time = 5f;
    public bool flw = false;
    private bool inAtkArea = false;
    private Animator anim;
    private Animator anim_monochrome;
    private monster_database md;
    public bool soundPlayed = false;
    [SerializeField]private TopdownMovement playerMovement;
    private SpriteRenderer sr;
    private Vector2 previousPosition;

    #endregion

    #region<SFX>
    [SerializeField] private AudioClip[] enterSoundClips;
    [SerializeField] private AudioClip flwSoundClips;
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
        

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = speed;
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        anim = GetComponent<Animator>();
        anim_monochrome = transform.Find("monochrome").GetComponent<Animator>();
        md = GetComponent<monster_database>();
        sr = GetComponent<SpriteRenderer>();

        
    }

    private void Update()
    {
        shine = md.GetShine();
        FlipChecks();
        currentState.UpdateState(this);
      
    }

    public void SwitchState(WeepingScarecrowBaseState state)
    {
        currentState.ExitState(this);
        currentState = state;
        state.EnterState(this);
    }

    private void FlipChecks()
    {

        float currentPosX = transform.position.x;
        float previousPosX = previousPosition.x;


        if (currentPosX > previousPosX)
        {
            sr.flipX = true;
        }
        else if (currentPosX < previousPosX)
        {
            sr.flipX = false;
        }


        previousPosition = transform.position;
    }
    #endregion

    #region<OnTriggerEnter/Exits>
    private void OnIdleTriggerEntered(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = collision.transform;
            playerMovement.ConfieldSpeed();
            
        }
    }

    private void OnIdleTriggerExited(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = null;
            flw = false;
            soundPlayed = false;
            Debug.Log("istriggerexit");
            SwitchState(idleState);
            playerMovement.OriginalSpeed();

            Debug.Log("isActivate" + anim.GetBool("isActivate"));
            if (anim.GetBool("isActivate") == true)
            {
                Debug.Log("idleaniation");
                anim.SetBool("isActivate", false);      anim_monochrome.SetBool("isActivate", false);
                anim.SetBool("isDeactivate", true);     anim_monochrome.SetBool("isDeactivate", true);
                StartCoroutine(Deactivate());
            }
           

        }
    }

    private void OnAtkTriggerEntered(Collider2D collision)
    {
        if (collision.CompareTag("Player") && target != null && anim.GetBool("isActivate") == true) 
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

    public monster_database GetMd()
    {
        return md;
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

    public AudioClip GetFlwSoundClips()
    {
        return flwSoundClips;
    }

    public AudioClip[] GetAtkSoundClips()
    {
        return atkSoundClips;
    }

    public AudioClip[] GetEnterSoundClips()
    {
        return enterSoundClips;
    }
    #endregion

    #region<Coroutine>
   

    private IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("isDeactivate", false);     anim_monochrome.SetBool("isDeactivate", false);
    }

    #endregion

    private void OnDrawGizmos()
    {
        BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
        Vector2 centerPosition = collider.bounds.center;
        // Draw the follow trigger area as a circle in the scene view
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(centerPosition, 8f);
    }

   public float Volume()
    {
        float volumeControl;
        if (Audio.Instance != null)
        {
            volumeControl = Audio.Instance.SFXSource.volume;

        }
        else
        {
            volumeControl = 1f;
        }
        return volumeControl;
    }
}
