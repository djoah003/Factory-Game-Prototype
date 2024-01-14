using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    private UIScript uiscript;
    private void Start()
    {
        uiscript = GameObject.Find("Machine info panel").GetComponent<UIScript>();
    }
    public void DestroyUI()
    {
        for (int i = 0; i < uiscript.Slots.Length; i++)
        {
            uiscript.Slots[i].addItem(0, 0);
        }
        for (int i = 0;i < uiscript.RecipeSlots.Length; i++)
        {
            uiscript.RecipeSlots[i].addRecipe(null, null);
        }
        uiscript.RecipesBuilt = false;
        uiscript.ItemsBuilt = false;
        uiscript.dragscript.ShowUI = false;
        uiscript.UI.gameObject.SetActive(false);
        uiscript.craftingMenu.gameObject.SetActive(false);
    }
}
