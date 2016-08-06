using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIScript : MonoBehaviour {
    public Text minionsText;
    public Text foodText;
    public SquadManager squad;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        minionsText.GetComponent<Text>().text = (squad.minions.Count+1).ToString();
	}
}
