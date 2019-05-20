﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class Health : MonoBehaviour
{
    private int health;

    [Range(100, 1000)]
    public int StartingHealth = 500;
    [Range(0.1f, 5f)]
    public float DeathDelay = 0.2f;
    
    public UnityEvent DeathEvent;

    public TextMeshProUGUI HealthText;
    public Slider HealthSlider;

    // Start is called before the first frame update
    void Start()
    {
        health = StartingHealth;
    }

    public void RemoveHealth(int ammount)
    {
        health = health - ammount;

        if (health <= 0)
        {
            DeathEvent.Invoke();
            Destroy(gameObject, DeathDelay);
        }
    }

    public void AddHealth(int ammount)
    {
        health += ammount;

        health = Mathf.Clamp(health, 1, StartingHealth);
    }

    public void ResetHealth()
    {
        health = StartingHealth;
    }

    public int GetHealth()
    {
        return health;
    }

    void Update()
    {
        float healthRatio = health / StartingHealth;

        HealthText.text = (healthRatio*100) + "%";

        HealthSlider.value = healthRatio;
    }
}
