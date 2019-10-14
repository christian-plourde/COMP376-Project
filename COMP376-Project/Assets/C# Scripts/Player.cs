using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    //Player Variables
    private float speed=5f;                     // Movement speed       
    private float jumpForce = 2f;
    private int health = 8;                  // assuming we have 8 bars of health and lose one health every hit 
    private int faceDirection = 1;         // default facing negative z axis

    //bools for animation & states
    bool isDead=false;
    bool isRunning;
    private bool isGrounded;
    bool controlLock=false;                       // stop playing movement, e.g. enable when using pull / push

    //animator component
    Animator animator;

    //potions
    int steelCount;
    int ironCount;
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
            if (ironCount > 0)
            {
                ironCount--;
                useIron();
            }
            else
            {
                //instantiate UI prefab that says not enough iron (dissappears after 2 seconds)
            }
        }          
        if (Input.GetKey(KeyCode.Alpha2))       // steel
        { }          
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
        if (Physics.Linecast(castPoint, new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z+0.215f))
            ||
            Physics.Linecast(castPoint, new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z))
            ||
            Physics.Linecast(castPoint, new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z-0.215f)))
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
        if (health > 0)
            health--;
        else
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
    private void useSteel() { }

    private void useIron() { }

    private void usePewter()
    {
        //speed boost
        // speed+=pewterSpeedBoost

        //onExpiry
        //speed-=pewterSpeedBoost

    }


    private void acceptMouseInput()                       // called from useiron, useSteel & usePewter
    {
        //enable cursor
        // raycast on mouse click (raycast on negative x axis)
        // use force if you click on interactable objects
    }
}
