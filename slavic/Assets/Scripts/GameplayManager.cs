using UnityEngine;
using System.Collections;

public class GameplayManager : MonoBehaviour 
{
    public TeamControll teamManager;
    public PlayerMovement playerControll;
    public EffectsManager effectsManager;
    public SquadManager squadManager;
    public MinionControll playerControlledMinion;

    private bool isDefeat;

    void Start () 
    {
        isDefeat = false;
    }
	
	void Update () 
    {
	}

    public void Defeat()
    {
        isDefeat = true;
    }
}
