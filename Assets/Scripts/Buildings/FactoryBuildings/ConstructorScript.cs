using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ConstructorScript : ProductionBuildingScript
{
    // Start is called before the first frame update
    void Start()
    {
        name = "Constructor";
        MaxDurability = 60;
    }

    public override void Update()
    {
        base.Update();
        ProcessRecipe();
    }
}