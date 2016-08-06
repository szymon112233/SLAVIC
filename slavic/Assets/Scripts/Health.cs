﻿using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    public GameObject corpse;    

    public float MAX_HEALTH;
    private float currentHealth;
    private bool alive;
    public float REGENERATION;

    private FXManager fxManager;
    private FurbyAnimatorScript animatorScript;

    void Awake()
    {
        fxManager = GetComponent<FXManager>();
        animatorScript = GetComponent<FurbyAnimatorScript>();
    }

    void Start()
    {
        currentHealth = MAX_HEALTH;
        alive = true;
    }

    void Update()
    {
        if (alive)
        {
            if (REGENERATION != 0)
            {
                ApplyChange(REGENERATION * Time.deltaTime);
            }
        }
    }

    public void ApplyChange(float change)
    {
        if (alive)
        {
            currentHealth += change;
            if (currentHealth > MAX_HEALTH)
            {
                currentHealth = MAX_HEALTH;
            }
            else if (currentHealth <= 0)
            {
                currentHealth = 0;
                alive = false;
                if (corpse != null)
                {
                    Vector3 up = new Vector3(transform.position.x, 10, transform.position.z);
                    Vector3 directionToAim = (transform.position - up).normalized;
                    Quaternion aimRotation = Quaternion.LookRotation(directionToAim);
                    Instantiate(corpse, transform.position, aimRotation);
                }
            }
            if (change < 0)
            {
                fxManager.PlayHurtClip();
                //TODO: Jacek efekty zranienia

                fxManager.PlayBloodSplatter();

                animatorScript.PlayHurtAnimation();
            }
            else
            {
                fxManager.PlayHealClip();
            }
        }
    }

    public bool IsAlive()
    {
        return alive;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
