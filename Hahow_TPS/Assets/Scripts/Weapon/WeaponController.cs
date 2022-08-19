using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponShootType
{
    Single,
    Automatic,
}

public class WeaponController : MonoBehaviour
{
    [Header("武器控制")]
    [Tooltip("要顯示的武器")] [SerializeField] GameObject weaponRoot;
    [Tooltip("槍口位置")] [SerializeField] Transform weaponMuzzle;

    [Header("射擊控制")]
    [Tooltip("射擊類型")] [SerializeField] WeaponShootType shootType;
    [Tooltip("射擊間隔")] [SerializeField] float delayBetweenShoots = 0.5f;
    [Tooltip("連射武器射擊一發消耗的子彈數量")] [SerializeField] int bulletPerShoot;

    [Header("彈匣控制")]
    [Tooltip("子彈物件")] [SerializeField] Projectile projectilePrefab;
    [Tooltip("每秒彈匣補充的子彈速度")] [SerializeField] float ammoReloadRate = 1;
    [Tooltip("可以換彈的延遲時間")] [SerializeField] float ammoReloadDelay = 2;
    [Tooltip("最大子彈數量")] [SerializeField] int maxAmmo = 8;


    //設定一個參數可以在外部更改（get外部設定，set為內部資訊）
    public GameObject sourcePrefab { get; set; }

    //當前子彈數量
    int currentAmmo;
    //距離上次射擊的時間
    float timeSinceLastShoot;
    //是否在瞄準狀態
    bool isAim;

    private void Awake()
    {
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAmmo();
    }

    private void UpdateAmmo()
    {

    }

    //要不要顯示武器
    public void ShowWeapon(bool value)
    {
        weaponRoot.SetActive(value);
    }

    //處理射擊行為
    public void HandleShootInput(bool inputDown, bool inputHeld, bool inputUp)
    {
        switch (shootType)
        {
            case WeaponShootType.Single:
                if (inputDown)
                {
                    print("Single 射擊");
                    TryShoot();
                }
                return;
            case WeaponShootType.Automatic:
                if (inputHeld)
                {
                    print("Automatic 射擊");
                    TryShoot();
                }
                return;
            default:
                return;
        }
    }

    private void TryShoot()
    {
        if (currentAmmo >= 1 && timeSinceLastShoot + delayBetweenShoots < Time.time)
        {
            HandleShoot();
            currentAmmo -= 1;
        }
    }

    private void HandleShoot()
    {
        for (int i = 0; i < bulletPerShoot; i++)
        {
            Projectile newProjectile = Instantiate(projectilePrefab, weaponMuzzle.position, Quaternion.LookRotation(weaponMuzzle.forward));
            newProjectile.Shoot();
        }

        timeSinceLastShoot = Time.time;
    }
}
