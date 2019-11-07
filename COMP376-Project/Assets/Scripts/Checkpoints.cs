using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{

    bool reachCheckpoint;

    Player Pscript;

    // Start is called before the first frame update
    void Start()
    {
        Pscript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // save checkpoint
            Pscript.checkpoint = transform.position;
            Pscript.ironCountCheckpoint = Pscript.ironCount;
            Pscript.steelCountCheckpoint = Pscript.steelCount;
            Pscript.pewterCountCheckpoint = Pscript.pewterCount;
            reachCheckpoint = true;
            Destroy(this.gameObject);             // because we dont want a previous check point to get updated when player goes back
        }
    }
}
