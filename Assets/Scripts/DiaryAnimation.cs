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

    public void onOpenAnimationEnd()
    {
        m_manager.showContentAfterOpen();
    }

    public void onCloseAnimationEnd()
    {
        m_manager.hideContentAfterClose();
    }
}
