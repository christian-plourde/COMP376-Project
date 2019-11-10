using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CemeteryGate : MonoBehaviour
{

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
        //while the player is close to the gate, if he presses e it should open the gates
        if (col.CompareTag("Player"))
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                GetComponent<Animator>().SetBool("opened", true);
            }
        }
    }
}
