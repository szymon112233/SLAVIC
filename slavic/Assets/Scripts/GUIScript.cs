using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIScript : MonoBehaviour 
{
    private GameplayManager gameplayManager;

    public Text minionsText;
    public Text foodText;

    void Start()
    {
        gameplayManager = FindObjectOfType<GameplayManager>();
    }
	void Update () 
    {
        minionsText.GetComponent<Text>().text = (gameplayManager.squadManager.minions.Count + 1).ToString();
        foodText.GetComponent<Text>().text = (gameplayManager.GetFood()).ToString();
	}
}
