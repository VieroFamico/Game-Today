using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pillar_Entity : Entities
{
    [Header("References")]
    public Slider currFillPercentage;
    public Slider activationPercentage;
    public Base_Room room;


    public float maxFillPercentage = 25f;
    public float fillRate = 1f;
    public float activateDistance;
    public float activateTimeNeeded;
    public bool isActive;
    public bool isCharged { get; private set;}

    private Player_Entity player;

    private float currActivatePercentage; 
    public override void Start()
    {
        currentHP = maxHP;
        player = FindAnyObjectByType<Player_Entity>();
        UpdateSlidersVisibility();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            Charge();
        }
        else
        {
            GetPlayerActivatingInput();
        }

        UpdateSliders();
    }
    private void Charge()
    {
        currActivatePercentage += Time.deltaTime * fillRate;

        if(currActivatePercentage >= maxFillPercentage)
        {
            isCharged = true;
            room.CheckPillarCharged();
            Deactivate(); 
        }
    }
    public override void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            currentHP = 0;

            Deactivate();
            return;
        }
        else if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    private void Deactivate()
    {
        if(isActive) isActive = false;
    }


    private void GetPlayerActivatingInput()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < activateDistance && !isCharged)
        {
            if (Input.GetKey(KeyCode.E))
            {
                Activating();
                
            }
        }
    }
    public void Activating()
    {
        currActivatePercentage += Time.deltaTime;

        if (currActivatePercentage >= activateTimeNeeded)
        {
            isActive = true;
            currActivatePercentage = 0;
            UpdateSlidersVisibility();
            currentHP = maxHP;
            UpdateHealthSlider();
        }
    }

    private void UpdateSliders()
    {
        if (isActive)
        {
            currFillPercentage.value = currActivatePercentage / maxFillPercentage;
        }
        else
        {
            activationPercentage.value = currActivatePercentage / activateTimeNeeded;
        }
    }

    private void UpdateSlidersVisibility()
    {
        currFillPercentage.gameObject.SetActive(isActive);
        activationPercentage.gameObject.SetActive(!isActive);
    }

}
