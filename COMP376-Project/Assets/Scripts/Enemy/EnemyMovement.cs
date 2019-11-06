using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float m_rangeToEngage;
    public float m_rangeToAttack;
    public float m_walkSpeed;
    public Transform[] m_moveSpots;

    Transform m_playerRef;
    public GameObject m_ragDollPrefab;

    bool m_isIdle;
    bool m_isWalking;
    bool m_chasingPlayer;
    int m_randomSpot;

    public float m_startPatrolWaitTime;
    float m_patrolWaitTime;
    float m_rotateSpeed = 10;

    bool m_isDead;

    //animator component
    [HideInInspector] public Animator animator;

    void Awake()
    {
        m_playerRef = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_isIdle = false;
        animator.SetBool("Idle", m_isIdle);
        m_isWalking = true;
        animator.SetBool("Idle", m_isWalking);
        m_chasingPlayer = false;
        animator.SetBool("ChasingPlayer", m_chasingPlayer);

        m_isDead = false;
        animator.SetBool("isDead", m_isDead);

        m_patrolWaitTime = m_startPatrolWaitTime;
        m_randomSpot = Random.Range(0, m_moveSpots.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isDead)
        {
            animator.SetBool("isDead", m_isDead);
            GameObject tempRef=Instantiate(m_ragDollPrefab, transform.position, Quaternion.identity);
            
            Destroy(gameObject);
        }
        else
        {
            if (m_playerRef.GetComponent<Player>().getIsDead())
            {
                m_isIdle = true;
                animator.SetBool("Idle", m_isIdle);
            }
            else
            {
                //chase the player if the player is in field of vision and patrol otherwise
                if (IsPlayerInVision())
                {
                    if (!IsPlayerInAttackRange())
                        ChasePlayer();

                    IsPlayerInAttackRange();
                }
                else
                {
                    Patrol();
                }
            }
        }
    }

    public bool IsDead {
        get { return m_isDead; }
        set { m_isDead = value;
            animator.SetBool("isDead", m_isDead);
        }
    }
    
    void Patrol()
    {
        if(m_isWalking)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_moveSpots[m_randomSpot].position, m_walkSpeed * Time.deltaTime);
            setEnemyDirection(m_moveSpots[m_randomSpot].position);
        }

        //When it reaches the move spot, wait before going to another location
        if (Vector3.Distance(transform.position, m_moveSpots[m_randomSpot].position) < 0.2f)
        {
            if (m_patrolWaitTime <= 0)
            {
                m_randomSpot = Random.Range(0, m_moveSpots.Length);
                //set idle to false and walking to true when enemy patrols again
                m_isWalking = true;
                animator.SetBool("Patrolling", m_isWalking);

                m_isIdle = false;
                animator.SetBool("Idle", m_isIdle);
                m_patrolWaitTime = m_startPatrolWaitTime;
            }
            else
            {
                //set idle to true when the enemy reaches a spot and stops walking
                m_isWalking = false;
                animator.SetBool("Patrolling", m_isWalking);

                m_isIdle = true;
                animator.SetBool("Idle", m_isIdle);
                m_patrolWaitTime -= Time.deltaTime;   
            }
        }
    }

    //Chase the player when not in attacking range but when in field of vision
    void ChasePlayer()
    {
        //This is so the enemy doesnt walk and attack at the same time. 
        if (m_chasingPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_playerRef.position, m_walkSpeed * Time.deltaTime);
            setEnemyDirection(m_playerRef.position);
        }
    }

    void setEnemyDirection(Vector3 targetPos)
    {
        Vector3 lookPosition = targetPos - transform.position;
        lookPosition.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * m_rotateSpeed);
    }

    //returns true if player is in LOS 
    public bool IsPlayerInVision()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 toPlayer = m_playerRef.transform.position - transform.position;
        float distanceToPlayer = Vector3.Distance(transform.position, m_playerRef.position);

        if (distanceToPlayer < m_rangeToEngage)
        {
            m_chasingPlayer = true;
            animator.SetBool("ChasingPlayer", m_chasingPlayer);

            //if the player is not in attacking range and too far for chasing, go back to patrolling
            m_isWalking = false;
            animator.SetBool("Patrolling", m_isWalking);

            return true;
        }
        else
        {
            m_chasingPlayer = false;
            animator.SetBool("ChasingPlayer", m_chasingPlayer);

            //if the player is not in attacking range and too far for chasing, go back to patrolling
            m_isWalking = true;
            animator.SetBool("Patrolling", m_isWalking);
        }


        return false;
    }

    //Returns true if player is in attacking range
    public bool IsPlayerInAttackRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, m_playerRef.position);
        if (distanceToPlayer < m_rangeToAttack)
        {
            m_isWalking = false;
            animator.SetBool("Patrolling", m_isWalking);

            m_isIdle = false;
            animator.SetBool("Idle", m_isIdle);

            m_chasingPlayer = false;
            animator.SetBool("ChasingPlayer", m_chasingPlayer);

            return true;
        }
        else if(distanceToPlayer > m_rangeToEngage) 
        {
         
        }

        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Interactable")
        {
            Rigidbody collided = collision.collider.GetComponent<Rigidbody>();
            Debug.Log("Enemy HIT");
            if (Mathf.Abs((collided.velocity.x)) > 1f || Mathf.Abs((collided.velocity.y)) > 1f || Mathf.Abs((collided.velocity.z)) > 1f)
            {
                m_isDead = true;
            }
        }

        if(collision.collider.tag =="Spikes")
        {
            m_isDead = true;
        }

        // testing punch
        if (collision.collider.tag == "JoraFist")
        {
            Debug.Log("HURTS OOWWW");
            //m_isDead = true;
        }
    }
}
