using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;



public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    [SerializeField] AudioMixer m_audioMixer;

    private AudioSource source;


    // Start is called before the first frame update
    private void OnEnable()
    {
        // foreach (Sound s in sounds)
        // {
        //     s.source = gameObject.AddComponent<AudioSource>();
        //     s.source.clip = s.clip;

        //     s.source.volume = s.volume;
        //     s.source.pitch = s.pitch;
        //     s.source.loop = s.loop;
        // }

        // if(SceneManager.GetActiveScene().buildIndex == 0)
        // Play("Life");

    PlayHere("atmos", this.gameObject, true);
    PlayHere("eerie", this.gameObject, true);



    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
    }


    public void PlayHere(string name, GameObject targetObj, bool bgm = false)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        source = targetObj.GetComponent<AudioSource>();
        if (source == null)
            source = targetObj.AddComponent<AudioSource>();
        source.clip = s.clip;

        source.volume = s.volume;
        source.pitch = s.pitch;
        source.loop = s.loop;
        source.outputAudioMixerGroup = m_audioMixer.FindMatchingGroups("Master")[0];

        if (!bgm)
        {
            source.spatialBlend = 1;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.maxDistance = 100f;
        }

        source.Play();
    }
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Stop();
    }

}
