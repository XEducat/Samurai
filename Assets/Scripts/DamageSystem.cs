using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange = 7.4f;
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] int damage = 20;

    public bool CheckAttackRadius()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        return hitEnemies.Length > 0;
    }

    void GiveDamage() // Called as an event in the Attack animation
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Entity entity = enemy.GetComponent<Entity>();
            entity.TakeHit(damage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawSphere(attackPoint.position, attackRange);
    }
}
