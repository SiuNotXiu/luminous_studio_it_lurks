using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public HealthEffects _healtheffects = null;
    private float damage = 10f;
    [SerializeField] private AudioClip[] damageAudios;
    

    public void Attack()
    {
        _healtheffects.currentHp -= damage;
        _healtheffects.TakeDamage();
        SoundEffectManager.instance.PlayRandomSoundFxClip(damageAudios, transform, 1f);
        
    }
}
