﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class cannonball : MonoBehaviour
{
    public GameObject player_hand;
    [HideInInspector]
    public bool in_hand;
    public float carry_x_offset;
    public float carry_y_offset;
    Player player;
    DateTime pick_up_time;
    bool reinstantiated = false;
    public bool reinstantiable;

    public bool InHand
    {
        get { return in_hand; }
        set {
            in_hand = value;

            if(in_hand)
            {
                this.transform.parent = player_hand.transform;
                this.transform.position = new Vector3(player_hand.transform.position.x + carry_x_offset, player_hand.transform.position.y + carry_y_offset, player_hand.transform.position.z);
                this.GetComponent<Rigidbody>().detectCollisions = false;
                this.GetComponent<Rigidbody>().isKinematic = true;
                pick_up_time = DateTime.Now;
            }

            else
            {
                this.transform.parent = null;
                this.GetComponent<Rigidbody>().detectCollisions = true;
                this.GetComponent<Rigidbody>().isKinematic = false;
            }
            
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        in_hand = false;
        player = FindObjectOfType<Player>();
        pick_up_time = DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {

        if ((DateTime.Now - pick_up_time).TotalSeconds > 1.0f && Input.GetKeyDown(KeyCode.E) && InHand)
        {
            InHand = false;
        }

        if (!player.usingPewter && InHand)
            InHand = false;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 250f))
        {
            if (hit.collider.gameObject == this.gameObject && Input.GetMouseButtonUp(0) && !reinstantiated && reinstantiable)
            {
                GameObject new_ball = Instantiate(this.gameObject, this.transform.position, this.transform.localRotation, this.transform.parent);
                new_ball.transform.localScale = this.transform.localScale;
                reinstantiated = true;
            }

        }
    }

    void OnTriggerStay(Collider col)
    {
        if(col.CompareTag("Player") && player.usingPewter)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!InHand)
                {
                    InHand = true;
                }
            }
        }
    }
}
