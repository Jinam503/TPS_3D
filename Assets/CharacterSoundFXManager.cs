using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundFXManager : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayFireSoundFX()
    {
        audioSource.PlayOneShot(WorldSoundFXManager.instance.fireSFX);
    }
}
