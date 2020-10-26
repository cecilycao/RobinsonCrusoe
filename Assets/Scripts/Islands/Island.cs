using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
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
    public GameObject damagedIsland;
    public GameObject inactiveIsland;
    public int rainIntensity = 0;
    public GameObject Icon;
    public GameObject CreateIslandEffect;
    public GameObject ShipwreckedEffect;
    //temp
    public UnityEvent IslandDamaged;
    public UnityEvent IslandDestroyed;

    //private
    [SerializeField]
    IslandCondition m_condition;
    [SerializeField]
    int durability;
    bool playerHere;
    bool isSick = false;

    //DATA VALUES
    //How much durability of this island decreases when nearby island destroyed.
    public static int NEARBY_ISLAND_DESTROY_EFFECT = 10;
    //threshold that player can repair the island
    public static int DAMAGED_DURABILITY = 60;
    //durability - DAMAGE_PER_DELTA_TIME every DELTA_TIME
    public static int DELTA_TIME = 5;
    public static int DAMAGE_PER_DELTA_TIME = 2;
    //public static int REPAIR_EFFECT = 50;
    public int MAX_DURABILITY = 100;

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

        GameEvents.Sigton.onNPCSicked
         .Subscribe(x =>
         {
             isSick = true;
         });
        GameEvents.Sigton.onNPCSickedEnd
         .Subscribe(x =>
         {
             isSick = false;
         });
        GameEvents.Sigton.onPlayerSicked
         .Subscribe(x =>
         {
             isSick = true;
         });
        GameEvents.Sigton.onPlayerSickedEnd
         .Subscribe(x =>
         {
             isSick = false;
         });



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
                createIsland();
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
        damagedIsland.SetActive(false);
        inactiveIsland.SetActive(false);
        durability = MAX_DURABILITY;
        m_condition = IslandCondition.CREATED;

    }

    private void initializeInactiveIsland()
    {
        activeIsland.SetActive(false);
        damagedIsland.SetActive(false);
        inactiveIsland.SetActive(true);
        durability = 0;
        m_condition = IslandCondition.DESTROYED;
    }


    //create Island with create Effect
    public void create()
    {
        GameObject createEffect = Instantiate(CreateIslandEffect, transform);
        StartCoroutine(createIslandCoroutine(createEffect));
    }

    IEnumerator createIslandCoroutine(GameObject createEffect)
    {
        yield return new WaitForSeconds(2);
        Destroy(createEffect);
        createIsland();

    }

    //create island
    public void createIsland()
    {
        initializeActiveIsland();
        if (!isCore)
        {
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
        damagedIsland.SetActive(false);
        activeIsland.SetActive(true);
        m_condition = IslandCondition.CREATED;
        
    }

    public void damaged()
    {
        if (playerHere)
        {
            Icon.transform.position = Camera.main.WorldToScreenPoint(transform.position + IconOffset);
        }
        m_condition = IslandCondition.DAMAGED;
        activeIsland.SetActive(false);
        damagedIsland.SetActive(true);
        IslandDamaged.Invoke();
        //todo: change Island Texture
    }

    public void destroy()
    {
        m_condition = IslandCondition.DESTROYED;
        if (playerHere)
        {
            PlayerRespawn respawn = FindObjectOfType<PlayerRespawn>();
            respawn.respawn();
            GameEvents.Sigton.onTheDestroyedIsland.Invoke();
        }
        activeIsland.SetActive(false);
        damagedIsland.SetActive(false);
        inactiveIsland.SetActive(true);
        GameObject destroyEffect = Instantiate(ShipwreckedEffect, transform);

        StartCoroutine(destroyIsland(destroyEffect));
    }

    IEnumerator destroyIsland(GameObject destroyEffect)
    {
        yield return new WaitForSeconds(4);
        Destroy(destroyEffect);
        
        foreach (Island nearbyIsland in nearbyIslands)
        {
            nearbyIsland.onNearbyIslandDestroy();
        }
        IslandDestroyed.Invoke();
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

    //private void OnDrawGizmos()
    //{
    //    Vector3 offset = new Vector3(0, 0, -0.3f);
    //    if (!playerHere)
    //    {
    //        Handles.Label(transform.position + offset, "" + durability);
    //    } else
    //    {
    //        Handles.Label(transform.position + offset, "" + durability + " | Player");
    //    }
        
    //}

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
        //Can not interact while sick
        if (!isCore && !isSick)
        {
            if (m_condition == IslandCondition.DAMAGED)
            {
                Icon.transform.position = Camera.main.WorldToScreenPoint(transform.position + IconOffset);
                Mediator.Sigton.StartInteraction(this);
                
            }
            
        }
    }

    public override void EndContact()
    {
        playerHere = false;
        if (!isCore && !isSick)
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
