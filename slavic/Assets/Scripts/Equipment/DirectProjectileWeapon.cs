using UnityEngine;
using System.Collections;

/**
 * Broń strzelająca bezpośrednio.
 * */
public class DirectProjectileWeapon : EquipmentPiece
{
	public AudioSource audioSource;
	public AudioClip firingSound;
	public AudioClip reloadSound;
	public GameObject bullet;
	
	public float ammoPerShot;
	public float bulletReloadTime;
	public float magazineReloadTime;
	public float maxAmmoInMagazine;	//określa ile razy można wystrzelić

	private float currentAmmoInMagazine;
	private float remainingReloadTime;
	private Vector3 aimpoint;
	private bool areWeAimingAtPoint;
	private const float BULLET_SPAWN_DISTANCE = 1f;


	void Start()
	{
		areWeAimingAtPoint = true;
		remainingReloadTime = 0;
		currentAmmoInMagazine = maxAmmoInMagazine;
		aimpoint = new Vector3 (0, 0, 0);
	}

	void Update()
	{
		if(remainingReloadTime > 0)
		{
			remainingReloadTime -= Time.deltaTime;
			if(remainingReloadTime < 0)
			{
				remainingReloadTime = 0;
			}
		}
	}

	public void Aim(GameObject gameObject)
	{
		if(gameObject != null)
		{
			Aim (gameObject.transform.position);
		}
		else
		{
			areWeAimingAtPoint = false;
			aimpoint = Vector3.zero;
		}
	}

	public void Aim(Vector3 point)
	{
		aimpoint = point;
		areWeAimingAtPoint = true;
	}

	public bool Fire()
	{
		if(!IsReadyToFire())
		{
			return false;
		}

		remainingReloadTime = bulletReloadTime;
		if(maxAmmoInMagazine > 0)
		{
            currentAmmoInMagazine -= ammoPerShot;
		}

		//Strzelanie pociskami
		Quaternion aimRotation;
		Vector3 directionToAim;
		if(areWeAimingAtPoint)
		{
			directionToAim = (aimpoint - transform.position).normalized;	//Strzelanie do celu
		}
		else
		{
			directionToAim = (transform.up).normalized;	//strzelanie z lufy
		}

		aimRotation = Quaternion.LookRotation(directionToAim);
		var newBulletInstance = (GameObject) Instantiate(bullet, transform.position + directionToAim * BULLET_SPAWN_DISTANCE, aimRotation) as GameObject;
        newBulletInstance.GetComponent<TeamID>().team = GetComponent<MinionControll>().GetTeamID().team;
		if(newBulletInstance.GetComponent<Rigidbody>() != null)
		{
			newBulletInstance.GetComponent<Rigidbody>().AddForce(directionToAim * newBulletInstance.GetComponent<Bullet>().getFiringForce());
		}

		if(audioSource != null && firingSound != null)
		{
			audioSource.PlayOneShot(firingSound);
		}

		return true;
	}

	public bool IsReadyToFire()
	{
		if(remainingReloadTime <= 0)
		{
			if(maxAmmoInMagazine <= 0)
			{
				return true;
			}
            if (currentAmmoInMagazine >= ammoPerShot)
			{
				return true;
			}
		}
		return false;
	}

	public bool ReloadWeapon(float newAmmoCount)
	{
		if(remainingReloadTime <= 0)
		{
			remainingReloadTime = magazineReloadTime;
			currentAmmoInMagazine = newAmmoCount;
			if(currentAmmoInMagazine > maxAmmoInMagazine)
			{
				currentAmmoInMagazine = maxAmmoInMagazine;
			}
			if(audioSource != null && reloadSound != null)
			{
				audioSource.PlayOneShot(reloadSound);
			}
			return true;
		}
		return false;
	}

	public float GetRemainingReloadTime()
	{
		return remainingReloadTime;
	}

	public float GetMagazineAmmoCount()
	{
		return currentAmmoInMagazine;
	}

    public float GetMagazineSize()
	{
		return maxAmmoInMagazine;
	}

    public float GetAmmoPerShot()
	{
		return ammoPerShot;
	}
}
