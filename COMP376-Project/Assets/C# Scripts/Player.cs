using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    //Player Variables
    private float speed=5f;                     // Movement speed       
    private float jumpForce = 3.5f;
    private int health = 8;                  // assuming we have 8 bars of health and lose one health every hit 
    private int faceDirection = 1;         // default facing negative z axis

    //bools for animation & states
    bool isDead=false;
    bool isRunning;
    private bool isGrounded;
    bool controlLock=false;                       // stop playing movement, e.g. enable when using pull / push

    //powerups bool
    bool activePower = false;

    //powerups variable
    float potiontime = 0.0f;

    //animator component
    Animator animator;

    //potions
    public int steelCount; bool usingSteel; float steelPotionTime=15f;
    public int ironCount;   bool usingIron; float ironPotionTime = 10f;         // low for testing purpose, increase later
    int pewterCount; float pewterSpeedBoost=1.5f;

    //respawn point
    [HideInInspector]public Vector3 checkpoint;                      // update value at checkpoints (update from trigger objects that are checkpoints)
    [HideInInspector] public int steelCountCheckpoint;
    [HideInInspector] public int ironCountCheckpoint;
    [HideInInspector] public int pewterCountCheckpoint;

    // UI object references
    // health
    // potions


    
    void Start()
    {
        Cursor.visible = false;             // we dont want the cursor to show unless player is using the push or pull ability
        //init variables
        //checkpoint = spawn postion
        checkpoint = transform.position;
        animator = GetComponent<Animator>();
        isRunning = false;
        isGrounded = true;

        //init potion count from save?? or set to zero if no saves, 
    }

    
    void Update()
    {
        checkGrounded();
        if (!isDead && !controlLock)                                  // dont allow movement if dead or controllock is on
            playerMovement();

        //powerups
        if (activePower && usingIron && !usingSteel)
        {
            useIron();
        }
        

        if(activePower && usingSteel && !usingIron)
        {
            useSteel();
        }
        

        //if active and using steel
    }

    //helper methods
    private void playerMovement()
    {
        // Horizontal Movemnet: Character default faces -ve Z, initial rotation 180 degrees, so he has to move on -ve z axis of his local scale
        //                      which translates to positive z axis on the world.
        //                      if player puts down opposite button, calls changeDirection() and rotates player 180 degrees again.
        if (Input.GetKey(KeyCode.A))
        {
            if (faceDirection == (-1))
            {
                transform.Translate(new Vector3(0,0,-speed*Time.deltaTime));
                isRunning = true;
                animator.SetBool("isRunning", true);
            }
            else
                changePlayerDirection();
        }

        if (Input.GetKey(KeyCode.D))
        {
            if (faceDirection == 1)
            {
                transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime));
                isRunning = true;
                animator.SetBool("isRunning", true);
            }
            else
                changePlayerDirection();
        }
        // when you let go of AD keys
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            isRunning = false;
            animator.SetBool("isRunning", false);
        }

        //jump
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            isGrounded = false;
            //GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetBool("isGrounded",false);
        }

        /// _____________________________________________________________________
        /// //power inputs:
        /// change hot keys if you want
        if (Input.GetKey(KeyCode.Alpha1))       // iron
        {
            if (ironCount > 0 && !activePower)
            {
                ironCount--;
                activePower = true;
                usingIron = true;
                Debug.Log("iron consumed.");
            }
            else
            {
                //instantiate UI prefab that says not enough iron (dissappears after 2 seconds) 
            }
        }          
        if (Input.GetKey(KeyCode.Alpha2))       // steel
        {
            if (steelCount > 0 && !activePower)
            {
                steelCount--;
                activePower = true;
                usingSteel = true;
                Debug.Log("steel consumed.");
            }
            else
            {
                //instantiate UI prefab that says not enough iron (dissappears after 2 seconds) 
            }
        }          
        if (Input.GetKey(KeyCode.Alpha3))       // pewter
        { }          // pewter


        // other things to do
        // if input Esc --> pause, have an exit button (create a prefab UI with an exit button that just loads main menu
    }

    private void checkGrounded()
    {
        // i m using line cast, but we can change to raycast later, line cast is cheaper
        // see if any of these 3 points touch something (i.e. middle point of character, and two offsets on each side)
        // if any of them return true we are on the ground.
        Vector3 castPoint = new Vector3( transform.position.x,transform.position.y+0.05f,transform.position.z);
        if (Physics.Linecast(castPoint, new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z+0.115f))
            ||
            Physics.Linecast(castPoint, new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z))
            ||
            Physics.Linecast(castPoint, new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z-0.115f)))
        {
            //Debug.Log("Touching.");
            isGrounded = true;
            animator.SetBool("isGrounded",true);
            
        }
    }

    private void changePlayerDirection()
    {
        if (faceDirection == 1)
            faceDirection = -1;
        else
            faceDirection = 1;
        transform.Rotate(new Vector3(0,180,0)); // flip player (rotate 180) when they press opposite button on horizontal movement
    }                    

    public void registerHit()                 // public method, enemy call this method to damage player
    {
        health--;
        if (health <= 0)
            killPlayer();
        
    }

    public void registerHit(int damage)                 // overloaded public method, enemy call this method to damage player with damage parameter
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            killPlayer();
        }
    }

    private void killPlayer()
    {
        if (health > 0)
            Debug.Log("Wrong Call killplayer on Player.");
        else
        {
            isDead = true;
            // respawn delay
            // respawn
        }
    }

    private void respawnPlayer()                  // respawn at checkpoint
    {
        //reset states
        isDead = false;
        health = 8;
        transform.position = checkpoint;

        //restore potion count to that of checkpoint
        steelCount = steelCountCheckpoint;
        ironCount = ironCountCheckpoint;
        pewterCount = pewterCountCheckpoint;

    }


    //potion powers

    private void usePewter()
    {
        //speed boost
        // speed+=pewterSpeedBoost

        //onExpiry
        //speed-=pewterSpeedBoost

    }


    private GameObject acceptMouseInput()                       // called from useiron, useSteel & usePewter
    {
        //enable cursor
        // raycast on mouse click (raycast on negative x axis)
        // use force if you click on interactable objects

        if (Input.GetMouseButton(0))
        {
            //ray cast from this position:
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Vector3 mousePosition = Camera.main.ScreenPointToRay(Input.mousePosition);
            //if (Physics.Raycast(ray, hit, 250f))
            if (Physics.Raycast(ray, out hit, 250f))
            {
                if (hit.collider.gameObject.tag == "Interactable")
                {
                    //Debug.Log("Click on an interacble");
                    return hit.collider.gameObject;
                }
                else
                    return null;
            }
            else
                return null;
            
        }
        else
            return null;
    }

    private void highlightInteractables(float time)
    {
        if (time == 0f)          // if nothing was given to this
            time = 30f;

        // foreach (GameObject in array) { some how hight light them}
        // on time expiry dont high light them anymore.
    }

    //potion functions

    private void useIron()                                   // is only called after validating iron stats.
    {
        if (potiontime < ironPotionTime)
        {
            Cursor.visible = true;
            potiontime += Time.deltaTime;

            // actual clicking calculations
            GameObject clickedOn = acceptMouseInput();
            if (clickedOn != null)              // meaning its interactable
            {
                // apply force of pull
                int forceDir;
                if (clickedOn.GetComponent<Transform>().position.z - transform.position.z > 0)
                    forceDir = -1;
                else
                    forceDir = 1;
                clickedOn.GetComponent<Rigidbody>().AddForce(forceDir*50f*Vector3.forward);   // forward i.e. towards you
            }
        }
        else
        {
            //potion expired
            Debug.Log("Potion expired");
            usingIron = false;
            activePower = false;
            potiontime = 0.0f;
            Cursor.visible = false;
        }
    }


    //steel potion
    private void useSteel()                                   // is only called after validating iron stats.
    {
        if (potiontime < steelPotionTime)
        {
            Cursor.visible = true;
            potiontime += Time.deltaTime;

            // actual clicking calculations
            GameObject clickedOn = acceptMouseInput();
            if (clickedOn != null)              // meaning its interactable
            {
                // apply force of pull
                int forceDir;
                if (clickedOn.GetComponent<Transform>().position.z - transform.position.z > 0)
                    forceDir = -1;
                else
                    forceDir = 1;
                clickedOn.GetComponent<Rigidbody>().AddForce(forceDir * 50f * Vector3.back);    // backward i.e. away from you
            }
        }
        else
        {
            //potion expired
            Debug.Log("Potion expired");
            usingIron = false;
            activePower = false;
            potiontime = 0.0f;
            Cursor.visible = false;
        }
    }

    //pewter potion
    private void usePewter();
}
