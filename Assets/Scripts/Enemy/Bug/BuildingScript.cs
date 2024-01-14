using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingScript : MonoBehaviour
{
    public int BuildingAggro = 0;
    public float MaxDurability = 100;
    public float CurrentDurability;

    public void TakeDamage(int damageAmount)
    {
        if (gameObject != null)
        {
            CurrentDurability -= damageAmount;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        CurrentDurability = MaxDurability;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
