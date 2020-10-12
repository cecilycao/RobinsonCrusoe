using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractMediatorTemplate 
{
    public virtual void OnInteractionStart() { }
    /// <summary>
    /// this microtine is applied to listen the event when the interact key pressed
    /// </summary>
    public abstract void ListenInteractKeyPressedEvent();
    
    public virtual void OnInteractionEnd() { }
}
