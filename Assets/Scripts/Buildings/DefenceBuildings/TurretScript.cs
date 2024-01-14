using System.Collections;
using System.Collections.Generic;
using UnityEditor;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;

public class TurretScript : BuildingSuperScript
{
    public int Range;
    public int RailgunDamage, ShotgunDamage;
    public float FireRate;
    public GameObject BulletSpawn;
    public GameObject screw;
    public GameObject TargetFinder;
    [HideInInspector]
    public Transform target;
    private float Distance;
    private float Speed = 10;
    private Collider Target, closestTarget;
    [HideInInspector]
    public bool TargetFound = false;
    private float bugDurability = 1;
    private Ray Railgun;
    private Ray ShotgunSpread;
    private float closest;
    public bool railgun, shotgun = false;
    private int ShotgunShots = 5;
    private int RailgunShots = 5;
    private GameObject instantiatedProjectile;
    private Turret turret;

    private void TargetSelection()
    {
        //finds all the bugs within range and assigns a target based on which one is the closest
        var ArrayObjects = Physics.OverlapSphere(TargetFinder.transform.position, Range, 1 << 8);
        foreach (Collider hitCollider in ArrayObjects)
        {
            var distance = Vector3.Distance(hitCollider.transform.position, BulletSpawn.transform.position);
            if (hitCollider == null)
                continue;
            if(distance < closest && Target == null)
            {
                closest = distance;
                closestTarget = hitCollider;
            }
        }
        if (Target == null && closestTarget != null)
            Target = closestTarget;
        if (Target != null)
        {
            Distance = Vector3.Distance(Target.transform.position, BulletSpawn.transform.position);
            TargetFound = true;
        }
        if (Distance >= Range)
            Target = null;
        if (Target == null)
        {
            TargetFound = false;
            closest += Range;
        }
    }
    private void Attack(Collider Target)
    {
            //damages every bug found in Raycast shot through target bug, waits for FireRate amount of time after before looping
            if (railgun) 
            {
                turret.Shoot();
                Railgun = new Ray(BulletSpawn.transform.position, Target.transform.position - BulletSpawn.transform.position);
                RaycastHit[] hits;
                hits = Physics.RaycastAll(Railgun, Range, 1 << 8);
                for (int i = 0; i < hits.Length; i++)
                {
                    RaycastHit hit = hits[i];
                    bugDurability = hit.collider.gameObject.GetComponent<BugLogic>().CurrentDurability;
                    hit.collider.gameObject.GetComponent<BugLogic>().TakeDamage(RailgunDamage);
                    TurretDamage();
                }
                //Inventory.Remove(ItemType.IronRod, 1);
            }
            //Shoots out up to 5 screws on every shot, damages bug if the screws hit
            if (shotgun)
            {
                turret.Shoot();
                float spreadZ = 0.0f;
                float spreadY = 0.0f;
                for (int i = 0; i < ShotgunShots; i++)
                {
                    Railgun = new Ray(BulletSpawn.transform.position, Target.transform.position - BulletSpawn.transform.position);
                    spreadY = Random.Range(0.0f, 1f);
                    spreadZ = Random.Range(0.0f, 1f);

                    Vector3 deviation = new Vector3(0.0f, spreadY, spreadZ);
                    Railgun.direction += deviation;
                    RaycastHit hit;
                    if (Physics.Raycast(Railgun, out hit, Range, 1 << 8))
                    {
                        GameObject bullet = Instantiate(screw, BulletSpawn.transform.position, transform.rotation) as GameObject;
                        bullet.GetComponent<Rigidbody>().velocity = Railgun.direction * Speed;
                        Destroy(bullet, 1.0f);
                        bugDurability = hit.collider.gameObject.GetComponent<BugLogic>().CurrentDurability;
                        hit.collider.gameObject.GetComponent<BugLogic>().TakeDamage(ShotgunDamage);
                    }
                    else
                    {
                        GameObject bullet = Instantiate(screw, BulletSpawn.transform.position, transform.rotation) as GameObject;
                        bullet.GetComponent<Rigidbody>().velocity = Railgun.direction * Speed;
                        Destroy(bullet, 1.0f);
                    }
                    TurretDamage();
                    //Inventory.Remove(ItemType.Screws, 1);
                }
            }
    }
    void Start()
    {
        closest += Range; //assigns closest range
        turret = this.gameObject.GetComponentInParent<Turret>();
    }

    public void Update()
    {
        /*if (Inventory.GetItemAmount(ItemType.Screws)>=0)
            ShotgunShots = Inventory.GetItemAmount(ItemType.Screws);
        if (Inventory.GetItemAmount(ItemType.IronRod) >= 0)
            RailgunShots = Inventory.GetItemAmount(ItemType.IronRod);

        if (RailgunShots == 0)
            railgun = false;
        else if (RailgunShots > 0 && shotgun == false)
            railgun = true;

        if (ShotgunShots == 0)
            shotgun = false;
        else if (ShotgunShots > 0 && railgun == false)
            shotgun = true;*/

        TargetSelection();
        if (Target != null)
            target = Target.transform;
        FireRate -= Time.deltaTime;
        if(FireRate <= 0 && TargetFound == true && Target != null)
        {
            Attack(Target);
            FireRate = 0.5f;
        }

    }
    private void TurretDamage()
    {
        //stops coroutine when bug durability is 0 and destroys the bug
        if (bugDurability <= 0 && Target != null)
        {
            Destroy(Target.gameObject);
            Target = null;
        }
    }
}
