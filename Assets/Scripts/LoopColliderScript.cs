using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopColliderScript : MonoBehaviour
{
    public EnnemysSpawn ennemySpawn;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ennemySpawn.SpawnEnnemies();
            ennemySpawn.SpawnMinesFields();
            ennemySpawn.SpawnBonuses();
        }
    }
}
