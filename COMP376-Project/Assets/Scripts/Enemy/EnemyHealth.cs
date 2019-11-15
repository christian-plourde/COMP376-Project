using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int m_startingHealth = 4;
    int m_currentHealth;


    bool m_isDead;

    // Start is called before the first frame update
    void Start()
    {
        m_isDead = false;
        m_currentHealth = m_startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetCurrentHealth()
    {
        return m_currentHealth;
    }

    public void TakeDamage(int amount)
    {

    }

    public void Death()
    {

    }

}
