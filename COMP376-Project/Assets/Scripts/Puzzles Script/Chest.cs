﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    bool opened = false;
    public int giveIronCount;
    public int giveSteelCount;
    public int givePewterCount;
    public GameObject giveNote;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("opened", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetKey(KeyCode.E))
            {
                Debug.Log("you has opened the chest");
                AudioManager.instance.Play("chest");
                if(!opened)
                {
                    opened = true;
                    animator.SetBool("opened", opened);
                }


                // player potion ++;

                // if note not null, instantiate it

                //this.GetComponent<Chest>().enabled = false;
            }
        }
    }
}
