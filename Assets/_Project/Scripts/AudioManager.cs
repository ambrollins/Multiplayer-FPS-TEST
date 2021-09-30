using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance = null;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip shootClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayWeaponShotAudio()
    {
        _audioSource.clip = shootClip;
        _audioSource.Play();
    }
}
