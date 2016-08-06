using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
    public GameObject controlledMinion;
    private Vector3 currentCameraPositionOffset = new Vector3(0.0f, 23.0f, -28.0f);
    private Vector3 movementDirection;
    private PatrolAI minionAI;
    public Transform currentCamera;
    public float speed =500f;
    private Rigidbody playerRig;

	// Use this for initialization
	void Start () 
    {
      playerRig = controlledMinion.GetComponent<Rigidbody>();
      minionAI = controlledMinion.GetComponent<PatrolAI>();
      minionAI.Deactivate();
	}
    //FUTURE : We will change it to using MinionControl
    public void PosessMinion(GameObject minion)
    {
        playerRig = minion.GetComponent<Rigidbody>();
        minionAI = minion.GetComponent<PatrolAI>();
        minionAI.Deactivate();
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
            //TODO: strzelanie
        }
    }
    private void CameraMovement()
    {
        currentCamera.transform.position = playerRig.transform.position + currentCameraPositionOffset;
    }

}
