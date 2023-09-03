using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    private static AudioManager instance;

    [SerializeField]
    private Sound[] sounds;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }


        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.output;

            s.source.ignoreListenerPause = s.ignorePause;

            s.source.playOnAwake = false;
        }
    }

    private void Start()
    {
        play("Theme");
    }

    public void play(string name)
    {
        Sound s = Array.Find(sounds, (sound) => sound.name == name);

        if (s == null)
        {
            Debug.LogError("Sound: " + name + " not found");
            return;
        }
        s.source.Play();
    }
}
