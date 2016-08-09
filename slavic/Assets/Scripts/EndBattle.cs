using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndBattle : MonoBehaviour 
{
    private bool isVictory;
    private bool isEscape;
    public List<EnemyPatrol> enemyPatrols;

    void Start()
    {
        isVictory = false;
        isEscape = false;
    }

	void Update () 
    {
	    if(!isVictory && !isEscape)
        {
            CheckVictory();
        }
	}

    private void CheckVictory()
    {
        if(isEscape)
        {
            return;
        }

        for (int i = 0; i < enemyPatrols.Count; i++)
        {
            if(enemyPatrols[i].minions.Count > 0)
            {
                return;
            }
        }
        if (!isVictory)
        {
            isVictory = true;
            FindObjectOfType<GameplayManager>().EndGame("Victory");
            return;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (!isVictory)
        {
            if (other.gameObject.GetComponent<MinionControll>() != null && other.gameObject.GetComponent<MinionControll>() == FindObjectOfType<GameplayManager>().playerControll.controlledMinion)
            {
                if (!isEscape)
                {
                    isEscape = true;
                    FindObjectOfType<GameplayManager>().EndGame("Escaped");
                }
            }
        }
    }
}

