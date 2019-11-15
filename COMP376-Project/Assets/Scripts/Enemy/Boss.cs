using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform m_startPoint;
    public Transform m_playerRef;

    EnemyHealth m_health;
    BossPhase1 m_phase1;

    int m_phase;
    int m_currentPhase;
    bool m_isPlayerHit;
    float m_playerHitTimer;
    float m_playerHitCooldown;

    bool m_isBossHit;
    float m_bossHitTimer;
    float m_bossHitCooldown;

    bool m_isRunning;

    // Start is called before the first frame update
    void Start()
    {
        m_phase = 1;
        m_health = GetComponent<EnemyHealth>();
        m_phase1 = GetComponent<BossPhase1>();

        m_isPlayerHit = false;
        m_playerHitCooldown = 2;
        m_playerHitTimer = m_playerHitCooldown;

        m_isBossHit = false;
        m_bossHitCooldown = 2;
        m_bossHitTimer = m_bossHitCooldown;

        m_isRunning = false;
    }

    // Update is called once per frame
    void Update()   
    {
        if(m_isPlayerHit)
        {
            m_playerHitTimer -= Time.deltaTime;
            if (m_playerHitTimer <= 0)
                m_isPlayerHit = false;
        }

        if(m_isBossHit)
        {
            m_bossHitTimer -= Time.deltaTime;
            if (m_bossHitTimer <= 0)
                m_isBossHit = false;
        }

        if(m_health.GetCurrentHealth() <= 7)
        {
            m_phase = -100; 
        }

        if (m_phase == 1)
        {

        }
        else if (m_phase == 2)
        {
           
        }
        else if (m_phase == 3)
        {

        }
        else
        {
            m_phase1.enabled = false;
            m_isRunning = true;
            transform.position = Vector3.MoveTowards(transform.position, m_startPoint.position, 5 * Time.deltaTime);
            if (Vector3.Distance(transform.position, m_startPoint.position) < 0.2f)
            {
                m_phase = 2;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(!m_isPlayerHit)
            {
                m_isPlayerHit = true;
                collision.gameObject.GetComponent<Player>().registerHit(1);  
                m_playerHitTimer = m_playerHitCooldown;
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "JoraFist")
        {
            m_isBossHit = true;
            m_health.TakeDamage(1);
            m_bossHitTimer = m_bossHitCooldown;
            Debug.Log("enemy punched");
        }

    }
}
