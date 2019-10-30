using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tripwire_arrow_delete : MonoBehaviour
{
    float timer;
    // Update is called once per frame
    void Update()
    {
        if (timer > 4f)
            Destroy(this.gameObject);
        else
            timer += Time.deltaTime;
    }
}
