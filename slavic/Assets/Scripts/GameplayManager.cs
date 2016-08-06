using UnityEngine;
using System.Collections;

public class GameplayManager : MonoBehaviour {

    //References to all Mechanics
    public GameObject teamManager, mainCamera, squadManger, playerControll;
    public static GameplayManager instance = null;    
	// Use this for initialization
	void Start () {
        instance = this;
        teamManager = GameObject.Find("TeamManager");
        mainCamera = GameObject.Find("Main Camera");
        squadManger = GameObject.Find("SquadManager");
        playerControll = GameObject.Find("PlayerControll");
    }
	
	// Update is called once per frame
	void Update () {
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
