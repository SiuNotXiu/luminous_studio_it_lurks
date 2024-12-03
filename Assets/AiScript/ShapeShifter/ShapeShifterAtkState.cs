using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeShifterAtkState : ShapeShifterBaseState
{
    private EnemyAttack atk;
    private bool attacking = false;
    private float knockbackForce = 15f;
    private Rigidbody2D rb;
    public override void EnterState(ShapeShifterManager shapeShifter)
    {
        atk = shapeShifter.GetComponent<EnemyAttack>();
        rb = shapeShifter.rb;
        Debug.Log("Hi Im atk");
    }

    public override void UpdateState(ShapeShifterManager shapeShifter)
    {
        if (attacking == false && shapeShifter.inAtkArea==true)
        {
            //shapeShifter.StartCoroutine(testing.instance.Knockback(1, knockbackForce, shapeShifter.transform));

            atk.Attack();
            shapeShifter.anim.SetBool("Atk", true);
            shapeShifter.StartCoroutine(ResetAtkAnimation(shapeShifter));
            SoundEffectManager.instance.PlayRandomSoundFxClip(shapeShifter.GetAtkAudio(), shapeShifter.transform, shapeShifter.Volume());
            attacking = true;
            shapeShifter.StartCoroutine(AtkCd(shapeShifter));
        }
    }

    public override void ExitState(ShapeShifterManager shapeShifter)
    {
       
    }

    private void ApplyKnockback(ShapeShifterManager shapeShifter)
    {
        if (rb != null)
        {
            Vector2 direction = (shapeShifter.target.position - shapeShifter.transform.position).normalized;

            if (direction == Vector2.zero)
            {
                Debug.LogWarning("Knockback direction is zero; skipping force application.");
                return;
            }

            Vector2 knockback = direction * knockbackForce;
            Debug.Log($"Applying knockback. Direction: {direction}, Force: {knockback}");

            rb.AddForce(knockback, ForceMode2D.Impulse);

            // Optional: Visualize the knockback direction
            Debug.DrawRay(shapeShifter.transform.position, direction * knockbackForce, Color.red, 1f);
        }
        else
        {
            Debug.LogWarning("Rigidbody2D is null; cannot apply knockback.");
        }
    }

    IEnumerator AtkDelay(ShapeShifterManager shapeShifter)
    {
        yield return new WaitForSeconds(2f);
        shapeShifter.SwitchState(shapeShifter.idleState);
    }

    IEnumerator AtkCd(ShapeShifterManager shapeShifter)
    {
        yield return new WaitForSeconds(3f);
        attacking = false;
    }

    IEnumerator ResetAtkAnimation(ShapeShifterManager shapeShifter)
    {
        yield return new WaitForSeconds(0.1f);
        shapeShifter.anim.SetBool("Atk", false);
    }
}
