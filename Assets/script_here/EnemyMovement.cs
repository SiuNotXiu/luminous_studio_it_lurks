using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;

public class EnemyMovement : MonoBehaviour
{
    private enum EnemyState
    {
        Idle,
        Stalking,
        Chasing,
        Attack,
        Fleeing
    }

    #region<PublicVariables>
    public float speed;
    public LayerMask worldMask;
    public float sightRange;
    public float stalkingDistance = 6f;
    public float waypointDistanceThreshold = 1f;
    public float waypointReachThreshold = 3f;
    public float waypointGizmoRadius = 1f;
    public CustomTrigger idleRangeTrigger;
    public CustomTrigger attackRangeTrigger;
    public CustomTrigger chaseRangeTrigger;
    public float postAttackDelay = 3f;
    public float chaseTimer = 0f;
    public float chaseTime = 10f;
    public float speedBoostTimer = 0f;
    public float speedBoostTime = 2f;
    public float idleTimer = 0f;
    public float idleTime = 2f;
    public float fleeTime = 5f;
    public float fleeTimer = 0f;
    public TopdownMovement player;

    #endregion

    #region<PrivateVariables>
    private Transform target;
    private List<Vector3> waypoints = new List<Vector3>();
    private Vector3 lastPlayerPosition;
    private EnemyState currentState = EnemyState.Idle;
    private bool speedBoosted = false;
    private bool attack = false;
    private float originalSpeed;
    private bool canMove = true;
    private float timeSinceLastAttack = 0f;
    private NavMeshAgent agent;
    private bool inAtkArea = false;
    #endregion

    #region<Sfx>
    [SerializeField] private AudioClip[] growlAudio;
    [SerializeField] private AudioClip[] chaseAudio;
    [SerializeField] private AudioClip[] stalkAudio;
    [SerializeField] private AudioClip[] shineAudio;
    [SerializeField] private AudioClip[] fleeAudio;

    private bool isStalkAudioPlaying = false;
    private bool isChaseAudioPlaying = false;
    private bool isGrowlingAudioPlaying = false;
    private bool isFleeAudioPlaying = false;
    private bool isShineAudioPlaying = false;
    #endregion

    void Start()
    {
        lastPlayerPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;
        agent.speed = speed;
    }

    private void Awake()
    {
        idleRangeTrigger.EnteredTrigger += OnIndleRangeTriggerEntered;
       // idleRangeTrigger.ExitedTrigger += OnIdleRangeTriggerExited;

        attackRangeTrigger.EnteredTrigger += OnAttackRangeTriggerEntered;
        attackRangeTrigger.ExitedTrigger += OnAttackRangeTriggerExited;
        chaseRangeTrigger.EnteredTrigger += OnChaseRangeTriggerEntered;
    }

    void Update()
    {

        if (!canMove)
        {
            AddWaypoint();
            timeSinceLastAttack += Time.deltaTime;
            if (timeSinceLastAttack >= postAttackDelay)
            {
                canMove = true;

                timeSinceLastAttack = 0f;
            }
        }

        switch (currentState)
        {
            case EnemyState.Idle:
                IdleState();
                break;

            case EnemyState.Stalking:
                StalkingState();
                break;

            case EnemyState.Chasing:
                ChasingState();
                break;

            case EnemyState.Attack:
                AttackState();
                break;

            case EnemyState.Fleeing:
                FleeingState();
                break;
        }

        if(inAtkArea)
        {
            currentState = EnemyState.Attack;
        }
    }

    private void IdleState()
    {
        if (target != null && LineOfSight())
        {
            currentState = EnemyState.Stalking;
        }


    }

