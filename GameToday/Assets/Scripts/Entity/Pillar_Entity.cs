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
    public Combat_Room_Module combatRoom;

    public float maxFillPercentage = 25f;
    public float fillRate = 1f;
    public float activateDistance;
    public float activateTimeNeeded;
    public bool isActive;
    public bool isCharged { get; private set;}

    private Player_Entity player;

    private float currChargePercentage = 0;
    private float currActivatePercentage = 0;
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
        currChargePercentage += Time.deltaTime * fillRate;

        if(currChargePercentage >= maxFillPercentage)
        {
            combatRoom.CheckPillarCharged();
            FullyCharged();
            Debug.Log("FullyCharged");
        }
    }
    public override void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP < maxHP && healthSlider != null && !healthSlider.gameObject.activeSelf)
        {
            ShowHealthSlider();
        }

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

        UpdateHealthSlider();
    }

    public void StartRoom()
    {
        isActive = true;
        UpdateSlidersVisibility();
    }

    private void FullyCharged()
    {
        isCharged = true;
        currentHP = maxHP;
        isActive = false;
    }

    private void Deactivate()
    {
        if(isActive) isActive = false;
        UpdateSlidersVisibility();
    }


    private void GetPlayerActivatingInput()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < activateDistance && !isCharged)
        {
            if (Input.GetKey(KeyCode.E))
            {
                Activating();
            }
            else
            {
                currActivatePercentage = 0f;
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
            currFillPercentage.value = currChargePercentage / maxFillPercentage;
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
