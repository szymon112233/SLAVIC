using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ExitEnter : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TeamID>().team == Team.FRIENDLY)
        {

        }
    }
}
