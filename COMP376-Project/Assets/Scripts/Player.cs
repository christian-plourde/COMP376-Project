using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    //experimental
    public float jumpDelay;
    public bool onLadder;
    public bool usingLadder;
    public Animation camera_pan;
    DateTime game_start;


    //Player Variables
    private const float SPEED = 4f, JUMPFORCE = 6f;        // make sure to update constants when  you update the speed and jump below
    private const int MAXHEALTH = 4;

    private float speed=4f;                     // Movement speed       
    private float jumpForce = 6f;
    public int health = 4;                  // assuming we have 8 bars of health and lose one health every hit 
    [HideInInspector]public int faceDirection = 1;         // default facing negative z axis

    //bools for animation & states
    bool isDead=false;
    bool isRunning=false;
    [HideInInspector]public bool isGrounded;
    public bool jumped = false;
    [HideInInspector] public bool controlLock=false;                       // stop playing movement, e.g. enable when using pull / push
    
    //powerups bool
    [HideInInspector]public bool activePower = false;

    //powerups variable
    [HideInInspector]public float potiontime = 0.0f;

    //animator component
    [HideInInspector]public Animator animator;

    //animation variables
    bool usePotionAnim;
    float potionAnimTimer = 0f;
    bool tookDamageAnim;
    float tookDamageTimer = 0f;


    //potions
    public int steelCount; public bool usingSteel;[HideInInspector] public float steelPotionTime=15f;
    public int ironCount; public bool usingIron;[HideInInspector] public float ironPotionTime = 30f;         // low for testing purpose, increase later
    public int pewterCount;[HideInInspector] public bool usingPewter;[HideInInspector] public float pewterPotionTime = 20f;

    // count limit:
    const int IRONLIMIT=6;
    const int STEELLIMIT=6;
    const int PEWTERLIMIT=3;


    //abilities parameters
    float ironPullPower=25f; float steelPushPower=20f;
    float pewterSpeedBoost=5f; float pewterJumpBoost = 6.5f;
    float mouseHoldTime = 1f;                        // for how much force

    //respawn point
    [HideInInspector]public Vector3 checkpoint;                      // update value at checkpoints (update from trigger objects that are checkpoints)
    [HideInInspector] public int steelCountCheckpoint;
    [HideInInspector] public int ironCountCheckpoint;
    [HideInInspector] public int pewterCountCheckpoint;

    Scene m_scene;

    private void Step()
    {
        if (isGrounded)
            AudioManager.instance.Play("step");
    }

    float respawnTimer;
    float respawnTime=5f;


    void Start()
    {
        m_scene = SceneManager.GetActiveScene(); 

        game_start = DateTime.Now;

        AudioManager.instance.Play("background1");
        AudioManager.instance.Play("background2");

        Cursor.visible = false;             // we dont want the cursor to show unless player is using the push or pull ability
        //init variables
        checkpoint = transform.position;
        steelCountCheckpoint = steelCount;
        ironCountCheckpoint = ironCount;
        pewterCountCheckpoint = pewterCount;
        animator = GetComponent<Animator>();
        isGrounded = true;
        //init potion count from save?? or set to zero if no saves, 
    }

    
    void Update()
    {


        // dont use regular checkGrounded, ray cast function works better
        //checkGrounded();             
        rayCastCheckGrounded();
        if (!isDead && !controlLock)                                  // dont allow movement if dead or controllock is on
        {

            if (m_scene.name == "BossTestScene")
            {
                Camera.main.GetComponent<Animator>().enabled = false;
                Camera.main.GetComponent<CameraFollow_Script>().enabled = false; 
                Camera.main.transform.rotation = Quaternion.Euler(0, 0, 0);
                playerMovement();
            }
            else if ((DateTime.Now - game_start).TotalSeconds > camera_pan.clip.length + 1)
            {
                playerMovement();  // wasd, space      
            }

            powerControls();                                          // hotkeys for powers
            activePowerUps();                                         // active power up will function when this method is called every frame
        }
        if (isDead)
        {
            AutoRespawn();
        }

        // delay animation --> works
        if (usePotionAnim)
            potionAnimationDelay();                                    // just a function that locks player control for a while

        //take damage anim
        if (tookDamageAnim)
            takeDamageAnimDelay();


        LimitPotionCount();

        //____________________________________________________

        if (Input.GetKeyDown(KeyCode.K))
            registerHit();

            // respawn key for testing
        if (Input.GetKeyDown(KeyCode.R))
            respawnPlayer();

        if (Input.GetKeyDown(KeyCode.Alpha4))
            transform.position=new Vector3(106.37f,-12.5f,157.47f);



    }

    //helper methods
    private void playerMovement()
    {
        
        // Horizontal Movemnet: Character default faces -ve Z, initial rotation 180 degrees, so he has to move on -ve z axis of his local scale
        //                      which translates to positive z axis on the world.
        //                      if player puts down opposite button, calls changeDirection() and rotates player 180 degrees again.
        if (Input.GetKey(KeyCode.A) && !usingLadder)
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

        if (Input.GetKey(KeyCode.D) && !usingLadder)
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
        //if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        if (Input.GetKeyDown(KeyCode.Space) && (jumpDelay<0.2f || onLadder))
        {           
            Vector3 jumpVector;
            isGrounded = false;
            AudioManager.instance.Play("jump");
            jumped = true;
            //if (jumpDelay < 0.2f)
            //jumpDelay =100f;
            if (faceDirection == 1)
                jumpVector = new Vector3(0.3f,1.0f,0);
            else
                jumpVector = new Vector3(-0.3f, 1.0f, 0);
            //GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            if(isRunning)
                GetComponent<Rigidbody>().AddForce(jumpVector * jumpForce, ForceMode.Impulse);
            else
                GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetBool("isGrounded",false);

            

        }

        
        // other things to do
        // if input Esc --> pause, have an exit button (create a prefab UI with an exit button that just loads main menu
    }

    private void powerControls()
    {

        /// _____________________________________________________________________
        /// //power inputs:
        /// change hot keys if you want
        if (Input.GetKeyDown(KeyCode.Alpha1) && isGrounded && (!punching && !comboPunch))       // iron
        {
            if (ironCount > 0)
            {
                mouseHoldTime = 1f;
                usePotionAnim = true;
                AudioManager.instance.Play("drink");
                potiontime = 0f;
                ironCount--;
                activePower = true;
                usingIron = true;
                usingSteel = false; usingPewter = false;
                animator.SetBool("usingPewter",false);
                speed = SPEED;
                jumpForce = JUMPFORCE;
                Debug.Log("iron consumed.");
            }
            else
            {
                //instantiate UI prefab that says not enough iron (dissappears after 2 seconds) 
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && isGrounded && (!punching && !comboPunch))       // steel
        {
            if (steelCount > 0)
            {
                mouseHoldTime = 1f;
                usePotionAnim = true;
                AudioManager.instance.Play("drink");
                potiontime = 0f;
                steelCount--;
                activePower = true;
                usingSteel = true;
                usingIron = false; usingPewter = false;
                animator.SetBool("usingPewter", false);
                speed = SPEED;
                jumpForce = JUMPFORCE;
                Debug.Log("steel consumed.");
            }
            else
            {
                //instantiate UI prefab that says not enough iron (dissappears after 2 seconds) 
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && isGrounded && (!punching && !comboPunch))       // pewter
        {
            if (pewterCount > 0)
            {
                mouseHoldTime = 1f;
                usePotionAnim = true;
                AudioManager.instance.Play("drink");
                potiontime = 0f;
                pewterCount--;
                activePower = true;
                usingPewter = true;
                usingIron = false; usingSteel = false;
                animator.SetBool("usingPewter", false);
                Debug.Log("pewter consumed.");
            }
            else
            {
                //instantiate UI prefab that says not enough iron (dissappears after 2 seconds) 
            }




        }

    }

    private void activePowerUps()
    {
        //powerups
        if (activePower && usingIron && !usePotionAnim)
            useIron();

        if (activePower && usingSteel && !usePotionAnim)
            useSteel();

        if (activePower && usingPewter && !usePotionAnim)
            usePewter();
    }


    private void rayCastCheckGrounded()
    {
        // allows  to jump off edge properly
        if (!isGrounded)
            jumpDelay += Time.deltaTime;
        else if (isGrounded)
            jumpDelay = 0;

        Vector3 castPoint1 = new Vector3(transform.position.x+0.15f, transform.position.y + 0.2f, transform.position.z);
        Vector3 castPoint2 = new Vector3(transform.position.x-0.15f, transform.position.y + 0.2f, transform.position.z);
        if (Physics.Raycast(castPoint1, Vector3.down, 0.3f) || Physics.Raycast(castPoint2, Vector3.down, 0.3f))
        {
            isGrounded = true;
            jumped = false;
            animator.SetBool("isGrounded", true);
        }
        else
        {
            isGrounded = false;
            animator.SetBool("isGrounded", false);
        }
    }

    public void changePlayerDirection()
    {
        if (faceDirection == 1)
            faceDirection = -1;
        else
            faceDirection = 1;
        GetComponent<Rigidbody>().velocity = new Vector3(0f, GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);
        transform.Rotate(new Vector3(0, 180, 0)); // flip player (rotate 180) when they press opposite button on horizontal movement

        // punching related, if you try to go away from punching, you gain your original speed right away.
        if (punching)
        {
            punchTimer = 10f;         // this should force exit punch animation
            punchTimer2 = 10f;         // this should force exit punch animation

        }
    }                

    public void registerHit()                 // public method, enemy call this method to damage player
    {
        health--;
        tookDamageAnim = true;
        AudioManager.instance.Play("hurt");
        animator.SetBool("tookDamage",true);
        //experimental
        if (usePotionAnim)            // cancel effect
        {
            activePower = false;
            usingPewter = false;
            usingIron = false;
            usingSteel = false;
            usePotionAnim = false;
            animator.SetBool("isUsingPotion",false);
            controlLock = false;
        }

        if (health <= 0)
            killPlayer();

        
    }

    public void registerHit(int damage)                 // overloaded public method, enemy call this method to damage player with damage parameter
    {
        health -= damage;
        tookDamageAnim = true;
        AudioManager.instance.Play("hurt");
        animator.SetBool("tookDamage", true);
        if (usePotionAnim)            // cancel effect
        {
            activePower = false;
            usingPewter = false;
            usingIron = false;
            usingSteel = false;
            usePotionAnim = false;
            animator.SetBool("isUsingPotion", false);
            controlLock = false;
        }
        if (health <= 0)
        {
            health = 0;
            killPlayer();
        }
    }

    public bool getIsDead()
    {
        return isDead;
    }

    private void killPlayer()
    {
        if (health > 0)
            Debug.Log("Wrong Call killplayer on Player.");
        else
        {
            AudioManager.instance.Play("death");
            Cursor.visible = false;
            isDead = true;
            animator.SetBool("isDead",true);
            animator.SetBool("isRunning",false);
            activePower = false;
            potiontime = 0f;
            Debug.Log("You are dead.");
            
            // respawn delay in update
            
        }
    }

    private void respawnPlayer()                  // respawn at checkpoint
    {
        //reset states
        isDead = false;
        animator.SetBool("isDead",false);
        animator.SetBool("Punch1",false);
        animator.SetBool("Punch2",false);
        animator.SetBool("usingPewter",false);
        punching = false;
        comboPunch = false;

        health = MAXHEALTH;
        transform.position = checkpoint;
        activePower = false;
        usingPewter = false;
        usingIron = false;
        usingSteel = false;
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
                Debug.Log("Object at pos: " + hit.collider.GetComponent<Transform>().position);

                if (hit.collider.GetComponent<Rigidbody>().isKinematic)  // basically anything thats in background, need to bring in foreground
                {
                    //if z plane was different
                    Vector3 newPos = new Vector3(hit.collider.GetComponent<Transform>().position.x, hit.collider.GetComponent<Transform>().position.y, transform.position.z);
                    hit.collider.GetComponent<Transform>().position = newPos;
                    hit.collider.GetComponent<Rigidbody>().isKinematic = false;

                    //scale down so looks consitent
                    Vector3 oldScale = hit.collider.transform.localScale;
                    hit.collider.transform.localScale = new Vector3(0.8f*oldScale.x,0.8f*oldScale.y,0.8f*oldScale.z);
                }
                   
                return hit.collider.gameObject;
            }
            // if lever 1
            else if (hit.collider.gameObject.tag == "DLever1")
            {
                hit.collider.gameObject.GetComponent<Animator>().enabled = true;
                GameObject db1=GameObject.FindGameObjectWithTag("Drawbridge1");
                db1.GetComponent<Animator>().enabled = true;
            }
            // if lever 2
            else if (hit.collider.gameObject.tag == "DLever2")
            {
                hit.collider.gameObject.GetComponent<Animator>().enabled = true;
                GameObject db1 = GameObject.FindGameObjectWithTag("Drawbridge2");
                db1.GetComponent<Animator>().enabled = true;
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
                    towardsPlayer.z = 0;
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
                    awayFromPlayer.z = 0;
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


    public bool punching;
    public bool comboPunch;
    float punch1Time=1.2f;
    float punch2Time=1.8f;
    float punchTimer;
    float punchTimer2;

    public GameObject LeftFistObject;
    public GameObject RightFistObject;


    //pewter potion
    private void usePewter()
    {
        //for prototype just boost speed duration
        if (potiontime < pewterPotionTime)
        {
            
            potiontime += Time.deltaTime;
            speed = pewterSpeedBoost;
            jumpForce = pewterJumpBoost;
            animator.SetBool("usingPewter",true);

            //actual controls:
            PewterCombat();
        }
        else
        {
            //reset speed and jump values
            speed = SPEED;
            jumpForce = JUMPFORCE;

            animator.SetBool("Punch1",false);
            animator.SetBool("Punch2",false);
            punching = false;
            comboPunch = false;

            //potion expired
            Debug.Log("Pewter Potion expired");
            usingPewter = false;
            activePower = false;
            animator.SetBool("usingPewter", false);
            potiontime = 0.0f;
            
        }
    }


    private void PewterCombat()
    {
        if (Input.GetMouseButtonDown(0) && isGrounded)
        {
            animator.SetBool("Punch1", true);
            RightFistObject.SetActive(true);
            punching = true;
        }
        // if you started running while punching. --> slow down move speed, and allow punching
        if (isRunning && punching)
        {
            speed = 1f;
            animator.SetBool("Punch1", true);
            RightFistObject.SetActive(true);
        }

        //if you fell or jumped while punching --> cancel
        if (!isGrounded && punching)
        {
            RightFistObject.SetActive(false);
            punching = false;
            punchTimer = 0;
            animator.SetBool("Punch1",false);
            animator.SetBool("Punch2",false);
        }
        



        if (punching)
        {
            if (punchTimer < punch1Time)
            {
                punchTimer += Time.deltaTime;

            }
            else
            {
                punchTimer = 0f;
                punching = false; 
                RightFistObject.SetActive(false);
                animator.SetBool("Punch1", false);
                if (speed != pewterSpeedBoost && usingPewter)
                    speed = pewterSpeedBoost;
                else if (speed != SPEED && !usingPewter)
                {
                    speed = SPEED;
                }
            }



            //combo punch
            if (punching)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    comboPunch = true;
                    Debug.Log("Combo Punch!!");
                }
            }

            

            
        }


        if (comboPunch)
        {
            if (!isGrounded)
            {
                animator.SetBool("Punch2", false);
                LeftFistObject.SetActive(false);
            }
            else
            {
                if (punchTimer2 < punch2Time)
                {
                    punchTimer2 += Time.deltaTime;
                    if (punchTimer2 > 0.5f)
                    {
                        animator.SetBool("Punch2", true);
                        LeftFistObject.SetActive(true);
                        if (isRunning && animator.GetBool("Punch2"))
                        {
                            speed = 1f;
                        }

                    }
                }
                else
                {
                    punchTimer2 = 0;
                    animator.SetBool("Punch2", false);
                    LeftFistObject.SetActive(false);

                    comboPunch = false;

                    //reset speeds
                    if (speed != pewterSpeedBoost && usingPewter)
                        speed = pewterSpeedBoost;
                    else if (speed != SPEED && !usingPewter)
                    {
                        speed = SPEED;
                    }
                }
            }

        }

    }




    //experimental if objects hit you too fast
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Interactable")
        {
            
            Rigidbody collided = collision.collider.GetComponent<Rigidbody>();
            Debug.Log("Object hit you at: " + collided.velocity);    // works pretty well, if absolute of velocity on z too hard?, damage player based on the rigid body's mass
            if (Mathf.Abs((collided.velocity.x)) > 2.5f || Mathf.Abs((collided.velocity.y)) > 2.5f || Mathf.Abs((collided.velocity.z)) > 2.5f)
            {
                registerHit();
            }
        }
        else if (collision.collider.tag == "Spikes")
        {
            //insta death
            registerHit(100);
        }

        //Collision from enemy mace
        if(collision.collider.tag == "EnemyMace")
        {
            registerHit(2);
            //Debug.Log("Ouchie");
        }

        if (collision.collider.tag == "TripwireArrow")
        {
            registerHit();
            Destroy(collision.collider.gameObject);
        }
    }


    private void potionAnimationEnable()
    {
        usePotionAnim = true;
        
    }

    private void potionAnimationDelay()
    {
        if (potionAnimTimer > 3f)            // drink animation length is 3.7ish, if we want to increase / decrease delay, we can always change animation speed and this value
        {
            animator.SetBool("isUsingPotion", false);
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

    private void takeDamageAnimDelay()
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

    private void LimitPotionCount()
    {
        if (ironCount > IRONLIMIT)
            ironCount = IRONLIMIT;

        if (steelCount > STEELLIMIT)
            steelCount = STEELLIMIT;

        if (pewterCount > PEWTERLIMIT)
            pewterCount = PEWTERLIMIT;
                
    }


    private void AutoRespawn()
    {
        if (respawnTimer < respawnTime)
        {
            respawnTimer += Time.deltaTime;
        }
        else
        {
            respawnPlayer();
            respawnTimer = 0;
        }
    }


}
