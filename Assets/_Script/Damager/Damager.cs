using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    #region Data
    [Header("Reference")]
    [SerializeField] GameObject CollisionParticle;

    [Header("Data")]
    [SerializeField] int Damage;

    [SerializeField] bool _forPlayer = false;
    #endregion


    private void Start()
    {
        Physics.queriesHitTriggers = true;
    }

    public void SetforPlayer()
    {
        _forPlayer = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Player") && _forPlayer) || (other.CompareTag("Enemy") && !_forPlayer))
        {
            if (other.transform.GetComponent<Stats>() != null)
            {
                other.GetComponent<Stats>().TakeDamage(Damage);
            }
        }

        if(other.CompareTag("Enemy") && _forPlayer)
        {
            return;
        }

        DestroySelf();
    }

    void DestroySelf()
    {
        CameraManager.instance.Shake(.3f, .3f);
        Destroy(this.gameObject);
    }
    private void OnDestroy()
    {
        //AudioManager.instance.PlayAudioClip(AudioManager.AudioType.BulletExplode);
        GameObject particle = Instantiate(CollisionParticle, transform.position, Quaternion.identity);
        Destroy(particle, .5f);
    }

}
