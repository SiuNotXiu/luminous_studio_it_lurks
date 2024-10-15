using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

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
    public float chaseTimer = 0;
    public float chaseTime = 10;
    public float speedBoostTimer = 0;
    public float speedBoostTime = 2;
    public float idleTimer = 0;
    public float idleTime = 2;
    public TopdownMovement player;


    private Transform target;
    private List<Vector3> waypoints = new List<Vector3>();
    private Vector3 lastPlayerPosition;
    private EnemyState currentState = EnemyState.Idle;
    private bool speedBoosted = false;
    private float originalSpeed;
    private bool canMove = true;
    private float timeSinceLastAttack = 0f;
    private NavMeshAgent agent;

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
        idleRangeTrigger.ExitedTrigger += OnIdleRangeTriggerExited;

        attackRangeTrigger.EnteredTrigger += OnAttackRangeTriggerEntered;
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
    }

    private void ChasingState()
    {
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

        FleeingChecks();
    }

    private void AttackState()
    {

        if (!speedBoosted)
        {
            originalSpeed = player.GetMoveSpeed();
            player.ChangeSpeed(originalSpeed + 8);
            speedBoosted = true;
        }

        canMove = false;
        chaseTimer = 0;
        speedBoostTimer += Time.deltaTime;


        if (speedBoostTimer >= 2)
        {
            player.ChangeSpeed(originalSpeed);
            speedBoostTimer = 0;
            speedBoosted = false;
            currentState = EnemyState.Stalking;
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

            }

        }
    }

    private bool LineOfSight()
    {

        if (target == null)
        {
            Debug.Log("raycast not working");
            return false;
        }
        else
        {
            Debug.Log("raycast is working");
        }
        Vector2 directionToPlayer = target.position - transform.position;

        // Perform the raycast
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, sightRange,worldMask);

        // Check if the ray hits the player
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {

            Debug.DrawRay(transform.position, directionToPlayer.normalized * sightRange, Color.green);
            return true;
        }
        else
        {

            Debug.DrawRay(transform.position, directionToPlayer.normalized * sightRange, Color.red);
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

    private void FleeingChecks()
    {
        if (gameObject.GetComponent<monster_database>().GetFlashed() == true)
        {
            currentState = EnemyState.Fleeing;
        }
    }

    private void OnIndleRangeTriggerEntered(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = collision.transform;
            Debug.Log("It works");

            if (currentState == EnemyState.Idle)
            {
                currentState = EnemyState.Stalking;
            }
        }
    }

    private void OnAttackRangeTriggerEntered(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Attacking");
            currentState = EnemyState.Attack;
        }
    }

    private void OnChaseRangeTriggerEntered(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Attacking");
            currentState = EnemyState.Chasing;
        }
    }

    private void OnIdleRangeTriggerExited(Collider2D collision)
    {
        Debug.Log("OnIdleRangeTriggerExited called");

        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player exited the idle range");

            if (currentState == EnemyState.Fleeing && target != null)
            {
                gameObject.GetComponent<monster_database>().SetFlee(false);
                gameObject.GetComponent<monster_database>().SetFlashed(false);
                currentState = EnemyState.Stalking;
            }

            Debug.Log("Player exited idle range trigger, current state: " + currentState);
        }
    }


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
