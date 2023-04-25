using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;



public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    [SerializeField] AudioMixer m_audioMixer;

    private AudioSource source;
    [Header("Background Music/Sound")]
    [SerializeField] bool m_playBGM;
    [SerializeField] string m_bgmName;


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
        
        if(m_playBGM)
        {
            PlayHere(m_bgmName, this.gameObject);
        }


    }

    

    public AudioSource PlayHere(string name, GameObject targetObj, bool fromObject = false, bool oneshot = false)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null){
            Debug.Log("Sound not found");
            return null;
        }
            
        
        if (!targetObj.TryGetComponent(out source))
            source = targetObj.AddComponent<AudioSource>();
        source.clip = s.clip;

        source.volume = s.volume;
        source.pitch = s.pitch;
        source.loop = s.loop;
        source.outputAudioMixerGroup = m_audioMixer.FindMatchingGroups("Master")[0];

        if (fromObject)
        {
            source.spatialBlend = 1;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.maxDistance = 100f;
        }
        if(oneshot)
        source.PlayOneShot(s.clip, s.volume);
        else
        source.Play();
        //Debug.Log("" + targetObj.name + " - " + s.name);
        return source;
    }
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Stop();
    }

}
