using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
    public MinionControll controlledMinion;
    public EffectsManager manager;
    private Vector3 currentCameraPositionOffset;
    private Vector3 movementDirection;
    public Transform currentCamera;
    private Transform currentCameraCamera;
    public float speed = 500f;
    private Rigidbody playerRig;
    private FXManager fxManager;
    private bool canStep = true;
    private SquadManager squadManager;

	// Use this for initialization
	void Start () 
    {
        fxManager = controlledMinion.GetComponentInChildren<FXManager>();
        playerRig = controlledMinion.GetComponent<Rigidbody>();
        currentCameraPositionOffset = new Vector3(0, 16.93f, -7.85f);
        controlledMinion.GetComponentInChildren<Image>().enabled = true;
        currentCameraCamera = currentCamera.GetComponentInChildren<Camera>().gameObject.transform;
        squadManager = FindObjectOfType<GameplayManager>().squadManager;
	}

    public void PosessMinion(MinionControll minion)
    {
        if (minion != null)
        {
            if (manager)
            {
                manager.SetScreenShake(true, 2, 0.8f, 0.05f);
            }
            controlledMinion = minion;
            fxManager = minion.GetComponent<FXManager>();
            playerRig = minion.GetComponent<Rigidbody>();
            controlledMinion.GetComponent<MinionControll>().GetPatrolAI().Deactivate();
            minion.GetComponentInChildren<Image>().enabled = true;
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
        yield return new WaitForSeconds(Random.Range(0.4f, 1f));
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

        if (squadManager.minions.Count > 0)
        {
            currentCameraCamera.localPosition = new Vector3(0, 0, -3) * Mathf.Floor(squadManager.minions.Count / 6);   
        }
    }
}
