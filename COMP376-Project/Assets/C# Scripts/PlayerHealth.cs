using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public int m_startingHealth = 4;
    public int m_currentHealth;

    bool m_isDead;

    void Awake()
    {
        m_currentHealth = m_startingHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int amount)
    {

    }

    public void Death()
    {

    }
}
