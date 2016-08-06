using UnityEngine;
using System.Collections;

/**
 * Opisuje typowy pocisk kierowany siłami fizycznymi.
 * */
public class Bullet : MonoBehaviour 
{
	private GameObject owner;
	private string ownerTag;
	public float damage;
	public float AOE;
	public float initialForce;
	private float flyingTime;
	public float flyingTimeLimit;

	void Start () 
	{
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
		if(collider.gameObject == owner)
		{
			//nie ma kolizji z samym sobą
			return;
		}
		if(collider.gameObject.tag == "Projectile")
		{
			//przelatuje przez inne pociski
			return;
		}
		if(Team.isItMyAlly(ownerTag, collider.gameObject))
		{
			//przelatuje przez sojuszników
			return;
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
			if((owner != null && !Team.isItMyAlly(owner, collidedGameObject)))
			{
				if(collidedGameObject.GetComponent<Health>() != null)
				{
					collidedGameObject.GetComponent<Health>().ApplyChange(-damage);
				}
			}
			else if(owner == null && !Team.isItMyAlly(ownerTag, collidedGameObject))
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
					if(owner != null && collidedGameObject != collidersInRange[i].gameObject && !Team.isItMyAlly(owner, collidersInRange[i].gameObject))
					{
						if(collidersInRange[i].gameObject.GetComponent<Health>() != null)
						{
							collidersInRange[i].gameObject.GetComponent<Health>().ApplyChange(-damage);
						}
					}
					else if(owner == null && collidedGameObject != collidersInRange[i].gameObject && !Team.isItMyAlly(ownerTag, collidersInRange[i].gameObject))
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
					if(owner != null && !Team.isItMyAlly(owner, collidersInRange[i].gameObject))
					{
						if(collidersInRange[i].gameObject.GetComponent<Health>() != null)
						{
							collidersInRange[i].gameObject.GetComponent<Health>().ApplyChange(-damage);
						}
					}
					else if(owner == null && !Team.isItMyAlly(ownerTag, collidersInRange[i].gameObject))
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

	public void setOwner(GameObject newOwner)
	{
		owner = newOwner;
		setOwnerTag (owner.tag);
	}

	public void setOwnerTag(string newOwnerTag)
	{
		ownerTag = newOwnerTag;
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
