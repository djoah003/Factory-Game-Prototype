using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

public class UIScript : MonoBehaviour
{
    [HideInInspector]
    public ProductionBuildingScript buildingscript;
    [HideInInspector]
    public DragNDrop dragscript;
    private GameObject Camera;
    private bool HasItem;
    private bool RecipeAdded = false;
    [HideInInspector]
    public bool RecipesBuilt = false;
    private bool ItemAdded = false;
    [HideInInspector]
    public bool ItemsBuilt = false;
    private float MaxDurability;
    private float CurrentDurability;
    private string MachineName;
    private TextMeshProUGUI machineName, itemsneeded1, itemsneeded2;
    private BuildingInventory inventoryscript;
    private List<Recipe> recipes;
    private List<Item> items;
    private List<ItemType> itemTypes;
    private GameObject MachineInventory;
    private GameObject RecipeParent;
    [HideInInspector]
    public InventorySlot[] Slots;
    [HideInInspector]
    public RecipeSlot[] RecipeSlots;
    [HideInInspector]
    public GameObject UI;
    private Image DurabilityBar;
    [HideInInspector]
    public GameObject craftingMenu;
    [HideInInspector]
    public float craftingTimer;
    [HideInInspector]
    public float MaxCraftTimer;
    [HideInInspector]
    public bool CraftMenu;
    private bool hasItem;
    private ItemType Null;
    private Image CraftTime, itemfillamount1, itemfillamount2;
    [HideInInspector]
    public int inputAmount1, inputAmount2;
    private int itemAmount1, itemAmount2;
    [HideInInspector]
    public ItemType Input1, Input2;
    [HideInInspector]
    public Recipe currentRecipe = null;
    // Start is called before the first frame update
    void Start()
    {
        UI = GameObject.Find("Background");
        Camera = GameObject.Find("MainCam");
        dragscript = Camera.GetComponent<DragNDrop>();
        Slots = GameObject.Find("Machine Inventory").GetComponentsInChildren<InventorySlot>();
        RecipeSlots = GameObject.Find("Recipes").GetComponentsInChildren<RecipeSlot>();
        machineName = GameObject.Find("Machine Name").GetComponent<TextMeshProUGUI>();
        itemTypes = new List<ItemType>();
        DurabilityBar = GameObject.Find("Durability").GetComponent<Image>();
        craftingMenu = GameObject.Find("Crafting");
        CraftTime = GameObject.Find("move this").GetComponent<Image>();
        itemsneeded1 = GameObject.Find("ItemsNeeded1").GetComponent<TextMeshProUGUI>();
        itemsneeded2 = GameObject.Find("ItemsNeeded2").GetComponent<TextMeshProUGUI>();
        itemfillamount1 = GameObject.Find("ItemFillAmount1").GetComponent<Image>();
        itemfillamount2 = GameObject.Find("ItemFillAmount2").GetComponent<Image>();
        UI.gameObject.SetActive(false);
        craftingMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Confined;
        if (dragscript.ShowUI == true && dragscript._object != null)
        {
            BuildUI();
        }
    }
    private void BuildUI()
    {
        
        buildingscript = dragscript._object.GetComponentInParent<ProductionBuildingScript>();
        if (buildingscript == null) return;
        UI.gameObject.SetActive(true);
        MaxDurability = buildingscript.MaxDurability;
        CurrentDurability = buildingscript.Currentdurability;
        MachineName = buildingscript.name;
        inventoryscript = buildingscript.Inventory;
        items = inventoryscript.Items;
        recipes = buildingscript.recipes;
        machineName.text = MachineName;

        Inventory();
        Recipes();

        DurabilityBar.fillAmount = CurrentDurability / MaxDurability;
        craftingTimer = buildingscript.craftingTime;
        if (CraftMenu == true)
        {
            CraftTime.fillAmount = craftingTimer / MaxCraftTimer;
            itemsneeded1.text = itemAmount1.ToString() + "/" + inputAmount1;
            itemsneeded2.text = itemAmount2.ToString() + "/" + inputAmount2;
            if(inputAmount1 != 0)
                itemfillamount1.fillAmount = itemAmount1 / inputAmount1;
            if(inputAmount2 != 0)
                itemfillamount2.fillAmount = itemAmount2 / inputAmount2;
            craftingMenu.SetActive(true);
        }
    }
    private void Inventory()
    {
        foreach (var item in items)
        {
            foreach (var itemType in itemTypes)
            {
                if (itemType == item.Type)
                {
                    HasItem = true;
                }
            }
            if (inventoryscript.HasItem(item.Type) && HasItem == true)
            {
                continue;
            }
            else
            {
                itemTypes.Add(item.Type);
            }
            HasItem = false;
        }
        if (ItemsBuilt == false)
        {
            foreach (var type in itemTypes)
            {
                var ItemAmount = inventoryscript.GetItemAmount(type);
                for (int i = 0; i < Slots.Length; i++)
                {
                    if (Slots[i].hasItem == false && ItemAdded == false)
                    {
                        ItemAdded = true;
                        Slots[i].addItem(type, ItemAmount);
                    }
                    else
                    {
                        Slots[i].addItem(0, 0);
                    }
                }
                ItemAdded = false;
            }
            ItemsBuilt= true;
        }
        foreach (var type in itemTypes) {
            var ItemAmount = inventoryscript.GetItemAmount(type);
            for (int i = 0; i < Slots.Length; i++)
            {
                var itemAmount = Slots[i].itemAmount;
                var ItemType = Slots[i].itemType;
                if (ItemType == Input1)
                {
                    itemAmount1 = itemAmount;
                }
                else if (ItemType == Input2)
                {
                    itemAmount2 = itemAmount;
                }
                if (Slots[i].itemAmount != ItemAmount && Slots[i].itemType == type)
                {
                    Slots[i].addItem(type, ItemAmount);
                }
                for (int z = 0; z < Slots.Length; z++)
                {
                    if (Slots[z].itemType == type)
                    {
                        hasItem = true;
                    }
                }
                if (Slots[i].itemType != type && hasItem == false)
                {
                    Slots[i].addItem(type, ItemAmount);
                }
            }
            hasItem = false;
        }
    }
    private void Recipes()
    {
        if(RecipesBuilt == false) 
        {
            foreach (var recipe in recipes)
            {
                for (int i = 0; i < RecipeSlots.Length; i++)
                {
                    RecipeSlots[i].Slots = Slots;
                    if (RecipeSlots[i].HasRecipe == false && RecipeAdded == false)
                    {
                        RecipeAdded = true;
                        RecipeSlots[i].addRecipe(recipe.name, recipe);
                    }
                }
                RecipeAdded = false;
            }
            for (int i = 0; i < RecipeSlots.Length; i++)
            {
                if (RecipeSlots[i].HasRecipe == false)
                {
                    RecipeSlots[i].addRecipe(null, null);
                }
            }
            RecipesBuilt= true;
        }
        buildingscript.currentRecipe = currentRecipe;
    }
    public void DestroyUI()
    {
        UI.gameObject.SetActive(false);
        RecipesBuilt= false;
        ItemsBuilt= false;
    }
}
