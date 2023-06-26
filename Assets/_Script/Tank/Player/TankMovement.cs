using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    #region REFERENCE VAR
    Rigidbody RB;
    #endregion

    #region Data
    [SerializeField] float Speed;
    [SerializeField] float DampSpeed = .2f;
    [SerializeField] float RotationSpeed;
    #endregion

    #region Local Var
    Vector2 Move;
    Vector2 CurrentMove;
    Vector2 DampVelocity;
    Vector2 DampRotation;
    float Direction;
    Vector2 Rotation;
    Vector2 CurrentRotation;
    #endregion

    #region Script Initialization
    private void Start()
    {
        AllocateStufs();
    }
    void AllocateStufs()
    {
        RB = this.GetComponent<Rigidbody>();
    }
    #endregion

    #region Script Updation
    void Update()
    {
        MoveTank();
        RotateTank();
    }

    void MoveTank()
    {
        Move = InputManager.Move.normalized;
        CurrentMove = Vector2.SmoothDamp(CurrentMove, Move, ref DampVelocity, DampSpeed);
        
        RB.velocity = new Vector3(CurrentMove.x * Speed, 0, CurrentMove.y * Speed);
    }
    void RotateTank()
    {
        if (InputManager.Move.normalized.magnitude > .9f)
        {
            Rotation = new Vector2(-CurrentMove.x,CurrentMove.y);
            CurrentRotation = Vector2.SmoothDamp(CurrentRotation, Rotation, ref DampRotation, RotationSpeed);
        }
        Direction = Mathf.Atan2(CurrentRotation.y, CurrentRotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, Direction, 0);
    }

    public void TeleportTank(Vector3 pos)
    {
        GetComponentInChildren<TrailRenderer>().enabled = false;
        Vector3 Pos = new Vector3(pos.x, 0.25f, pos.z);
        transform.position = Pos;
        GetComponentInChildren<TrailRenderer>().enabled = true;
    }
    
    #endregion
}
