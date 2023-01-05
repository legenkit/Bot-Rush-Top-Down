using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHandeler : MonoBehaviour
{
    [SerializeField] Image[] Units;
    [SerializeField] int UnitRemain;

    private void Awake()
    {
        UnitRemain = Units.Length;
    }

    public void UpdateUnit(int health)
    {
        if (health > 0)
        {
            if (health > UnitRemain) health = UnitRemain;
            UnitRemain -= health;

            for (int i = 0; i < health; i++)
            {
                Units[UnitRemain + i].enabled = false;
            }
            return;
        }


        if (UnitRemain + health > Units.Length) health = Units.Length - UnitRemain;
        UnitRemain -= health;
        for (int i = 0; i < Mathf.Abs(health); i++)
        {
            Units[UnitRemain - i - 1].enabled = true;
        }

    }

    public void ActivateAllUnits()
    {
        for (int i = 0; i < Units.Length; i++)
        {
            Units[i].enabled = true;
        }
        UnitRemain = Units.Length;
    }


}
