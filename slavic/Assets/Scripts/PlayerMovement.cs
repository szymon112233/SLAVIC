﻿using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
    public MinionControll controlledMinion;
    private Vector3 currentCameraPositionOffset;
    private float cameraXRotation = 66.62901f;
    private Vector3 movementDirection;
    public Transform currentCamera;
    public float speed = 500f;
    private Rigidbody playerRig;
    private FXManager fxManager;
    private bool canStep = true;

	// Use this for initialization
	void Start () 
    {
        fxManager = controlledMinion.GetComponentInChildren<FXManager>();
        playerRig = controlledMinion.GetComponent<Rigidbody>();
        currentCameraPositionOffset = new Vector3(0, 16.93f, -7.85f);    //currentCamera.position - playerRig.transform.position;
	}

    public void PosessMinion(MinionControll minion)
    {
        if (minion != null)
        {
            controlledMinion = minion;
            fxManager = minion.GetComponent<FXManager>();
            playerRig = minion.GetComponent<Rigidbody>();
            controlledMinion.GetComponent<MinionControll>().GetPatrolAI().Deactivate();
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (controlledMinion != null && controlledMinion.GetComponent<MinionControll>().GetHealth().IsAlive())
        {
            controlledMinion.GetComponent<MinionControll>().GetPatrolAI().Deactivate();
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
