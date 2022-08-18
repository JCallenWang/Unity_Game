using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    [Header("攻擊參數")]
    [SerializeField] float attackDamage = 10;
    [SerializeField] float attackRange = 2;
    [SerializeField] float attackInterval = 2;

    Mover mover;
    Animator animator;
    Health health;
    Health targetHealth;

    float LastAttackDuration = Mathf.Infinity;

    void Start()
    {
        mover = GetComponent<Mover>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();

        health.onDead += OnDead;
    }

    void Update()
    {
        //如果角色已死亡
        if (targetHealth == null || targetHealth.IsDead())
        {
            CancelTarget();
            return;
        }

        if (IsInAttackRange())
        {
            mover.CancelMove();
            AttackBehaviour();
        }
        else if (LastAttackDuration > attackInterval)
        {
            animator.SetBool("IsAttack", false);
            mover.MoveTo(targetHealth.transform.position, 1);
        }

        LastAttackDuration += Time.deltaTime;
    }

    //攻擊行為
    private void AttackBehaviour()
    {
        transform.LookAt(targetHealth.transform);

        if(LastAttackDuration > attackInterval)
        {
            LastAttackDuration = 0;
            animator.SetBool("IsAttack", true);
            //TriggerAttack();
        }
    }


    //觸發攻擊動畫
    private void TriggerAttack()
    {
        animator.SetBool("IsAttack", true);
    }

    //動畫攻擊揮手才扣血
    private void Hit()
    {
        if (targetHealth == null) return;

        if (IsInAttackRange())
        {
            targetHealth.TakeDamage(attackDamage);
        }
        
    }

    //檢查是否在攻擊範圍內
    private bool IsInAttackRange()
    {
        return Vector3.Distance(transform.position, targetHealth.transform.position) < attackRange;
    }


    public void Attack(Health target)
    {
        targetHealth = target;
    }

    public void CancelTarget()
    {
        animator.SetBool("IsAttack", false);
        targetHealth = null;
    }

    private void OnDead()
    {
        enabled = false;
    }

    //顯示攻擊範圍
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
