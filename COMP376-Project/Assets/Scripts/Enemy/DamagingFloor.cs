using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingFloor : MonoBehaviour
{
    public Transform m_playerRef;
    Player m_player;

    float m_playerHitTimer;
    float m_playerHitCooldown;

    // Start is called before the first frame update
    void Start()
    {
        m_playerHitTimer = 3;
        m_playerHitCooldown = 3;
        m_player = m_playerRef.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        m_playerHitTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        float dist = m_playerRef.position.y - this.transform.position.y;
        if (this.enabled && dist < 0.001f)
        {
            if(m_playerHitTimer >= m_playerHitCooldown)
            {
                m_player.registerHit(1);
                m_playerHitTimer = 0;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
  
    }
}
