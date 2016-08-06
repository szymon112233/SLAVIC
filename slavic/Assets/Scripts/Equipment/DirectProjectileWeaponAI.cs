using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Opisuje sztuczną inteligencję dla broni strzelających bezpośrednio (np. pistolet, karabin itp.)
 * */
public class DirectProjectileWeaponAI : EquipmentPieceAI
{
	public DirectProjectileWeapon weapon;
	public float personalSpaceRadius = 3;
	private Vector3[] aimArray = new Vector3[7]; 
	private float targettingOffset = 0.9f;

	void Start()
	{
		initializeTargetArray ();
		base.equipmentOrWeapon = weapon;
	}

	void Update()
	{
		if(weapon.GetMagazineSize() > 0 && weapon.GetMagazineAmmoCount() <= 0)
		{
			weapon.ReloadWeapon(weapon.GetMagazineSize());
		}
		if(base.getPermissionToOperate())
		{
			if(privateTarget != null && CheckTargetValidity(privateTarget) && RayTargetCheck(privateTarget))
			{
				weapon.Aim(DetermineShootingPoint(privateTarget));
				weapon.Fire();
			}
		}
	}

	public override List<TargetRate> rateTargets(List<GameObject> detectedGameObjects)
	{
		List<TargetRate> targetRateList = new List<TargetRate> ();
		privateTarget = null;
		if(weapon != null)
		{
			if(detectedGameObjects != null && detectedGameObjects.Count > 0)
			{
				//Wstępna filtracja obiektów
				List<GameObject> tempDetectedObjects = new List<GameObject>();
				for(int i = 0; i < detectedGameObjects.Count; i++)
				{
					if(CheckTargetValidity(detectedGameObjects[i]))
					{
						tempDetectedObjects.Add(detectedGameObjects[i]);
					}
				}

				//Rating wybranych obiektów
				if(tempDetectedObjects.Count > 0)
				{
					float rate = 0f;
					float rateNormalization = 1f;
					float currentHealth = 0f;
					for(int i = 0; i < tempDetectedObjects.Count; i++)
					{
						//sprawdzenie poziomu życia
						currentHealth = tempDetectedObjects[i].GetComponent<Health>().GetCurrentHealth() / tempDetectedObjects[i].GetComponent<Health>().MAX_HEALTH;
						if(currentHealth < 0.5)
						{
							rate = 5;
						}
						else
						{
							rate = 0;
						}
						rateNormalization = 5;

						//sprawdzenie czy poprzednio to był nasz cel
						EquipmentAI equipmentAI = GetComponent<EquipmentAI>();
						if(equipmentAI != null && equipmentAI.lastTarget != null && equipmentAI.lastTarget == tempDetectedObjects[i])
						{
							rate += 10;
							rateNormalization += 10;
						}

						//sprawdzenie odległości
						if(Vector3.Distance(transform.position, tempDetectedObjects[i].transform.position) < personalSpaceRadius)
						{
							rate += 10;
							rateNormalization += 10;
						}

						//sprawdzenie czy możemy do niego strzelać
						if(RayTargetCheck(tempDetectedObjects[i]))
						{
							rate += 30;
							rateNormalization += 30;
						}

						//normalizacja i zapisanie wyniku
						rate = rate/rateNormalization;
						targetRateList.Add(new TargetRate(tempDetectedObjects[i], rate));
					}
				}

				//Wybranie własnego celu
				TargetRate maxTargetRate = new TargetRate(null, 0);
				for(int i=0; i < targetRateList.Count; i++)
				{
					if(targetRateList[i].getTargetRate() > maxTargetRate.getTargetRate())
					{
						maxTargetRate = targetRateList[i];
					}
				}
				privateTarget = maxTargetRate.getTarget();
			}
		}
		return targetRateList;
	}

	/**
	 * Sprawdza czy możemy strzelać do danego celu.
	 * */
	private bool CheckTargetValidity(GameObject target)
	{
		if(weapon != null)
		{
			if (target == null | target.GetComponent<Collider>() == null) 
			{
				//brak celu lub brak jego collidera
				return false;
			}

			Health healthScript = target.GetComponent<Health> ();
			if (healthScript == null || !healthScript.IsAlive()) 
			{
				//brak życia
				return false;
			}

			if (healthScript.IsAlive()) 
			{
				if(Team.isItMyEnemy(gameObject, target))
				{
					//to wróg
					return true;
				}
			}
		}
		//cel nie żyje lub nie jest wrogiem 
		return false;
	}

	/**
	 * Sprawdza czy możemy wycelować w dany cel.
	 * */
	private bool RayTargetCheck(GameObject target)
	{
		if(DetermineShootingPoint(target) != transform.position)
		{
			return true;
		}
		return false;
	}

	/**
	 * Określa w który punkt w przestrzeni powinniśmy strzelać by trafić dany cel.
	 * */
	private Vector3 DetermineShootingPoint(GameObject target)
	{
		Collider collider = target.GetComponent<Collider> ();
		if(collider == null)
		{
			return transform.position;
		}
		
		if(weapon == null)
		{
			return transform.position;
		}
		
		//TODO start promienia jako końcówka broni
		Ray shootRay = new Ray();
		RaycastHit shootHit;
		Vector3 direction;
		shootRay.origin = transform.position;

		//określenie punktów krańcowych collidera
		initializeTargetArray();
		aimArray[1] = new Vector3((collider.bounds.size.x/2)*targettingOffset, 0, 0);
		aimArray[2] = new Vector3((-collider.bounds.size.x/2)*targettingOffset, 0, 0);
		//aimArray[3] = new Vector3(0, (collider.bounds.size.y/2)*targettingOffset, 0);
		//aimArray[4] = new Vector3(0, (-collider.bounds.size.y/2)*targettingOffset, 0);
		aimArray[5] = new Vector3(0, 0, (collider.bounds.size.z/2)*targettingOffset);
		aimArray[6] = new Vector3(0, 0, (-collider.bounds.size.z/2)*targettingOffset);

		//sprawdzenie punktów krańcowych i centrum
		for( int i=0; i < aimArray.Length; i++)
		{
			direction = (target.transform.position + aimArray[i]) - transform.position;
			shootRay.direction = direction.normalized;
			
			if(Physics.Raycast (shootRay, out shootHit, weapon.maxRange))
			{
				if(shootHit.collider.gameObject == target)
				{
					return shootHit.point;
				}
			}
		}
		return transform.position;
	}

	/**
	 * Zapełnia targetArray zerowymi wektorami.
	 * */
	private void initializeTargetArray()
	{
		aimArray[0] = new Vector3(0,0,0);
		aimArray[1] = new Vector3(0,0,0);
		aimArray[2] = new Vector3(0,0,0);
		aimArray[3] = new Vector3(0,0,0);
		aimArray[4] = new Vector3(0,0,0);
		aimArray[5] = new Vector3(0,0,0);
		aimArray[6] = new Vector3(0,0,0);
	}
}