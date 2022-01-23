using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ennemy"))
        {
            Debug.Log("Ennemy");
        }else if (other.CompareTag("Player"))
        {
            Debug.Log("Player");
        }
    }
}
