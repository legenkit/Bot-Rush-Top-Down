using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankTop : MonoBehaviour
{
    #region Reference Var
    [SerializeField] UnitHandeler BulletUnit;
    [SerializeField] Camera cam;
    [SerializeField] GameObject Bullet;
    [SerializeField] Transform ShootPoint;
    #endregion

    #region Data

    [SerializeField] float RotationSpeed;
    [SerializeField] float ShootForce;
    [SerializeField] float ShootInterval=1;
    [SerializeField] float ReloadingTime;
    [SerializeField] float SelfReloadingTime = .5f;
    [SerializeField] float SelfReloadStartTime = 2;
    #endregion

    #region Local Var

    Vector2 mousepos;
    Vector2 tankPos;
    Vector2 direction;
    Vector3 ShootDir;
    Vector3 RotationDamping;

    int bulletLimit = 10;
    int BulletLeft = 10;

    float c_SelfReloadStartTime;
    float angle;
    bool ReadyToShoot = true;
    bool reloading = false;
    #endregion

    #region Script Updation

    void Update()
    {
        RotateTop();
        if(InputManager.Attack && ReadyToShoot && BulletLeft > 0)Shoot();
    }

    private void FixedUpdate()
    {
        if (c_SelfReloadStartTime > 0)
        {
            c_SelfReloadStartTime -= Time.deltaTime;
        }
        else if (!reloading && BulletLeft < bulletLimit)
        {
            reloading = true;
            this.wait(Reload, SelfReloadingTime);
        }
    }

    void RotateTop()
    {
        if (InputManager.IsController())
        {
            direction = InputManager.Aim;
        }
        else
        {
            mousepos = Mouse.current.position.ReadValue();
            tankPos = cam.WorldToScreenPoint(transform.position);
            direction = (mousepos - tankPos).normalized;
        }

        ShootDir = Vector3.SmoothDamp(ShootDir, new Vector3(direction.x, 0, direction.y), ref RotationDamping, RotationSpeed);
        transform.rotation = Quaternion.LookRotation(ShootDir);
    }

    void Shoot()
    {
        ReadyToShoot = false;
        AudioManager.instance.PlayAudioClip(AudioManager.AudioType.Shoot);
        CameraManager.instance.Shake(.1f, .1f);

        Rigidbody bullet = Instantiate(Bullet, ShootPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        Vector3 ShootDir = new Vector3(direction.x, 0, direction.y);
        bullet.AddForce(ShootDir * ShootForce, ForceMode.Impulse);
        Destroy(bullet.gameObject, 2);


        c_SelfReloadStartTime = SelfReloadStartTime;
        BulletLeft--;
        BulletUnit.UpdateUnit(1);

        this.wait(ResetShoot, ShootInterval);
    }

    void ResetShoot()
    {
        ReadyToShoot = true;
    }

    void Reload()
    {
        reloading = false;
        BulletLeft++;
        BulletUnit.UpdateUnit(-1);
    }
    public void ReloadAll()
    {
        reloading = false;
        BulletLeft = bulletLimit;
        BulletUnit.ActivateAllUnits();
    }
    #endregion

}
