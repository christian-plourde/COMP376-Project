using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            foreach(GameObject n in GameObject.FindGameObjectsWithTag("Note"))
            {
                if(n.transform.parent == GameObject.Find("UI-Canvas").transform)
                {
                    return;
                }

                if (GameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }

            }
            
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.visible = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.visible = true;
    }

    public void loadMenu()
    {

        SceneManager.LoadScene(0);
    }
}
