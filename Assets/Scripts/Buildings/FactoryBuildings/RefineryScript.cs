using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefineryScript : ProductionBuildingScript
{
    // Start is called before the first frame update
    void Start()
    {
        name = "Refinery";
        MaxDurability = 40;
    }

    public override void Update()
    {
        base.Update();
        ProcessRecipe();
    }
}
