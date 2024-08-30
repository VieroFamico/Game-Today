using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class PlayerState_Manager : MonoBehaviour
{
    public static PlayerState_Manager instance;

    public Player_Entity player;
    public enum PlayerState
    {
        Moving,
        Dashing,
        Attacking,
    }

    public PlayerState state;

    [Header("Moving")]
    public bool isMoving;
    public bool isAbleToMove = true;

    [Header("Dashing")]
    public bool isDashing;
    public bool isAbleToDash = true;

    [Header("Attacking")]
    public bool isAttacking;
    public bool isAbleToAttack = true;

    [Header("Harmony State")]
    public bool inHarmonyState;
    public bool isAbleToHarmonyState = false;
    public float maxHarmony = 100;
    public float currHarmonyPercentage = 0f;
    public float harmonyIncreaseRate = 1f;
    public float harmonyDecreaseRate = 1f;
    public float harmonyChangeDelay = 0.5f; // Delay before decreasing Harmony

    [Header("Harmony Bar")]
    public Slider harmonySlider;
    public Image fillImage;

    [Header("Post Processing")]
    public TrailRenderer trailEffect;
    public Volume globalVolume;  // Reference to the Global Volume in your scene
    private FilmGrain filmGrain;
    private ChannelMixer channelMixer;
    private ChromaticAberration chromaticAberration;

    private Coroutine decreaseHarmonyCoroutine;
    private bool canChangeState = true;

    public bool isHavingUIDisplayed = false;

    public bool isDead = false;

    public int currentRoom;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        
    }
    private void Start()
    {
        DisablePlayer();

        harmonySlider.maxValue = maxHarmony;
        harmonySlider.value = currHarmonyPercentage;
        harmonySlider.gameObject.SetActive(false);

        trailEffect.enabled = false;
        globalVolume.profile.TryGet<FilmGrain>(out filmGrain);
        globalVolume.profile.TryGet<ChromaticAberration>(out chromaticAberration);
        globalVolume.profile.TryGet(out channelMixer);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canChangeState)
        {
            StartCoroutine(ChangeStateWithEffects());
        }

        if (inHarmonyState)
        {
            IncreaseHarmony();
        }
        else
        {
            if (decreaseHarmonyCoroutine == null)
            {
                decreaseHarmonyCoroutine = StartCoroutine(DecreaseHarmonyAfterDelay());
            }
        }


        UpdatePostProcessingEffects();
        UpdateSliderColor();

    }

    public void InitializePlayer()
    {
        isAbleToMove = true;
        isAbleToDash = true;
        isAbleToAttack = true;
        isAbleToHarmonyState = true;
        isDead = false;
    }

    public void DisablePlayer()
    {
        isAbleToMove = false;
        isAbleToDash = false;
        isAbleToAttack = false;
        isAbleToHarmonyState = false;
        inHarmonyState = false;
    }
    public void EnablePlayer()
    {
        isAbleToMove = true;
        isAbleToDash = true;
        isAbleToAttack = true;
        isAbleToHarmonyState = true;
        inHarmonyState = false;
    }

    #region Harmony/Unstable State
    public void ChangeState()
    {
        if(inHarmonyState)
        {
            inHarmonyState = false;
            trailEffect.enabled = false;
        }
        else
        {
            if(isAbleToHarmonyState)
            {
                inHarmonyState = true;
                trailEffect.enabled = true;
            }
        }
    }

    private void IncreaseHarmony()
    {
        if (currHarmonyPercentage < maxHarmony)
        {
            currHarmonyPercentage += harmonyIncreaseRate * Time.deltaTime;
            harmonySlider.value = currHarmonyPercentage;
            harmonySlider.gameObject.SetActive(true);
        }
        else
        {
            currHarmonyPercentage = maxHarmony;
            harmonySlider.value = currHarmonyPercentage;
            HarmonyStateReachedMax();
        }
    }

    private IEnumerator DecreaseHarmonyAfterDelay()
    {
        yield return new WaitForSeconds(harmonyChangeDelay);

        while (!inHarmonyState && currHarmonyPercentage > 0)
        {
            currHarmonyPercentage -= harmonyDecreaseRate * Time.deltaTime;
            harmonySlider.value = currHarmonyPercentage;

            if(currHarmonyPercentage < 0)
            {
                currHarmonyPercentage = 0;
                harmonySlider.value = currHarmonyPercentage;
                harmonySlider.gameObject.SetActive(false);
            }
            yield return null;
        }

        decreaseHarmonyCoroutine = null;
    }

    private void UpdatePostProcessingEffects()
    {
        // Assuming the intensity ranges from 0 to 1
        float intensity = currHarmonyPercentage / maxHarmony;

        if (filmGrain != null)
        {
            filmGrain.intensity.value = intensity;
        }
        if(channelMixer != null)
        {
            channelMixer.redOutRedIn.value = 100 + currHarmonyPercentage;
        }

        /*if (chromaticAberration != null)
        {
            chromaticAberration.intensity.value = intensity;
        }*/
    }
    private void UpdateSliderColor()
    {
        // Lerp from green to yellow to red based on currHarmonyPercentage
        Color green = Color.green;
        Color yellow = Color.yellow;
        Color red = Color.red;

        if (currHarmonyPercentage <= 50f)
        {
            fillImage.color = Color.Lerp(green, yellow, currHarmonyPercentage / 50f);
        }
        else
        {
            fillImage.color = Color.Lerp(yellow, red, (currHarmonyPercentage - 50f) / 50f);
        }
    }

    private IEnumerator ChangeStateWithEffects()
    {
        if (!isAbleToHarmonyState)
        {
            inHarmonyState = false;
            StopCoroutine(ChangeStateWithEffects());
        }

        canChangeState = false;

        ChangeState();

        // Increase chromatic aberration intensity to signify state change
        float elapsedTime = 0f;
        float startIntensity = inHarmonyState ? 0f : 1f;
        float endIntensity = inHarmonyState ? 1f : 0f;

        while (elapsedTime < 0.5f)
        {
            chromaticAberration.intensity.value = Mathf.Lerp(startIntensity, endIntensity, elapsedTime / 0.5f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        chromaticAberration.intensity.value = endIntensity;

        canChangeState = true;
    }


    private void HarmonyStateReachedMax()
    {
        // Implement what happens when Harmony reaches 100%
        Debug.Log("Harmony state has reached maximum level!");
        player.Dead();
    }

    #endregion

    public void SetMoving_Dashing_Attacking(bool ableToMove, bool ableToDash, bool ableToAttack)
    {
        isAbleToMove = ableToMove;
        isAbleToDash = ableToDash;
        isAbleToAttack = ableToAttack;
    }

}