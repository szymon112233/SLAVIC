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

        isVictory = true;
        FindObjectOfType<GameplayManager>().GetAnnoucmentText().text = "Victory";
        FindObjectOfType<GameplayManager>().EndGame();
        return;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<MinionControll>() != null && other.gameObject.GetComponent<MinionControll>() == FindObjectOfType<GameplayManager>().playerControll.controlledMinion)
        {
                isEscape = true;
                FindObjectOfType<GameplayManager>().GetAnnoucmentText().text = "Escaped";
                FindObjectOfType<GameplayManager>().EndGame();
        }
    }
}

