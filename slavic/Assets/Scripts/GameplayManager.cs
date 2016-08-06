using UnityEngine;
using System.Collections;

public class GameplayManager : MonoBehaviour {

    //References to all Mechanics
    public GameObject teamManager, mainCamera, squadManger, playerControll;

	// Use this for initialization
	void Start () {
        teamManager = GameObject.Find("TeamManager");
        mainCamera = GameObject.Find("Main Camera");
        squadManger = GameObject.Find("SquadManager");
        playerControll = GameObject.Find("PlayerControll");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
