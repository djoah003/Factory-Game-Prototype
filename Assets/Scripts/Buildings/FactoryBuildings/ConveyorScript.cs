using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorScript : ProductionBuildingScript
{
    [SerializeField]
    private float speed;
    private Vector3 direction;
    [SerializeField]
    private int MaxMaterials;
    private List<GameObject> Items = new List<GameObject>();
    private List<GameObject> MovingItems = new List<GameObject>();

    private void Start()
    {
        direction = this.transform.forward;
        name = "Conveyor";
        MaxDurability = 40;
    }
    void FixedUpdate()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if(MovingItems.Count < MaxMaterials)
            {
                MovingItems.Add(Items[i]);
                Items.Remove(Items[i]);
            }
        }
        for (int i = 0; i < MovingItems.Count; i++)
        {
            if (MovingItems[i] != null)
                MovingItems[i].GetComponent<Rigidbody>().AddForce(speed * direction);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Item")
        {
            Items.Add(collision.gameObject);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        MovingItems.Remove(collision.gameObject);
    }
}
