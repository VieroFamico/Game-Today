using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entities : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHP = 100;
    public int currentHP;

    [Header("UI Elements")]
    public Slider healthSlider;

    public virtual void Start()
    {
        currentHP = maxHP;
        InitializeHealthSlider();
    }

    public virtual void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP < maxHP && healthSlider != null && !healthSlider.gameObject.activeSelf)
        {
            ShowHealthSlider();
        }

        if (currentHP <= 0)
        {
            currentHP = 0;
            Dead();
            return;
        }
        else if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }
        UpdateHealthSlider();
    }

    public virtual void Dead()
    {
        PlayerState_Manager.instance.isDead = true;

        if (healthSlider != null)
        {
            healthSlider.gameObject.SetActive(false);
        }
    }
    public void InitializeHealthSlider()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHP;
            healthSlider.value = currentHP;
            healthSlider.gameObject.SetActive(false); // Initially hide the slider
        }
    }
    public void UpdateHealthSlider()
    {
        if (healthSlider != null)
        {
            if(currentHP == maxHP)
            {
                healthSlider.gameObject.SetActive(false);
            }
            healthSlider.value = currentHP;
        }
    }
    public void ShowHealthSlider()
    {
        if (healthSlider != null)
        {
            healthSlider.gameObject.SetActive(true);
        }
    }

}
