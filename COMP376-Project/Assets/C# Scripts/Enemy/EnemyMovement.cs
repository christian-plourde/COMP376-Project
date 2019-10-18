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
    HealthScript m_health;

    bool m_isIdle;
    bool m_isWalking;
    int m_randomSpot;

    public float m_startPatrolWaitTime;
    float m_patrolWaitTime;

    void Awake()
    {
        m_playerRef = GameObject.FindGameObjectWithTag("Player").transform;

        m_isIdle = true;
        m_isWalking = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_patrolWaitTime = m_startPatrolWaitTime;
        m_randomSpot = Random.Range(0, m_moveSpots.Length);
    }

    // Update is called once per frame
    void Update()
    {
        //chase the player if the player is in field of vision and patrol otherwise
        if (IsPlayerInVision())
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        //check every frame if player is in attack range to start attacking
        IsPlayerInRange();
    }

    void Patrol()
    {
        if(m_isWalking)
            transform.position = Vector3.MoveTowards(transform.position, m_moveSpots[m_randomSpot].position, m_walkSpeed * Time.deltaTime);
            
        //When it reaches the move spot, wait before going to another location
        if(Vector3.Distance(transform.position, m_moveSpots[m_randomSpot].position) < 0.2f)
        {
            if (m_patrolWaitTime <= 0)
            {
                m_randomSpot = Random.Range(0, m_moveSpots.Length);
                m_isWalking = true;
                m_isIdle = false;
                m_patrolWaitTime = m_startPatrolWaitTime;
            }
            else
            {
                //set idle to true when the enemy reaches a spot and stops walking
                m_isWalking = false;
                m_isIdle = true;
                m_patrolWaitTime -= Time.deltaTime;
            }
        }
    }

    //walks towards the player
    void ChasePlayer()
    {
        if (m_isWalking)
            transform.position = Vector3.MoveTowards(transform.position, m_playerRef.position, m_walkSpeed * Time.deltaTime);
    }

    //returns true if player is in LOS 
    public bool IsPlayerInVision()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 toPlayer = m_playerRef.transform.position - transform.position;
        float distanceToPlayer = Vector3.Distance(transform.position, m_playerRef.position);

        if (Vector3.Dot(forward, toPlayer) > 0 && distanceToPlayer < m_rangeToEngage)
        {
            return true;
        }
        return false;
    }

    //Returns true if player is in attacking range
    public bool IsPlayerInRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, m_playerRef.position);
        if (distanceToPlayer < m_rangeToAttack)
        {
            m_isWalking = false;
            m_isIdle = false;
            return true;
        }

        m_isWalking = true;

        return false;
    }

}
