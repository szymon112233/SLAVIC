using UnityEngine;
using System.Collections;

public class MinionControll : MonoBehaviour 
{
    private PatrolAI patrolAI;
    private EquipmentManager equipmentManager;
    private OmniSense omniSense;
    private Health health;
    private TeamID teamID;

	void Start () 
    {
        patrolAI = GetComponent<PatrolAI>();
        equipmentManager = GetComponent<EquipmentManager>();
        omniSense = GetComponent<OmniSense>();
        health = GetComponent<Health>();
        teamID = GetComponent<TeamID>();
	}

    void Update()
    {
        if(!health.IsAlive())
        {
            patrolAI.Deactivate();
        }
    }

    public PatrolAI GetPatrolAI()
    {
        return patrolAI;
    }

    public EquipmentManager GetEquipmentManager()
    {
        return equipmentManager;
    }

    public OmniSense GetOmniSense()
    {
        return omniSense;
    }

    public Health GetHealth()
    {
        return health;
    }

    public TeamID GetTeamID()
    {
        return teamID;
    }
}
