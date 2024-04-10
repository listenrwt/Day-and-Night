using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    public AudioMixerGroup audioMixerGroup;
    public Sound[] sounds;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.outputAudioMixerGroup = audioMixerGroup;
        }

    }

    private void Update()
    {
        //easter egg
        if (Input.GetKeyDown(KeyCode.B))
            PlayAudio("bruh");
        //

        if (SceneManager.GetActiveScene().name != "MainMenu")
            return;

        PlayCheckAudio("BGM");
      
    }

    public void PlayAudio(string name)
    {
        foreach (Sound sound in sounds)
        {
            if(name == sound.soundName)
            {
                sound.source.Play();
                return;
            }
        }
        Debug.LogWarning(name + " is not exist in AudioSource");
    }

    public void PlayCheckAudio(string name)
    {
        foreach (Sound sound in sounds)
        {
            if (name == sound.soundName)
            {
                if(!sound.source.isPlaying)
                sound.source.Play();
                return;
            }
        }
        Debug.LogWarning(name + " is not exist in AudioSource");
    }

    public void PlayAudio(string name, float fadeInTime)
    {
        foreach (Sound sound in sounds)
        {
            if (name == sound.soundName)
            {
                StartCoroutine(fadeIn(sound, fadeInTime));
                return;
            }
        }
        Debug.LogWarning(name + " is not exist in AudioSource");
    }

    public void StopAudio(string name)
    {
        foreach (Sound sound in sounds)
        {
            if (name == sound.soundName)
            {
                sound.source.Stop();
                return;
            }
        }
        Debug.LogWarning(name + " is not exist in AudioSource");
    }

    public void StopAudio(string name, float fadeOutTime)
    {
        foreach (Sound sound in sounds)
        {
            if (name == sound.soundName)
            {
                StartCoroutine(fadeOut(sound, fadeOutTime));
                return;
            }
        }
        Debug.LogWarning(name + " is not exist in AudioSource");
    }

    IEnumerator fadeOut(Sound sound, float fadeOutTime)
    {
        float timer = fadeOutTime;
        while(timer > 0f)
        {
            sound.source.volume -= Time.deltaTime * sound.volume;
            timer -= Time.deltaTime;
            yield return new WaitForSeconds(0);
        }

        sound.source.Stop();
        sound.source.volume = sound.volume;
    }

    IEnumerator fadeIn(Sound sound, float fadeInTime)
    {
        sound.source.volume = 0f;
        sound.source.Play();

        float timer = 0f;
        while (timer <= fadeInTime)
        {
            sound.source.volume += Time.deltaTime * sound.volume;
            timer += Time.deltaTime;
            yield return new WaitForSeconds(0);
        }

        sound.source.volume = sound.volume;
    }
}
