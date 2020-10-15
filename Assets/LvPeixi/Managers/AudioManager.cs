using System.Collections.Generic;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine;
using UniRx;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class SoundInfo
    {
        public AudioClip clip;
        public AudioMixerGroup mixerGroup;
        [Range(0,1)]
        public float volume = 0.5f;
        public bool playerOnAwake = false;
        public bool loop = false;
    }
    [SerializeField]
    private List<SoundInfo> sounds;
    private static Dictionary<string, AudioSource> audioSourceDic = new Dictionary<string, AudioSource>();
    private void Awake()
    {
        InitAM();
    }
    void InitAM()
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            GameObject _obj = new GameObject(sounds[i].clip.name);
            _obj.transform.SetParent(transform);
            AudioSource audio = _obj.AddComponent<AudioSource>();

            audio.clip = sounds[i].clip;
            audio.outputAudioMixerGroup = sounds[i].mixerGroup;
            audio.volume = sounds[i].volume;
            audio.playOnAwake = sounds[i].playerOnAwake;
            audio.loop = sounds[i].loop;

            audioSourceDic.Add(sounds[i].clip.name, audio);
        }
    }

    public static void PlayerAudio(string audioClipName)
    {
        bool containsKey = audioSourceDic.ContainsKey(audioClipName);
        if (!containsKey)
        {
            Debug.LogError("Failed to find the AudioClip name" + audioClipName + " check if the name is correct");
            return;
        }

        AudioSource _audio = audioSourceDic[audioClipName];
        if (_audio.isPlaying)
        {
            _audio.Stop();
            _audio.Play();
        }
        else
        {
            _audio.Play();
        }
    }

    public static void PauseAudio(string audioClipName)
    {
        bool containsKey = audioSourceDic.ContainsKey(audioClipName);
        if (!containsKey)
        {
            Debug.LogError("Failed to find the AudioClip name" + audioClipName + " check if the name is correct");
            return;
        }
        AudioSource _audio = audioSourceDic[audioClipName];
        _audio.Stop();
    }
}
