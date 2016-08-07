using UnityEngine;
using System.Collections.Generic;

public class EnemyPatrol : MonoBehaviour 
{
    public List<MinionControll> minions;
    public List<Vector3> defaultPatrolRoute;
    public float chaseDistance;
    
    private TeamID teamID;
    private GameObject currentTarget;

	void Update () 
    {
        CleanDeadMinions();
        if (currentTarget != null && Vector3.Distance(transform.position, currentTarget.transform.position) < chaseDistance)
        {
            ChaseCurrentTarget();
        }
        else
        {
            currentTarget = null;
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

    private void ChaseCurrentTarget()
    {
        for (int i = 0; i < minions.Count; i++)
        {
            minions[i].GetPatrolAI().SetPatrolRoute(currentTarget.transform.position); 
        }
    }

    private void RestoreOriginalPatrolRoute()
    {
        for (int i = 0; i < minions.Count; i++)
        {
            minions[i].GetPatrolAI().SetPatrolRoute(defaultPatrolRoute);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (currentTarget == null)
        {
            if (FindObjectOfType<GameplayManager>().teamManager.IsHostile(gameObject, other.gameObject))
            {
                currentTarget = other.gameObject;
            }
        }
    }
}
