using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform m_startPoint;
    public Transform m_playerRef;

    EnemyHealth m_health;
    BossPhase1 m_phase1;
    BossPhase2 m_phase2;
    BossPhase3 m_phase3;

    int m_phase;
    int m_currentPhase;
    bool m_isPlayerHit;
    float m_playerHitTimer;
    float m_playerHitCooldown;

    bool m_isBossHit;
    float m_bossHitTimer;
    float m_bossHitCooldown;

    bool m_isRunning;

    [HideInInspector]
    public bool m_isImmune;

    [HideInInspector]
    public Animator m_animator;


    // hammer object references to toggle on/off
    public GameObject backHammer;
    public GameObject heldHammer;



    // Start is called before the first frame update
    void Start()
    {
        m_phase = 1;
        m_health = GetComponent<EnemyHealth>();
        m_phase1 = GetComponent<BossPhase1>();
        m_phase2 = GetComponent<BossPhase2>();
        m_phase3 = GetComponent<BossPhase3>();

        m_phase1.enabled = false;
        m_phase2.enabled = false;
        m_phase3.enabled = false;

        m_isPlayerHit = false;
        m_playerHitCooldown = 2;
        m_playerHitTimer = m_playerHitCooldown;

        m_isBossHit = false;
        m_bossHitCooldown = 2;
        m_bossHitTimer = m_bossHitCooldown;

        m_isRunning = false;
        m_isImmune = false;


        //animation events
        backHammer.SetActive(true);
        heldHammer.SetActive(false);

        //audio
        AudioManager.instance.Play("BossMusic_Phase1");
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
            m_phase1.enabled = true;
        }
        else if (m_phase == 2)
        {
            m_phase1.enabled = true;
        }
        else if (m_phase == 3)
        {
            m_phase1.enabled = true;
        }
        else
        {
            StartCoroutine(GoToStartPosition(m_phase1));
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
        if (collision.tag == "JoraFist" && !m_isImmune)
        {
            m_isBossHit = true;
            m_health.TakeDamage(1);
            m_bossHitTimer = m_bossHitCooldown;
            Debug.Log("enemy punched");
        }

    }

    public bool IsRunning
    {
        get { return m_isRunning; }
        set
        {
            m_isRunning = value;
            this.GetComponent<Animator>().SetBool("Dashing", m_isRunning);
        }
    }

    void setEnemyDirection(Vector3 targetPos)
    {
        Vector3 lookPosition = targetPos - transform.position;
        lookPosition.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);
    }

    void SetNextPhase()
    {

    }

    void StartNextPhase()
    {
        if (m_phase == 1)
            m_phase = 2;
        else if (m_phase == 2)
            m_phase = 3;
    }

    IEnumerator GoToStartPosition(Object currPhase)
    {
        m_isImmune = true;

        m_phase1.enabled = false;
        m_phase2.enabled = false;
        m_phase3.enabled = false;

        yield return new WaitForSeconds(1f);
        IsRunning = true;
        transform.position = Vector3.MoveTowards(transform.position, m_startPoint.position, 5 * Time.deltaTime);
        setEnemyDirection(m_startPoint.position);
        if (Vector3.Distance(transform.position, m_startPoint.position) < 0.2f)
        {
            m_isImmune = false;
            IsRunning = false;
            this.transform.rotation = Quaternion.Euler(0, -90, 0);
            StartNextPhase();
        }
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

    
}
