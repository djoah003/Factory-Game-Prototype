using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Turret : MonoBehaviour
{
    private Animator animator;
    private bool isWeight; // to check if the railgun is folded up or not
    private TurretScript turretscript;

    [Header("Setup Fields")]
    public Rig targetTracking; // Railgun > TargetTracking
    public Transform target; // Railgun > Target

    private void Start()
    {
        animator = GetComponent<Animator>();
        targetTracking.weight = 0; // set rig weight to 0 so it doesn't overwrite animations while unfolding
        turretscript = this.gameObject.GetComponentInParent<TurretScript>();
    }

    private void Update()
    {
        if (turretscript.target != null)
            target = turretscript.target;
        // Animation controls for testing
        if (animator != null)
        {
            if (turretscript.TargetFound == true) // unfold
            {
                animator.SetBool("Folded_Out", true);
                isWeight = true;
                EnableTracking();
            }

            if (turretscript.TargetFound == false && target == null) // fold up
            {
                animator.SetBool("Folded_Out", false);
                DisableTracking();

                isWeight = false;
            }
        }
    }

    public void EnableTracking() // Listening for event from Idle animation (event is on the 10th frame)
    {
        isWeight = true; // set unfolded state
        StartCoroutine(LerpTracking()); // enemy tracking
    }
    public void Shoot()
    {
        animator.SetTrigger("Shoot");
    }

    public void Death()
    {
        animator.SetTrigger("Death");
        DisableTracking();
    }

    private IEnumerator LerpTracking() // enemy tracking
    {
        float rotationSpeed = 2f;

        while (targetTracking.weight < 0.9) // while animation is playing
        {
            if (!isWeight) // if turret is folded up rig weight stays 0
            {
                targetTracking.weight = 0f;
                yield break;
            }

            // set rig weight from 0 to 1 with lerp so turret doesn't snap to target
            targetTracking.weight = Mathf.Lerp(targetTracking.weight, 2.0f, Time.deltaTime * rotationSpeed);
            yield return null;
        }
    }

    // set rig weight to 0
    public void DisableTracking()
    {
        isWeight = false; // set state folded up

        StartCoroutine(DropWeightToZero()); // set rig weight from1 to 0 with lerp before playing folding up animation
    }

    // set rig weight from1 to 0 with lerp before playing folding up animation
    private IEnumerator DropWeightToZero()
    {
        float duration = .5f; // the time the turret has to return to default position before animation plays
        float elapsed = 0f; // counting elapsed time

        while (elapsed < duration) // reducing rig weight to 0 over elapsed/duration seconds
        {
            elapsed += Time.deltaTime; // start counting time

            targetTracking.weight = Mathf.Lerp(targetTracking.weight, 0, elapsed / duration); // reduce weight from current weight to 0 over elapsed/duration seconds

            yield return null;
        }
    }
}
