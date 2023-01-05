using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] int connectedLevelIndex;
    [SerializeField] int connectedGateIndex;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") && LevelManager.instance._CurrentLevel.LevelCleared)
        {
            this.wait(RequestLevelChange, 0);
        }
    }
    void RequestLevelChange()
    {
        LevelManager.instance.ChangeLevel(connectedLevelIndex, connectedGateIndex);
    }
}
