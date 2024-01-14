using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smelter_AnimationTest : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        bool working = animator.GetBool("Working");

        if (animator != null){
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // fold out animation
                animator.SetBool("Folded_Out", true);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                // fold up animation
                animator.SetBool("Folded_Out", false);
                animator.SetBool("Working", false);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                // start working animation
                animator.SetBool("Working", true);

                if (working)
                {
                    // stop working animation
                    animator.SetBool("Working", false);
                }

            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                // death animation
                animator.SetTrigger("Death");
            }
        }
    }
}
