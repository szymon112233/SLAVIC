using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameplayManager : MonoBehaviour 
{
    public TeamControll teamManager;
    public PlayerMovement playerControll;
    public EffectsManager effectsManager;
    public SquadManager squadManager;
    public MinionControll playerControlledMinion;
    public Text annoucmentText;
    private string previousAnnoucment;
    private bool isEndLevel;
    private float endLevelTimeout = 3f;
    private bool isPause;
    void Start () 
    {
        isEndLevel = false;
        isPause = false;
        annoucmentText.text = "";
    }
	
	void Update () 
    {
        if (isEndLevel)
        {
            endLevelTimeout -= Time.deltaTime;
            if (endLevelTimeout <= 0)
            {
                Application.LoadLevel("MainMenu");
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPause)
            {
                isPause = false;
                annoucmentText.text = previousAnnoucment;
                Time.timeScale = 1;
            }
            else
            {
                isPause = true;
                previousAnnoucment = annoucmentText.text;
                annoucmentText.text = "Pause";
                Time.timeScale = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel("MainMenu");
        }
	}

    public void EndGame()
    {
        isEndLevel = true;
    }

    public Text GetAnnoucmentText()
    {
        return annoucmentText;
    }
}
