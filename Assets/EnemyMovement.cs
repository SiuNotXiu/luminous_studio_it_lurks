using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyMovement : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float chaseRadius;
    public float roamingRadius;
    public float minWanderTime;
    public float maxWanderTime;
    public Color detectionColor = Color.red;


    private bool chaseState = false;
    private float distance;
    private float detectionRadius;
    private Vector2 wanderDirection;
    private float wanderTimer;

    // Start is called before the first frame update
    void Start()
    {
        detectionRadius = roamingRadius;
    }

    // Update is called once per frame
    void Update()
    {

        distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance <= detectionRadius)
        {
            chaseState = true;

        }
        else
        {
            chaseState = false;
        }

        if (chaseState == true)
        {
            Chasing();
        }
        else
        {
            Wander();
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = detectionColor;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    void Chasing()
    {
        
        detectionRadius = chaseRadius;
        Vector2 direction = (player.transform.position - transform.position).normalized;
        transform.position = (Vector2)transform.position + (direction * speed * Time.deltaTime);
       
    }

    void Wander()
    {
        detectionRadius = roamingRadius;
        Vector2 direction = wanderDirection;
        transform.position = (Vector2)transform.position + (direction * speed * Time.deltaTime);

        wanderTimer -= Time.deltaTime;

        if (wanderTimer <= 0) 
        {
            NewWanderDirection();
        }
    }

    void NewWanderDirection()
    {
        wanderDirection = Random.insideUnitCircle.normalized;
        wanderTimer = Random.Range(minWanderTime, maxWanderTime);
    }

   
}
