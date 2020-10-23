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
                item.resourceAccount = 25;
            }
        };
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
