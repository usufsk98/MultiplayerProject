using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] bool isSoundMuted = false;

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource buttonSource;
    
    
    [SerializeField] AudioSource soundEffectOneShotSource;
    [SerializeField] AudioSource soundEffectOneShotSource1;

    [SerializeField] List<AudioClip> musicClips;
    [SerializeField] List<AudioClip> buttonClips;

    [SerializeField] List<AudioClip> soundEffectClips;

    //[Header("Audio Clips")]
   

 
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void PlayMusic(int index)
    {
        if (isSoundMuted) return;
        if (musicSource.isPlaying)
            musicSource.Stop();

        musicSource.clip = musicClips[index];
        musicSource.Play();
    }


    public void PlaySoundEffectOneShot(AudioClip clip)
    {
        if (isSoundMuted) return;
        if (clip == null)
        {
            Debug.LogError("No clip found, please check variable and pass clip accordingly.");
            return;
        }

        soundEffectOneShotSource.PlayOneShot(clip);
    }

    public void PlaySoundEffectOneShot1(AudioClip clip)
    {
        if (isSoundMuted) return;
        if (clip == null)
        {
            Debug.LogError("No clip found, please check variable and pass clip accordingly.");
            return;
        }

        soundEffectOneShotSource1.PlayOneShot(clip);
    }   

    public void StopOneShotSource()
    {
        soundEffectOneShotSource.Stop();
    }

    public void PlayButtonSound(int index)
    {
        if (isSoundMuted) return;
        if (buttonSource.isPlaying)
            buttonSource.Stop();

        buttonSource.clip = buttonClips[index];
        buttonSource.Play();
    }
}


