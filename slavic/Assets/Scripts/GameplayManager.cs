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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPause)
            {
                isPause = false;
                Time.timeScale = 1;
            }
            else
            {
                isPause = true;
                Time.timeScale = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
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
