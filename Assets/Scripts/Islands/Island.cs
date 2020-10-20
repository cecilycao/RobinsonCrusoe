using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System;
using UniRx;

public class Island : RestoreIslandSample
{
    
    public bool isCore;
    public bool initialActive = false;
    public Island[] nearbyIslands;
    public GameObject activeIsland;
    public GameObject inactiveIsland;
    public int rainIntensity = 0;
    public GameObject Icon;
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
    public static int NEARBY_ISLAND_DESTROY_EFFECT = 10;
    //threshold that player can repair the island
    public static int DAMAGED_DURABILITY = 60;
    //durability - DAMAGE_PER_DELTA_TIME every DELTA_TIME
    public static int DELTA_TIME = 5;
    public static int DAMAGE_PER_DELTA_TIME = 2;
    //public static int REPAIR_EFFECT = 50;
    static int MAX_DURABILITY = 100;

    public Vector3 IconOffset = new Vector3(0, 7, 0);

    int delta_time = 0;

    public override string MaterialType => "BuildingMaterial";

    public override int MaterialCost => (int)Mathf.Round(10*(1-durability/100));

    public override string InteractObjectType => "Island";

    private void Awake()
    {
        /*
        Collider collider = activeIsland.GetComponent<Collider>();
        ColliderBridge cb = collider.gameObject.AddComponent<ColliderBridge>();
        cb.Initialize(this);
        */
    }

    private void OnEnable()
    {
        
    }

    // Start is called before the first frame update
    //inactive at start
    void Start()
    {
        Icon = FindObjectOfType<IconManager>().RepairIslandIcon;
        if (Icon == null)
        {
            Debug.LogError("Icon haven't been assigned to IconManager");
        }

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
        if (durability > 0)
        {
            delta_time++;
            if (delta_time == DELTA_TIME)
            {

                durability -= (rainIntensity + DAMAGE_PER_DELTA_TIME);

                delta_time = 0;
            }
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
        if (playerHere)
        {
            Icon.transform.position = Camera.main.WorldToScreenPoint(transform.position + IconOffset);
        }
        m_condition = IslandCondition.DAMAGED;
        IslandDamaged.Invoke();
        //todo: change Island Texture
    }

    public void destroy()
    {
        if (playerHere)
        {
            //todo: 传送player 回家，写日记并开始新的一天
            PlayerRespawn respawn = FindObjectOfType<PlayerRespawn>();
            respawn.respawn();
            //attr.gameObject.transform.position = 
            GameEvents.Sigton.onTheDestroyedIsland.Invoke();
        }
        inactiveIsland.SetActive(true);
        activeIsland.SetActive(false);
        m_condition = IslandCondition.DESTROYED;
        foreach (Island nearbyIsland in nearbyIslands)
        {
            nearbyIsland.onNearbyIslandDestroy();
        }
        IslandDestroyed.Invoke();
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

    public override void StartContact()
    {
        playerHere = true;
        if (!isCore)
        {
            print("Enter Island Space " + this.name);
            
            if (m_condition == IslandCondition.DAMAGED)
            {
                Mediator.Sigton.StartInteraction(this);
                
            }
            
        }
    }

    public override void EndContact()
    {
        playerHere = false;
        if (!isCore)
        {
            Mediator.Sigton.EndInteract();
            
        }
    }

    public override void StartInteract()
    {
        
    }

    public override void EndInteract(object result)
    {
        repair();
        print("play island restore animation");
    }

    public override void ShowIcon()
    {
        Icon.SetActive(true);
    }

    public override void HideIcon()
    {
        Icon.SetActive(false);
    }

}

public enum IslandCondition
{
    CREATED,
    DAMAGED,
    DESTROYED
}
