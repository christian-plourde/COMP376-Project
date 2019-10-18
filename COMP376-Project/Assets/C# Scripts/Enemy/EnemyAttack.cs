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
        if (m_movement.IsPlayerInRange())
        {
            m_isAttacking = true;
        }
        else
        {
            m_isAttacking = false;
        }
 
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && m_isAttacking)
        {
            AttackPlayer(other.gameObject);
        }
    }

    void AttackPlayer(GameObject player)
    {
        Debug.Log("Player hit!");
        //Decrease player hp
        //add knockback to player
    }
}
