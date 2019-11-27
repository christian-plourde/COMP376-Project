﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BossUI : MonoBehaviour
{
    public Image health; 
    EnemyHealth bossRef;
    // Start is called before the first frame update
    void Start()
    {
        bossRef = GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        float HP = (float) bossRef.GetCurrentHealth() / (float) bossRef.m_startingHealth;


        health.fillAmount = HP;
    }
}
