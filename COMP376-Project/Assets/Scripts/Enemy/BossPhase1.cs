using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhase1 : MonoBehaviour
{
    public Transform m_camera;
    public Transform[] m_dashSpots;
    public Transform m_damagingFloor;

    Boss m_boss;

    public float m_dashSpeed = 5.0f;

    bool m_start = false;

    int m_dashCounter;
    int m_dashSpotIndex;

    bool m_isDashing;
    bool m_isIdle;
    bool m_slamAttack;

    float m_startTimer;
    float m_rotateSpeed = 10;
    float m_dashTimer;
    float m_dashCooldown;

    float m_slamAttackTimer;
    float m_slamAttackLength;

    // Start is called before the first frame update
    void Start()
    {
        IsIdle = true;
        Dashing = false;
        IsSlamAttack = false;

        m_startTimer = 7.0f;

        m_dashCounter = 0;
        m_dashSpotIndex = 0;

        m_dashCooldown = 2;

        m_damagingFloor.GetComponent<DamagingFloor>().enabled = false;
        m_damagingFloor.gameObject.SetActive(false);

        m_slamAttackTimer = 0;
        m_slamAttackLength = 1.7f;
    }

    // Update is called once per frame
    void Update()
    {

        //let the boss start dashing after a delay
        if (!m_start)
        {
            this.GetComponent<Boss>().m_isImmune = true;
            m_startTimer -= Time.deltaTime;
            if (m_startTimer <= 0)
            {
                this.GetComponent<Boss>().m_isImmune = false;
                Dashing = true;
                m_start = true;
            }
        }
        else
        {
            if (m_isDashing)
            {
                DashToSpot();
            }
            else if (m_slamAttack)
            {
                StartCoroutine(SlamAttack());
            }
            else 
            {
                 WaitForNextDash();
            }
        }
    }

    void DashToSpot()
    {
        if (m_isDashing)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_dashSpots[m_dashSpotIndex].position, m_dashSpeed * Time.deltaTime);
        }

        //when it reaches a move spot
        if (Vector3.Distance(transform.position, m_dashSpots[m_dashSpotIndex].position) < 0.2f)
        {
            FlipEnemy();
            SetDashSpotIndex();
            if (m_dashCounter == 2)
            {
                m_dashCounter = 0;
                m_dashTimer = 0;
                Dashing = false;
                IsSlamAttack = true;
                m_damagingFloor.gameObject.SetActive(true);
                return;
            }

            m_dashTimer = 0;
            Dashing = false;
            IsIdle = true;
            m_dashCounter++;
        }
    }

    void WaitForNextDash()
    {
        m_dashTimer += Time.deltaTime;
        if (m_dashTimer >= m_dashCooldown)
        {
            Dashing = true; 
            IsIdle = false;
        }
    }

    IEnumerator SlamAttack()
    {
        m_slamAttackTimer += Time.deltaTime;
        //the enemies hammer will slam down on the ground and add a camera shake
        if (m_slamAttackTimer > m_slamAttackLength)
        {
            //enable the collider of the damaging floor
            m_damagingFloor.GetComponent<DamagingFloor>().enabled = true;
            m_camera.GetComponent<CameraShake>().ShakeCamera(0.007f, 0.001f);

            //after the slam happens, wait a quick second before going to the next state
            yield return new WaitForSeconds(1f);
            IsSlamAttack = false;
            m_damagingFloor.gameObject.SetActive(false);
            m_damagingFloor.GetComponent<DamagingFloor>().enabled = false;
            m_slamAttackTimer = 0;
        }
    }

    void FlipEnemy()
    {
        transform.forward = transform.forward * -1;
    }

    void SetDashSpotIndex()
    {
        if (m_dashSpotIndex == 0)
            m_dashSpotIndex = 1;
        else
            m_dashSpotIndex = 0;
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

    void setEnemyDirection(Vector3 targetPos)
    {
        Vector3 lookPosition = targetPos - transform.position;
        lookPosition.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * m_rotateSpeed);
    }
}
