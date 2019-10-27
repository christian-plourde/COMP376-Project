using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class scenechanger : MonoBehaviour
{
    public VideoPlayer videoPlayer;
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
        if (!videoPlayer.isPlaying && videoStarted)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
