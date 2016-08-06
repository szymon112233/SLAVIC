using UnityEngine;
using System.Collections;

/**
 * Określa ocenę zainteresowania przedmiotu.
 * -Jaki przedmiot
 * -Jak bardzo uznajemy jego zdanie (po skalowaniu na priorytety przedmiotów)
 * -Czym jest zainteresowany
 * */
public class EquipmentRate
{
    private EquipmentPieceAI equipmentPieceAI;
    private float rate;
    private GameObject target;

    public EquipmentRate(EquipmentPieceAI newEquipmentPiece, float newRate, GameObject newTarget)
    {
        equipmentPieceAI = newEquipmentPiece;
        rate = newRate;
        target = newTarget;
    }

    public float getEquipmentRate()
    {
        return rate;
    }

    public void setRate(float newRate)
    {
        rate = newRate;
    }

    public EquipmentPieceAI getEquipmentPiece()
    {
        return equipmentPieceAI;
    }

    public GameObject getTarget()
    {
        return target;
    }
}
