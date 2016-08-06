using UnityEngine;
using System.Collections;

public class GameplayManager : MonoBehaviour 
{

    //References to all Mechanics
    public TeamControll teamManager;
    public Camera mainCamera;
    public PlayerMovement playerControll;
    public EffectsManager effectsManager;
    
    public static GameplayManager instance = null;    
	
    void Start () 
    {
        instance = this;
    }
	
	// Update is called once per frame
	void Update () 
    {
        /*if (squadManager.hasWon()) {
            //TO DO: GUI to load next level
        }

        else if (squadManager.hasLost())
        {
            Destroy(playerControll.gameObject);
            // TO DO: GUI to reload or exit
        }*/
	}


}
