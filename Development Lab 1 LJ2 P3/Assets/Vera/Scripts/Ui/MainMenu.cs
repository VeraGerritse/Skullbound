using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string nameScene;
    public bool Main;
    public Canvas myCanvas;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        if (Input.GetButtonDown("Cancel") && !Main)
        {
            if(myCanvas.enabled == false)
            {
                myCanvas.enabled = true;
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                GameManager.instance.paused = true;
            }
            else if(!Main)
            {
                myCanvas.enabled = false;
                Time.timeScale = 1;
                GameManager.instance.paused = false;
            }
        }
	}

    public void ResumeGame()
    {
        GameManager.instance.paused = false; 
        myCanvas.enabled = false;
        Time.timeScale = 1;
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        if (DungeonGeneratorManager.instance != null)
        {
            Destroy(DungeonGeneratorManager.instance.player);
        }

        else if (GameManager.instance != null)
        {
            if(GameManager.instance.PlayerInTEstScenes != null)
            {
                Destroy(GameManager.instance.PlayerInTEstScenes);
            }
        }
            SceneManager.LoadScene(nameScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
