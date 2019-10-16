using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow_Script : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;

    void Start()
    {
        //transform.position = player.GetComponent<Transform>().position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerpos= player.GetComponent<Transform>().position;
        transform.position=new Vector3(transform.position.x,playerpos.y+1.5f,playerpos.z+3.5f);
        
    }
}
