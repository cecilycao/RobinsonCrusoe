using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandManager : MonoBehaviour
{
    private static IslandManager _instance;
    public static IslandManager Instance { get { return _instance; } }

    public PlayerIslandSet m_islandSet;
    public NPCIslandSet[] NPCIslandSets;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

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
