﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject playerRef;
    Rigidbody playerRigidBody;

    public bool onLadder;
    public bool usingLadder;

    float climbSpeed = 3f;
    int facingSide = 1;                    // if players facing side is different, flip player, so that player's front is towars the ladder


    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        playerRigidBody = playerRef.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ladderControls();
        checkFacingDirection();

        if (usingLadder && playerRef.GetComponent<Player>().isGrounded)
        {
            usingLadder = false;
            playerRef.GetComponent<Player>().usingLadder = false;
        }
    }

    private void ladderControls()
    {

        // w
        if (Input.GetKey(KeyCode.W) && onLadder)
        {
            usingLadder = true;
            playerRef.GetComponent<Player>().usingLadder = true;
            playerRigidBody.useGravity = false;
            playerRigidBody.velocity = Vector3.zero;
            // controls
            playerRef.GetComponent<Transform>().Translate(new Vector3(0, climbSpeed * Time.deltaTime, 0));
            
        }

        // s
        if (Input.GetKey(KeyCode.S) && onLadder)
        {
            usingLadder = true;
            playerRef.GetComponent<Player>().usingLadder=true;
            playerRigidBody.useGravity = false;
            playerRigidBody.velocity = Vector3.zero;
            // controls
            playerRef.GetComponent<Transform>().Translate(new Vector3(0, -climbSpeed * Time.deltaTime, 0));
        }



        // space
        if (Input.GetKey(KeyCode.Space))
        {
            usingLadder = false;
            playerRef.GetComponent<Player>().usingLadder = false;
            playerRigidBody.useGravity = true;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            
            onLadder = true;
            playerRef.GetComponent<Animator>().SetBool("onLadder",true);
            playerRef.GetComponent<Player>().onLadder = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        { 
            onLadder = false;
            playerRef.GetComponent<Player>().onLadder = false;
            playerRef.GetComponent<Animator>().SetBool("onLadder", false);
            usingLadder = false;
            playerRef.GetComponent<Player>().usingLadder = false;
            playerRigidBody.useGravity = true;
            Debug.Log("Exited ladder");

        }
    }

    private void checkFacingDirection()
    {
        if (playerRef.GetComponent<Player>().faceDirection == facingSide && usingLadder)
        {
            playerRef.GetComponent<Player>().changePlayerDirection();
        }
    }
}
