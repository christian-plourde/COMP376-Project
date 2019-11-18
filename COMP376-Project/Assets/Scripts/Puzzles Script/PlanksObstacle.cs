using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlanksObstacle : MonoBehaviour
{

    public float force_factor;
    bool destroyed;
    DateTime destroyed_time;
    public float fade_step;
    public float destroy_velocity;
    public bool horizontal;

    // Start is called before the first frame update
    void Start()
    {
        destroyed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(destroyed)
        {
            for (int i = 1; i < transform.childCount; i++)
            {
                GameObject curr = transform.GetChild(i).gameObject;
                Renderer renderer = curr.GetComponent<Renderer>();

                foreach (Material m in renderer.materials)
                {
                    Color color = m.color;
                    color.a -= fade_step;
                    m.color = color;

                    if (m.color.a <= 0)
                    {
                        Destroy(curr);
                        continue;
                    }  
                }
            }

            if (transform.childCount <= 1)
                Destroy(this.gameObject);
        }
        
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Interactable"))
        {
            if(col.gameObject.GetComponent<Rigidbody>().velocity.magnitude > destroy_velocity)
            {
                for (int i = 1; i < transform.childCount; i++)
                {
                    transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
                    if(!horizontal)
                        transform.GetChild(i).GetComponent<Rigidbody>().AddForce(new Vector3((Mathf.Sign(col.gameObject.GetComponent<Rigidbody>().velocity.x))*col.gameObject.GetComponent<Rigidbody>().velocity.magnitude * force_factor, 0, 0));
                
                    else
                        transform.GetChild(i).GetComponent<Rigidbody>().AddForce(new Vector3(0, (Mathf.Sign(col.gameObject.GetComponent<Rigidbody>().velocity.y))*col.gameObject.GetComponent<Rigidbody>().velocity.magnitude * force_factor, 0));
                }

                destroyed = true;
            }
        }
        
    }
}
