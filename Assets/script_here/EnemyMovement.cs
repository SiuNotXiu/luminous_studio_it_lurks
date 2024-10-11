using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private enum EnemyState
    {
        Idle,
        Chasing,
        Attack
    }

    public float stuckThreshold = 1f;
    public float unstuckWaypointRadius = 3f;
    public float speed;
    public LayerMask worldMask;
    public float sightRange;
    public float waypointDistanceThreshold = 1f;
    public float waypointReachThreshold = 3f;
    public float waypointGizmoRadius = 1f;
    public CustomTrigger idleRangeTrigger;
    public CustomTrigger attackRangeTrigger;
    public float postAttackDelay = 3f;
    public float chaseTimer = 0;
    public float chaseTime = 10;
    public float speedBoostTimer = 0;
    public float speedBoostTime = 2;
    public TopdownMovement player;

    private Transform target;
    private List<Vector3> waypoints = new List<Vector3>();
    private Vector3 lastPosition;
    private float stuckTime = 0f;
    private EnemyState currentState = EnemyState.Idle;
    private bool speedBoosted = false;
    private float originalSpeed;
    private bool canMove = true; 
    private float timeSinceLastAttack = 0f; 

    void Start()
    {
        lastPosition = transform.position; 
    }

    private void Awake()
    {
        idleRangeTrigger.EnteredTrigger += OnIndleRangeTriggerEntered;
        attackRangeTrigger.EnteredTrigger += OnAttackRangeTriggerEntered;
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

            case EnemyState.Chasing:
                ChasingState();
                break;

            case EnemyState.Attack:
                AttackState();
                break;
        }
    }

    private void IdleState()
    {
        if (target != null && LineOfSight())
        {
            currentState = EnemyState.Chasing;
        }
    }

   /* private void ChasingState()
    {
       
        
        if (!canMove) return;

        chaseTimer += Time.deltaTime;
        if(chaseTimer > chaseTime)
        {
            speed = 7;
        }
        else
        {
            speed = 3;
        }

        if (target == null || !LineOfSight())
        {
            // Generate waypoints when the enemy loses sight of the player
            AddWaypoint();
            ChaseWaypoints();
            DetectStuck();
        }
        else
        {
            waypoints.Clear();
            // Move towards the player only if movement is allowed
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }

    }*/

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
                DetectStuck();
            }
        }
        else
        {
            waypoints.Clear();
            // Move towards the player only if movement is allowed
            if (canMove)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
        }
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
            currentState = EnemyState.Chasing;  
        }
    }

    private bool LineOfSight()
    {
        Vector2 directionToPlayer = target.position - transform.position;

        // Perform the raycast
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, sightRange, worldMask);

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
            transform.position = Vector2.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, currentWaypoint) < waypointReachThreshold)
            {
                waypoints.RemoveAt(0);
            }
        }
    }

    private void DetectStuck()
    {
        float minMovementThreshold = 0.2f; // Adjust this value to a reasonable threshold

        if (Vector3.Distance(transform.position, lastPosition) < minMovementThreshold)
        {
            stuckTime += Time.deltaTime;
            Debug.Log("Timer Starts");
            if (stuckTime > stuckThreshold)
            {
                InsertUnstuckWaypoint();
                Debug.Log("Timer Resets");
                stuckTime = 0f;
            }
        }
        else
        {
            stuckTime = 0f;
            // Update lastPosition only if the enemy has moved significantly
            if (Vector3.Distance(transform.position, lastPosition) > minMovementThreshold)
            {
                lastPosition = transform.position;
            }
        }
    }

    private void InsertUnstuckWaypoint()
    {
        
        Vector2 randomInCircle = Random.insideUnitCircle.normalized * unstuckWaypointRadius;
        Vector3 newWaypoint = new Vector3(randomInCircle.x, randomInCircle.y, 0) + transform.position;

        Collider2D overlap = Physics2D.OverlapCircle(newWaypoint, 0.5f, worldMask);
        if (overlap == null)
        {
            waypoints.Insert(0, newWaypoint);
            Debug.Log("Unstuck waypoint added.");
        }
        else
        {
            Debug.Log("Unstuck waypoint overlap");
            InsertUnstuckWaypoint();
        }
    }


    private void OnIndleRangeTriggerEntered(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = collision.transform;
            Debug.Log("It works");
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach (Vector3 waypoint in waypoints)
        {
            Gizmos.DrawSphere(waypoint, waypointGizmoRadius);
        }
    }
}
