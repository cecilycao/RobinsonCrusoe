using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameEventAudioViewer : MonoBehaviour
{
    int previousHunger = 0;
    int previousFatigue = 0;

    bool hungerAudioStartPlay = false;
    private void Start()
    {
        DelayStart();
    }

    void DelayStart()
    {
        Observable.Timer(System.TimeSpan.FromMilliseconds(50))
            .Subscribe(x =>
            {
                WhenHungerValueIncreased();

                OnInteractBtnReleasedWhenPressed();

                OnInteractComplete();
            });
    }
    void WhenHungerValueIncreased()
    {
        GUIEvents.Singleton.Hunger
        .Where(x => x < previousHunger)
        .Subscribe(x =>
        {
            AudioManager.Singleton.PlayAudio("Player_hungerValueIncreased");
            previousHunger = x;
        });
    }
    void WhenFatigueValueIncreased()
    {
        GUIEvents.Singleton.Fatigue
            .Where(x => x > previousFatigue)
            .Subscribe(x =>
            {
                AudioManager.Singleton.PlayAudio("Player_FatigueValueIncreased");
            });
    }
    void OnInteractBtnReleasedWhenPressed()
    {
        GameEvents.Sigton.InteractEventDictionary[InteractEventTags.onInteractBtnPressed]
            .Subscribe(x =>
            {
                AudioManager.Singleton.PlayAudio("Interact_startContactTipSound");
            });
    }
    void OnInteractComplete()
    {
        GameEvents.Sigton.GetEvent<Subject<SubjectArg>>(InteractEventTags.onInteractionCompleted)
            .Subscribe(x =>
            {
                if (x.subjectMes is InteractableObjectType)
                {
                     var msg = (InteractableObjectType)x.subjectMes;

                    if (msg == InteractableObjectType.IslandBuilder ||
                        msg == InteractableObjectType.Island ||
                        msg == InteractableObjectType.FoodProcessPlant
                        )
                    {
                        AudioManager.Singleton.PlayAudio(AudioConfigKeys.Interact_build_restoreIsland_processFoodComplete);
                    }
                    if (msg == InteractableObjectType.PositiveCollect)
                    {
                        AudioManager.Singleton.PlayAudio(AudioConfigKeys.Interact_positiveCollectResourceComplete);
                    }
                    if (msg == InteractableObjectType.NegativeCollect)
                    {
                        AudioManager.Singleton.PlayAudio(AudioConfigKeys.Interact_resourceCollectComplete);
                    }
                }
            });
    }
}
