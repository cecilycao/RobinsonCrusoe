﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Fungus;

public class SickManager : MonoBehaviour
{
    public int NPCSickedDay = 7;
    public int PlayerSickedDay = 13;
    public int NPCHelpPreference = 20;
    public Flowchart playerFlowchart;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.Sigton.timeSystem
            .Where(x => x.DayCount == NPCSickedDay && x.IsDay)
            .First()
            .Subscribe(x =>
            {
                GameEvents.Sigton.onNPCSicked.OnNext(NPCSickedDay);
            });

        GameEvents.Sigton.timeSystem
            .Where(x => x.DayCount == PlayerSickedDay && x.IsDay)
            .First()
            .Subscribe(x =>
            {
                NPCSample NPC = FindObjectOfType<NPCSample>();
                if(NPC == null)
                {
                    Debug.LogError("No NPC in the scene");
                }
                if (NPC.preference >= NPCHelpPreference)
                {
                    playerFlowchart.SendFungusMessage("PlayerSickWithHelp");
                }
                else
                {
                    playerFlowchart.SendFungusMessage("PlayerSick");
                }
                GameEvents.Sigton.onPlayerSicked.OnNext(PlayerSickedDay);
            });

        GameEvents.Sigton.timeSystem
            .Where(x => x.DayCount == PlayerSickedDay + 1 && x.IsDay)
            .First()
            .Subscribe(x =>
            {
                GameEvents.Sigton.onPlayerSickedEnd.OnNext(PlayerSickedDay + 1);
            });
    }

    public void NPCSickEnd()
    {
        Debug.Log("NPC Sick End");
        GameEvents.Sigton.onNPCSickedEnd.OnNext(NPCSickedDay + 1);
    }

    public void checkPreference()
    {

    }

    public void lockPlayerWhenSick()
    {
        Mediator.Sigton.playerSick();
    }
}

