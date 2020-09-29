using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderBridge : MonoBehaviour
{
    Island m_island;

    public void Initialize(Island island)
    {
        m_island = island;
    }

    private void OnCollisionEnter(Collision collision)
    {
        m_island.OnCollisionEnter(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        m_island.OnCollisionExit(collision);
    }
}
