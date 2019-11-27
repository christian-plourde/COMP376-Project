using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    bool ifUsed=false;
    public GameObject endDoorsReference;


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !ifUsed)
        {
            if (Input.GetKey(KeyCode.E))
            {
                Debug.Log("You pulled the lever.");
                GetComponent<Animator>().enabled = true;
                GetComponent<Lever>().enabled = false;

                //open doors
                endDoorsReference.GetComponent<Animator>().SetBool("on_plate", true);
            }
        }
    }
}
