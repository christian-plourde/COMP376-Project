using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhase1 : MonoBehaviour
{
    public float m_dashSpeed = 5.0f;
    public Transform[] m_dashSpots;

    bool m_start = false;

    int m_dashCounter;
    int m_dashSpotIndex;
    bool m_isRunning;
    bool m_isIdle;
    float m_startTimer;
    float m_rotateSpeed = 10;
    float m_dashTimer;
    float m_dashCooldown;

    // Start is called before the first frame update
    void Start()
    {
        m_isRunning = false ;
        m_isIdle = true;
        m_startTimer = 5.0f;

        m_dashCounter = 0;
        m_dashSpotIndex = 0;

        m_dashCooldown = 3;
    }

    // Update is called once per frame
    void Update()
    {

        //let the boss start dashing after a delay
        if(!m_start)
        {
            m_startTimer -= Time.deltaTime;
            if(m_startTimer <= 0)
            {
                m_isRunning = true;
                m_start = true;
            }
        }

        if (m_isRunning)
        {
            //start dashing
            DashToSpot();
        }
        else
        {
            WaitForNextDash();
        }
    }

    void DashToSpot()
    {
        if(m_isRunning)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_dashSpots[m_dashSpotIndex].position, m_dashSpeed * Time.deltaTime);
           // setEnemyDirection(m_dashSpots[m_dashSpotIndex].position);
        }

        //when it reaches a move spot
        if (Vector3.Distance(transform.position, m_dashSpots[m_dashSpotIndex].position) < 0.2f)
        {
            if(m_dashCounter == 3)
            {
                //do some kind of attack

                return;
            }

            FlipEnemy();
            SetDashSpotIndex();
            m_isRunning = false;
            m_isIdle = true;
            m_dashCounter++;
            m_dashTimer = 0;
        }
    }

    void WaitForNextDash()
    {
        m_dashTimer += Time.deltaTime;
        if(m_dashTimer >= m_dashCooldown)
        {
            m_isRunning = true;
        }
    }

    void setEnemyDirection(Vector3 targetPos)
    {
        Vector3 lookPosition = targetPos - transform.position;
        lookPosition.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * m_rotateSpeed);
    }

    void FlipEnemy()
    {
        transform.forward = transform.forward * -1;
    }

    void SetDashSpotIndex()
    {
        if (m_dashSpotIndex == 0)
            m_dashSpotIndex = 1;
        else
            m_dashSpotIndex = 0;
    }
}
