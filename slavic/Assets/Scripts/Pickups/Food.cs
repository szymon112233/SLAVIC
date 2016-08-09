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
            if (other.gameObject.GetComponent<MinionControll>() == FindObjectOfType<GameplayManager>().playerControlledMinion)
            {
                if (manager)
                {
                    manager.IncreaseFood();
                }
                AudioSource.PlayClipAtPoint(clip, transform.position);
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
