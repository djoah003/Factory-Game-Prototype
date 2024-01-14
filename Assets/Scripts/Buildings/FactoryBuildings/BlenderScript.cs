using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlenderScript : ProductionBuildingScript
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