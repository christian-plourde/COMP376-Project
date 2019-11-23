using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavePlatform : MonoBehaviour
{
    public bool destroyed;
    public float fade_step;


    // Start is called before the first frame update
    void Start()
    {
        destroyed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (destroyed)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;

                GameObject curr = transform.GetChild(i).gameObject;
                Renderer renderer = curr.GetComponent<Renderer>();

                foreach (Material m in renderer.materials)
                {
                    Color color = m.color;
                    color.a -= fade_step;
                    m.color = color;

                    if (m.color.a <= 0)
                    {
                        Destroy(curr);
                        continue;
                    }
                }
            }

            if (transform.childCount <= 1)
                Destroy(this.gameObject);
        }

    }
}
