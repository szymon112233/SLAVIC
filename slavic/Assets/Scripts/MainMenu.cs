using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
    public Canvas mainMenu;
    public Canvas helpMenu;

    public void Start()
    {
        helpMenu.enabled = false;
    }

    public void StartGame()
    {
        Application.LoadLevel("Level_01");
    }

    public void Help()
    {
        mainMenu.enabled = false;
        helpMenu.enabled = true;
    }

    public void Back()
    {
        mainMenu.enabled = true;
        helpMenu.enabled = false;
    }

    public void EndGame()
    {
        Application.Quit();
    }

}
