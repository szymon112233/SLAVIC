using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdditionalMinions : MonoBehaviour {

    public List<MinionControll> minions;
    public GameObject[] minionsPrefabs;
        
	// Use this for initialization
	void Start () 
    {
        minions = new List<MinionControll>();
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<MinionControll>() == FindObjectOfType<GameplayManager>().playerControlledMinion)
        {
            int numberOfMinions = Random.Range(1, 3);
            for (int i = 0; i < numberOfMinions; i++)
            {
                var temp = (GameObject) Instantiate(minionsPrefabs[Random.Range(0, 2)], transform.position, transform.rotation) as GameObject;
                FindObjectOfType<GameplayManager>().squadManager.AddSquadMember(temp.GetComponent<MinionControll>());
            }   
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
