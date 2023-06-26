using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Stats : MonoBehaviour
{
    public enum Type { Player , Enemy}
    [SerializeField] Type TankType;
    [SerializeField] Stats Body;
    [SerializeField] int Health = 10;

    [Header("Death")]
    [SerializeField] GameObject DeathParticle;
    [SerializeField] GameObject HealthText;
    [SerializeField] Color32 HealthTextColor;

    public UnitHandeler H_unitHandler;
    public void TakeDamage(int damage)
    {
        TextMesh text = Instantiate(HealthText, transform.position, Quaternion.Euler(50,0,0)).GetComponent<TextMesh>();
        text.color = HealthTextColor;
        text.transform.DOMoveY(2, .15f).SetEase(Ease.Linear);
                
        if (Body == null)
        {
            Health -= damage;
                        
            text.text = Health.ToString();
            if (Health <= 0)
                Death();
        }
        else
        {
            Body.Health -= damage;

            if (TankType == Type.Player)
            {
                Body.H_unitHandler.UpdateUnit(damage);
            }

            text.text = Body.Health.ToString();
            if (Body.Health <= 0)
                Body.Death();
        }
        
        Destroy(text.gameObject,.2f); 
    }

    void Death()
    {
        AudioManager.instance.PlayAudioClip(AudioManager.AudioType.TankExplode);
        GameObject particle = Instantiate(DeathParticle, transform.position, Quaternion.identity);
        Destroy(particle, 2);

        CameraManager.instance.Shake(2f, 2f);
        if (TankType == Type.Enemy)
        {
            Destroy(this.gameObject);
            return;
        }

        this.transform.GetComponent<TankMovement>().enabled = false;
        this.transform.GetComponent<MeshRenderer>().enabled = false;
        this.transform.GetChild(0).gameObject.SetActive(false);
        DataManager.instance.PlayerDead = true;
        this.wait(LevelManager.instance.RestartLevel, 1f);

    }
}
