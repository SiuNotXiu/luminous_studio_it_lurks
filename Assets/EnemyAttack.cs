using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public HealthEffects _healtheffects;
    private float damage = 25f;
    [SerializeField] private AudioClip[] damageAudios;

    private void Start()
    {

        GameObject healthcontroller = GameObject.Find("HealthControll");
        if (healthcontroller != null)
        {
            _healtheffects = healthcontroller.GetComponent<HealthEffects>();
        }
        else
        {
            Debug.LogError("HealthControll GameObject not found!");
        }

        if (_healtheffects == null)
        {
            Debug.LogError("HealthEffects component not found on HealthControll GameObject!");
        }
    }

    public void Attack()
    {
        _healtheffects.currentHp -= damage;
        _healtheffects.TakeDamage();
        SoundEffectManager.instance.PlayRandomSoundFxClip(damageAudios, transform, 1f);
        
    }
}
