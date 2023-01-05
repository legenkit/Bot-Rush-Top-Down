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
    #endregion
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            if (other.transform.GetComponent<Stats>() != null)
            {
                other.GetComponent<Stats>().TakeDamage(Damage);
            }
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

    private void Start()
    {
        Physics.queriesHitTriggers = true;
    }
}
