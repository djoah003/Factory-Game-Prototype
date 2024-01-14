using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ProductionBuildingScript : BuildingSuperScript
{
   public List<Recipe> recipes;
   public Recipe currentRecipe;

   protected virtual void ProcessRecipe()
   {
        if (currentRecipe != null)
        {
            foreach (Recipe.RecipeRequirement requirement in currentRecipe.Requirements)
            {
                if (Inventory.GetItemAmount(requirement.Type) < requirement.NeededAmount)
                    return;
            }
            craftingTime -= Time.deltaTime;

            if (craftingTime <= 0)
            {
                foreach (Recipe.RecipeRequirement requirement in currentRecipe.Requirements)
                {
                    Inventory.Remove(requirement.Type, requirement.NeededAmount);
                }

                GameObject prefab = ItemConfig.Instance.GetPrefab(currentRecipe.OutputType);

                for (int i = 0; i < currentRecipe.OutputAmount; i++)
                    Instantiate(prefab, output);
            }
        }
   }
   private bool FitsRecipe(Item item) //Check that the item fits the recipe requirements. Defined in Item.cs
   {
        if (currentRecipe != null)
        {
            foreach (var requirement in currentRecipe.Requirements) //Check in the recipe.cs, if the given item is needed in the recipe.
            {
                if (item.Type == requirement.Type) //If the item fits the requirements, return true.
                {
                    return true;
                }
            }
        }
        return false; //Else return false.
   }
   public virtual void ItemInInput(Collider other) //When the item enters the buildings trigger collider (input)
   {
       
      ItemInfo worldItem = other.GetComponent<ItemInfo>(); //Get the Item info from the item and declare it as worldItem.
      if (worldItem != null /*&& FitsRecipe(worldItem.Item)*/) //Checks if there is an worldItem and that the item fits the recipe.
      {
         Debug.Log(worldItem);
         Inventory.Add(worldItem.Item); //If the item meets the given requirements, add it to the inventory
            
         // Destroy(worldItem); 
         Destroy(other.gameObject); //and destroy it from the world.
      }
   }
   public virtual void Update()
   {
      if (checkInventory)
      {
         Inventory.CheckInventory(); //Call the inventory function CheckInventory();.
         checkInventory = false; //Return false.
      }
   }
}
