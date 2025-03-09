using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    private AudioSource sfxSource;
    
    public List<AudioClip> sfxClips = new();

    public void Initialize()
    {
        DontDestroyOnLoad(gameObject);
        sfxSource = GetComponent<AudioSource>();
        Debug.Log("AudioManager initialized");
    }

    public void PlaySFX(int id)
    {
        if (id >= sfxClips.Count) return;
        AudioClip clip = sfxClips[id];
        if (clip)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
