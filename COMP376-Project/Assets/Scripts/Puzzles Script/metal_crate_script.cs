using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class metal_crate_script : MonoBehaviour
{
    public Transform player_transform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, player_transform.position.z);
    }
}
