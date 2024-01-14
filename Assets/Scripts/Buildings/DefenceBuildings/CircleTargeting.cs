using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleTargeting : MonoBehaviour
{
    [SerializeField] Transform aimTarget;
    private Transform enemyTarget;
    private TurretScript turretscript;

    [SerializeField] float radius;

    [SerializeField] float speed = 1.0f;

    private void Start()
    {
        turretscript = this.gameObject.GetComponentInParent<TurretScript>();
    }
    private void Update()
    {
        if (turretscript.target != null) {
            enemyTarget = turretscript.target;
            if (enemyTarget)
            {
                Vector3 dir = (enemyTarget.position - transform.position).normalized;
                Vector3 aimTargetPosition = (transform.position + dir) * radius;

                aimTarget.position = Vector3.RotateTowards(aimTarget.position, aimTargetPosition, speed * Time.deltaTime, speed);

            }
        }
    }
}
