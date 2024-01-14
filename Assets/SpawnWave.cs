using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Animations;
using UnityEngine;

public class SpawnWave : MonoBehaviour
{
    private NewTerrain terrainscript;
    void Start()
    {
        terrainscript = GameObject.Find("GameManager").GetComponent<NewTerrain>();
    }

    public void Spawn()
    {
        for (int i = 0; i < terrainscript.spawnerscript.Count; i++)
        {
            terrainscript.spawnerscript[i].Spawn = true;
        }
    }
}
