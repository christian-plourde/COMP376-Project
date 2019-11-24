using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinshotEyesight : MonoBehaviour
{

    Coinshot CoinshotScriptRef;

    void Start()
    {
        CoinshotScriptRef = gameObject.GetComponentInParent<Coinshot>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            CoinshotScriptRef.Target = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            CoinshotScriptRef.Target = null;
    }
}
