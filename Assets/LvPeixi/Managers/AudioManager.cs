using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
