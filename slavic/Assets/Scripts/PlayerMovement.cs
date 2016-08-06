﻿using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
    public MinionControll controlledMinion;
    private Vector3 currentCameraPositionOffset;
    private Vector3 movementDirection;
    public Transform currentCamera;
    public float speed = 500f;
    private Rigidbody playerRig;
    private FXManager fxManager;
    private bool canStep = true;

	// Use this for initialization
	void Start () 
    {
        if (controlledMinion != null && controlledMinion.GetComponent<MinionControll>().GetHealth().IsAlive())
        {
            fxManager = controlledMinion.GetComponentInChildren<FXManager>();
            playerRig = controlledMinion.GetComponent<Rigidbody>();
            controlledMinion.GetComponent<MinionControll>().GetPatrolAI().Deactivate();
        }
        currentCameraPositionOffset = currentCamera.position - playerRig.transform.position;
	}

    //FUTURE : We will change it to using MinionControl
    public void PosessMinion(MinionControll minion)
    {
        fxManager = minion.GetComponent<FXManager>();
        controlledMinion = minion;
        playerRig = minion.GetComponent<Rigidbody>();
        controlledMinion.GetComponent<MinionControll>().GetPatrolAI().Deactivate();
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (controlledMinion != null && controlledMinion.GetComponent<MinionControll>().GetHealth().IsAlive())
        {
            Move();
            AimAndShoot();
            if (currentCamera != null)
            {
                CameraMovement();
            }
        }
        else
        {
            PosessMinion(FindObjectOfType<GameplayManager>().playerControlledMinion);
        }
	}
       
    void Move() 
    {
        movementDirection.z = Input.GetAxisRaw("Vertical");
        movementDirection.x = Input.GetAxisRaw("Horizontal");
        movementDirection.y = 0;
        movementDirection = movementDirection.normalized;
        if (playerRig != null)
        {
            playerRig.velocity = movementDirection * speed * Time.deltaTime;
            if ((movementDirection.z > 0 || movementDirection.x > 0) && fxManager != null && canStep)
            {
                canStep = false;
                StartCoroutine("Step");
            }
        }
    }

    IEnumerator Step()
    {
        fxManager.PlayHealClip();
        yield return new WaitForSeconds(Random.RandomRange(0.4f, 1f));
        canStep = true;
    }
    void AimAndShoot()
    {
        RaycastHit hit;
        Ray ray = currentCamera.GetComponentInChildren<Camera>().ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 aimPoint = hit.point;
            aimPoint.y = 1.5f;
            if (Input.GetButtonDown("Fire1"))
            {
                controlledMinion.GetComponent<MinionControll>().GetEquipmentManager().GetCurrentWeapon().Aim(aimPoint);
                controlledMinion.GetComponent<MinionControll>().GetEquipmentManager().GetCurrentWeapon().Fire();
            }
        }
    }
    private void CameraMovement()
    {
        currentCamera.transform.position = playerRig.transform.position + currentCameraPositionOffset;
    }

}
