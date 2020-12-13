using System.Collections.Generic;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine;
using UniRx;

public class AudioManager : MonoBehaviour
{
    private  Dictionary<string, AudioInfo> audioSourceDic = new Dictionary<string, AudioInfo>();
    private  Queue<AudioSource> audioSourceQueue = new Queue<AudioSource>();
    private Queue<AudioSource> audioPlayingQueue = new Queue<AudioSource>();
    private static AudioManager instance;
    public static AudioManager Singleton
    {
        get => instance;
        set
        {
            if (instance == null)
            {
                instance = value;
            }
        }
    }
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        InitAM();
    }
    void InitAM()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject _obj = new GameObject("AudioSourceTemplate" + i);
            _obj.transform.SetParent(transform);
            AudioSource audio = _obj.AddComponent<AudioSource>();
            audioSourceQueue.Enqueue(audio);
        }
    }

    public void PlayAudio(string audioClipName)
    {
        var _config = GameConfig.Singleton.SoundConfig;
        bool containsKey = _config.ContainsKey(audioClipName);
        if (!containsKey)
        {
            Debug.LogError("Failed to find the AudioClip named " + audioClipName + " check if the name is correct");
            return;
        }
        AudioInfo _info = _config[audioClipName];

        if (audioSourceQueue.Count > 0)
        {
            AudioSource _audioSource = audioSourceQueue.Dequeue();

            //-----set audio source-----
            _audioSource.clip = _info.clip;
            _audioSource.volume = _info.volume;
            _audioSource.Play();
            audioPlayingQueue.Enqueue(_audioSource);
            //-----enquene the audio source after finishing the play
            Observable.Timer(System.TimeSpan.FromSeconds(_info.clip.length))
                .First()
                .Subscribe(x =>
                {
                    _audioSource = audioPlayingQueue.Dequeue();
                    audioSourceQueue.Enqueue(_audioSource);
                });
        }
        else//if the audio source template is not enough then create a new one
        {
            GameObject _obj = new GameObject("AudioSourceTemplate" + audioSourceQueue.Count);
            _obj.transform.SetParent(transform);
            AudioSource audio = _obj.AddComponent<AudioSource>();
            audioSourceQueue.Enqueue(audio);
        }
    }
    
    public void PauseAudio(string clipName)
    {
        foreach (AudioSource item in audioPlayingQueue)
        {
            if (item.clip.name == clipName)
            {
                item.Stop();
            }   
        }
    }
}
/// <summary>
/// 音频配置储存格式
/// </summary>
public struct AudioInfo
{
    public AudioClip clip;
    public string mixerGroup;
    public float volume;
    public bool playerOnAwake;
    public bool loop;
}
