using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lvl3_MorePewter : MonoBehaviour
{

    public GameObject PlayerRef;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerRef.GetComponent<Player>().pewterCount <= 0 && !PlayerRef.GetComponent<Player>().usingPewter)
        {
            PlayerRef.GetComponent<Player>().pewterCount += 1;
        }
    }
}
