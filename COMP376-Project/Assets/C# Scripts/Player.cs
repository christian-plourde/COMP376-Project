using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    //experimental - delete or relocate
    


    //Player Variables
    private const float SPEED = 5f, JUMPFORCE = 10f;        // make sure to update constants when  you update the speed and jump below
    private const int MAXHEALTH = 3;

    private float speed=5f;                     // Movement speed       
    private float jumpForce = 8.5f;
    public int health = 3;                  // assuming we have 8 bars of health and lose one health every hit 
    private int faceDirection = 1;         // default facing negative z axis

    //bools for animation & states
    bool isDead=false;
    bool isRunning;
    public bool isGrounded;
    bool controlLock=false;                       // stop playing movement, e.g. enable when using pull / push

    //powerups bool
    bool activePower = false;

    //powerups variable
    public float potiontime = 0.0f;

    //animator component
    Animator animator;

    //animation variables
    bool usePotionAnim;
    float potionAnimTimer = 0f;
    bool tookDamageAnim;
    float tookDamageTimer = 0f;


    //potions
    public int steelCount; bool usingSteel; float steelPotionTime=15f;
    public int ironCount;   bool usingIron; float ironPotionTime = 30f;         // low for testing purpose, increase later
    public int pewterCount; bool usingPewter; float pewterPotionTime = 20f;

    //abilities parameters
    float ironPullPower=25f; float steelPushPower=25f;
    float pewterSpeedBoost=7.5f; float pewterJumpBoost = 12f;
    float mouseHoldTime = 1f;                        // for how much force

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
        steelCountCheckpoint = steelCount;
        ironCountCheckpoint = ironCount;
        pewterCountCheckpoint = pewterCount;
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
        if (activePower && usingIron && !usePotionAnim)
            useIron();
        
        if(activePower && usingSteel && !usePotionAnim)
            useSteel();

        if (activePower && usingPewter && !usePotionAnim)
            usePewter();

        // delay animation --> works
        if (usePotionAnim)
        {
            if (potionAnimTimer > 3.8f)            // drink animation length is 3.7ish, if we want to increase / decrease delay, we can always change animation speed and this value
            {
                animator.SetBool("isUsingPotion",false);
                usePotionAnim = false;
                controlLock = false;
                potionAnimTimer = 0f;
                animator.SetBool("isRunning", false);
            }
            else
            {
                potiontime = 0f;                           // you dont want the potion time to go down when drinking animation is happening
                potionAnimTimer += Time.deltaTime;
                controlLock = true;
                animator.SetBool("isUsingPotion", true);
            }

        }

        //take damage anim
        if (tookDamageAnim)
        {
            if (tookDamageTimer > 0.5f)
            {
                tookDamageTimer = 0f;
                tookDamageAnim = false;
                animator.SetBool("tookDamage", false);
            }
            else
            {
                tookDamageTimer += Time.deltaTime;
            }
        }


        //testing purposes:
        // respawn key for testing
        if (Input.GetKey(KeyCode.R))
            respawnPlayer();

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
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isGrounded = false;
            //GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetBool("isGrounded",false);
        }

        /// _____________________________________________________________________
        /// //power inputs:
        /// change hot keys if you want
        if (Input.GetKeyDown(KeyCode.Alpha1) && isGrounded)       // iron
        {
            if (ironCount > 0)
            {
                usePotionAnim = true;
                potiontime = 0f;
                ironCount--;
                activePower = true;
                usingIron = true;
                usingSteel = false;usingPewter = false;
                Debug.Log("iron consumed.");
            }
            else
            {
                //instantiate UI prefab that says not enough iron (dissappears after 2 seconds) 
            }
        }          
        if (Input.GetKeyDown(KeyCode.Alpha2) && isGrounded)       // steel
        {
            if (steelCount > 0)
            {
                usePotionAnim = true;
                potiontime = 0f;
                steelCount--;
                activePower = true;
                usingSteel = true;
                usingIron = false;usingPewter = false;
                Debug.Log("steel consumed.");
            }
            else
            {
                //instantiate UI prefab that says not enough iron (dissappears after 2 seconds) 
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && isGrounded)       // pewter
        {
            if (pewterCount > 0)
            {
                usePotionAnim = true;
                potiontime = 0f;
                pewterCount--;
                activePower = true;
                usingPewter = true;
                usingIron = false;usingSteel = false;
                Debug.Log("pewter consumed.");
            }
            else
            {
                //instantiate UI prefab that says not enough iron (dissappears after 2 seconds) 
            }



            
        }

        
        // other things to do
        // if input Esc --> pause, have an exit button (create a prefab UI with an exit button that just loads main menu
    }

    private void checkGrounded()
    {
        // i m using line cast, but we can change to raycast later, line cast is cheaper
        Vector3 castPoint = new Vector3( transform.position.x,transform.position.y,transform.position.z);
        if(Physics.Linecast(castPoint,new Vector3(transform.position.x,transform.position.y-0.05f,transform.position.z)))
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
        tookDamageAnim = true;
        animator.SetBool("tookDamage",true);
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
            Cursor.visible = false;
            isDead = true;
            animator.SetBool("isDead",true);
            activePower = false;
            potiontime = 0f;
            Debug.Log("You are dead.");
            transform.Translate(new Vector3(0,0,transform.position.z+0.2f));
            // respawn delay in update
            
        }
    }

    private void respawnPlayer()                  // respawn at checkpoint
    {
        //reset states
        isDead = false;
        animator.SetBool("isDead",false);
        health = MAXHEALTH;
        transform.position = checkpoint;
        activePower = false;
        potiontime = 0f;

        //if pewter was active
        speed = SPEED;
        jumpForce = JUMPFORCE;


        //restore potion count to that of checkpoint
        steelCount = steelCountCheckpoint;
        ironCount = ironCountCheckpoint;
        pewterCount = pewterCountCheckpoint;

    }


    //potion powers

        


    private GameObject acceptMouseInput()                       // called from useiron, useSteel & usePewter
    {
        
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 250f))
        {
              if (hit.collider.gameObject.tag == "Interactable")
              {
                    return hit.collider.gameObject;
              }
        }
        
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
            //experimental
            if (Input.GetMouseButton(0) && mouseHoldTime<3f)
                mouseHoldTime += Time.deltaTime;                    // we use this time to increase the force


            if (Input.GetMouseButtonUp(0))
            {
                GameObject clickedOn = acceptMouseInput();
                if (clickedOn != null)              // clicked on interactable, tag check in acceptMouseInput function.
                {
                    Vector3 towardsPlayer = transform.position - clickedOn.GetComponent<Transform>().position;
                    towardsPlayer.y += 2f;         // aim at the head instead of the chest
                    Debug.Log("Force added mouse: "+mouseHoldTime + " and force: "+ironPullPower);
                    clickedOn.GetComponent<Rigidbody>().AddForce((mouseHoldTime*2)*ironPullPower * towardsPlayer);   // pulls towards your direction
                    mouseHoldTime = 1f;
                    //Debug.Log("Velocity on clicked body: "+clickedOn.GetComponent<Rigidbody>().velocity);
                }
            }
        }
        else
        {
            //potion expired
            Debug.Log("Time you held the click:"+ mouseHoldTime);
            Debug.Log("Iron Potion expired");
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
            //experimental
            if (Input.GetMouseButton(0) && mouseHoldTime < 3f)
                mouseHoldTime += Time.deltaTime;                    // we use this time to increase the force


            if (Input.GetMouseButtonUp(0))
            {
                GameObject clickedOn = acceptMouseInput();
                if (clickedOn != null)              // clicked on interactable, tag check in acceptMouseInput function.
                {
                    Vector3 awayFromPlayer = clickedOn.GetComponent<Transform>().position-transform.position;
                    awayFromPlayer.y += 1f;         // little positive offset on y so that object can actually fly
                    Debug.Log("Force added mouse: " + mouseHoldTime + " and force: " + steelPushPower);
                    clickedOn.GetComponent<Rigidbody>().AddForce((mouseHoldTime * 2) * steelPushPower * awayFromPlayer);   // pulls towards your direction
                    mouseHoldTime = 1f;
                    Debug.Log("Velocity on clicked body: " + clickedOn.GetComponent<Rigidbody>().velocity);
                }
            }
        }
        else
        {
            //potion expired
            Debug.Log("Steel Potion expired");
            usingSteel = false;
            activePower = false;
            potiontime = 0.0f;
            Cursor.visible = false;
        }
    }

    //pewter potion
    private void usePewter()
    {
        //for prototype just boost speed duration
        if (potiontime < pewterPotionTime)
        {
            
            potiontime += Time.deltaTime;
            speed = pewterSpeedBoost;
            jumpForce = pewterJumpBoost;
        }
        else
        {
            //reset speed and jump values
            speed = SPEED;
            jumpForce = JUMPFORCE;

            //potion expired
            Debug.Log("Pewter Potion expired");
            usingPewter = false;
            activePower = false;
            potiontime = 0.0f;
            
        }
    }


    //experimental if objects hit you too fast
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Interactable")
        {
            Rigidbody collided = collision.collider.GetComponent<Rigidbody>();
            Debug.Log("Object hit you at: "+collided.velocity);    // works pretty well, if absolute of velocity on z too hard?, damage player based on the rigid body's mass
            if (Mathf.Abs((collided.velocity.y)) > 2.5f || Mathf.Abs((collided.velocity.z)) > 2.5f)
            {
                registerHit();
            }
        }

        //if collision from an enemy collider or use istrigger
    }


    private void potionAnimationEnable()
    {
        usePotionAnim = true;
        
    }

}
