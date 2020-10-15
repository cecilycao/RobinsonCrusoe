using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

public class GameConfig 
{
    private static GameConfig config;
    private Dictionary<string, float> interactionConfig = new Dictionary<string, float>();
    private Dictionary<string, float> playerConfig = new Dictionary<string, float>();
    public Dictionary<string,float> InteractionConfig { get => interactionConfig; }
    public Dictionary<string, float> PlayerConfig => playerConfig;
    int t = 0;
   
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
    }

    /// <summary>
    /// 将json data转化为float dic储存起来
    /// </summary>
    /// <param name="_floatDic"></param>
    /// <param name="jsonFileName"></param>
    void JsonToFloat(ref Dictionary<string, float> _floatDic,string jsonFileName)
    {
        string configJsonData = File.ReadAllText(Application.dataPath + "/GameConfig/"+ jsonFileName);
        if (configJsonData == null)
        {
            throw new System.Exception("failed to find the json file" + jsonFileName);
        }
        JsonData _jsonData = JsonMapper.ToObject(configJsonData);
        foreach (JsonData v in _jsonData)
        {
            var _temp = (float)(double)v["Value"];
            _floatDic.Add(v["Key"].ToString(), _temp);
        }
    }
}
