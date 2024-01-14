using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmelterScript : ProductionBuildingScript
{
    // Start is called before the first frame update
    void Start()
    {
        name = "Smelter";
        MaxDurability = 40; //Durability of the smelter.
    }

    public override void Update()
    {
        base.Update();
        ProcessRecipe();
    }
}
