using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public GameObject corpse;

    public float MAX_HEALTH;
    private float currentHealth;
    private bool alive;
    public float REGENERATION;

    private FXManager fxManager;
    private FurbyAnimatorScript animatorScript;
    private Slider slider;

    void Awake()
    {
        fxManager = GetComponent<FXManager>();
        animatorScript = GetComponent<FurbyAnimatorScript>();
    }

    void Start()
    {
        currentHealth = MAX_HEALTH;
        alive = true;
        slider = GetComponentInChildren<Slider>(); //We get a health slider, and set its start state.
        if (slider != null)
        {
            slider.value = currentHealth;
            slider.gameObject.SetActive(false);
        }
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
            if (slider != null)
            {
                slider.gameObject.SetActive(true);   //Turn on the health slider and change its value.
                slider.value = currentHealth;
            }
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
                    Instantiate(corpse, transform.position, transform.rotation);
                }
            }
            if (change < 0)
            {
                if (fxManager != null)
                {
                    fxManager.PlayHurtClip();
                    fxManager.PlayBloodSplatter();
                }

                if (animatorScript != null)
                {
                    animatorScript.PlayHurtAnimation();
                }
            }
            else
            {
                if (fxManager != null)
                {
                    fxManager.PlayHealClip();
                }
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
