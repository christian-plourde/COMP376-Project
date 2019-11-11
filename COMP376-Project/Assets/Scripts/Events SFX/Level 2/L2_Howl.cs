using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L2_Howl : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        AudioManager.instance.Play("Howl");
        Destroy(this.gameObject);

    }
}
