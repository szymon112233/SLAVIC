using UnityEngine;
using System.Collections;

public class MinionControll : MonoBehaviour 
{
    private PatrolAI patrolAI;
    private EquipmentManager equipmentManager;
    private OmniSense omniSense;
    private Health health;
    private TeamID teamID;
    private float deathAnimationDuration = 0.1f;

	void Awake() 
    {
        patrolAI = GetComponent<PatrolAI>();
        equipmentManager = GetComponent<EquipmentManager>();
        omniSense = GetComponent<OmniSense>();
        health = GetComponent<Health>();
        teamID = GetComponent<TeamID>();
	}

    void Update()
    {
        if (!health.IsAlive())
        {
            patrolAI.Deactivate();
            deathAnimationDuration -= Time.deltaTime;
            FindObjectOfType<GameplayManager>().squadManager.DeleteSquadMember(this);
            
            //TODO: animacja śmierci

            if (deathAnimationDuration <= 0)
            {
                //TODO: ciało
                Destroy(gameObject);
            }
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
