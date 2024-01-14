using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public GameObject ghost;
    public void SpawnGhost()
    {
        Instantiate(ghost);
    }
}
