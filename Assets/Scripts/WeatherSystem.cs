﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSystem : MonoBehaviour
{
    static WeatherSystem weatherSystem;
    public weatherState m_state;
    public ParticleSystem rain;
    public ParticleSystem heavyRain;
    public ParticleSystem storm;


    public enum weatherState
    {
        SUNNY,
        RAIN,
        STORM
    }

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
        rain.Stop();
        heavyRain.Stop();

        GameEvents.Sigton.OnRainStart += () =>
        {
            if (m_state != weatherState.RAIN)
            {
                m_state = weatherState.RAIN;
                startRain();
            }
        };

        GameEvents.Sigton.OnRainEnd += () =>
        {
            m_state = weatherState.SUNNY;
            endRain();
        };

        GameEvents.Sigton.OnStormStart += () =>
        {
            if (m_state != weatherState.STORM)
            {
                m_state = weatherState.STORM;
                startStorm();
            }
        };

        GameEvents.Sigton.OnStormEnd += () =>
        {
            m_state = weatherState.SUNNY;
            endStorm();
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
        rain.Play();
    }

    void startStorm()
    {
        heavyRain.Play();
    }

    void endRain()
    {
        rain.Stop();
    }

    void endStorm()
    {
        heavyRain.Stop();
    }
}
