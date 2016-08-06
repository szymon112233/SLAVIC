using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdditionalMinions : MonoBehaviour {

    public List<MinionControll> minions; 
        
	// Use this for initialization
	void Start () {
        minions = new List<MinionControll>();
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<MinionControll>() == FindObjectOfType<GameplayManager>().playerControlledMinion)
        {
            Instantiate();
            FindObjectOfType<GameplayManager>().squadManager.AddSquadMember();
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update () {
	
	}
}
