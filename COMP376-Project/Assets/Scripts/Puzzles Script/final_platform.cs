using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class final_platform : MonoBehaviour
{

    bool colliding;
    public Animator animator;
    int colliding_count;
    public Transform platform_transform;

    public bool Colliding
    {
        get { return colliding; }
        set { colliding = value; animator.SetBool("colliding", colliding); }
    }

    // Start is called before the first frame update
    void Start()
    {
        Colliding = true;
        colliding_count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        
        if(col.gameObject.transform.position.y > platform_transform.position.y && (col.CompareTag("Player") || col.CompareTag("Interactable")))
        {
            colliding_count++;
            Colliding = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        colliding_count--;
            if (colliding_count <= 0)
            {
                colliding_count = 0;
                Colliding = false;
            }
                
        
    }
}
