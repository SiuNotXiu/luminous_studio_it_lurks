using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class testing : MonoBehaviour
{
    public HealthEffects _healtheffects = null;
    private float damage = 10f;
    private void start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            _healtheffects.currentHp -= damage;
            _healtheffects.TakeDamage();
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }
    }
}
