using UnityEngine;
using System.Collections;

public class ExitEnter : MonoBehaviour {
    public ScenarioManager man;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        Minion minion = other.GetComponent<Minion>();
        if (minion.team == Team.FRIENDLY)
            man.CheckWin();

    }
}
