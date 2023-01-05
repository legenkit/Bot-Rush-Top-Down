using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotation : MonoBehaviour
{

    #region Variable
    [Header("REFERENCE")]
    [SerializeField] GameObject Bullet;
    [SerializeField] Transform ShootPoint;

    [Header("DATA")]
    [SerializeField] float RotationSpeed;
    [SerializeField] float ShootRange;
    [SerializeField] float ShootForce;
    [SerializeField] float ShootDelay;
    [SerializeField] float ReloadingTime;
    [SerializeField] int BulletLimit;

    private int bulletleft;
    private Transform Player;
    private Vector3 Direction;
    private Vector3 CurrentDirection;
    private bool CanShoot = true;
    private bool Reloading;
    #endregion

    #region Script Initialization
    void Start()
    {
        InitializeStufs();
    }
    private void InitializeStufs()
    {
        bulletleft = BulletLimit;
        Player = DataManager.instance.Player.GetComponent<Transform>();
    }

    #endregion

    #region Script Updation
    void Update()
    {
        RotateTop();
        if(LevelManager.instance.LevelStarted && !DataManager.instance.PlayerDead && Vector3.Distance(transform.position,Player.position) < ShootRange && CanShoot && bulletleft > 0)
        {
            CanShoot = false;
            this.wait(Shoot, ShootDelay);
        }
    }

    private void FixedUpdate()
    {
        if(bulletleft==0 && !Reloading)
        {
            Reloading = true;
            this.wait(Reload, ReloadingTime);
        }
    }
    void RotateTop()
    {
        Direction = (Player.position - transform.position).normalized;
        Direction.y = 0;
        CurrentDirection = Vector3.Lerp(CurrentDirection, Direction, RotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(CurrentDirection);
    }
    void Shoot()
    {
        CanShoot = true;

        var ray = new Ray(ShootPoint.position, (Player.position - transform.position).normalized);
        if(Physics.Raycast(ray , out RaycastHit info , ShootRange) && info.collider.CompareTag("Player"))
        {
            AudioManager.instance.PlayAudioClip(AudioManager.AudioType.Shoot);
            Rigidbody bullet = Instantiate(Bullet, ShootPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            bullet.AddForce(CurrentDirection * ShootForce, ForceMode.Impulse);
            Destroy(bullet.gameObject, 2);
        }
        bulletleft--;
    }

    void Reload()
    {
        Reloading = false;
        bulletleft = BulletLimit;
    }
    #endregion

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.position, ShootRange);
    }

}
