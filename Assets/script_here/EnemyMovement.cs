using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

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
    private SpriteRenderer sr;
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
    private Animator anim;
    private Animator anim_monochrome;
    private Vector2 previousPosition;
    [SerializeField] private float cornfieldRange;
    [SerializeField] LayerMask cornfieldMask;
    private bool cornfield = false;
    private bool cornfieldRangeCheck = false;
    [SerializeField]private CustomTrigger cornfieldTrigger;
    [SerializeField] private float verticalThreshold = 2f; // Adjust as per your gameplay needs
    private float attackRange = 4.5f;
    private bool playerDmg = false;
    private SpriteRenderer sr_monochrome;
    private player_flashlight_on_off flashlight;
    private bool shineAnim = false;
    private Vector3 fleePosition;
    private bool hasFleePosition = false;
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
        anim = GetComponent<Animator>();
        anim_monochrome = transform.Find("monochrome").GetComponent<Animator>();
        sr_monochrome = transform.Find("monochrome").GetComponent<SpriteRenderer>();
        flashlight = GameObject.Find("player_dont_change_name").GetComponent<player_flashlight_on_off>();

        agent = GetComponent<NavMeshAgent>();
        sr = GetComponent<SpriteRenderer>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;
        agent.speed = speed;
        previousPosition = transform.position;



    }

    private void Awake()
    {
        idleRangeTrigger.EnteredTrigger += OnIndleRangeTriggerEntered;
       // idleRangeTrigger.ExitedTrigger += OnIdleRangeTriggerExited;

        attackRangeTrigger.EnteredTrigger += OnAttackRangeTriggerEntered;
        attackRangeTrigger.ExitedTrigger += OnAttackRangeTriggerExited;
        cornfieldTrigger.EnteredTrigger += OnCornFieldTriggerEntered;
        cornfieldTrigger.ExitedTrigger += OnCornFieldTriggerExited;
        chaseRangeTrigger.EnteredTrigger += OnChaseRangeTriggerEntered;
      
    }

    void Update()
    {
        FlipChecks();
        FlashChecks();

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

        if (cornfieldRangeCheck)
        {
            CornfieldRangeCheck();
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
                if (player_database.in_safe_zone == false)
                    ChasingState();
                else
                    StalkingState();
                break;

            case EnemyState.Attack:
                if (player_database.in_safe_zone == false)
                    AttackState();
                else
                    StalkingState();
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
        if (agent.isStopped == false)
        {
            agent.isStopped = true;
        }

        if (target != null && cornfield == false) 
        {
            currentState = EnemyState.Stalking;
        }

    }

    private void StalkingState()
    {
        shineAnim = false;
        anim.SetBool("isWalking", true);    anim_monochrome.SetBool("isWalking", true);
        anim.SetBool("isRunning", false);   anim_monochrome.SetBool("isRunning", false);
        if (anim.GetBool("noShine") == false)
        {
            anim.SetBool("noShine", true); anim_monochrome.SetBool("noShine", true);
        }
        if (agent.isStopped)
        {
            agent.isStopped = false;
        }
        speed = 3;
        agent.speed = speed;
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        Vector2 centerPosition = collider.bounds.center;

        float distanceToPlayer = Vector2.Distance(centerPosition, target.position);
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
        shineAnim = false;
        if (gameObject.GetComponent<monster_database>().GetShine() == false)
        {
            anim.SetBool("isRunning", true); anim_monochrome.SetBool("isRunning", true);
            anim.SetBool("isWalking", false); anim_monochrome.SetBool("isWalking", false);
            if (anim.GetBool("noShine") == false)
            {
                anim.SetBool("noShine", true); anim_monochrome.SetBool("noShine", true);
            }
            
        }
   
        if (agent.isStopped)
        {
            agent.isStopped = false;
        }
        // Update the chase timer for movement speed changes
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        if (distanceToPlayer < 5f) // Close range, move slower
        {
            speed = 9f;
        }
        else if (distanceToPlayer >= 5f && distanceToPlayer <= 20f) // Mid range, normal speed
        {
            speed = 13f;
        }
        else if (distanceToPlayer > 20f) // Far range, move faster
        {
            speed = 20f;
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
        shineAnim = false;
        if (agent.isStopped == false)
        {
            agent.isStopped = true;
        }
        if (target.position.x > transform.position.x)
        {
            sr.flipX = true; sr_monochrome.flipX = true;
        }
        else
        {
            sr.flipX = false; sr_monochrome.flipX = false;
        }
        anim.SetBool("isWalking", false);   anim_monochrome.SetBool("isWalking", false);
        anim.SetBool("isRunning", false);   anim_monochrome.SetBool("isRunning", false);
        anim.SetBool("isAtking", false); anim_monochrome.SetBool("isAtking", false);
        anim.SetBool("isShine", false);  anim_monochrome.SetBool("isShine", false);



        if (!attack)  
        {
            attack = true;
            anim.SetBool("isAtking", true); anim_monochrome.SetBool("isAtking", true);



            StartCoroutine(EndAttackAfterAnimation());

        }

        if (playerDmg == true)
        {
            if (!speedBoosted)
            {
                /// originalSpeed = player.GetMoveSpeed();
                //player.SpeedBoost();
                speedBoosted = true;
            }
        }
      

        agent.isStopped = true;
        chaseTimer = 0;
        speedBoostTimer += Time.deltaTime;


        if (speedBoostTimer >= 2)
        {
            //player.OriginalSpeed();
            speedBoostTimer = 0;
            speedBoosted = false;
            currentState = EnemyState.Stalking;
            attack = false;
            
            
        }

        FleeingChecks();
    }

    private void FleeingState()
    {
        Debug.Log("fleengstat");
        if (target != null)
        {
            if (anim.GetBool("isRunning") == false)
            {
                anim.SetBool("isRunning", true); anim_monochrome.SetBool("isRunning", true);
                if (anim.GetBool("noShine") == false)
                {
                    anim.SetBool("noShine", true); anim_monochrome.SetBool("noShine", true);
                }
            }
            agent.SetDestination(transform.position);

            if(gameObject.GetComponent<monster_database>().GetFlee()==true)
            {
                if (agent.isStopped == true)
                {
                    agent.isStopped = false;
                }
                if (!isFleeAudioPlaying)
                {
                    SoundEffectManager.instance.PlayRandomSoundFxClip(fleeAudio, transform, Volume());
                    isFleeAudioPlaying = true;
                }

                speed = 9;
                agent.speed = speed;
      
                
                    // Calculate the direction away from the player
                    Vector2 fleeDirection = (transform.position - target.position).normalized;

                    // Set a destination in the flee direction
                    fleePosition = transform.position + (Vector3)fleeDirection;

                
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



            }
            else
            {
                if (agent.isStopped == false)
                {
                    agent.isStopped = true;
                }

                if(gameObject.GetComponent<monster_database>().GetFlashed() == false)
                {
                    currentState = EnemyState.Chasing;
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

    private Vector3 FindValidFleePosition(Vector3 startPosition, Vector2 direction)
    {
        const float stepDistance = 0.5f; // Distance to step each iteration
        const int maxSteps = 10; // Max steps to search for a valid position

        for (int i = 0; i < maxSteps; i++)
        {
            Vector3 newPosition = startPosition + (Vector3)(direction * stepDistance * i);

            // Check if this new position is valid
            if (Physics2D.OverlapCircle(newPosition, 0.5f, worldMask) == null)
            {
                return newPosition; // Return the first valid position
            }
        }

        // If no valid position is found, return the original position
        return startPosition;
    }

    private void FleeingChecks()
    {
        if (gameObject.GetComponent<monster_database>().GetFlee() == true)
        {

            inAtkArea = false;
            currentState = EnemyState.Fleeing;
            Debug.Log("flee check");    
            

        }
        
    }

    private void FlashChecks()
    {
        
        if (gameObject.GetComponent<monster_database>().GetShine() == true && currentState != EnemyState.Fleeing) 
        {
            if (anim.GetBool("isShine") == false) 
            {
                anim.SetBool("noShine", false); anim_monochrome.SetBool("noShine", false);
                anim.SetBool("isShine", true); anim_monochrome.SetBool("isShine", true);
                StartCoroutine(EndShineAfterAnimation());
            }
            anim.SetBool("isWalking", false); anim_monochrome.SetBool("isWalking", false);
            anim.SetBool("isRunning", false); anim_monochrome.SetBool("isRunning", false);
            anim.SetBool("isAtking", false); anim_monochrome.SetBool("isAtking", false);

            if (agent.isStopped == false)
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
            }

            if (!isShineAudioPlaying)
            {
                SoundEffectManager.instance.PlayRandomSoundFxClip(shineAudio, transform, Volume());
                isShineAudioPlaying = true;
            }

        }
        else
        {
            isShineAudioPlaying = false;
        }
            
       
    }

    private void FlipChecks()
    {
        
            float currentPosX = transform.position.x;
            float previousPosX = previousPosition.x;


            if (currentPosX > previousPosX)
            {
                sr.flipX = true; sr_monochrome.flipX = true;
            }
            else if (currentPosX < previousPosX)
            {
               sr.flipX = false; sr_monochrome.flipX = false;
            }


            previousPosition = transform.position;
        
       
    }

    private void CornfieldRangeCheck()
    {
        //Debug.Log("cornfield ray");
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        Vector2 StartPos = collider.bounds.center;
        Vector2 Direction = (target.position - transform.position).normalized;

        RaycastHit2D Hit = Physics2D.Raycast(StartPos, Direction, cornfieldRange, cornfieldMask);

        if (cornfield == false)
        {
            if (Hit.collider != null && Hit.collider.CompareTag("Cornfield"))
            {
                Debug.DrawRay(StartPos, Direction * cornfieldRange, Color.black); // Debug visualization for hit
                currentState = EnemyState.Idle;
                cornfield = true;
                anim.SetBool("isWalking", false);   anim_monochrome.SetBool("isWalking", false);
                anim.SetBool("isRunning", false);   anim_monochrome.SetBool("isRunning", false);


            }
            else
            {
                Debug.DrawRay(StartPos, Direction * cornfieldRange, Color.red);
            }
        }

       

      

    }

    private void AtkChecks()
    {
        // Get the BoxCollider2D component
        BoxCollider2D collider = GetComponent<BoxCollider2D>();

        // Use the bounds.center to get the world-space center of the collider
        Vector2 enemyCenterPosition = collider.bounds.center;

        // Player's position
        Vector2 playerPosition = target.position;

        // Calculate the vertical and horizontal distances
        float verticalDistance = Mathf.Abs(playerPosition.y - enemyCenterPosition.y);
        float horizontalDistance = Mathf.Abs(playerPosition.x - enemyCenterPosition.x);

        // Check vertical distance first
        if (verticalDistance < verticalThreshold)
        {
            // Check if the horizontal distance is within attack range
            if (horizontalDistance <= attackRange)
            {
                // Conditions met; allow attack
                gameObject.GetComponent<EnemyAttack>().Attack();
                flashlight.TurnOffFlashlight();
                playerDmg = true;
                Debug.Log("Conditions met: Attacking the player!");
            }
            else
            {
                Debug.Log("Player is too far horizontally to attack.");
            }
        }
        else
        {
            Debug.Log("Player is above or below the enemy, attack not allowed.");
        }

    }

    #region<OntriggerEntered/Exit>

    private void OnIndleRangeTriggerEntered(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = collision.transform;


            if (currentState == EnemyState.Idle && cornfield == false) 
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
        if (collision.CompareTag("Player") && cornfield == false) 
        {
            Debug.Log("Chasing");
            currentState = EnemyState.Chasing;
        }
    }

    private void OnCornFieldTriggerEntered(Collider2D collision)
    {
        if (collision.CompareTag("Player") && target != null) 
        {
            cornfieldRangeCheck = true;
            Debug.Log("cornfieldRangeCheck");
        }
       
    }

    private void OnCornFieldTriggerExited(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cornfieldRangeCheck = false;
            cornfield = false;
            Debug.Log("Cornfield false");
        }
    }


    #endregion

    #region<Coroutine>
    private IEnumerator PlayStalkingSound()
    {
        float rand = Random.Range(1f, 2f);
        SoundEffectManager.instance.PlayRandomSoundFxClip(stalkAudio, transform, Volume());
        yield return new WaitForSeconds(rand);
        isStalkAudioPlaying = false;
        
       
    }

    private IEnumerator PlayChaseSound()
    {
        float rand = Random.Range(0.3f, 0.5f);
        SoundEffectManager.instance.PlayRandomSoundFxClip(chaseAudio, transform, Volume());
        yield return new WaitForSeconds(rand);
        isChaseAudioPlaying = false;
    }

    private IEnumerator PlayGrowlSound()
    {
        float rand = Random.Range(7f, 9f);
        SoundEffectManager.instance.PlayRandomSoundFxClip(growlAudio, transform, Volume());
        yield return new WaitForSeconds(rand);
        isGrowlingAudioPlaying = false;
    }

    private IEnumerator EndAttackAfterAnimation()
    {
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("isAtking", false);    anim_monochrome.SetBool("isAtking", false);
    }

    private IEnumerator EndShineAfterAnimation()
    {
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("isShine", false); anim_monochrome.SetBool("isShine", false);
    }



    #endregion

    #region<PlaySfx>



    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach (Vector3 waypoint in waypoints)
        {
            Gizmos.DrawSphere(waypoint, waypointGizmoRadius);
        }

        // Draw stalking range
        Gizmos.color = Color.black;
        Vector2 stalkingCenter = GetComponent<BoxCollider2D>().bounds.center; // Use the actual stalking center
        Gizmos.DrawWireSphere(stalkingCenter, stalkingDistance);

        if (target != null)
        {
            Gizmos.color = Color.green; // Color for thresholds
            Vector3 position = GetComponent<BoxCollider2D>().bounds.center;

            // Upper threshold
            Gizmos.DrawLine(position, position + Vector3.up * verticalThreshold);

            // Lower threshold
            Gizmos.DrawLine(position, position + Vector3.down * verticalThreshold);

            // Visual connection to the target
            Gizmos.color = Color.yellow; // Line to player for reference
            Gizmos.DrawLine(position, target.position);

            Gizmos.color = Color.red;
            float halfVerticalThreshold = verticalThreshold / 2f;
            float halfAttackRange = attackRange / 2f;
        }

        if (currentState == EnemyState.Fleeing) // Optional: Only draw when fleeing
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(fleePosition, 0.5f); // 0.5f is the radius of the circle
        }
    }

    #region Sound
    float Volume()
    {
        float volumeControl;
        if(Audio.Instance !=null)
        {
            volumeControl = Audio.Instance.SFXSource.volume;
           
        }
        else
        {
            volumeControl = 1f;
        }
        return volumeControl;
    }
    #endregion
}
