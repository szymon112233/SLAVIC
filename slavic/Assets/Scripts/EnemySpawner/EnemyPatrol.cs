using UnityEngine;
using System.Collections.Generic;

public class EnemyPatrol : MonoBehaviour 
{
    public List<MinionControll> minions;
    public List<Vector3> defaultPatrolRoute;
    public float chaseDistance;

    private List<GameObject> currentTargets;
    private bool chase = false;
    private TeamID teamID;

    void Start()
    {
        currentTargets = new List<GameObject>();
        RestoreOriginalPatrolRoute();
    }

	void Update () 
    {
        CleanDeadMinions();
        UpdateTargets();
        if (currentTargets.Count > 0)
        {
            chase = true;
            ChaseCurrentTarget();
        }
        else if(chase)
        {
            chase = false;
            RestoreOriginalPatrolRoute();
        }
	}

    private void CleanDeadMinions()
    {
        int i = 0;
        while (i < minions.Count)
        {
            if (!minions[i].GetHealth().IsAlive())
            {
                minions.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }
    }

    /**
     * Usuwa z początku listy nieodpowiednie lub nieaktualne cele.
     * */
    private void UpdateTargets()
    {
        while(currentTargets.Count > 0)
        {
            if(!(CheckTarget(currentTargets[0])))
            {
                currentTargets.RemoveAt(0);
            }
            else
            {
                break;
            }
        }
    }

    /**
     * Sprawdza czy obiekt ciągle może być celem.
     * */
    private bool CheckTarget(GameObject target)
    {
        if (target != null && Vector3.Distance(transform.position, target.transform.position) < chaseDistance && target.GetComponent<Health>() != null && target.GetComponent<Health>().IsAlive() && FindObjectOfType<GameplayManager>().teamManager.IsHostile(gameObject, target))
        {
            return true;
        }
        return false;
    }

    private void ChaseCurrentTarget()
    {
        for (int i = 0; i < minions.Count; i++)
        {
            minions[i].GetPatrolAI().SetPatrolRoute(currentTargets[0].transform.position); 
        }
    }

    private void RestoreOriginalPatrolRoute()
    {
        for (int i = 0; i < minions.Count; i++)
        {
            if (defaultPatrolRoute.Count > 0)
            {
                minions[i].GetPatrolAI().SetPatrolRoute(defaultPatrolRoute);
            }
            else
            {
                minions[i].GetPatrolAI().SetPatrolRoute(transform.position);
            }
        }
    }

    /**
     * Sprawdza czy w obszarze sa nowe cele.
     * Jeśli tak to dodajemy je do listy.
     * */
    void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if(CheckTarget(other.gameObject))
            {
                currentTargets.Add(other.gameObject);
            }
        }
    }
}
