using UnityEngine;
using System.Collections;

public class TeamControll : MonoBehaviour {

    private Hashtable friendlyRelations, hostileRelations;


    // Use this for initialization
    void Start()
    {
        friendlyRelations = new Hashtable();
        friendlyRelations.Add(Team.FRIENDLY, true);
        friendlyRelations.Add(Team.HOSTILE, false);
        friendlyRelations.Add(Team.NEUTRAL, false);

        hostileRelations = new Hashtable();
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
            TeamID myTeam = me.GetComponent<TeamID>();
            TeamID anotherTeam = another.GetComponent<TeamID>();

            if (myTeam != null && anotherTeam != null)
            {
                switch (myTeam.team)
                {
                    case Team.FRIENDLY:
                        return friendlyRelations[anotherTeam.team].Equals(false);

                    case Team.HOSTILE:
                        return hostileRelations[anotherTeam.team].Equals(false);

                    default:
                        return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
            return false;
    }

    public bool IsFriendly(GameObject me, GameObject another)
    {
        if (me != null && another != null)
        {
            TeamID myTeam = me.GetComponent<TeamID>();
            TeamID anotherTeam = another.GetComponent<TeamID>();

            if (myTeam != null && anotherTeam != null)
            {
                switch (myTeam.team)
                {
                    case Team.FRIENDLY:
                        return friendlyRelations[anotherTeam.team].Equals(true);

                    case Team.HOSTILE:
                        return hostileRelations[anotherTeam.team].Equals(true);

                    default:
                        return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
            return false;
    }
}
