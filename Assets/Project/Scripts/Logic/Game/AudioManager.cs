using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    private void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip[0];
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = s.spatialBlend;
        }
    }

    private Sound GetSound (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound : " + name + " not found !");
        }
        return s;
    }

    public void Play (string name, int clip = 1)
    {
        Sound s = GetSound(name);
        if (s == null) return;
        s.source.clip = s.clip[clip];
        s.source.Play();
    }

    public void Stop (string name)
    {
        Sound s = GetSound(name);
        if (s == null) return;
        s.source.Stop();
    }

    public void Pause (string name)
    {
        Sound s = GetSound(name);
        if (s == null) return;
        s.source.Pause();
    }

    public void Reinit ()
    {
        foreach (Sound s in sounds)
        {
            s.source.Stop();
        }
    }
}
