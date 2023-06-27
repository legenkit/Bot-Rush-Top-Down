using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class MortarRotation : MonoBehaviour
{


    #region Variable
    [Header("REFERENCE")]
    [SerializeField] GameObject Bullet;
    [SerializeField] Transform PipePivot;
    [SerializeField] Transform ShootPoint;

    [Header("DATA")]
    [SerializeField] float RotationSpeed;
    [SerializeField] float firingAngle;
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
        if (LevelManager.instance.LevelStarted && !DataManager.instance.PlayerDead && Vector3.Distance(transform.position, Player.position) < ShootRange && CanShoot && bulletleft > 0)
        {
            CanShoot = false;
            this.wait(Shoot, ShootDelay);
        }
    }

    private void FixedUpdate()
    {
        if (bulletleft == 0 && !Reloading)
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

    #endregion


    #region Shoot & Reload
    void Shoot()
    {
        CanShoot = true;

        var ray = new Ray(ShootPoint.position, (Player.position - transform.position).normalized);
        if (Physics.Raycast(ray, out RaycastHit info, ShootRange) && info.collider.CompareTag("Player"))
        {
            AudioManager.instance.PlayAudioClip(AudioManager.AudioType.Shoot);
            StartCoroutine(SimulateProjectile(Player.position));
/*
            Rigidbody bullet = Instantiate(Bullet, ShootPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            bullet.AddForce(CalulateDirection(Player.position, ShootPoint.position), ForceMode.Impulse);
            bullet.transform.GetComponent<Damager>().SetforPlayer();
            Destroy(bullet.gameObject, 2);*/
        }
        bulletleft--;
    }

    void Reload()
    {
        Reloading = false;
        bulletleft = BulletLimit;
    }
    #endregion

    #region Claculation

    IEnumerator SimulateProjectile(Vector3 Target)
    {
        Transform Projectile = Instantiate(Bullet, ShootPoint.position, Quaternion.identity).transform;
        Destroy(Projectile.gameObject, 4);
        // Move projectile to the position of throwing object + add some offset if needed.
        Projectile.position = ShootPoint.position + new Vector3(0, 0.0f, 0);

        // Calculate distance to target
        float target_Distance = Vector3.Distance(Projectile.position, Target);

        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / Physics.gravity.magnitude);

        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        // Calculate flight time.
        float flightDuration = target_Distance / Vx;

        // Rotate projectile to face the target.
        Projectile.rotation = Quaternion.LookRotation(Target - Projectile.position);
        float elapse_time = 0;

        while (elapse_time < flightDuration)
        {
            if(!Projectile)
            { 
                break; 
            }
            Projectile.Translate(0, (Vy - (Physics.gravity.magnitude * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

            elapse_time += Time.deltaTime;

            yield return null;
        }
        if (Projectile)
        {
            Projectile.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    #endregion

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.position, ShootRange);
    }
}
