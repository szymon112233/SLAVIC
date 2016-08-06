   using UnityEngine;
using System.Collections;

public class ScenarioManager : MonoBehaviour {

    public GameObject squadManager;
    public float currentFood;
    private bool won = false;
    private bool lost = false;
	// Use this for initialization
	void Start () {
        squadManger = GameObject.Find("SquadManager");
	}
	
	// Update is called once per frame
	void Update () {
        checkLoose();
	}

    public void CheckWin() {
        if (currentFood >= squadManager.NumberOfSquadMembers())
            won = true;
    }

    private void CheckLoose()
    {
        if (squadManager.NumberOfSquadMembers() == 0)
        {
            won = false;
            lost = true;
        }
    }
    public bool hasWon(){
        return won;
    }
    public bool hasLost(){
        return lost;
    }

    
    
}
