using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewTerrain : MonoBehaviour
{
    public GameObject GroundPrefab; //Ground terrain.  
    public GameObject WaterPrefab; //Water.
    public GameObject SpawnerPrefab;
    public GameObject copperDeposit;
    public GameObject ironDeposit;
    private int _ironDepositCounter, _copperDepositCounter, _depositCounter = 5;
    
    public int cellSize = 1; //Width of the ground prefab.
    public const int size = 100; //Size of the array grid. Ex. 30x30.
    private bool[,] map = new bool[,] //The grid, where we place the prefabs.
    {
        { false, false, false, false }, //Test values. Are overridden by map = GenerateMap();
        { false, true, true, false },
        { false, true, true, false },
        { false, true, false, false }
   };
    private Vector3[,] SpawnerPos = new Vector3[size,size]; //Array to save the location of the terrain at a given grid location.
    private bool[,] IsSpawnerSpawned = new bool[size,size]; //Array to save spawner position. Need this to check for duplicates.

    private int rndSpawner;
    [HideInInspector]
    public List<SpawnEnemy> spawnerscript;
    // Start is called before the first frame update
    void Start()
    {
        map = GenerateMap(); //SAVE the generated map, whilst calling function.
        InstantiateMapObjects(); //After generating and saving the map array, spawn the terrain prefabs.
        instantiateSpawners(); //Spawn spawners after terrain generation.
        BakeNavMesh();
    }

    private void GenerateDeposits(int y, int x)
    {
        if (Random.Range(1,3) == 1 && _copperDepositCounter <= _depositCounter) //if rnd 1-2 = 1 AND the game hasn't spawned the max amount of said deposit
        { //Do stuff
            Instantiate(copperDeposit, new Vector3(x * cellSize, 0, y * cellSize), Quaternion.identity);
            _copperDepositCounter++; //Add to the counter to check for max
        }
        else if (_ironDepositCounter <= _depositCounter) //else spawn different deposit AND check for max amount.
        {
            Instantiate(ironDeposit, new Vector3(x * cellSize, 0, y * cellSize), Quaternion.identity);
            _ironDepositCounter++;
        }
        else //else else spawn ground
            Instantiate(GroundPrefab, new Vector3(x * cellSize, 0, y * cellSize), Quaternion.identity);

    }
    private void instantiateSpawners() //Spawn the spawners. HOX! NEEDS WORK.
    {
        for (int y = 0; y < map.GetLength(0); y++) //This for loop places the spawners in the edge of the spawned terrain with the help of the map[y,x] array.
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                rndSpawner = Random.Range(0, 10); //Throws a "dice" to decide if to place a spawner..
                if (map[y, x] == true && IsSpawnerSpawned[y,x] == false && rndSpawner <= 1) //Checks if the area is terrain and if there is a spawner there already.
                {
                    if (x != 0 && map[y, x - 1] == false || x != 0 && map[y, x + 1] == false || //Checks for the edge. If the area next to it is water, it is considered an edge.
                        x != 0 && map[y - 1, x] == false || x != 0 && map[y + 1, x] == false)
                    {
                        GameObject Spawner = Instantiate(SpawnerPrefab, SpawnerPos[y, x], Quaternion.identity); //Instantiates the spawner.
                        spawnerscript.Add(Spawner.GetComponent<SpawnEnemy>());
                        IsSpawnerSpawned[y, x] = true; //Marks the spot to prevent spawner duplicates.
                    }
                    else IsSpawnerSpawned[y, x] = false; //If the code decides not to place a spawner in the spot, it marks it so.
                }
            }
        }
    }

    private bool[,] GenerateMap() //Generate the map array.
    {
        bool[,] map = new bool[size,size]; //Define the map and save it.
        float[,] noiseMap = new float[size, size]; //Define the perlinnoise map.
        float xOffset = Random.Range(-10000f, 10000f); //Generate a random offset for x value between -10000 and 10000.
        float zOffset = Random.Range(-10000f, 10000f); //Same for the z value.

        for (int x = 0; x < size; x++) //Define the perlinnoise value for every array value.
        { 
            for (int z = 0; z < size; z++) //size variable is the array length and width (can be changed so that they are different values).
            {
                float noiseValue = Mathf.PerlinNoise(x * .1f + xOffset, z * .1f + zOffset); //Generate the perlinnoise value at [x, z] 
                noiseMap[x, z] = noiseValue; //Set generated perlinnoise value at [x, z] at the noisemap array.
            }
        }
        
        float[,] falloffMap = new float[size, size]; //Create a falloffmap array at with the size of the actual map grid.
        for (int x = 0; x < size; x++) //Same dance as above, do some calculations at said array value.
        { 
            for (int z = 0; z < size; z++)
            {
                float xv = x / (float)size * 2 - 1; //Change the const int of size to a float value so future math can work.
                float zv = z / (float)size * 2 - 1; //v as in vector?
                float v = Mathf.Max(Mathf.Abs(xv), Mathf.Abs(zv)); //Math :)
                falloffMap[x, z] = Mathf.Pow(v, 5f) / (Mathf.Pow(v, 5f) + Mathf.Pow(2.2f - 2.2f * v, 5f)); //Magic numbers that make it work.
            }
        }

        for (int x = 0; x < size; x++) //Generate the bool values for terrain spawning.
        {
            for (int z = 0; z < size; z++)
            {
                float noiseValue = noiseMap[x, z]; //Set the noise value as the values of the array.
                noiseValue -= falloffMap[x, z]; //Subtract the falloffmap values from the noisevalue.
                if (noiseValue < .3f) //.3 is the magic number. CAN be changed to have some more wacky terrain generation.
                {
                    map[z, x] = false; //If the noisevalue is smaller that the magic number, then set false.
                }
                else
                {
                    map[z, x] = true; //else set true.
                }
            }
        }

        return map; //Need to return the map value to SAVE it for later. Also because function is bool.
    }

    private void InstantiateMapObjects() //Instantiate terrain.
    {
        for (int y = 0; y < map.GetLength(0); y++) //array.GetLength(0) returns the size of the first dimension of the array.
        {                                          //GetLength is needed when we have a 2 dimension array [,]
            for (int x = 0; x < map.GetLength(1); x++) //array.GetLength(1) returns the size of the second dimension of the array. 
            {
                if (map[y, x] == true) //When said value of the array is true, then instantiate land.
                {
                    if (!(x != 0 && map[y, x - 1] == false || x != 0 && map[y, x + 1] == false || //Checks for the edge. If the area next to it is water, it is considered an edge.
                        x != 0 && map[y - 1, x] == false || x != 0 && map[y + 1, x] == false) && 1 > Random.Range(0, 101)) //Also with rnd. If number is below 1, then spawn deposit.
                        GenerateDeposits(y, x);
                    else 
                        Instantiate(GroundPrefab, new Vector3(x * cellSize, 0, y * cellSize), Quaternion.identity); //else just instantiate ground
                    SpawnerPos[y, x] = new Vector3(x * cellSize, 0, y * cellSize); //save the coords for the spawners
                }
                // else //Else instantiate water.
                //    Instantiate(WaterPrefab, new Vector3(x * cellSize, 0, y * cellSize), Quaternion.identity);
            }
        }
    }

    private void BakeNavMesh()
    {
        var sources = new List<NavMeshBuildSource>();
        Bounds b;
        b = new Bounds(new Vector3(0, 0, 0), new Vector3(1000, 1000, 1000));
        NavMeshBuilder.CollectSources(b, 1 << 9, NavMeshCollectGeometry.RenderMeshes, 0, new List<NavMeshBuildMarkup>(), sources);
        NavMeshData navmesh = NavMeshBuilder.BuildNavMeshData(NavMesh.GetSettingsByID(0), sources, new Bounds(Vector3.zero, new Vector3(10000, 10000, 10000)), Vector3.down, Quaternion.Euler(Vector3.up));
        NavMeshDataInstance NavMeshDataInstance = NavMesh.AddNavMeshData(navmesh);
    }
}