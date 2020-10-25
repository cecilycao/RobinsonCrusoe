using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class DiaryModify : MonoBehaviour
{
    // Start is called before the first frame update
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
    }
    void InitIcons()
    {
        var iconManagers = FindObjectOfType<IconManager>();
        var negativeCollector = FindObjectOfType<NegativeResourceCollector>();
       
    }
}
