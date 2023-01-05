using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyMovement : MonoBehaviour
{
    #region VARIABLE
    [Header("DATA")]
    [SerializeField] float Speed;
    [SerializeField] List<Transform> Targets;

    Vector3 CurrentTar;
    NavMeshAgent Agent;
    GameObject Player;

    #endregion

    #region Script Initialization
    private void Start()
    {
        InitializeStufs();
        EnableActions();
    }
    void InitializeStufs()
    {
        Agent = this.GetComponent<NavMeshAgent>();
        Agent.speed = Speed;

        Player = DataManager.instance.Player;
        CurrentTar = transform.position;
    }

    private void EnableActions()
    {
        if (EventManager.EnemyCount != null)
            EventManager.EnemyCount(1);
    }
    private void OnDisable()
    {
        if (EventManager.EnemyCount != null)
            EventManager.EnemyCount(-1);
    }
    #endregion

    #region Script Updation
    private void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, CurrentTar);
        if(LevelManager.instance.LevelStarted && !DataManager.instance.PlayerDead && distance < 1f)
        {
            Agent.SetDestination(CurrentTar = FindNewDestination());
        }
    }

    Vector3 FindNewDestination()
    {
        Vector3 nextTar = CurrentTar;

        while(nextTar == CurrentTar)
        {
            nextTar = Targets[Random.Range(0, Targets.Count)].position;
        }
        return nextTar;
    }
    #endregion
}
