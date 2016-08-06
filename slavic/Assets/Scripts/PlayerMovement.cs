using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
    private GameObject controlledMinion;
    private Vector3 currentCameraPositionOffset = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector2 movementDirection;
    public Transform currentCamera;
    public float speed =1f;
    public Rigidbody2D playerRig;

	// Use this for initialization
	void Start () 
    {
        playerRig = controlledMinion.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (controlledMinion != null)
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
        movementDirection.y = Input.GetAxisRaw("Vertical");
        movementDirection.x = Input.GetAxisRaw("Horizontal");
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
