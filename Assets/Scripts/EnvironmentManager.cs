using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    Animator EnvironmentAnimator;
    AnimationClip Day2Night;
    AnimationClip Sunny2Rain;

    float DayLength = 120;

    // Start is called before the first frame update
    void Start()
    {
        EnvironmentAnimator = GetComponent<Animator>();
        AnimationClip[] clips = EnvironmentAnimator.runtimeAnimatorController.animationClips;
        //Debug.Log("current environment clips count: " + clips.Length);
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "ENV_Morning2Night_Anim":
                    Day2Night = clip;
                    Debug.Log("Day2Night length: " + clip.length);
                    break;
                case "ENV_Sunny2Rain2Storm_Anim":
                    Debug.Log("Sunny2Rain length: " + clip.length);
                    Sunny2Rain = clip;
                    break;
            }
        }
        //get dayLength;

        //set Day2Night length = daylength
        float day2NightSpeed = Day2Night.length / DayLength;
        //Debug.Log("day2NightSpeed: " + day2NightSpeed);
        EnvironmentAnimator.SetFloat("DayLength", day2NightSpeed);

        //No onDayStart event in the first day
        EnvironmentAnimator.SetTrigger("DayStart");

        GameEvents.Sigton.onDayStart += () =>
        {
            Debug.Log("Day Start......");
            EnvironmentAnimator.SetTrigger("DayStart");
        };

        GameEvents.Sigton.onDayEnd += () =>
        {
            Debug.Log("Day End......");
            EnvironmentAnimator.SetTrigger("DayEnd");
        };

        GameEvents.Sigton.OnRainStart += () =>
        {
            Debug.Log("Rain Start......");
            EnvironmentAnimator.SetBool("isRain", true);
        };

        GameEvents.Sigton.OnRainEnd += () =>
        {
            Debug.Log("Rain End......");
            EnvironmentAnimator.SetBool("isRain", false);
        };

        GameEvents.Sigton.OnStormStart += () =>
        {
            Debug.Log("Storm Start......");
            EnvironmentAnimator.SetBool("isStorm", true);
        };

        GameEvents.Sigton.OnStormEnd += () =>
        {
            Debug.Log("Storm End......");
            EnvironmentAnimator.SetBool("isStorm", false);
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
