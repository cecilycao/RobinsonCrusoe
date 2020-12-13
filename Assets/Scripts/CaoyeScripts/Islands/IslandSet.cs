using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandSet : MonoBehaviour
{
    public List<Island> coreIslands;
    public List<Island> normalIslands;

    // Start is called before the first frame update
    public void initialize()
    {
        Island[] allIslands =  GetComponentsInChildren<Island>(true);
        foreach (Island island in allIslands)
        {
            if (island.isCore)
            {
                coreIslands.Add(island);
            } else
            {
                normalIslands.Add(island);
            }
        }
        Debug.Log("coreIslandCount: " + coreIslands.Count);
        Debug.Log("normalIslandsCount: " + normalIslands.Count);
    }

    public Island nextIsland()
    {
        for (int i = 0; i < normalIslands.Count; i++)
        {
            if (!normalIslands[i].isActive())
            {
                return normalIslands[i];
            }
        }
        return null;
    }

    public bool createIsland()
    {
        Island next = nextIsland();
        
        if(next != null)
        {
            next.create();
            return true;
        }
        Debug.Log("All islands have been created");
        return false;
    }
}
