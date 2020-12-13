using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTest : MonoBehaviour
{
    MeshRenderer mesh;
    Color[] colors = new Color[4];
    int n = 0;
    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        mesh.material.color = Color.white;

        colors[0] = Color.red;
        colors[1] = Color.blue;
        colors[2] = Color.green;
        colors[3] = Color.yellow;
    }
    public void OnPlayerEnter()
    {
        n++;
        if (n>=3)
        {
            n = 0;
        }
        mesh.material.color = colors[n];
    }

    public void OnPlayerExit()
    {
        mesh.material.color = Color.white;
    }
}
