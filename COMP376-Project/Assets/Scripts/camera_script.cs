using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class camera_script : MonoBehaviour
{
    // Start is called before the first frame update

    public float move_speed;
    public GameObject player;
    public Animation camera_pan;
    DateTime game_start;

    void Start()
    {
        game_start = DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        if ((DateTime.Now - game_start).TotalSeconds > camera_pan.clip.length + 1)
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 2.0f, transform.position.z);
    }
}
