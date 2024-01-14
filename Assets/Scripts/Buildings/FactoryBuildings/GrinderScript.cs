using System.Collections;
using System.Collections.Generic;
using UnityEditor;
//using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.Animations;

public class GrinderScript : ProductionBuildingScript
{
    private Queue<Item> queue = new Queue<Item>(); //A "queue" for the grinding process.
    public float grindingTimer = 30f;
    // Start is called before the first frame update
    void Start()
    {
        name = "Grinder";
        MaxDurability = 80;
    }

    public override void ItemInInput(Collider other) //Adds the item to the queue.
    {
        ItemInfo worldItem = other.GetComponent<ItemInfo>(); //Get the Item info from the item and declare it as worldItem.
        if (worldItem != null) //Checks if there is an worldItem and that the item fits the recipe.
        {
            Debug.Log(worldItem);
            queue.Enqueue(worldItem.Item); //Add to the queue.

            // Destroy(worldItem); 
            Destroy(other.gameObject); //and destroy it from the world.
        }
    }

    protected override void ProcessRecipe()
    {
        if (queue.Count > 0)
        {
            grindingTimer -= Time.deltaTime; // countdown
            if (grindingTimer <= 0) //when countdown 0, do stuff
            {
                Item item = queue.Dequeue(); //Get the next item in queue and remove it afterward.
                Recipe recipe = RecipeConfig.Instance.GetRecipe(item.Type); //Get the recipe from the item type from the RecipeConfig instance.
                for (int i = 0; i < recipe.Requirements.Count; i++) //recipe.Requirements.Count = the amount of requirements in the recipe.
                {
                    int amount = 0; //Declare amount.
                    Recipe.RecipeRequirement requirement =  recipe.Requirements[i]; //Get the requirements from recipe.
                    var type = requirement.Type; //Get the itemType from the recipe.
                    if(requirement.NeededAmount == 1) //If the item amount is 1 then set the amount as 1.
                        amount = 1;
                    else //Else the item amount is greater than one, then do math.
                        amount = requirement.NeededAmount / 2; //Half the amount, for the grinder stuff :)
                
                    GameObject groundItem = ItemConfig.Instance.GetPrefab(type); //Get the ground item game object, that can be spawned.
                    
                    var worldItem = Instantiate(groundItem, output.position, Quaternion.identity).GetComponent<ItemInfo>(); //Instantiate the item.
                    worldItem.Item.Amount = amount; //Set the amount for the worldItem. CAN BE CHANGED FOR A FOR LOOP TO GET INDIVIDUAL PIECES.
               
                    
                }
                grindingTimer = 30f; //Reset the timer.
            }
        }

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update(); //Get the update of the BuildingSuperScript
        ProcessRecipe();
    }


}