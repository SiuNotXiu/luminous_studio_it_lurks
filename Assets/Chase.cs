using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour
{
    public GameObject player;
    public float minSpeed;
    public float maxSpeed;
    public float speedMultiplier;

    private float distance;
    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        speed = Mathf.Lerp(maxSpeed, minSpeed, distance * speedMultiplier);
        

        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        speed = minSpeed + (1f / distance) * speedMultiplier;
    }
}
