using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class l2_p2_key_door : MonoBehaviour
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
        if (col.CompareTag("Key"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GetComponent<Animator>().SetBool("opened", true);
                Destroy(col.gameObject);
            }
        }
    }
    
}
