using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class l2_puzzle2_plate : MonoBehaviour
{

    public GameObject door;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Interactable"))
        {
            door.GetComponent<Animator>().SetBool("opened", true);
        }
    }
}
