using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{    
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
            LevelManager.instance.StartLevel();
    }
}
