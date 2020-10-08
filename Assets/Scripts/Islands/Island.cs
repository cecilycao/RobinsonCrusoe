using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System;
using UniRx;

public class Island : MonoBehaviour, IInteractableIsland
{
    
    public bool isCore;
    public bool initialActive = false;
    public Island[] nearbyIslands;
    public GameObject activeIsland;
    public GameObject inactiveIsland;
    public int rainIntensity = 0;
    //temp
    public UnityEvent IslandDamaged;
    public UnityEvent IslandDestroyed;

    //private
    [SerializeField]
    IslandCondition m_condition;
    [SerializeField]
    int durability;
    bool playerHere;

    //DATA VALUES
    //How much durability of this island decreases when nearby island destroyed.
    public static int NEARBY_ISLAND_DESTROY_EFFECT = 20;
    public static int DAMAGED_DURABILITY = 60;
    //public static int REPAIR_EFFECT = 50;
    static int MAX_DURABILITY = 100;

    public string MaterialType => "BuildingMaterial";

    public int MaterialCost => 15;

    public string InteractObjectType => "Island";

    private void Awake()
    {
        Collider collider = activeIsland.GetComponent<Collider>();
        ColliderBridge cb = collider.gameObject.AddComponent<ColliderBridge>();
        cb.Initialize(this);
    }

    private void OnEnable()
    {
        
    }

    // Start is called before the first frame update
    //inactive at start
    void Start()
    {
        GameEvents.Sigton.OnRainStart += () =>
        {
            rainIntensity = 1;
        };

        GameEvents.Sigton.OnRainEnd += () =>
        {
            rainIntensity = 0;
        };

        GameEvents.Sigton.OnStormStart += () =>
        {
            rainIntensity = 2;
        };

        GameEvents.Sigton.OnStormEnd += () =>
        {
            rainIntensity = 0;
        };

        if (isCore)
        {
            initializeActiveIsland();
        }
        else
        {
            if (!initialActive)
            {
                initializeInactiveIsland();
            }
            else
            {
                create();
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!isCore && m_condition != IslandCondition.DESTROYED)
        {
            
            //durability -= 1;
            if(durability <= 0)
            {
                destroy();
            } else if (durability <= DAMAGED_DURABILITY)
            {
                damaged();
            }
        }
    }

    void reduceDurability()
    {
        if(durability > 0)
        {
            durability -= (rainIntensity + 1);
        }
        
    }

    private void initializeActiveIsland()
    {
        activeIsland.SetActive(true);
        inactiveIsland.SetActive(false);
        durability = MAX_DURABILITY;
        m_condition = IslandCondition.CREATED;
    }

    private void initializeInactiveIsland()
    {
        activeIsland.SetActive(false);
        inactiveIsland.SetActive(true);
        durability = 0;
        m_condition = IslandCondition.DESTROYED;
    }

    public void create()
    {

        initializeActiveIsland();
        if (!isCore)
        {
            //InvokeRepeating("tempTimer", 0.0f, 1.0f);
            GameEvents.Sigton.timeSystem
                .Subscribe(_data =>
                {
                    reduceDurability();
                });
        }
    }

    public void onNearbyIslandDestroy()
    {
        if(!isCore && m_condition != IslandCondition.DESTROYED)
        {
            Debug.Log("boom, affects " + gameObject.name);
            durability -= NEARBY_ISLAND_DESTROY_EFFECT;
        }
    }

    //completely repair the island.
    //need to check and find out how many resources need for repairing
    public void repair()
    {
        durability = 100;
        //todo: change Island Texture
    }

    public void damaged()
    {
        m_condition = IslandCondition.DAMAGED;
        IslandDamaged.Invoke();
        //todo: change Island Texture
    }

    public void destroy()
    {
        if (!playerHere)
        {
            inactiveIsland.SetActive(true);
            activeIsland.SetActive(false);
            m_condition = IslandCondition.DESTROYED;
            foreach (Island nearbyIsland in nearbyIslands)
            {
                nearbyIsland.onNearbyIslandDestroy();
            }
            IslandDestroyed.Invoke();
        } else
        {

        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(m_condition != IslandCondition.DESTROYED && collision.gameObject.tag == "Player")
        {
            playerHere = true;
            if (m_condition == IslandCondition.DAMAGED)
            {
                //todo: enable repair
            }
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (m_condition != IslandCondition.DESTROYED && collision.gameObject.tag == "Player" && playerHere)
        {
            playerHere = false;
        }
    }

    public bool isActive()
    {
        if(m_condition == IslandCondition.DESTROYED)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 offset = new Vector3(0, 0, -0.3f);
        Handles.Label(transform.position + offset, "" + durability);
    }

    public void OnIslandRestoreStart()
    {
        repair();
    }

    public void OnIslandRestoreEnd()
    {
        print("play island restore animation");
    }

    public void StartInteractWithPlayer()
    {
        playerHere = true;
        if (!isCore)
        {
            print("Enter Island Space " + this.name);
            
            if (m_condition == IslandCondition.DAMAGED)
            {
                Mediator.Sigton.StartRestoreIsland(this);
            }
            
        }
    }

    public void EndInteractWithPlayer()
    {
        playerHere = false;
        if (!isCore)
        {
            Mediator.Sigton.EndInteract();
        }
    }
}

public enum IslandCondition
{
    CREATED,
    DAMAGED,
    DESTROYED
}
