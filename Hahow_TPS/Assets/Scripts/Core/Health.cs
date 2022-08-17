using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("血量參數")]
    [SerializeField] float maxHealth = 10;
    [SerializeField] float currentHealth;

    public event Action onDamage;
    public event Action<float> onHealed;
    public event Action onDead;

    bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {

        return maxHealth;
    }

    public float GetHealthRatio()
    {
        return currentHealth / maxHealth;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        if(gameObject.tag == "Player")
        {
            print("Player目前血量為：" + currentHealth);
        }

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        //如果血量>0會發放受到傷害的通知
        if (currentHealth > 0)
        {
            //?代表onDamage != null，代表有訂閱者（沒有訂閱者系統會報錯）
            onDamage?.Invoke();
        }

        if(currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    //如果血量<0會發放角色死亡的通知
    private void HandleDeath()
    {
        if (isDead) return;

        if (currentHealth <= 0)
        {
            isDead = true;
            onDead?.Invoke();
        }
    }
}
