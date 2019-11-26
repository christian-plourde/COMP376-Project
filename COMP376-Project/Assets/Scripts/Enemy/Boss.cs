using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform m_startPoint;
    public Transform m_playerRef;
    public Transform m_damagingFloor;

    public ParticleSystem m_implosionTeleport;
    public ParticleSystem m_explosionteleport;
    float m_fadeTime = 3;
    float _alpha = 0.0f;
    float _startAlpha = 0f;
    float _endAlpha = 1.0f;
    MeshRenderer m_bossRenderer;

    EnemyHealth m_health;
    BossPhase1 m_phase1;
    BossPhase2 m_phase2;
    int m_numOfTimesHit;

    int m_phase;
    int m_previousPhase;
    bool m_isPhase2;
    bool m_isPlayerHit;
    float m_playerHitTimer;
    float m_playerHitCooldown;

    bool m_isBossHit;
    float m_bossHitTimer;
    float m_bossHitCooldown;

    bool m_isDashing;
    bool m_isIdle;
    bool m_slamAttack;

    [HideInInspector]
    public bool m_isImmune;

    [HideInInspector]
    public Animator m_animator;


    // hammer object references to toggle on/off
    public GameObject backHammer;
    public GameObject heldHammer;

    bool m_implosionCreated = false;
    bool m_explosionCreated = false;

    Vector3 m_bossPosition;
    Vector3 m_teleportOffset;

    // Start is called before the first frame update
    void Start()
    {
        m_numOfTimesHit = 0;
        IsIdle = true;
        Dashing = false;
        IsSlamAttack = false;

        m_phase = 1;
        m_previousPhase = 0;
        m_health = GetComponent<EnemyHealth>();
        m_phase1 = GetComponent<BossPhase1>();
        m_phase2 = GetComponent<BossPhase2>();

        m_phase1.enabled = false;
        m_phase2.enabled = false;

        m_isPlayerHit = false;
        m_playerHitCooldown = 2;
        m_playerHitTimer = m_playerHitCooldown;

        m_isPhase2 = false;
        m_isBossHit = false;
        m_bossHitCooldown = 2;
        m_bossHitTimer = m_bossHitCooldown;

        m_isImmune = false;

        //animation events
        backHammer.SetActive(true);
        heldHammer.SetActive(false);

        //audio
        AudioManager.instance.Play("Boss_Phase1_Music");

        m_teleportOffset = new Vector3(0, 1, 0);

        m_bossRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        m_bossPosition = transform.position;
        if (m_isPlayerHit)
        {
            m_playerHitTimer -= Time.deltaTime;
            if (m_playerHitTimer <= 0)
                m_isPlayerHit = false;
        }

     /*   if (m_isBossHit)
        {
            m_bossHitTimer -= Time.deltaTime;
            if (m_bossHitTimer <= 0)
                m_isBossHit = false;
        }*/

        if (GetComponent<EnemyHealth>().GetCurrentHealth() <= 0 && !m_isPhase2)
        {
            m_previousPhase = m_phase;
            m_phase = -1;
            m_isPhase2 = true;
        }

        if (m_phase == 1)
        {
            m_phase1.enabled = true;
        }
        else if (m_phase == 2)
        {
            m_phase2.enabled = true;
        }
        else
        {
            StartCoroutine(ResetBoss());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!m_isPlayerHit)
            {
                m_isPlayerHit = true;
                collision.gameObject.GetComponent<Player>().registerHit(1);
                m_playerHitTimer = m_playerHitCooldown;
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "JoraFist" && !m_isImmune)
        {
            collision.gameObject.SetActive(false);
            m_health.TakeDamage(1);
            m_isBossHit = true;
            m_numOfTimesHit++;
            m_bossHitTimer = m_bossHitCooldown;
            Debug.Log("enemy punched");
        }

    }

    void setEnemyDirection(Vector3 targetPos)
    {
        Vector3 lookPosition = targetPos - transform.position;
        lookPosition.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);
    }

    void StartNextPhase()
    {
        if (m_previousPhase == 1)
            m_phase = 2;
    }

   // events
    public void EquipWeapon()
    {
        backHammer.SetActive(false);
        heldHammer.SetActive(true);
    }


    // audio events
    public void Footstep()
    {
        AudioManager.instance.Play("Boss_Footstep");
    }

    public void HammerHit()
    {
        AudioManager.instance.Play("Boss_Hammer_Hit");
        AudioManager.instance.Play("Boss_Ground_Shake");
    }

    //teleports boss back to the starting position when starting a new phase
    IEnumerator ResetBoss()
    {
        m_isImmune = true;

        m_damagingFloor.gameObject.SetActive(false);

        StartCoroutine(FadeTo(0.0f, 2.0f));
        if (!m_implosionCreated)
        {
            Instantiate(m_implosionTeleport, m_bossPosition + m_teleportOffset, transform.rotation);
            m_implosionTeleport.Emit(1);    
            m_implosionCreated = true;
        }


        if (!m_explosionCreated)
        {
            Instantiate(m_explosionteleport, m_startPoint.position + m_teleportOffset, transform.rotation);
            m_explosionteleport.Emit(1);
            StartCoroutine(FadeTo(1.0f, 2.0f));
            m_explosionCreated = true;
        }


        //reset animation states
        IsIdle = true;
        Dashing = false;
        IsSlamAttack = false;

        m_phase1.enabled = false;
        m_phase2.enabled = false;

        yield return new WaitForSeconds(3.5f);

        this.transform.rotation = Quaternion.Euler(0, -90, 0);
        this.transform.position = m_startPoint.position;

        m_isImmune = false;

        StartNextPhase();
    }

    public bool IsIdle
    {
        get { return m_isIdle; }
        set
        {
            m_isIdle = value;
            this.GetComponent<Animator>().SetBool("Idle", m_isIdle);
        }
    }

    public bool Dashing
    {
        get { return m_isDashing; }
        set
        {
            m_isDashing = value;
            this.GetComponent<Animator>().SetBool("Dashing", m_isDashing);
        }
    }

    public bool IsSlamAttack
    {
        get { return m_slamAttack; }
        set
        {
            m_slamAttack = value;
            this.GetComponent<Animator>().SetBool("usingHammer", m_slamAttack);
        }
    }

    IEnumerator FadeTo(float aValue, float aTime)
    {
        float alpha = transform.GetComponent<MeshRenderer>().material.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            transform.GetComponent<MeshRenderer>().material.color = newColor;
            yield return null;
        }
    }

}
