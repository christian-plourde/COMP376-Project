using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow_Script : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject player;
    private const float XOFFSET= -30f;
    private const float YOFFSET= 3f;

    float xOffset=-30f;
    float yOffset=3f;

    public bool enableRotation;

    float smoothSpeed=3.45f;
    const float SMOOTHSPEED =3.45f;

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
        if(enableRotation)
            smoothRotate();
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
            if (xOffset >= -XOFFSET+2.5f) // -5f
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

    private void smoothRotate()
    {
        // rotation try --- positive values since euler angles cap itself withing 0 to 360 only
        if (transform.eulerAngles.y<(275f) && player.GetComponent<Player>().faceDirection==1)
        {
            Debug.Log(transform.eulerAngles.y);
            transform.Rotate(new Vector3(0, 0.4f * Time.deltaTime, 0));
        }
        else if (transform.eulerAngles.y > (265f) && player.GetComponent<Player>().faceDirection == -1)
        {
            transform.Rotate(new Vector3(0, -0.4f * Time.deltaTime, 0));
        }
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
