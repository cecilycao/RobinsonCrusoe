using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryAnimation : MonoBehaviour
{
    public DiaryManager m_manager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onAnimationEnd()
    {
        m_manager.showContentAfterOpen();
    }
}
