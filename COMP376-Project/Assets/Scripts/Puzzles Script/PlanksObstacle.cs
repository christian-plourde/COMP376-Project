using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanksObstacle : MonoBehaviour
{
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
            Debug.Log(col.gameObject.GetComponent<Rigidbody>().velocity.magnitude);
            for (int i = 1; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
            }
        }
        
    }
}
