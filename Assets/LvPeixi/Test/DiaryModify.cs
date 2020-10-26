using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class DiaryModify : MonoBehaviour
{
    // Start is called before the first frame updatet
    public Animator anim;
    void Start()
    {
        
        GameEvents.Sigton.MechanismEventDictionary[MechanismEventTags.onDayTimeOut]
          .Subscribe(x =>
          {
              Diary diaty = FindObjectOfType<Diary>();
              if (diaty != null)
              {
                  diaty.canWriteDiary = true;
              }
          });

        GameEvents.Sigton.onDayStart += () =>
        {
            ResourceCollectorSample[] collectors = FindObjectsOfType<ResourceCollectorSample>();
            foreach (ResourceCollectorSample item in collectors)
            {
                int _build = (int)GameConfig.Singleton.InteractionConfig[InteractConfigKeys.posCollect_buildingMat_defaut];
                int _food = (int)GameConfig.Singleton.InteractionConfig[InteractConfigKeys.posCollect_foodMat_defaut];
                item.resourceAccount_foodMaterial = _food;
                item.resourceAccount_buildingMaterial = _build;
            }

            int neg_build = (int)GameConfig.Singleton.InteractionConfig[InteractConfigKeys.negCollect_buildingMat_default];
            int neg_food = (int)GameConfig.Singleton.InteractionConfig[InteractConfigKeys.negCollect_foodMat_default];
            var negativeCollector = FindObjectOfType<NegativeResourceCollector>();
            negativeCollector.resourceAccount_buildingMaterial = neg_build;
            negativeCollector.resourceAccount_foodMaterial = neg_food;
        };

        InitIcons();

        EnableLight();

        BGM();
    }
    void InitIcons()
    {
        var iconManagers = FindObjectOfType<IconManager>();
        var negativeCollector = FindObjectOfType<NegativeResourceCollector>();
       
    }

    void EnableLight()
    {
        GameEvents.Sigton.timeSystem
            .Subscribe(x =>
            {
                var _blend = anim.GetFloat("Blend");
                var _time = x.TimeCountdown;
                var _timeNormalized = 1 - _time / 120.0f;
                _blend = 0.5f - Mathf.Abs(_timeNormalized - 0.5f);
                anim.SetFloat("Blend", _blend);
            });
    }

    void BGM()
    {
        GameEvents.Sigton.timeSystem
            .Where(x => x.DayCount >= 1 && x.DayCount <= 4)
            .Subscribe(x =>
            {
                AudioManager.Singleton.PlayAudio("BGM_01");
            });

        GameEvents.Sigton.timeSystem
            .Where(x => x.DayCount == 3)
            .Subscribe(x =>
            {
                AudioManager.Singleton.PlayAudio("BGM_03");
            });

        GameEvents.Sigton.timeSystem
            .Where(x => x.DayCount == 5)
            .Subscribe(x =>
            {
                AudioManager.Singleton.PlayAudio("BGM_02");
            });

        GameEvents.Sigton.timeSystem
            .Where(x => x.DayCount == 7)
            .Subscribe(x =>
            {
                AudioManager.Singleton.PlayAudio("BGM_04");
            });
        GameEvents.Sigton.timeSystem
            .Where(x => x.DayCount > 7 || x.DayCount <= 12)
            .Subscribe(x =>
            {
                AudioManager.Singleton.PlayAudio("BGM_05");
            });
        GameEvents.Sigton.timeSystem
            .Where(x => x.DayCount >= 13 || x.DayCount <= 15)
            .Subscribe(x =>
            {
                AudioManager.Singleton.PlayAudio("BGM_06");
            });
        GameEvents.Sigton.timeSystem
            .Where(x => x.DayCount == 16)
            .Subscribe(x =>
            {
                AudioManager.Singleton.PlayAudio("BGM_07");
                AudioManager.Singleton.PlayAudio("BGM_08");
            });
    }
}
