using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class scenechanger : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public bool backToMenu;
    bool videoStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (videoPlayer.isPrepared)
        {
            videoStarted = true;
        }
        if ((!videoPlayer.isPlaying && videoStarted) || Input.GetKeyDown(KeyCode.Space))
        {
            if (backToMenu)
            {
                SceneManager.LoadScene("MainMenu");

            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            }
        }
    }
}
