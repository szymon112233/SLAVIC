using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentPieceAI : MonoBehaviour
{
    protected GameObject privateTarget = null;
    protected bool permissionToOperate = false;
    protected EquipmentInfo equipmentInfo;

    public float priority = 1;

    public virtual List<TargetRate> rateTargets(List<GameObject> detectedGameObjects)
    {
        List<TargetRate> targetRateList = new List<TargetRate>();
        return targetRateList;
    }

    public void setPermissionToOperate(bool newPermission)
    {
        permissionToOperate = newPermission;
    }

    public bool getPermissionToOperate()
    {
        return permissionToOperate;
    }

    public float getMaxRange()
    {
        if (equipmentInfo != null)
        {
            return equipmentInfo.maxRange;
        }
        else
        {
            return 0f;
        }
    }

    public float getOptimalRange()
    {
        if (equipmentInfo != null)
        {
            return equipmentInfo.optimalRange;
        }
        else
        {
            return 0f;
        }
    }
}
