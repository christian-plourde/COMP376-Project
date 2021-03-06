﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhase2 : MonoBehaviour
{
    public Transform m_camera;
    public Transform[] m_teleportPoints;

    public Transform[] m_platforms;
    public Transform[] m_planks;
    public Transform m_ground;

    public float m_timeOfVulnerability;
    public float m_numOfTeleports;

    Boss m_bossScript;
    ParticleSystem m_implosionTeleport;
    ParticleSystem m_explosionTeleport;

    SphereCollider m_sphereCollider;

    int m_teleportCounter;
    int m_teleportSpotIndex;
    int m_lastTeleportSpot;

    bool m_isTeleporting;
    bool m_isVulnerable;

    bool m_implosionCreated = false;
    bool m_explosionCreated = false;

    bool m_start;

    bool m_isIdle;

    float m_startTimer;

    float m_teleportCooldown;
    float m_teleportTimer;

    float m_implodeTimer;
    float m_implodeCooldown;


    Vector3 m_bossPosition;
    Vector3 m_teleportOffset;

    ParticleSystem implosion;
    ParticleSystem explosion;

    // Start is called before the first frame update
    void Start()
    {
        m_implodeTimer = 0;
        m_implodeCooldown = 2.0f;
        m_teleportCooldown = 3f;
        m_teleportTimer = 0;

        m_bossScript = this.transform.GetComponent<Boss>();
        m_implosionTeleport = m_bossScript.m_implosionTeleport;
        m_explosionTeleport = m_bossScript.m_explosionteleport;

        m_teleportCounter = 0;
        m_teleportSpotIndex = 0;
        m_lastTeleportSpot = m_teleportSpotIndex;

        m_isTeleporting = false;
        m_isVulnerable = false;

        m_start = false;

        m_startTimer = 5;

        this.GetComponent<Boss>().m_isImmune = true;

        Debug.Log("Phase 2 begins.");

        m_teleportOffset = new Vector3(0, 1, 0);

        m_sphereCollider = GetComponent<SphereCollider>();
        m_sphereCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        m_bossPosition = transform.position;
        if (!m_start)
        {
            IsIdle = true;
            this.transform.GetComponent<Rigidbody>().isKinematic = true;
            m_startTimer -= Time.deltaTime;
            if (m_startTimer <= 0)
            {
                IsTeleporting = true;
                m_start = true;

                Destroy(m_ground.gameObject);
                AudioManager.instance.Play("Boss_Ground_Shake");
                DestroyPlatforms();
                m_camera.GetComponent<CameraShake>().ShakeCamera(1f, 1f);
                this.transform.GetComponent<Rigidbody>().isKinematic = false;
                m_sphereCollider.enabled = true;

                ParticleSystem.MainModule implode = m_implosionTeleport.main;
                implode.startSize = 4f;
                implode.duration = 0.5f;

                ParticleSystem.MainModule explode = m_explosionTeleport.main;
                explode.startSize = 4f;
                explode.duration = 0.2f;
            }
        }

        if (m_isTeleporting)
        {
            this.GetComponent<Boss>().m_isImmune = true;
            Teleport();
        }
        else
        {
            this.GetComponent<Boss>().m_isImmune = false;
        }

        if(m_teleportCounter == m_numOfTeleports)
        {
            SetIsVulnerable();
        }
    }

    void Teleport()
    {
        m_teleportTimer += Time.deltaTime;

        if (!m_implosionCreated)
        {
            implosion = Instantiate(m_implosionTeleport, m_bossPosition + m_teleportOffset, transform.rotation);
            m_implosionTeleport.Emit(1);
            m_implosionCreated = true;
        }
        else
        {
            m_implodeTimer += Time.deltaTime;
            if (m_implodeTimer >= m_implodeCooldown)
            {
                this.transform.position = new Vector3(50, 50, 50);
            }
        }

        if (!m_explosionCreated)
        {
            SetRandomTeleportSpot();
            explosion = Instantiate(m_explosionTeleport, m_teleportPoints[m_teleportSpotIndex].position + m_teleportOffset, transform.rotation);
            m_explosionTeleport.Emit(1);
            m_explosionCreated = true;
        }

        if (m_teleportTimer >= m_teleportCooldown)
        {
            SetRotation();
            this.transform.position = m_teleportPoints[m_teleportSpotIndex].position;
            m_teleportCounter++;
            m_teleportTimer = 0;
            m_implodeTimer = 0;
            m_implosionCreated = false;
            m_explosionCreated = false;
            StartCoroutine(DestroyParticles(implosion));
            StartCoroutine(DestroyParticles(explosion));
        }
    }

    void DestroyPlatforms()
    {
        for (int i = 0; i < m_planks.Length; i++)
        {
            m_planks[i].GetComponent<PlanksObstacleBossFight>().destroyed = true;
        }

        for (int i = 0; i < m_platforms.Length; i++)
        {
            m_platforms[i].GetComponent<CavePlatform>().destroyed = true;
        }
    }

    void SetRandomTeleportSpot()
    {
        m_teleportSpotIndex = Random.Range(0, m_teleportPoints.Length);
        while (m_teleportSpotIndex == m_lastTeleportSpot)
            m_teleportSpotIndex = Random.Range(0, m_teleportPoints.Length);
        m_lastTeleportSpot = m_teleportSpotIndex;
    }

    void SetIsVulnerable()
    {
        IsTeleporting = false;
        IsTired = true;
        m_sphereCollider.enabled = false;
        this.transform.tag = "Interactable";
        this.transform.GetComponent<Rigidbody>().mass = 1.8f;
        m_teleportCounter = 0;
        StartCoroutine(SetTeleporting());
    }

    IEnumerator SetTeleporting()
    {
        yield return new WaitForSeconds(m_timeOfVulnerability);
        IsTeleporting = true;
        IsTired = false;
        m_sphereCollider.enabled = true;
        this.transform.GetComponent<Rigidbody>().mass = 100.0f;
    }

    void SetRotation()
    {
        if(m_teleportSpotIndex == 1)
            this.transform.rotation = Quaternion.Euler(0, 90, 0);
        else
            this.transform.rotation = Quaternion.Euler(0, -90, 0);
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

    public bool IsTired
    {
        get { return m_isVulnerable; }
        set
        {
            m_isVulnerable = value;
            this.GetComponent<Animator>().SetBool("Tired", m_isVulnerable);
        }
    }

    public bool IsTeleporting
    {
        get { return m_isTeleporting; }
        set
        {
            m_isTeleporting = value;
            this.GetComponent<Animator>().SetBool("Teleporting", m_isTeleporting);
        }
    }

    IEnumerator DestroyParticles(ParticleSystem particles)
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(particles);
    }
}
