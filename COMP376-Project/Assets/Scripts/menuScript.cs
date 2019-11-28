using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuScript : MonoBehaviour
{
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            LevelChanger.instance.FadeToLevel(5);
        }
    }

    public void Play()
    {
        LevelChanger.instance.FadeToNextLevel();

    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Credit()
    {
        SceneManager.LoadScene("Credit");

    }
}
