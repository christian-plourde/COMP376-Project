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

    bool m_isIdle;
    bool m_isWalking;
    bool m_chasingPlayer;
    int m_randomSpot;

    public float m_startPatrolWaitTime;
    float m_patrolWaitTime;
    float m_rotateSpeed = 10;

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
        m_isIdle = true;
        animator.SetBool("Idle", m_isIdle);
        m_isWalking = false;
        animator.SetBool("Idle", m_isWalking);
        m_chasingPlayer = false;
        animator.SetBool("ChasingPlayer", m_chasingPlayer);

        m_patrolWaitTime = m_startPatrolWaitTime;
        m_randomSpot = Random.Range(0, m_moveSpots.Length);
    }

    // Update is called once per frame
    void Update()
    {
        //chase the player if the player is in field of vision and patrol otherwise
        if (IsPlayerInVision())
        {
            if(!IsPlayerInAttackRange())
                ChasePlayer();

            IsPlayerInAttackRange();
        }
        else
        {
            Patrol();
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
        //Maybe we will have that functionality?
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

            return true;
        }
        else
        {
            m_chasingPlayer = false;
            animator.SetBool("ChasingPlayer", m_chasingPlayer);
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
            //if the player is not in attacking range and too far for chasing, go back to patrolling
            m_isWalking = true;
            animator.SetBool("Patrolling", m_isWalking);
        }

        return false;
    }

}
