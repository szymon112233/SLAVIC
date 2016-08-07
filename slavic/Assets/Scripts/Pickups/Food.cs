using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour {

    public GameplayManager manager;

	// Use this for initialization
	void Start ()
    {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TeamID>())
        {
            if (other.gameObject.GetComponent<TeamID>().team == Team.FRIENDLY)
            {
                if (manager)
                    manager.SetFood(manager.GetFood() + 1);
                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update ()
    {
	
	}
}
