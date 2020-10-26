using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSystem : MonoBehaviour
{
    static WeatherSystem weatherSystem;
    public WeatherState m_state;
    public GameObject rain;
    public GameObject storm;

    public ParticleSystem[] rainParticles;
    public ParticleSystem[] stormParticles;


    public static WeatherSystem Sigton
    {
        get => weatherSystem;
        set
        {
            if (weatherSystem == null)
            {
                weatherSystem = value;
            }
        }
    }

    private void Awake()
    {
        weatherSystem = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rainParticles = rain.GetComponentsInChildren<ParticleSystem>();
        stormParticles = storm.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem rain in rainParticles)
        {
            rain.Stop();
        }
        foreach (ParticleSystem storm in stormParticles)
        {
            storm.Stop();
        }
        AudioManager.Singleton.PlayAudio("GameEvent_sunnyDay");
        GameEvents.Sigton.OnRainStart += () =>
        {
            if (m_state != WeatherState.RAIN)
            {
                AudioManager.Singleton.PauseAudio("GameEvent_stormComing");
                AudioManager.Singleton.PauseAudio("GameEvent_sunnyDay");
                AudioManager.Singleton.PlayAudio("GameEvent_rainDay");
                m_state = WeatherState.RAIN;
                startRain();
            }
        };

        GameEvents.Sigton.OnRainEnd += () =>
        {
            if (m_state != WeatherState.SUNNY)
            {
                AudioManager.Singleton.PauseAudio("GameEvent_rainDay");
                AudioManager.Singleton.PauseAudio("GameEvent_stormComing");
                AudioManager.Singleton.PlayAudio("GameEvent_sunnyDay");
                m_state = WeatherState.SUNNY;
                endRain();
            }
        };

        GameEvents.Sigton.OnStormStart += () =>
        {
            if (m_state != WeatherState.STORM)
            {
                AudioManager.Singleton.PauseAudio("GameEvent_sunnyDay");
                AudioManager.Singleton.PauseAudio("GameEvent_rainDay");
                AudioManager.Singleton.PlayAudio("GameEvent_stormComing");
                m_state = WeatherState.STORM;
                startStorm();
            }
        };

        GameEvents.Sigton.OnStormEnd += () =>
        {
            if (m_state != WeatherState.SUNNY)
            {
                AudioManager.Singleton.PauseAudio("GameEvent_rainDay");
                AudioManager.Singleton.PauseAudio("GameEvent_stormComing");
                AudioManager.Singleton.PlayAudio("GameEvent_sunnyDay");
                m_state = WeatherState.SUNNY;
                endStorm();
            }
        };

    }

    //temp
    public void invokeRain()
    {
        
        GameEvents.Sigton.OnStormEnd.Invoke();
        GameEvents.Sigton.OnRainStart.Invoke();
    }

    public void invokeStorm()
    {
        
        GameEvents.Sigton.OnRainEnd.Invoke();
        GameEvents.Sigton.OnStormStart.Invoke();
    }

    public void invokeSunny()
    {
        
        GameEvents.Sigton.OnRainEnd.Invoke();
        GameEvents.Sigton.OnStormEnd.Invoke();
    }

    void startRain()
    {
        
        foreach (ParticleSystem rain in rainParticles)
        {
            rain.Play();
        }
        
    }

    void startStorm()
    {
        foreach (ParticleSystem storm in stormParticles)
        {
            storm.Play();
        }
    }

    void endRain()
    {
        foreach (ParticleSystem rain in rainParticles)
        {
            rain.Stop();
        }
    }

    void endStorm()
    {
        foreach (ParticleSystem storm in stormParticles)
        {
            storm.Stop();
        }
    }
}

public enum WeatherState
{
    SUNNY,
    RAIN,
    STORM
}
