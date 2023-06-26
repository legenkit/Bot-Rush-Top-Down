using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateHandler : MonoBehaviour
{
    public Animator[] Gates;

    public void OpenGate()
    {
        foreach (Animator anim in Gates)
        {
            anim.SetBool("Open", true);
        }
    }
    public void CloseGate()
    {
        if (!LevelManager.instance._CurrentLevel.LevelCleared)
        {
            foreach (Animator anim in Gates)
            {
                anim.SetBool("Open", false);
            }
        }        
    }

    public void LoadLevel(int level_index)
    {

    }
}
