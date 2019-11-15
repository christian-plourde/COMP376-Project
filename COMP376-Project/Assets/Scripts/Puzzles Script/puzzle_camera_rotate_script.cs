using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzle_camera_rotate_script : MonoBehaviour
{

    public Camera camera;
    CameraFollow_Script cam_script;
    public float rotate_amount;
    public float zoom_factor;
    public float x_offset_factor;
    // Start is called before the first frame update
    void Start()
    {
        cam_script = FindObjectOfType<CameraFollow_Script>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            camera.transform.Rotate(Vector3.up, rotate_amount);
            cam_script.xOffset -= x_offset_factor*rotate_amount;
            cam_script.ZOffset_Zoom -= zoom_factor*rotate_amount;
        }
            
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            camera.transform.Rotate(Vector3.up, -rotate_amount);
            cam_script.xOffset += x_offset_factor * rotate_amount;
            cam_script.ZOffset_Zoom += zoom_factor*rotate_amount;
        }
            
    }
}
