using UnityEngine;
using System.Collections;
using System.Threading;
using System.Collections.Generic;

public class SquadManager : MonoBehaviour
{

    public Vector3 playerPosition;

    public List<MinionControll> minions;

    public float distanceBetweenCircles = 3f;
    public int membersInMinimalCircle = 6;

    // Use this for initialization
    void Awake()
    {
        minions = new List<MinionControll>();
    }

    // Update is called once per frame
    void Update()
    {
        int circleNumber = 1;
        int currentMinionNumber = 0;

        if(FindObjectOfType<GameplayManager>().playerControlledMinion == null)
        {
            if (minions.Count > 0)
            {
                FindObjectOfType<GameplayManager>().playerControlledMinion = minions[minions.Count - 1];
                DeleteLastSquadMember();
            }
            else if (!FindObjectOfType<GameplayManager>().GetIsEndLevel())
            {
                FindObjectOfType<GameplayManager>().EndGame("Defeat");
            }
        }
        else
        {
            playerPosition = FindObjectOfType<GameplayManager>().playerControlledMinion.transform.position;
        }

        for(int i = 0; i < minions.Count; i++)
        {
            float spaceBetweenMinions = 360 / (membersInMinimalCircle * circleNumber);
            Vector3 positionInCircle = new Vector3(Mathf.Cos(spaceBetweenMinions * currentMinionNumber * Mathf.Deg2Rad),0, Mathf.Sin(spaceBetweenMinions * currentMinionNumber * Mathf.Deg2Rad));
            positionInCircle *= distanceBetweenCircles * circleNumber;
            minions[i].GetPatrolAI().SetPatrolRoute(playerPosition + positionInCircle);
            currentMinionNumber++;
            if(currentMinionNumber>= membersInMinimalCircle *circleNumber)
            {
                circleNumber++;
                currentMinionNumber = 0;
            }
        }
    }

    public void AddSquadMember(MinionControll tempMinionControll)
    {
        minions.Add(tempMinionControll);
    }

    public void AddMutipleSquadMembers(MinionControll[] tempMinionControllList)
    {
        for (int i = 0; i < tempMinionControllList.Length; i++)
        {
            AddSquadMember(tempMinionControllList[i]);
        }
    }

    public void DeleteSquadMember(MinionControll minionToDelete)
    {
        minions.Remove(minionToDelete);
    }

    public void DeleteLastSquadMember()
    {
        if(minions.Count>0)
        {
            minions[minions.Count - 1].GetPatrolAI().SetPatrolRoute(minions[minions.Count - 1].GetComponent<Rigidbody>().transform.position);
            minions.RemoveAt(minions.Count - 1);
        }
            
    }

    public void DeleteSquadMember(int index)
    {
        if (minions.Count > 0 && minions.Count < index )
        {
            minions[index].GetPatrolAI().SetPatrolRoute(minions[index].GetComponent<Rigidbody>().transform.position);
            minions.RemoveAt(index);
            minions = fixMinions();
        }
    }

    private List<MinionControll> fixMinions()
    {
        if (minions == null)
            return null;

        List<MinionControll> tempMinions = new List<MinionControll>();

        for(int i =0 ; i<=minions.Count ; i++)
        {
            if (minions[i] != null)
                tempMinions.Add(minions[i]);
        }
        return tempMinions;
    }
}
