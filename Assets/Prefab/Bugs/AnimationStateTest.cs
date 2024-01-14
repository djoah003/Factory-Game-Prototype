using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateTest : MonoBehaviour
{
    Animator animator;
    int isRunningHash;

    public float attackCooldown = 1.7f;
    private float nextAttack;

    private bool isRunning;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        isRunningHash = Animator.StringToHash("isRunning");
    }

    private void Update()
    {
        bool isRunning = animator.GetBool(isRunningHash);

        if (Input.GetKeyDown(KeyCode.Alpha1) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && Time.time > nextAttack)
        {
            animator.SetInteger("AttackIndex", Random.Range(0, 2));
            animator.SetTrigger("Attack");

            nextAttack = Time.time + attackCooldown;
        }
    }
}
