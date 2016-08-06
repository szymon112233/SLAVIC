using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
    private Vector3 currentCameraPositionOffset = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector2 movementDirection;
    public Transform currentCamera;
    public float speed =1f;
    public Rigidbody2D playerRig;
    public Equipment weapon_1;

	// Use this for initialization
	void Start () {
        if (currentCamera != null)
        {
            currentCameraPositionOffset = currentCamera.transform.position - playerRig.transform.position;
        }
	}
	
	// Update is called once per frame
	void Update () {
        Move();
        AimAndShoot();
        if (currentCamera != null)
        {
            CameraMovement();
        }
	
	}
       
    void Move() {
        if (currentCamera != null){

            movementDirection.y = Input.GetAxisRaw("Vertical");
            movementDirection.x = Input.GetAxisRaw("Horizontal");
            movementDirection = movementDirection.normalized ;
            if (playerRig != null)
            {
                playerRig.velocity = movementDirection * speed *Time.deltaTime;
            }
        }
    }
    void AimAndShoot()
    {
        Vector3 observationPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        //observationPoint.y = 0.5f;
        Debug.Log(observationPoint);
        //Strzał
        if (Input.GetButtonDown("Fire1"))
        {
            if (weapon_1 != null)
            {
                weapon_1.Aim(observationPoint);
                weapon_1.Fire();
            }
        }
    }
    private void CameraMovement()
    {
        currentCamera.transform.position = playerRig.transform.position + currentCameraPositionOffset;
    }

}
