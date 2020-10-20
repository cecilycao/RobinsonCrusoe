using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIslandSet : IslandSet
{
    public string NPCName;
    public Collider InitialCollider;
    public Vector3 appearPosition;
    public Vector3 combinedPosition;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void initializeIsland()
    {
        initialize();
        foreach (Island island in normalIslands)
        {
            island.IslandDestroyed.AddListener(checkFloatingAway);
        }
        InitialCollider.enabled = true;
    }

    public void appear()
    {
        gameObject.SetActive(true);
        initializeIsland();
        gameObject.transform.position = appearPosition;
    }

    public void combine()
    {
        gameObject.transform.position = combinedPosition;
        InitialCollider.enabled = false;
    }

    public void checkFloatingAway()
    {
        if (!hasActiveBound())
        {
            floatingAway();
        }
    }

    public bool hasActiveBound()
    {
        bool hasActiveBound = false;
        foreach (Island island in normalIslands)
        {
            if (island.isActive())
            {
                hasActiveBound = true;
            }
        }
        return hasActiveBound;
    }

    public void floatingAway()
    {
        Debug.Log("NPC Island Floating Away");
        //check time, if night
        //destroy all.
        gameObject.SetActive(false);
    }
}
