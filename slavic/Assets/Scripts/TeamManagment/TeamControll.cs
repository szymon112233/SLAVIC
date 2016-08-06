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

    public bool IsHostile(Minion me, Minion another)
    {
        if(me!=null && another!=null)
        {
            if (me.team == Team.FRIENDLY)
                return friendlyRelations[another.team].Equals(false);
            else if (me.team == Team.HOSTILE)
                return hostileRelations[another.team].Equals(false);
            else
                return false;
        }
        else
            return false;
    }
    public bool IsHostile(Bullet me, Minion another)
    {
        if (me != null && another != null)
        {
            if (me.team == Team.FRIENDLY)
                return friendlyRelations[another.team].Equals(false);
            else if (me.team == Team.HOSTILE)
                return hostileRelations[another.team].Equals(false);
            else
                return false;
        }
        else
            return false;
    }

    public bool IsFriendly(Minion me, Minion another)
    {
        if (me != null && another != null)
        {
            if (me.team == Team.FRIENDLY)
                return friendlyRelations[another.team].Equals(true);
            else if (me.team == Team.HOSTILE)
                return hostileRelations[another.team].Equals(true);
            else
                return false;
        }
        else
            return false;   
    }
    public bool IsFriendly(Bullet me, Minion another)
    {
        if (me != null && another != null)
        {
            if (me.team == Team.FRIENDLY)
                return friendlyRelations[another.team].Equals(true);
            else if (me.team == Team.HOSTILE)
                return hostileRelations[another.team].Equals(true);
            else
                return false;
        }
        else
            return false;
    }
}
