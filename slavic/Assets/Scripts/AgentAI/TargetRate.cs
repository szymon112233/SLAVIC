using UnityEngine;
using System.Collections;

/**
 * Określa parę cel-stopień zainteresowania celem..
 * */
public class TargetRate
{
    private GameObject target;
    private float rate;

    public TargetRate(GameObject newTarget, float newRate)
    {
        target = newTarget;
        rate = newRate;
    }

    public float getTargetRate()
    {
        return rate;
    }

    public void setRate(float newRate)
    {
        rate = newRate;
    }

    public GameObject getTarget()
    {
        return target;
    }
}
