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
    private FurbyAnimatorScript animatorScript;
    private Vector3 previousPosition;

	void Awake() 
    {
        patrolAI = GetComponent<PatrolAI>();
        equipmentManager = GetComponent<EquipmentManager>();
        omniSense = GetComponent<OmniSense>();
        health = GetComponent<Health>();
        teamID = GetComponent<TeamID>();
        animatorScript = GetComponentInChildren<FurbyAnimatorScript>();
        previousPosition = transform.position;
	}

    void Update()
    {
        if (!health.IsAlive())
        {
            patrolAI.Deactivate();
            deathAnimationDuration -= Time.deltaTime;
            FindObjectOfType<GameplayManager>().squadManager.DeleteSquadMember(this);
            
            if (deathAnimationDuration <= 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            //FIXME: Animacje powodują zmianę pozycji sprite miniona. Dochodzi również do spadku wydajności.
            //MoveAnimation();
        }
    }

    void MoveAnimation()
    {
        if (animatorScript != null)
        {
            if (transform.position != previousPosition)
            {
                animatorScript.PlayWalkAnimationIfWalking(transform.position - previousPosition);
                previousPosition = transform.position;
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
