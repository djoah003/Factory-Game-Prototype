using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New_Recipe", menuName = "RecipeData/NewRecipe")] //This is the place where the recipes are made.
public class Recipe : ScriptableObject
{
    public List<RecipeRequirement> Requirements; //Declare the list of items that are used in the recipes.
    public ItemType OutputType; //Declare the output that the recipes make.
    public int OutputAmount; //Declare the amount of the output.
    public float CraftingTime; //Declare the amount of time needed to create the item.

    [Serializable]
    public struct RecipeRequirement //This is basically the brains of the recipes.
    {
        public ItemType Type; //Declare the item in the recipe.
        public int NeededAmount; //Declare the amount needed.
    }

    public RecipeRequirement GetRequirement(ItemType type) //Get the recipe requirements
    {
        foreach (var requirement in Requirements) //Checks for every requirement
        {
            if (requirement.Type == type) //Checks for how many items is needed
            {

                return requirement; //return the needed amount
            }
        }

        return default; //else return none
    }
}
