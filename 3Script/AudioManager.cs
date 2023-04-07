using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    private AudioSource bgmSource;

    [SerializeField]
    private string[] sceneNames;
    [SerializeField]
    private AudioClip[] bgm_Clips;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {

        for (int i = 0; i < sceneNames.Length; i++)
        {
            if(sceneNames[i] == arg0.name)
            {
                if(bgm_Clips[i] != null)
                    BGMPlay(bgm_Clips[i]);
                return;
            }
        }

    }

    private void BGMPlay(AudioClip _clip)
    {
        bgmSource.clip = _clip;
        bgmSource.loop = true;
        bgmSource.Play();

    }

    public void EffectPlay(AudioClip _clip,float _volume = 1)
    {
        GameObject _go = new GameObject();
        AudioSource _audio = _go.AddComponent<AudioSource>();
        _audio.volume = _volume;
        _audio.clip = _clip;
        _audio.Play();

        Destroy(_go, _clip.length);
    }

}
