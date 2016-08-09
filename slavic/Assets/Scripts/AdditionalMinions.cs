using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdditionalMinions : MonoBehaviour {

    public GameObject[] minionsPrefabs;
        
	// Use this for initialization
	void Start () {
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<MinionControll>() == FindObjectOfType<GameplayManager>().playerControlledMinion)
        {
            int numberOfMinions = 2;
            if (Random.Range(0f, 100f) <= 50f)
            {
                numberOfMinions++;
            }
            for (int i = 0; i < numberOfMinions; i++)
            {
                int minionType = 0;
                float rollForMinionType = Random.Range(0f, 100f);
                if (rollForMinionType <= 30f)
                {
                    minionType = 0;
                }
                else if (rollForMinionType <= 60f)
                {
                    minionType = 1;
                }
                else
                {
                    minionType = 2;
                }
                var temp = (GameObject)Instantiate(minionsPrefabs[minionType], transform.position, transform.rotation) as GameObject;
                FindObjectOfType<GameplayManager>().squadManager.AddSquadMember(temp.GetComponent<MinionControll>());
            }   
            Destroy(gameObject);
        }
    }
}
