using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapdoor : MonoBehaviour
{
    bool opened = false;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            if (Input.GetKey(KeyCode.E))
            {

                if (!opened)
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
