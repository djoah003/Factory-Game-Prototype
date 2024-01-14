using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Net;

public class RecipeSlot : MonoBehaviour
{
    private TextMeshProUGUI RecipeName;
    [HideInInspector]
    public bool HasRecipe = false;
    private Transform Parent;
    private GameObject craftingMenu;
    private UIScript uiscript;
    private float MaxCraftTime, CraftTime;
    private ItemType Output, Input1, Input2;
    private int OutputAmount, InputAmount1, InputAmount2;
    private Image craftTime, itemfillamount1, itemfillamount2;
    private TextMeshProUGUI OutputName,InputName1, InputName2, crafttime;
    private string Outputname;
    private List<Recipe.RecipeRequirement> inputRequirements;
    private bool CraftMenu = false;
    private TextMeshProUGUI output, input1, input2;
    [HideInInspector]
    public InventorySlot[] Slots;
    private Recipe currentRecipe;
    void Awake()
    {
        RecipeName = GetComponentInChildren<TextMeshProUGUI>();
        OutputName = GameObject.Find("Crafted Item Name").GetComponent<TextMeshProUGUI>();
        InputName1 = GameObject.Find("Input Name 1").GetComponent<TextMeshProUGUI>();
        InputName2 = GameObject.Find("Input Name 2").GetComponent<TextMeshProUGUI>();
        crafttime = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
        Parent = GameObject.Find("recipespanel").GetComponent<Transform>();
        craftingMenu = GameObject.Find("Crafting");
        uiscript = GameObject.Find("Machine info panel").GetComponent<UIScript>();
        craftTime = GameObject.Find("move this").GetComponent<Image>();
        output = GameObject.Find("Output per minute").GetComponent<TextMeshProUGUI>();
        input1 = GameObject.Find("Input per minute 1").GetComponent<TextMeshProUGUI>();
        input2 = GameObject.Find("Input per minute 2").GetComponent<TextMeshProUGUI>();
    }
    
    public void addRecipe(string name, Recipe recipe)
    {
        if (name != null)
        {
            RecipeName.text = name;
            Outputname = name;
        }
        else
        {
            RecipeName.text = string.Empty;
            Outputname = null;
        }
        if (recipe != null)
        {
            HasRecipe = true;
            inputRequirements = recipe.Requirements;
            MaxCraftTime = recipe.CraftingTime;
            Output = recipe.OutputType;
            OutputAmount = recipe.OutputAmount;
            Input1 = inputRequirements[0].Type;
            if (inputRequirements.Count > 1)
                Input2 = inputRequirements[1].Type;
            InputAmount1 = inputRequirements[0].NeededAmount;
            if (inputRequirements.Count > 1)
                InputAmount2 = inputRequirements[1].NeededAmount;
            currentRecipe = recipe;
        }
        else
            HasRecipe = false;
    }
    public void CraftingMenu()
    {
        Parent.gameObject.SetActive(false);
        OutputName.text = Outputname;
        crafttime.text = MaxCraftTime.ToString();
        InputName1.text = Input1.ToString();
        InputName2.text = Input2.ToString();
        craftingMenu.SetActive(true);
        CraftMenu = true;
        uiscript.MaxCraftTimer = MaxCraftTime;
        uiscript.CraftMenu= CraftMenu;
        var CraftPerMin = 60 / MaxCraftTime;
        var CraftPerMin1 = CraftPerMin * InputAmount1;
        var CraftPerMin2 = CraftPerMin * InputAmount2;
        output.text = CraftPerMin.ToString() + "/min";
        input1.text = CraftPerMin1.ToString() + "/min";
        input2.text = CraftPerMin2.ToString() + "/min";
        uiscript.inputAmount1 = InputAmount1;
        uiscript.inputAmount2 = InputAmount2;
        uiscript.Input1 = Input1;
        uiscript.Input2 = Input2;
        uiscript.buildingscript.craftingTime = currentRecipe.CraftingTime;
        uiscript.currentRecipe = currentRecipe;
    }
    public void CloseCraftingMenu()
    {
        if (craftingMenu != null)
        {
            Parent.gameObject.SetActive(true);
            craftingMenu.SetActive(false);
        }
        OutputName.text = string.Empty;
        crafttime.text = string.Empty;
        InputName1.text = string.Empty;
        InputName2.text = string.Empty;
        CraftMenu = false;
        uiscript.CraftMenu= CraftMenu;
        output.text = string.Empty;
        input1.text = string.Empty;
        input2.text = string.Empty;
    }
}
