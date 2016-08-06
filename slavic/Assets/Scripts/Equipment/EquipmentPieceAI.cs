using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentPieceAI : MonoBehaviour
{
    protected GameObject privateTarget = null;
    protected bool permissionToOperate = false;
    protected EquipmentPiece equipmentPiece;

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
        if (equipmentPiece != null)
        {
            return equipmentPiece.maxRange;
        }
        else
        {
            return 0f;
        }
    }

    public float getOptimalRange()
    {
        if (equipmentPiece != null)
        {
            return equipmentPiece.optimalRange;
        }
        else
        {
            return 0f;
        }
    }
}
