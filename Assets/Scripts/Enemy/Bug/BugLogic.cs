using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AI;

public class BugLogic : MonoBehaviour
{
    public int CurrentDurability;
    public int Damage;
    public float AttackDelay;
    private int buildingAggro = 0;
    private int Aggro = 0;
    private Transform Target;
    private BuildingSuperScript buildingscript;
    private Turret turret;
    private int SearchRadius = 15;
    private bool TargetFound;
    NavMeshAgent _navMeshAgent;
    private Transform _destination;
    Coroutine lastRoutine = null;
    private Animator animator;
    private bool isRunning = false;
    private bool isAttacking = false;
    private float buildingDurability;
    void bugLogic()
    {
        {
            //finds highest aggro building in range and assigns as target, increasing search radius if not found
            var ArrayObjects = Physics.OverlapSphere(this.transform.position, SearchRadius, 1 << 7);
            if (ArrayObjects.Length == 0 && TargetFound == false)
            {
                SearchRadius++;
            }
            if (SearchRadius > 15 && TargetFound == true)
            {
                SearchRadius--;
            }
            foreach (Collider hitCollider in ArrayObjects)
            {
                if (hitCollider == null)
                    continue;
                    buildingscript = hitCollider.GetComponentInParent<BuildingSuperScript>();
                    buildingAggro = buildingscript.BuildingAggro;
                    turret = hitCollider.GetComponentInParent<Turret>();
                if (buildingAggro > Aggro || Target == null)
                {
                    Aggro = buildingAggro;
                    Target = hitCollider.GetComponentInParent<Transform>();
                }
            }
            if(ArrayObjects.Length == 0)
            {
                Target = null;
            }
            if (Target != null)
                TargetFound = true;
            if (Target == null)
                TargetFound = false;
        }
    }
    //starts coroutine when collider enters trigger
    void OnTriggerEnter(Collider other)
    {
        lastRoutine = StartCoroutine(Attack(other));
        isAttacking = true;
    }
    //damages building and waits for AttackDelay amount of time before doing it again
    IEnumerator Attack(Collider other)
    {
        while (other != null)
        {
            yield return new WaitForSeconds(AttackDelay);
            if(other != null)
                other.gameObject.GetComponent<BuildingSuperScript>().Durability(Damage);
        }
    }
    //finds crucial components to use
    void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponent<Animator>();
    }

    // runs functions, assigns destination for navmesh
    void Update()
    {
        bugLogic();
        if (TargetFound == true && Target != null) 
        {
            _destination = Target.transform;
            SetDestination();
            buildingDurability = Target.gameObject.GetComponent<BuildingSuperScript>().Currentdurability;
        }
        Animation();
        if (_navMeshAgent.velocity == Vector3.zero)
            isRunning = false;
        BugDamage();
    }
    //assigns destination for navmesh
    private void SetDestination()
    {
        if(_destination != null)
        {
            Vector3 targetVector = _destination.transform.position;
            _navMeshAgent.SetDestination(targetVector);
        }
    }
    //sets animations using a set of booleans and velocity of bug
    private void Animation()
    {
        if (_navMeshAgent.velocity != Vector3.zero)
            isRunning = true;
        else
            isRunning = false;

        if (isRunning == true)
            animator.SetBool("isRunning", true);
        else
            animator.SetBool("isRunning", false);

        if (isAttacking == true)
        {
        animator.SetInteger("AttackIndex", Random.Range(0, 1));
        animator.SetBool("Attack", true);
        }
        else
            animator.SetBool("Attack", false);
    }
    //stops coroutine and destroys target when durability 0
    private void BugDamage()
    {
        if (buildingDurability <= 0 && lastRoutine != null && Target != null)
        {
            StopCoroutine(lastRoutine);
            isAttacking = false;
            turret.Death();
            Destroy(Target.gameObject, 2.0f);
        }
    }
    //take damage function
    public void TakeDamage(int damageAmount)
    {
        if (gameObject != null)
        {
            CurrentDurability -= damageAmount;
        }
    }
}
