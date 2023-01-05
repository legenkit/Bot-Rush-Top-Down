using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; set; }

    #region DATA
    [Header("REFERENCE")]
    public GameObject Player;

    
    #endregion

    #region Script Initialization
    private void Awake()
    {
        MakeInstance();
    }
    void MakeInstance()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        InitializeStuffs();
    }

    private void OnEnable()
    {

    }
    private void OnDisable()
    {

    }
    void InitializeStuffs()
    {
        Time.timeScale = 0;
    }
    #endregion

    #region Game Start Methords
    public void StartGame()
    {
        Time.timeScale = 1;
        InputManager.instance.enabled = true;
    }
    #endregion

    #region Event Methods
    
    #endregion
}
