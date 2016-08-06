using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
    public GameObject controlledMinion;
    private Vector3 currentCameraPositionOffset = new Vector3(0.0f, 23.0f, -28.0f);
    private Vector3 movementDirection;
    public Transform currentCamera;
    public float speed = 500f;
    private Rigidbody playerRig;

	// Use this for initialization
	void Start () 
    {
        if (controlledMinion != null && controlledMinion.GetComponent<MinionControll>().GetHealth().IsAlive())
        {
            playerRig = controlledMinion.GetComponent<Rigidbody>();
            controlledMinion.GetComponent<MinionControll>().GetPatrolAI().Deactivate();
        }
	}

    //FUTURE : We will change it to using MinionControl
    public void PosessMinion(GameObject minion)
    {
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
