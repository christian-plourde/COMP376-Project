using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tripwire : MonoBehaviour
{

    public GameObject wire;
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
        //Debug.Log("tripwire hit");
        Destroy(wire);
    }
}
