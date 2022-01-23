using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayercontainerManager : MonoBehaviour
{
    private int offset = 30;
    void Update()
    {
        transform.position = Camera.main.transform.position + (Camera.main.transform.forward * offset) - Camera.main.transform.up;
        transform.rotation = Camera.main.transform.rotation;
    }
}
