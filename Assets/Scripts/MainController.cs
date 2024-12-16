using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class MainController : MonoBehaviour
{
    public static MainController Instance;

    public MainMenuController mainMenuController;
    public GameController gameController;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }    
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        mainMenuController.CheckForResume();
        gameController.Close();
    }

    public void FreezeGAme()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
    }

    public void UnFreezeGame()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
    }
}
