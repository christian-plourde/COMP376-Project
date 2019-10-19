using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float m_timeBetweenAttacks = 1f;
    public int m_attackDamage = 4;

    Transform m_playerRef;
    EnemyHealth m_enemyHealth;
    EnemyMovement m_movement;

    bool m_isAttacking;

    float m_attackTimer;


    void Awake()
    {
        //set references
        m_playerRef = GameObject.FindGameObjectWithTag("Player").transform;
        m_movement = GetComponent<EnemyMovement>();

        m_isAttacking = false;
    }

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        //if player is in attack range and the enemy attack is off cooldown
        if (m_movement.IsPlayerInRange() && (m_attackTimer >= m_timeBetweenAttacks))
        {
            m_isAttacking = true;
        }
        else
        {
            m_isAttacking = false;
        }

        m_attackTimer += Time.deltaTime;
    }

    void OnCollisionEnter(Collision other)
    {
        //if enemy collides with the player and the enemy is attacking
        if (other.gameObject == m_playerRef.gameObject && m_isAttacking)
        {
            AttackPlayer(other.gameObject);
        }
    }

    void AttackPlayer(GameObject player)
    {
        m_attackTimer = 0.0f;

        Debug.Log("Player hit!");
        //Decrease player hp
        //add knockback to player
    }
}
