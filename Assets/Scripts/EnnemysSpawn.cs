using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemysSpawn : MonoBehaviour
{
    public EnnemyGroup[] ennemysGroup;
    public GameObject minesField;
    public GameObject bonuses;

    public void SpawnEnnemies()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        Array.ForEach(ennemysGroup, e => StartCoroutine(nameof(SpawnEnemyGroup), e));
    }

    public void SpawnMinesFields()
    {
        Instantiate(minesField, transform.position, transform.rotation, transform);
    }

    public void SpawnBonuses()
    {
        Instantiate(bonuses, transform.position, transform.rotation, transform);
    }

    IEnumerator SpawnEnemyGroup(EnnemyGroup group)
    {
        yield return new WaitForSeconds(group.timeToWait);
        Instantiate(group.group, transform.position, transform.rotation, transform);
    }
}

[Serializable]
public class EnnemyGroup
{
    public GameObject group;
    public float timeToWait;
}
