using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchwayBarLever : MonoBehaviour
{
    public GameObject[] archwayBars;
    ArchwayBars[] bars;
    Animator animator;
    Player player;
    public int id;
    bool on;

    // Start is called before the first frame update
    void Start()
    {
        on = false;
        player = FindObjectOfType<Player>();
        bars = new ArchwayBars[archwayBars.Length];
        for(int i = 0; i < archwayBars.Length; i++)
        {
            bars[i] = archwayBars[i].GetComponent<ArchwayBars>();
        }
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ArchwayBarLever ray_check = null;
        if (check_ray_cast(ref ray_check))
        {
            if(ray_check != null)
            {
                if(ray_check.id == this.id)
                {
                    if(Input.GetKey(KeyCode.Mouse0))
                    {
                        toggle_lever();
                    }
                }
            }
        }
    }

    public bool check_ray_cast(ref ArchwayBarLever obj)
    {
        if (!(player.usingIron || player.usingSteel))
            return false;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 250f))
        {
            if (hit.collider.gameObject.tag == "ArchwayBarsLever")
            {
                obj = hit.collider.gameObject.GetComponent<ArchwayBarLever>();
                return true;
            }
        }

        return false;
    }

    void toggle_lever()
    {
        animator.SetBool("on", !on);
        on = !on;
        foreach (ArchwayBars a in bars)
        {
            a.toggle_gate();
        }
    }
}
