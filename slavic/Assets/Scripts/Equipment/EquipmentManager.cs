using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentManager : MonoBehaviour 
{
    private bool isActive;
    private int currentWeapon;
    private EquipmentAI equipmentAI;
    private DirectProjectileWeapon[] directProjectileWeapons;

    void Start()
    {
        isActive = true;
        currentWeapon = 0;
        equipmentAI = GetComponent<EquipmentAI>();
        directProjectileWeapons = GetComponents<DirectProjectileWeapon>();
    }

    public DirectProjectileWeapon GetCurrentWeapon()
    {
        if (isActive)
        {
            if (directProjectileWeapons == null || directProjectileWeapons.Length <= 0)
            {
                return null;
            }
            return directProjectileWeapons[currentWeapon];
        }
        return null;
    }

    public void switchWeapon(bool increment)
    {
        if (increment)
        {
            currentWeapon++;
        }
        else
        {
            currentWeapon--;
        }
        if (currentWeapon > directProjectileWeapons.Length-1)
        {
            currentWeapon = 0;
        }
        else if (currentWeapon < 0)
        {
            currentWeapon = directProjectileWeapons.Length - 1;
        }
    }

    public EquipmentAI GetEquipmentAI()
    {
        return equipmentAI;
    }

    public void Activate()
    {
        isActive = true;
    }

    public void Deactivate()
    {
        isActive = false;
    }
}
