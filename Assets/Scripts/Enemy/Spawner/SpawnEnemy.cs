using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{

    public GameObject Enemy = null; //Enemy (e.g Bug), that we want the spawner to spawn.
    public int EnemySpawnAmount = 0; //Tells the amount of spawned enemies currently on the map.
    public bool Spawn;
    public float SpawnTimer = 0;
    private int CurrentAmount = 0;
    private bool SpawnStarted = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {
        if (Spawn && CurrentAmount != EnemySpawnAmount && SpawnStarted == false)
            StartCoroutine("SpawnEnemyTimer");
        if (!Spawn)
            StopCoroutine("SpawnEnemyTimer");
    }

   public IEnumerator SpawnEnemyTimer()
    {
        SpawnStarted= true;
        for (int i = 0; i < EnemySpawnAmount; i++)//While the Enemy amount is under 10.
        {
            Instantiate(Enemy, this.gameObject.transform.position, this.gameObject.transform.rotation); //Should spawn enemies on spawner coordinates.
            CurrentAmount = i;
            yield return new WaitForSeconds(SpawnTimer); //Delay for if functions.
        }
        Spawn = false;
        SpawnStarted = false;
        CurrentAmount = 0;
    }
}
