using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Nadzoruje wybranie zalecanego przedmiotu ekwipunku do użycia, zalecanego celu oraz pożądanego dystansy do niego.
 * */
public class EquipmentAI : MonoBehaviour
{
    private bool isActive;
    public EquipmentPieceAI[] equipmentPieceAIs;
    private MinionControll minionControll;
    public float consequenceBonus = 0.5f;

    public GameObject lastTarget;
    public EquipmentPieceAI lastUsedEquipmentPiece;

    public GameObject currentTarget;
    public EquipmentPieceAI currentlyUsedEquipmentPiece;
    public float maximalDistanceToTarget;
    public float optimalDistanceToTarget;

    void Start()
    {
        isActive = true;
        minionControll = GetComponent<MinionControll>();
        lastTarget = null;
        lastUsedEquipmentPiece = null;
        currentTarget = null;
        currentlyUsedEquipmentPiece = null;
        optimalDistanceToTarget = 0;
        maximalDistanceToTarget = 0;
    }

    void Update()
    {
        if (!isActive || (minionControll.GetHealth() != null && !minionControll.GetHealth().IsAlive()))
        {
            currentTarget = null;
            currentlyUsedEquipmentPiece = null;
            optimalDistanceToTarget = 0;
            maximalDistanceToTarget = 0;

            if (equipmentPieceAIs != null && equipmentPieceAIs.Length > 0)
            {
                for (int i = 0; i < equipmentPieceAIs.Length; i++)
                {
                    equipmentPieceAIs[i].setPermissionToOperate(false);
                }
            }
        }
    }

    /**
     * Wyznaczenie nowego używanego ekwipunku i zalecanego celu.
     * */
    public void determineNewFavourites()
    {
        lastTarget = currentTarget;
        lastUsedEquipmentPiece = currentlyUsedEquipmentPiece;
        if (minionControll.GetOmniSense() != null && equipmentPieceAIs != null && equipmentPieceAIs.Length > 0 && minionControll.GetOmniSense().GetObjectsInRange() != null)
        {
            if (minionControll.GetOmniSense().GetObjectsInRange().Count > 0)
            {
                EquipmentRate maxEquipmentRate = new EquipmentRate(null, 0, null);	//obecny maksymalny rating przedmiotu
                for (int i = 0; i < equipmentPieceAIs.Length; i++)
                {
                    equipmentPieceAIs[i].setPermissionToOperate(false);
                    List<TargetRate> ratingList = equipmentPieceAIs[i].rateTargets(minionControll.GetOmniSense().GetObjectsInRange());	//żądanie by przedmiot ocenił cele
                    TargetRate maxTargetRate = new TargetRate(null, 0);	//obecny maksymalny rating konkretnego celu

                    //Wyznaczanie celu najbardziej zalecanego przez ten przedmiot
                    if (ratingList != null && ratingList.Count > 0)
                    {
                        for (int j = 0; j < ratingList.Count; j++)
                        {
                            if (maxTargetRate.getTargetRate() < ratingList[j].getTargetRate())
                            {
                                maxTargetRate = ratingList[j];	//przypisanie większej wartości jako maksimum
                            }
                        }
                    }
                    //equipmentPieceAIs[i] najbardziej poleca maxTargetRate, wyliczanie oceny przedmotu ekwipunku
                    EquipmentRate calculatedEquipmentRate = calculateEquipmentRate(maxTargetRate, equipmentPieceAIs[i]);

                    //wybranie przedmiotu faworyta w ekwipunku
                    if (calculatedEquipmentRate != null && maxEquipmentRate.getEquipmentRate() < calculatedEquipmentRate.getEquipmentRate())
                    {
                        maxEquipmentRate = calculatedEquipmentRate;	//przypisanie większego faworyta-przedmiotu w ekwipunku
                    }
                }

                //zakończenie wybierania, maxEquipmentRate- najbardziej polecany przedmiot
                currentTarget = maxEquipmentRate.getTarget();
                currentlyUsedEquipmentPiece = maxEquipmentRate.getEquipmentPiece();
                if (currentlyUsedEquipmentPiece != null)
                {
                    currentlyUsedEquipmentPiece.setPermissionToOperate(true);
                }
                if (maxEquipmentRate.getEquipmentPiece() != null)
                {
                    optimalDistanceToTarget = maxEquipmentRate.getEquipmentPiece().getOptimalRange();
                    maximalDistanceToTarget = maxEquipmentRate.getEquipmentPiece().getMaxRange();
                }
                else
                {
                    optimalDistanceToTarget = 0f;
                    maximalDistanceToTarget = 0f;
                }
            }
            else
            {
                currentTarget = null;
                currentlyUsedEquipmentPiece = null;
                optimalDistanceToTarget = 0;
                maximalDistanceToTarget = 0;
            }
        }
    }

    /**
     * Kalkuluje ocenę przedmiotu ekwipunku przy braniu pod uwagę priorytet przedmiotu w ekwipunku i konsekwencję w wyborze tego samego celu.
     * originalTargetRate- to jest polecane w takim stopniu
     * equipmentPiece- to jest ten który wydaje polecenie
     * */
    private EquipmentRate calculateEquipmentRate(TargetRate originalTargetRate, EquipmentPieceAI equipmentPiece)
    {
        if (originalTargetRate == null || originalTargetRate.getTarget() == null || equipmentPiece == null)
        {
            return null;
        }
        EquipmentRate equipmentRatePostScaling = new EquipmentRate(equipmentPiece, originalTargetRate.getTargetRate(), originalTargetRate.getTarget());
        equipmentRatePostScaling.setRate(equipmentRatePostScaling.getEquipmentRate() * equipmentPiece.priority);
        if (equipmentPiece == lastUsedEquipmentPiece)
        {
            equipmentRatePostScaling.setRate(equipmentRatePostScaling.getEquipmentRate() + consequenceBonus);
        }
        return equipmentRatePostScaling;
    }

    public void Activate()
    {
        isActive = true;
    }

    public void Deactivate()
    {
        isActive = false;
    }
}
