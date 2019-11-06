using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    bool opened = false;
    bool onCooldown = false;
    public int giveIronCount;
    public int giveSteelCount;
    public int givePewterCount;
    public GameObject giveNote;
    public Animator animator;

    public float cooldown=45f;
    float cooldownTimer;

    Player playerScriptRef;
    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("opened", false);
        playerScriptRef = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (onCooldown)
        {
            if (cooldownTimer < cooldown)
            {
                cooldownTimer += Time.deltaTime;
                Debug.Log("Time before you can use chest again. " + (cooldown - cooldownTimer));
            }
            else
            {
                cooldownTimer = 0;
                onCooldown = false;
            }

        }

        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetKey(KeyCode.E) && !onCooldown)
            {
                //Debug.Log("you has opened the chest");

                if (!opened)
                {
                    opened = true;
                    animator.SetBool("opened", opened);
                }

                onCooldown = true;

                // player potion ++;
                playerScriptRef.pewterCount += givePewterCount;
                playerScriptRef.ironCount += giveIronCount;
                playerScriptRef.steelCount += giveSteelCount;


                // if note not null, instantiate it
                if (giveNote != null)
                {
                    //instantiate note ui object, when we press Esc lets say, it gets destroyed.
                }

                //this.GetComponent<Chest>().enabled = false;
            }
            
        }
    }
}
