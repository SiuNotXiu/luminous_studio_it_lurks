using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class Chase : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float chaseRadius;
    public float roamingRadius;
    public Color detectionColor = Color.red;


    private bool chaseState = false;
    private float distance;
    private float detectionRadius;

    // Start is called before the first frame update
    void Start()
    {
        detectionRadius = roamingRadius;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);

        if(distance <= detectionRadius && chaseState == false)
        {
            chaseState = true;
            
        }

        if(chaseState==true)
        {
            detectionRadius = chaseRadius;
            
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        
        }

        if (distance >= detectionRadius && chaseState == true)
        {
            chaseState = false;
            detectionRadius = roamingRadius;

        }



    }

    private void OnDrawGizmos()
    {
        Gizmos.color = detectionColor;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
