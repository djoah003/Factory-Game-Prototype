using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BuildingInventory
{
    public List<Item> Items; //Declare the list of items that the building can hold.

    public  void CheckInventory() //Debug. For inventory check.
    {
        Debug.Log("-------- INVENTORY --------");//Flair
        foreach (var type in Items)
        {
            Debug.Log(type); //Print the inventory 
        }
        Debug.Log("-------- INVENTORY END --------");
    }

    public bool HasItem(ItemType ItemInInventory) //Checks for the item in the inventory
    {
        return GetItemAmount(ItemInInventory) > 0; //Returns true if the amount is bigger than zero.
    }
    
    public BuildingInventory()
    {
        Items = new List<Item>(); //Every inventory has to have its own list.
    }
    
    public void Add(Item item) //Add item to the inventory.
    {
        if (HasItem(item.Type)) //if inventory already has the item
        {
            foreach (var _item in Items)
            {
                if (_item.Type == item.Type)
                {
                    _item.Amount += item.Amount; //add it to the amount
                    break;
                }
            }
        }
        else
        {
            Items.Add(item); //Add item
        }
    }

    public void Remove(ItemType type, int amount) //Remove the item from the inventory.
    {
        foreach (var item in Items) //checks for the item
        {
            if (item.Type == type) //if the type is the given type
            {
                if (item.Amount - amount >= 0) //if the amount is bigger than zero
                {
                    item.Amount -= amount; //remove the given amount
                }
                break; //else or after remove break
            }
        }
    }
    
    public int GetItemAmount(ItemType type) //Checks the item amount in inventory,
    {
        foreach (var item in Items) //for each item in items
        {
            if (item.Type == type) //check if the item is the given item
            {
                return item.Amount; //return the items amount value.
            }
        }
        return 0; //else return zero.
    }

}
