using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeConfig : MonoBehaviour //COMMENT THIS
{
    public static RecipeConfig Instance;

    public List<Recipe> Recipes;
    // Start is called before the first frame update
    public void Awake()
    {
        Instance = this;
    }

    public Recipe GetRecipe(ItemType itemType)
    {
        foreach (Recipe recipe in Recipes)
        {
            if (recipe.OutputType == itemType)
                return recipe;
        }
        return null;
    }
}
