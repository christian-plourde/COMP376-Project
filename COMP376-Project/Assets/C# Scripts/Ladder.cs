using System.Collections;
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

    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        playerRigidBody = playerRef.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ladderControls();
    }

    private void ladderControls()
    {

        // w
        if (Input.GetKey(KeyCode.W) && onLadder)
        {
            usingLadder = true;
            playerRigidBody.useGravity = false;
            playerRigidBody.velocity = Vector3.zero;
            // controls
            playerRef.GetComponent<Transform>().Translate(new Vector3(0, climbSpeed * Time.deltaTime, 0));
            
        }

        // s
        if (Input.GetKey(KeyCode.S) && onLadder)
        {
            usingLadder = true;
            playerRigidBody.useGravity = false;
            playerRigidBody.velocity = Vector3.zero;
            // controls
            playerRef.GetComponent<Transform>().Translate(new Vector3(0, -climbSpeed * Time.deltaTime, 0));
        }



        // space
        if (Input.GetKey(KeyCode.Space))
        {
            usingLadder = false;
            playerRigidBody.useGravity = true;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            onLadder = true;
            playerRef.GetComponent<Animator>().SetBool("onLadder",true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        { 
            onLadder = false;
            playerRef.GetComponent<Animator>().SetBool("onLadder", false);
            usingLadder = false;
            playerRigidBody.useGravity = true;
            Debug.Log("Exited ladder");

        }
    }
}
