using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pressure_plate : MonoBehaviour
{
    // Start is called before the first frame update

    bool on_plate;
    public Animator plate_animator;
    public Animator door_animator;

    public bool OnPlate
    {
        get { return on_plate; }
        set { on_plate = value;
            plate_animator.SetBool("on_plate", on_plate);
            door_animator.SetBool("on_plate", on_plate);
        }
    }

    void Start()
    {
        OnPlate = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        OnPlate = true;
    }

    void OnTriggerExit(Collider col)
    {
        OnPlate = false;
    }
}
