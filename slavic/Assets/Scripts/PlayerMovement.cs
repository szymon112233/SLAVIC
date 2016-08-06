﻿using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
    public MinionControll controlledMinion;
    private Vector3 currentCameraPositionOffset = new Vector3(0.0f, 23.0f, -28.0f);
    private Vector3 movementDirection;
    public Transform currentCamera;
    public float speed =500f;
    private Rigidbody playerRig;

	// Use this for initialization
	void Start () 
    {
        playerRig = controlledMinion.GetComponent<Rigidbody>();
        controlledMinion.GetComponent<MinionControll>().GetPatrolAI().Deactivate();
	}

    //FUTURE : We will change it to using MinionControl
    public void PosessMinion(MinionControll minion)
    {
        controlledMinion = minion;
        playerRig = minion.GetComponent<Rigidbody>();
        controlledMinion.GetComponent<MinionControll>().GetPatrolAI().Deactivate();
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (controlledMinion != null )
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
        }
    }
    void AimAndShoot()
    {
        Vector3 observationPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        
        //Strzał
        if (Input.GetButtonDown("Fire1"))
        {
            controlledMinion.GetComponent<MinionControll>().GetEquipmentManager().GetCurrentWeapon().Aim(observationPoint);
            controlledMinion.GetComponent<MinionControll>().GetEquipmentManager().GetCurrentWeapon().Fire();
        }
    }
    private void CameraMovement()
    {
        currentCamera.transform.position = playerRig.transform.position + currentCameraPositionOffset;
    }

}
