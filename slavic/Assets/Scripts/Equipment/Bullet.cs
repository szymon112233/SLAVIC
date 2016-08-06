using UnityEngine;
using System.Collections;

/**
 * Opisuje typowy pocisk kierowany siłami fizycznymi.
 * */
public class Bullet : MonoBehaviour 
{
	public float damage;
	public float AOE;
	public float initialForce;
	private float flyingTime;
	public float flyingTimeLimit;

    private TeamControll teamManager;

	void Start () 
	{
        GameplayManager gameplayManager = FindObjectOfType<GameplayManager>();
        teamManager = gameplayManager.teamManager;
		flyingTime = 0f;
	}

	void Update () 
	{
		flyingTime += Time.deltaTime;
		if(flyingTime >= flyingTimeLimit)
		{
			detonate();
		}
	}

	void OnTriggerEnter(Collider collider)
	{
        if (teamManager != null)
        {
            if (teamManager.IsFriendly(gameObject, collider.gameObject))
            {
                //przelatuje przez sojuszników
                return;
            }
        }
        //detonuje
		detonate (collider.gameObject);
	}

	/**
	 * Detonuje pocisk.
	 * */
	private void detonate()
	{
		detonate (null);
	}

	private void detonate(GameObject collidedGameObject)
	{
		if(collidedGameObject != null)
		{
			//zranienie trafionego celu
            if (!teamManager.IsFriendly(gameObject, collidedGameObject))
			{
				if(collidedGameObject.GetComponent<Health>() != null)
				{
					collidedGameObject.GetComponent<Health>().ApplyChange(-damage);
				}
			}

			//wybuch AOE po zetknięciu z celem
			Collider[] collidersInRange = Physics.OverlapSphere(transform.position, AOE);
			if(collidersInRange != null && collidersInRange.Length > 0)
			{
				for(int i=0; i<collidersInRange.Length; i++)
				{
                    if (collidedGameObject != collidersInRange[i].gameObject && !teamManager.IsFriendly(gameObject, collidersInRange[i].gameObject))
					{
						if(collidersInRange[i].gameObject.GetComponent<Health>() != null)
						{
							collidersInRange[i].gameObject.GetComponent<Health>().ApplyChange(-damage);
						}
					}
				}
			}
		}
		else
		{
			//wybuch bez zetknięcia z celem
			Collider[] collidersInRange = Physics.OverlapSphere(transform.position, AOE);
			if(collidersInRange != null && collidersInRange.Length > 0)
			{
				for(int i=0; i<collidersInRange.Length; i++)
				{
                    if (!teamManager.IsFriendly(gameObject, collidersInRange[i].gameObject))
					{
						if(collidersInRange[i].gameObject.GetComponent<Health>() != null)
						{
							collidersInRange[i].gameObject.GetComponent<Health>().ApplyChange(-damage);
						}
					}
				}
			}
		}
		Destroy(gameObject); 
	}

	public void setDamage(float newDamage)
	{
		damage = newDamage;
	}

	public void setAOE(float newAOE)
	{
		AOE = newAOE;
	}

	public void setSpeed(float newInitialForce)
	{
		initialForce = newInitialForce;
	}

	public float getFiringForce()
	{
		return initialForce;
	}

	public void setFlyingTimeLimit(float newFlyingTimeLimit)
	{
		flyingTimeLimit = newFlyingTimeLimit;
	}
}
