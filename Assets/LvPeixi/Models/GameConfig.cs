using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Audio;

public class GameConfig 
{
    private static GameConfig config;
    private Dictionary<string, float> interactionConfig = new Dictionary<string, float>();
    private Dictionary<string, float> playerConfig = new Dictionary<string, float>();
    private Dictionary<string, AudioInfo> soundConfig = new Dictionary<string, AudioInfo>();
    private Dictionary<int, string> diaryData = new Dictionary<int, string>();

    public Dictionary<string,float> InteractionConfig { get => interactionConfig; }
    public Dictionary<string, float> PlayerConfig => playerConfig;
    public Dictionary<string, AudioInfo> SoundConfig => soundConfig;

    public static GameConfig Singleton
    {
        set
        {
            if (config == null)
            {
                config = value;
            }
        }
        get => config;
    }
    public GameConfig()
    {
        JsonToFloat(ref interactionConfig, "InteractConfig.json");
        JsonToFloat(ref playerConfig, "PlayerConfig.json");

        JsonToSoundInfo();
    }

    /// <summary>
    /// 将json data转化为float dic储存起来
    /// </summary>
    /// <param name="_floatDic"></param>
    /// <param name="jsonFileName"></param>
    void JsonToFloat(ref Dictionary<string, float> _floatDic,string jsonFileName)
    {
        string configJsonData = File.ReadAllText(Application.dataPath + "/StreamingAssets/" + jsonFileName);
        if (configJsonData == null)
        {
            throw new System.Exception("failed to find the json file" + jsonFileName);
        }
        JsonData _jsonData = JsonMapper.ToObject(configJsonData);
        foreach (JsonData v in _jsonData)
        {
            if (v != null)
            {
                var _temp = (float)(double)v["Value"];
                _floatDic.Add(v["Key"].ToString(), _temp);
            }
        }
    }

    void JsonToSoundInfo()
    {
        string configJsonData = File.ReadAllText(Application.dataPath + "/StreamingAssets/AudioConfig.json");

        JsonData _jsonData = JsonMapper.ToObject(configJsonData);
        foreach (JsonData item in _jsonData)
        {
            //Debug.Log(item["ClipName"].ToString());
            //Debug.Log(item["AudioMixerGroup"].ToString());
            //Debug.Log((double)item["Volume"]);
            //Debug.Log((bool)item["PlayerOnAwake"]);
            //Debug.Log((bool)item["Loop"]);

            AudioInfo _audioInfo = new AudioInfo();
    
            AudioClip _audioClip = Resources.Load<AudioClip>("Audios/" + item["ClipName"].ToString());
            _audioInfo.clip = _audioClip;
            _audioInfo.mixerGroup = item["AudioMixerGroup"].ToString();
            _audioInfo.volume = (float)(double)item["Volume"];
            _audioInfo.playerOnAwake = (bool)item["PlayerOnAwake"];
            _audioInfo.loop = (bool)item["Loop"];

            soundConfig.Add(item["ClipName"].ToString(), _audioInfo);
        }
    }

    void JsonToDiaryData()
    {

    }
}
