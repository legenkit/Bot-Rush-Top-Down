using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DataManager : MonoBehaviour
{
    public static DataManager instance { get; set; }

    [Header("UI")]
    [SerializeField] TextMeshProUGUI EnemyText;

    #region DATA
    [Header("REFERENCE")]
    public GameObject Player;

    [Header("Game Data")]
    public int EnemyCount = 0;

    [Header("Game Status")]
    public bool PlayerDead;

    #endregion

    #region Script Initialization
    private void Awake()
    {
        MakeInstance();
        EventSubscription();
    }
    void MakeInstance()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void EventSubscription()
    {
        EventManager.EnemyCount += UpdateEnemyCount;
    }
    private void OnDisable()
    {
        EventManager.EnemyCount -= UpdateEnemyCount;
    }
    #endregion

    #region Event Method
    void UpdateEnemyCount(int i)
    {
        EnemyCount += i;
        EnemyText.SetText($"ENEMY = {EnemyCount}");
        if (EnemyCount == 0 && EventManager.LevelCleared != null)
            EventManager.LevelCleared();
    }
    #endregion
}
