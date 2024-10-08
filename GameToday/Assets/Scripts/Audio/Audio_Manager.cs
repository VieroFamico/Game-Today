using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Audio_Manager : MonoBehaviour
{
    public static Audio_Manager instance;

    [Header("Audio Settings")]
    public AudioMixerGroup defaultMixerGroup;

    public AudioClip openingBackgroundSong;

    private AudioSource audioSource;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

    }
    void Start()
    {
        SetAllAudioSourcesToMixer();
        audioSource = GetComponent<AudioSource>();
        PlaySong(openingBackgroundSong);
    }

    public void SetAllAudioSourcesToMixer()
    {
        // Find all AudioSources in the scene
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.outputAudioMixerGroup = defaultMixerGroup;
        }
    }

    // Optional: Function to manually update AudioSources
    public void UpdateAudioSources()
    {
        SetAllAudioSourcesToMixer();
    }

    public void PlaySong(AudioClip songToPlay)
    {
        audioSource.Stop();

        if(songToPlay)
        {
            audioSource.clip = songToPlay;
            audioSource.Play();
        }
    }

    public void PlayOpeningSong()
    {
        PlaySong(openingBackgroundSong);
    }
}
