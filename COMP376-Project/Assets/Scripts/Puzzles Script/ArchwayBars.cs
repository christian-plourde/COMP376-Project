using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchwayBars : MonoBehaviour
{
    Animator animator;
    public bool start_opened;
    bool opened;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        opened = false;
        if (start_opened)
            toggle_gate();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggle_gate()
    {
        animator.SetBool("opened", !opened);
        opened = !opened;
    }

}
