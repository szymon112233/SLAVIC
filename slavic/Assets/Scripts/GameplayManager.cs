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
    public Text foodText;
    public int foodNeededToVictory = 10;
    private string previousAnnoucment;
    private bool isEndLevel;
    private float endLevelTimeout = 5f;
    private bool isPause;
    private int foodGathered;

    public int GetFood() 
    { 
        return foodGathered; 
    }

    public void IncreaseFood()
    {
        foodGathered++;
    }

    void Start () 
    {
        foodGathered = 0;
        isEndLevel = false;
        isPause = false;
        annoucmentText.text = "";
        foodText.text = "0";
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

    public void EndGame(string text)
    {
        if (!isEndLevel)
        {
            GetAnnoucmentText().text = text;
            FindObjectOfType<EffectsManager>().SetTransition(true, 3);
            isEndLevel = true;
        }
    }

    public Text GetAnnoucmentText()
    {
        return annoucmentText;
    }

    public bool GetIsEndLevel()
    {
        return isEndLevel;
    }
}
