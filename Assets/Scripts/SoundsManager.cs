using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class Sounds
{
    [SerializeField]
    ESoundName _soundName;
    [SerializeField]
    AudioClip _soundAudioClip;

    public ESoundName SoundName => _soundName;
    public AudioClip SoundAudioClip => _soundAudioClip;
}

[RequireComponent(typeof(AudioSource))]
public class SoundsManager :MonoBehaviour
{
    [SerializeField]
    Sounds[] sfxSounds, musicSounds;

    [SerializeField] AudioSource _sfxSource, _musicSource;

    public static SoundsManager Instance;

    public AudioSource SFXSource => _sfxSource;

    public AudioSource MusicSource => _musicSource;

    private void Awake()
    {
        if (Instance is null)
            Instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySfx(ESoundName sfxName)
    {
        Sounds s = Array.Find(sfxSounds, x => x.SoundName == sfxName);
        if (s == null)
            Debug.Log(sfxName + " Not Found");
        else
        {
            _sfxSource.clip = s.SoundAudioClip;
            _sfxSource.PlayOneShot(_sfxSource.clip);
        }
    }

    public void PlayMusic(ESoundName musicName)
    {
        Sounds s = Array.Find(musicSounds, x => x.SoundName == musicName);
        if (s == null)
            Debug.Log(musicName + " Not Found");
        else
        {
            _musicSource.clip = s.SoundAudioClip;
            _musicSource.Play();
        }
    }
}

public enum ESoundName
{
    BubblePop,
    PipeDrain,
    StartWhistle,
    Win,
    Lose,
}