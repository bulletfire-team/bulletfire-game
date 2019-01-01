﻿using UnityEngine;

public class EffectAudio : MonoBehaviour
{

    private AudioSettings settings;

    private void Start()
    {
        settings = GameObject.Find("AudioManager").GetComponent<AudioSettings>();
        Actualize();
    }

    public void Actualize()
    {
        float volume = settings.settings.effectsVolume / 100f;
        foreach (AudioSource audio in gameObject.GetComponents<AudioSource>())
        {
            audio.volume = volume;
        }
    }
}
