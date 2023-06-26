using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance {get; set;}
    InputController control;
    static PlayerInput PI;



    #region Variables
    public static Vector2 Move;
    public static Vector2 Aim;
    public static bool Attack;
    
    #endregion

    #region Initialize Script
    private void Awake()
    {
        MakeInstance();
        InitializeStufs();
    }
    void MakeInstance()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
        {
            instance = this;
        }
    }

    void InitializeStufs()
    {
        PI = this.GetComponent<PlayerInput>();
        control = new InputController();
        control.GamePlay.Move.performed += ctx => Move = ctx.ReadValue<Vector2>();
        control.GamePlay.Move.canceled += ctx => Move = Vector2.zero;

        control.GamePlay.Aim.performed += ctx => Aim = ctx.ReadValue<Vector2>();

        control.GamePlay.Attack.performed += ctx => Attack = true;
        control.GamePlay.Attack.canceled += ctx => Attack = false;
    }

    private void OnEnable()
    {
        control.GamePlay.Enable();
    }
    private void OnDisable()
    {
        control.GamePlay.Disable();
    }
    #endregion

    #region Public Function
    public static bool IsController()
    {
        return PI.currentControlScheme.Equals("GamePad") ? true : false;
    }
    #endregion
}
