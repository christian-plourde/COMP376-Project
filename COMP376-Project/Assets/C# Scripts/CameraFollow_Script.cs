using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow_Script : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject player;
    private const float XOFFSET= 5f;
    private const float YOFFSET= 2f;

    float xOffset=5f;
    float yOffset=2f;
    public float smoothSpeed=3.5f;
    const float SMOOTHSPEED =3.5f;

    int defaulDirection;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        defaulDirection = player.GetComponent<Player>().faceDirection;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerpos= player.GetComponent<Transform>().position;
        transform.position=new Vector3(transform.position.x,playerpos.y+yOffset,playerpos.z+xOffset);
        smoothSwitchDirection2();
        resetSmoothSpeed();
    }


    // not using this one
    private void smoothSwitchDirection()
    {
        //Debug.Log(player.GetComponent<Player>().faceDirection);
        if (player.GetComponent<Player>().faceDirection == -1)
        {
            if (xOffset >= -XOFFSET/2) // -5f
                xOffset -= (smoothSpeed)*Time.deltaTime;
            else if(xOffset >= -XOFFSET)
                xOffset -= (smoothSpeed/2) * Time.deltaTime;
        }
        else if(player.GetComponent<Player>().faceDirection == 1)
        {
            if (xOffset <= XOFFSET/2)
                xOffset += smoothSpeed*Time.deltaTime;
            else if(xOffset <= XOFFSET)
                xOffset += (smoothSpeed/2) * Time.deltaTime;
        }

    }

    // this one looks better, atleast for the prototype
    private void smoothSwitchDirection2()
    {
        //Debug.Log(player.GetComponent<Player>().faceDirection);
        if (player.GetComponent<Player>().faceDirection == -1)
        {
            if (xOffset >= -XOFFSET / 2) // -5f
                xOffset -= (smoothSpeed) * Time.deltaTime;
            
        }
        else if (player.GetComponent<Player>().faceDirection == 1)
        {
            if (xOffset <= XOFFSET)
                xOffset += smoothSpeed * Time.deltaTime;
            
        }
        if(smoothSpeed>0.2f)
            smoothSpeed -= Time.deltaTime/1.2f;
    }

    private void resetSmoothSpeed()
    {
        if (player.GetComponent<Player>().faceDirection != defaulDirection)
        {
            smoothSpeed = SMOOTHSPEED;
            defaulDirection *= -1;
        }
    }


}
