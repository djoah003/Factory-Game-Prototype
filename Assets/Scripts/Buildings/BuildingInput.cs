using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BuildingInput : MonoBehaviour //THIS SCRIPT IN A NUTSHELL IS THE BUILDINGS INPUT
{
    private ProductionBuildingScript ParentScript;
    private void Awake()
    {
       ParentScript = this.transform.parent.GetComponent<ProductionBuildingScript>();
    }

        
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("TriggerHasHappened");
        ParentScript.ItemInInput(other);
    }
}
