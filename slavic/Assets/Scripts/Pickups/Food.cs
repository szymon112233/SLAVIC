using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour {

    private GameplayManager manager;
    public AudioClip clip;

	// Use this for initialization
	void Awake ()
    {
        manager = FindObjectOfType<GameplayManager>();

	}

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.GetComponent<TeamID>())
        {
            if (other.gameObject.GetComponent<TeamID>().team == Team.FRIENDLY)
            {
                if (manager)
                    manager.SetFood(manager.GetFood() + 1);
                AudioSource.PlayClipAtPoint(clip, transform.position);
                Destroy(transform.parent.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update ()
    {
	
	}
}
