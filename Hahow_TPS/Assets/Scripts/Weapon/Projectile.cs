using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("子彈控制")]
    [Tooltip("子彈飛行速度")] [SerializeField] float projectileSpeed;
    [Tooltip("子彈存活時間")] [SerializeField] float maxLifeTime = 3;
    [Tooltip("重力向下的力")] [SerializeField] float gravityDownForce = 0;

    //飛行速度
    Vector3 currentVelocity;

    private void OnEnable()
    {
        Destroy(gameObject, maxLifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += currentVelocity * Time.deltaTime;

        if(gravityDownForce > 0)
        {
            currentVelocity += Vector3.down * gravityDownForce * Time.deltaTime;
        }
    }

    public void Shoot()
    {
        currentVelocity = transform.forward * projectileSpeed;
    }
}
