using UnityEngine;
using System.Collections;

public class MinionControll : MonoBehaviour 
{
    private PatrolAI patrolAI;
    private EquipmentManager equipmentManager;

	void Start () 
    {
        patrolAI = GetComponent<PatrolAI>();
        equipmentManager = GetComponent<EquipmentManager>();
	}
}
