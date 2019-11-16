using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class UIScript : MonoBehaviour
{
    public Image timer;
    float width;

    public Image health1;
    public Image health2;
    public Image health3;
    public Image health4;

    public Image orange;
    public Image silver;
    public Image purple;

    public Text orangeV;
    public Text silverV;
    public Text purpleV;


    Player playerRef ;

    // Start is called before the first frame update
    void Start()
    {
        playerRef = GetComponent<Player>();
        timer.enabled = false;
        width = timer.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        //check the player's health
        if(playerRef.health == 4)
        {
            health1.enabled = true;
            health2.enabled = true;
            health3.enabled = true;
            health4.enabled = true;

        }
        else if(playerRef.health == 3)
        {
            health1.enabled = true;
            health2.enabled = true;
            health3.enabled = true;
            health4.enabled = false;

        }
        else if(playerRef.health == 2)
        {
            health1.enabled = true;
            health2.enabled = true;
            health3.enabled = false;
            health4.enabled = false;

        }
        else if(playerRef.health == 1)
        {
            health1.enabled = true;
            health2.enabled = false;
            health3.enabled = false;
            health4.enabled = false;

        }
        else
        {
            health1.enabled = false;
            health2.enabled = false;
            health3.enabled = false;
            health4.enabled = false;
        }

        //Checking what potions is active
        if (playerRef.usingIron)
        {
            orange.enabled = true;
            silver.enabled = false;
            purple.enabled = false;
            timer.enabled = true;
            float time = 1 - (playerRef.potiontime / playerRef.ironPotionTime);
            timer.fillAmount = time;
        }
        else if (playerRef.usingSteel)
        {
            orange.enabled = false;
            silver.enabled = true;
            purple.enabled = false;
            timer.enabled = true;
            float time = 1 - (playerRef.potiontime / playerRef.steelPotionTime);
            timer.fillAmount = time;
        }
        else if (playerRef.usingPewter)
        {
            orange.enabled = false;
            silver.enabled = false;
            purple.enabled = true;
            timer.enabled = true;
            float time = 1 - (playerRef.potiontime / playerRef.pewterPotionTime);
            timer.fillAmount = time;
        }
        else
        {
            orange.enabled = false;
            silver.enabled = false;
            purple.enabled = false;
            timer.enabled = false;
        }

        orangeV.text = "" + playerRef.ironCount;
        silverV.text = "" + playerRef.steelCount;
        purpleV.text = "" + playerRef.pewterCount;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(GameObject.FindGameObjectWithTag("Note"));
            playerRef.controlLock = false;
        }

    }
}
