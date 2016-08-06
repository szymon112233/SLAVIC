using UnityEngine;
using System.Collections;

public class TeamControll : MonoBehaviour {

    private Hashtable friendlyRelations, hostileRelations;


    // Use this for initialization
    void Start()
    {
        friendlyRelations.Add(Team.FRIENDLY, true);
        friendlyRelations.Add(Team.HOSTILE, false);
        friendlyRelations.Add(Team.NEUTRAL, false);

        hostileRelations.Add(Team.FRIENDLY, false);
        hostileRelations.Add(Team.HOSTILE, true);
        hostileRelations.Add(Team.NEUTRAL, false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsHostile(GameObject me, GameObject another)
    {
        if(me!=null && another!=null)
        {
            Team myTeam = me.GetComponent<Team>();
            Team anotherTeam = another.GetComponent<Team>();

            if (myTeam != null && anotherTeam != null)
            {
                switch (myTeam)
                {
                    case Team.FRIENDLY:
                        return friendlyRelations[anotherTeam].Equals(false);
                        break;

                    case Team.HOSTILE:
                        return hostileRelations[anotherTeam].Equals(false);
                        break;

                    default:
                        return false;
                    break;
                }
            }
        }
        else
            return false;
    }

    public bool IsFriendly(GameObject me, GameObject another)
    {
        if (me != null && another != null)
        {
            Team myTeam = me.GetComponent<Team>();
            Team anotherTeam = another.GetComponent<Team>();

            if (myTeam != null && anotherTeam != null)
            {
                switch (myTeam)
                {
                    case Team.FRIENDLY:
                        return friendlyRelations[anotherTeam].Equals(true);
                        break;

                    case Team.HOSTILE:
                        return hostileRelations[anotherTeam].Equals(true);
                        break;

                    default:
                        return false;
                        break;
                }
            }
        }
        else
            return false;
    }
}
