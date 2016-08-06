using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Określa zmysł omnisense widzący przez wszystkie przeszkody.
 * */
public class OmniSense : MonoBehaviour
{
    public float omniSenseRange;	//Określa zasięg omnisense
    private List<GameObject> gameObjectsInRange;    //Lista obiektów wykrytych ostatnim razem.

    void Start()
    {
        gameObjectsInRange = new List<GameObject>();
    }

    /**
     * Aktualizuje listę wykrytych obiektów.
     * */
    public void DetectGameObjects()
    {
        gameObjectsInRange.Clear();
        Collider[] collidersInRange = Physics.OverlapSphere(transform.position, omniSenseRange);
        if (collidersInRange == null || collidersInRange.Length == 0)
        {
            return;
        }

        for (var i = 0; i < collidersInRange.Length; i++)
        {
            //TODO: filtracja obiektów nie istotnych
            gameObjectsInRange.Add(collidersInRange[i].gameObject);
        }
        return;
    }

    /**
     * Sprawdza czy "gameObject" był ostatnio wykryty.
     * */
    public bool IsDetected(GameObject gameObject)
    {
        return gameObjectsInRange.Contains(gameObject);
    }

    /**
     * Zwraca aktualną listę wykrytych obiektów
     * */
    public List<GameObject> GetObjectsInRange()
    {
        return gameObjectsInRange;
    }
}
