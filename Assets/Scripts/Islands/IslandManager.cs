using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandManager : MonoBehaviour
{
    public PlayerIslandSet m_islandSet;
    public NPCIslandSet[] NPCIslandSets;

    // Start is called before the first frame update
    void Start()
    {
        if(m_islandSet == null)
        {
            m_islandSet = GetComponentInChildren<PlayerIslandSet>();
        }
        if(NPCIslandSets.Length == 0)
        {
            NPCIslandSets = GetComponentsInChildren<NPCIslandSet>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void createIsland()
    {
        m_islandSet.createIsland();
    }

    public void NPCIslandAppear(string NPCName)
    {
        foreach (NPCIslandSet set in NPCIslandSets)
        {
            if(set.NPCName == NPCName)
            {
                set.appear();
            }
        }
        
    }
    public void NPCIslandCombine(string NPCName)
    {
        foreach (NPCIslandSet set in NPCIslandSets)
        {
            if (set.NPCName == NPCName)
            {
                set.combine();
            }
        }
    }
}
