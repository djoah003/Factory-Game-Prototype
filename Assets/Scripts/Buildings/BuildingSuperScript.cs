using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BuildingSuperScript : MonoBehaviour
{

    public new string name; //Is this really needed? T. Aleksi
    public Transform output; //Location for the finished product to be instantiated.
    public Animator animator;
 
    //DURABILITY
    public int BuildingAggro = 0;
    [HideInInspector]
    public float Currentdurability; //The building's current durability. The maximum is set at start.
    public float MaxDurability;
    //DURABILITY END
    public float craftingTime; //Crafting time in script.
    public BuildingInventory Inventory = new BuildingInventory(); //Define the script that is used in the BuildingInventoryScript. new defines that every building has it's own inventory.
    public bool checkInventory = false; //Boolean for inventory.
    
    public void Durability(int damageAmount)
    {
        Currentdurability -= damageAmount; //Subtract the damage given to the buildings durability.
    }

    public void TransportSize() //TO BE ADDED LATER BY ALEKSI!!!
    {
        
    }
    public void Awake()
    {
        Currentdurability = MaxDurability;
    }

    public void Death()
    {
        animator.SetTrigger("Death");
    }
}
