using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleRotateToScreenCenter : MonoBehaviour
{
    [Header("射線最大距離")]
    [SerializeField] float maxDistance;

    Ray ray;
    RaycastHit hit;

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        transform.rotation = Quaternion.LookRotation(ray.GetPoint(maxDistance));
        Debug.DrawLine(transform.position, ray.GetPoint(maxDistance), Color.red);
    }
}
