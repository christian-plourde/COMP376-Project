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

    [HideInInspector] public Animator animator;

    void Awake()
    {
        //set references
        m_playerRef = GameObject.FindGameObjectWithTag("Player").transform;
        m_movement = GetComponent<EnemyMovement>();
        animator = GetComponent<Animator>();
        m_isAttacking = false;
        animator.SetBool("Attacking", m_isAttacking);
    }

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        //if player is in attack range and the enemy attack is off cooldown
        if (m_movement.IsPlayerInAttackRange() && (m_attackTimer >= m_timeBetweenAttacks))
        {
            m_isAttacking = true;
            animator.SetBool("Attacking", m_isAttacking);
            m_attackTimer = 0.0f;
        }
        else
        {
            m_isAttacking = false;
            animator.SetBool("Attacking", m_isAttacking);
        }

        m_attackTimer += Time.deltaTime;
    }

    void OnCollisionEnter(Collision other)
    {
        //if enemy collides with the player and the enemy is attacking
        if (other.gameObject == m_playerRef.gameObject)
        {
            Debug.Log("Player hit!");
            m_playerRef.GetComponent<Player>().registerHit(100);
        }
    }
}