    private void StalkingState()
    {
        if (agent.isStopped)
        {
            agent.isStopped = false;
        }
        speed = 3;
        agent.speed = speed;
        float distanceToPlayer = Vector2.Distance(transform.position, target.position);
        float closeEnoughThreshold = 0.1f;

        // If the enemy is too close to the player, move away (flee)
        if (distanceToPlayer < stalkingDistance - closeEnoughThreshold)
        {
            waypoints.Clear();

            // Calculate the direction away from the player
            Vector2 fleeDirection = (transform.position - target.position).normalized;

            // Set a destination in the flee direction
            Vector3 fleePosition = transform.position + (Vector3)fleeDirection * stalkingDistance;

            // Use NavMeshAgent to flee away from the player
            agent.SetDestination(fleePosition);
        }
        // If the enemy is too far from the player, move closer
        else if (distanceToPlayer > stalkingDistance + closeEnoughThreshold)
        {
            if (target == null || !LineOfSight())
            {
                // Generate waypoints when the enemy loses sight of the player
                AddWaypoint();

                // Continue chasing the waypoints
                if (canMove)
                {
                    ChaseWaypoints();
                }
            }
            else
            {
                waypoints.Clear();

                
                if (canMove)
                {
                    agent.SetDestination(target.position);
                }
            }
        }
        
        else if (Mathf.Abs(distanceToPlayer - stalkingDistance) <= closeEnoughThreshold)
        {
            
            agent.SetDestination(transform.position); 
        }

        
        Vector3 currentPlayerPosition = target.transform.position;

        if (currentPlayerPosition == lastPlayerPosition) 
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= idleTime)
            {
                currentState = EnemyState.Chasing; 
                Debug.Log("Player idle for too long. Switching to Chasing state.");
            }
        }
        else 
        {
            idleTimer = 0; 
        }

        lastPlayerPosition = currentPlayerPosition; // Update last known player position
        FleeingChecks();

        if (!isStalkAudioPlaying)
        {
            StartCoroutine(PlayStalkingSound());
            isStalkAudioPlaying = true;
        }

        if(!isGrowlingAudioPlaying)
        {
            StartCoroutine(PlayGrowlSound());
            isGrowlingAudioPlaying = true;
        }
     
    }

    private void ChasingState()
    {
        if (agent.isStopped)
        {
            agent.isStopped = false;
        }
        // Update the chase timer for movement speed changes
        chaseTimer += Time.deltaTime;
        if (chaseTimer > chaseTime)
        {
            speed = 7;
        }
        else
        {
            speed = 3;
        }
        agent.speed = speed;

        // Check for line of sight; if lost, create waypoints even if stunned
        if (target == null || !LineOfSight())
        {
            // Generate waypoints when the enemy loses sight of the player
            AddWaypoint();

            // The enemy will continue chasing waypoints even if it cannot move
            if (canMove)
            {
                AddWaypoint();
                ChaseWaypoints();
            }
        }
        else
        {
            waypoints.Clear();
            // Move towards the player only if movement is allowed
            if (canMove)
            {
                agent.SetDestination(target.position);
            }
        }

        if (!isChaseAudioPlaying)
        {
            StartCoroutine(PlayChaseSound());
            isChaseAudioPlaying = true;
        }

        FleeingChecks();
    }

    private void AttackState()
    {
        
        if (!attack)  
        {
            attack = true;
            gameObject.GetComponent<EnemyAttack>().Attack();
        }
        

        if (!speedBoosted)
        {
            originalSpeed = player.GetMoveSpeed();
            player.ChangeSpeed(originalSpeed + 8);
            speedBoosted = true;
        }

        agent.isStopped = true;
        chaseTimer = 0;
        speedBoostTimer += Time.deltaTime;


        if (speedBoostTimer >= 2)
        {
            player.ChangeSpeed(originalSpeed);
            speedBoostTimer = 0;
            speedBoosted = false;
            currentState = EnemyState.Stalking;
            attack = false;
        }

        FleeingChecks();
    }

    private void FleeingState()
    {

        if (target != null)
        {
            agent.SetDestination(transform.position);
            if(gameObject.GetComponent<monster_database>().GetFlee()==true)
            {
                speed = 7;
                agent.speed = speed;
                // Calculate the direction away from the player
                Vector2 fleeDirection = (transform.position - target.position).normalized;

                // Set a destination in the flee direction
                Vector3 fleePosition = transform.position + (Vector3)fleeDirection;

                // Use NavMeshAgent to flee away from the player
                agent.SetDestination(fleePosition);
                fleeTimer += Time.deltaTime;
                if (fleeTimer >= fleeTime)
                {
                    currentState = EnemyState.Stalking;
                    fleeTimer = 0f;
                    gameObject.GetComponent<monster_database>().SetFlee(false);
                    gameObject.GetComponent<monster_database>().SetFlashed(false);
                    isFleeAudioPlaying = false;

                }

                if (!isFleeAudioPlaying)
                {
                    SoundEffectManager.instance.PlayRandomSoundFxClip(fleeAudio, transform, 1f);
                    isFleeAudioPlaying = true;
                }

            }

        }
    }

    #region<LineOfSightFunctions>
    private bool LineOfSight()
    {

        if (target == null)
        {
           // Debug.Log("raycast not working");
            return false;
        }
        else
        {
            //Debug.Log("raycast is working");
        }

        // Get the center of the BoxCollider2D
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        Vector2 startPosition = collider.bounds.center;

        Vector2 directionToPlayer = target.position - (Vector3)startPosition;

        // Perform the raycast
        RaycastHit2D hit = Physics2D.Raycast(startPosition, directionToPlayer, sightRange, worldMask);

        // Check if the ray hits the player
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            Debug.DrawRay(startPosition, directionToPlayer.normalized * sightRange, Color.green);
            return true;
        }
        else
        {
            Debug.DrawRay(startPosition, directionToPlayer.normalized * sightRange, Color.red);
            return false;
        }
    }

    private void AddWaypoint()
    {
        if (waypoints.Count == 0 || Vector3.Distance(waypoints[waypoints.Count - 1], target.position) > waypointDistanceThreshold)
        {
            waypoints.Add(target.position); // Add player's last known position as a waypoint
        }
    }

    private void ChaseWaypoints()
    {
        if (waypoints.Count > 0)
        {
            Vector3 currentWaypoint = waypoints[0];

            // Move towards the current waypoint only if movement is allowed
            agent.SetDestination(currentWaypoint);

            if (Vector3.Distance(transform.position, currentWaypoint) < waypointReachThreshold)
            {
                waypoints.RemoveAt(0);
            }

        }
    }

    #endregion

    private void FleeingChecks()
    {
        if (gameObject.GetComponent<monster_database>().GetFlashed() == true)
        {
            currentState = EnemyState.Fleeing;
        }
    }

    private void FlashChecks()
    {
        if (gameObject.GetComponent<monster_database>().GetShine() == true)
        {
            if (!isShineAudioPlaying)
            {
                SoundEffectManager.instance.PlayRandomSoundFxClip(shineAudio, transform, 1f);
                isShineAudioPlaying = true;
            }
            else
            {
                isShineAudioPlaying = false;
            }
        }
       
    }

    #region<OntriggerEntered/Exit>

    private void OnIndleRangeTriggerEntered(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = collision.transform;
            

            if (currentState == EnemyState.Idle)
            {
                currentState = EnemyState.Stalking;
                Debug.Log("stalking");
            }
        }
    }

    private void OnAttackRangeTriggerEntered(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Attacking");
            currentState = EnemyState.Attack;
            inAtkArea = true;
            Debug.Log("AttackArea" + inAtkArea);
        }
    }

    private void OnAttackRangeTriggerExited(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {   
            inAtkArea = false;
            Debug.Log("AttackArea" + inAtkArea);
        }
    }

    private void OnChaseRangeTriggerEntered(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Chasing");
            currentState = EnemyState.Chasing;
        }
    }

    #endregion

    #region<Coroutine>
    private IEnumerator PlayStalkingSound()
    {
        float rand = Random.Range(1f, 2f);
        SoundEffectManager.instance.PlayRandomSoundFxClip(stalkAudio, transform, 1f);
        yield return new WaitForSeconds(rand);
        isStalkAudioPlaying = false;
        
       
    }

    private IEnumerator PlayChaseSound()
    {
        float rand = Random.Range(0.3f, 0.5f);
        SoundEffectManager.instance.PlayRandomSoundFxClip(chaseAudio, transform, 1f);
        yield return new WaitForSeconds(rand);
        isChaseAudioPlaying = false;
    }

    private IEnumerator PlayGrowlSound()
    {
        float rand = Random.Range(7f, 9f);
        SoundEffectManager.instance.PlayRandomSoundFxClip(growlAudio, transform, 1f);
        yield return new WaitForSeconds(rand);
        isGrowlingAudioPlaying = false;
    }



    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach (Vector3 waypoint in waypoints)
        {
            Gizmos.DrawSphere(waypoint, waypointGizmoRadius);
        }

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, stalkingDistance);
    }
}
