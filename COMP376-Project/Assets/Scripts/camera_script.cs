using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_script : MonoBehaviour
{
    // Start is called before the first frame update

    public float move_speed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.Translate(Vector3.right * move_speed * Time.deltaTime);
        }

        if(Input.GetAxis("Horizontal") < 0)
        {
            transform.Translate(Vector3.left * move_speed * Time.deltaTime);
        }

        if(Input.GetAxis("Vertical") < 0)
        {
            transform.Translate(Vector3.down * move_speed * Time.deltaTime);
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            transform.Translate(Vector3.up * move_speed * Time.deltaTime);
        }

    }
}
