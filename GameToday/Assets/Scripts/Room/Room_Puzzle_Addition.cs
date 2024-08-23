using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Room_Puzzle_Addition : MonoBehaviour
{
    public Room_Intro room;
    public Light2D roomLight;
    public AudioClip puzzleSong;

    [Header("Variables")]
    public float songActiveIntervalMin;
    public float songActiveIntervalMax;
    public float songInactiveIntervalMin;
    public float songInactiveIntervalMax;

    public float flickerDuration = 0.5f; // The duration of the flicker before the light switches
    public float flickerSpeed = 0.1f; // The speed of the flicker
    private bool isFlickering = false; // To track if the flicker is currently happening

    public Item_Container itemToDisplay;
    public Item_Container[] items;

    private AudioSource puzzleAudioSource;
    private bool puzzleStarted = false;
    private float currTime;
    private bool musicStopped = false;

    public void Awake()
    {
        room = GetComponent<Room_Intro>();
        puzzleAudioSource = GetComponent<AudioSource>();
        puzzleAudioSource.clip = puzzleSong;

        foreach (var item in items)
        {
            item.room = this;
        }
    }

    void Update()
    {
        if (room.dialogCompleted && !puzzleStarted)
        {
            StartPuzzle();
        }

        if (musicStopped)
        {
            if (StateManager_Player.instance.isMoving && !StateManager_Player.instance.isDead)
            {
                StateManager_Player.instance.player.Dead();

            }
        }

        KeepTrackOfMusic();
    }
    public void StartPuzzle()
    {
        puzzleStarted = true;
        currTime = Random.Range(songActiveIntervalMin, songActiveIntervalMax);
        puzzleAudioSource.Play();
        roomLight.enabled = false;
        musicStopped = false;
    }

    private void KeepTrackOfMusic()
    {
        if (!puzzleStarted) return;

        // Countdown the timer
        currTime -= Time.deltaTime;

        if (!isFlickering && currTime <= flickerDuration)
        {
            StartCoroutine(FlickerLight());
        }

        if (currTime <= 0f)
        {
            StopCoroutine(FlickerLight());
            isFlickering = false;

            if (musicStopped)
            {
                // Green Light
                currTime = Random.Range(songActiveIntervalMin, songActiveIntervalMax);
                puzzleAudioSource.Play();
                roomLight.enabled = false;
                musicStopped = false;
            }
            else
            {
                // Red Light
                currTime = Random.Range(songInactiveIntervalMin, songInactiveIntervalMax);
                puzzleAudioSource.Stop();
                roomLight.enabled = true;
                musicStopped = true;
            }
        }
    }
    private IEnumerator FlickerLight()
    {
        isFlickering = true;
        
        float endTime = currTime;
        while (endTime > 0)
        {
            Debug.Log("Flickering");
            roomLight.enabled = !roomLight.enabled; // Toggle the light on and off
            endTime -= flickerSpeed;
            yield return new WaitForSeconds(flickerSpeed);
        }

        isFlickering = false;
    }

    public void CheckForItemsPickedUp()
    {
        foreach (var item in items)
        {
            if (!item.isPickedUp)
            {
                return; 
            }
        }

        RoomCompleted();

    }
    private void RoomCompleted()
    {
        puzzleAudioSource.Stop();
        roomLight.enabled = true; // Keep the light on
        puzzleStarted = false;
        ItemDisplay_Manager.instance.ShowItem(itemToDisplay.itemSO);

        // TODO: Implement door opening logic
    }

}
