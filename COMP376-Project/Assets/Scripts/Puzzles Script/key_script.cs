using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key_script : MonoBehaviour
{
    public GameObject player_hand;
    bool in_hand;
    public float carry_x_offset;
    public float carry_y_offset;

    public bool InHand
    {
        get { return in_hand; }
        set
        {
            in_hand = value;

            if (in_hand)
            {
                this.transform.parent = player_hand.transform;
                this.transform.position = new Vector3(player_hand.transform.position.x + carry_x_offset, player_hand.transform.position.y + carry_y_offset, player_hand.transform.position.z);
            }

            else
            {
                this.transform.parent = null;
            }

        }
    }


    // Start is called before the first frame update
    void Start()
    {
        in_hand = false;
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
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
