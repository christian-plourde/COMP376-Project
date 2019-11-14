﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    int m_phase;
    EnemyHealth m_health;
    BossPhase1 m_phase1;

    bool m_isPlayerHit;
    float m_playerHitTimer;
    float m_playerHitCooldown;
    // Start is called before the first frame update
    void Start()
    {
        m_phase = 1;
        m_health = GetComponent<EnemyHealth>();
        m_phase1 = GetComponent<BossPhase1>();

        m_isPlayerHit = false;
        m_playerHitCooldown = 2;
        m_playerHitTimer = m_playerHitCooldown;
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(!m_isPlayerHit)
            {
                collision.gameObject.GetComponent<Player>().registerHit(1);
                m_isPlayerHit = true;
                m_playerHitTimer = m_playerHitCooldown;
            }
        }
    }
}
