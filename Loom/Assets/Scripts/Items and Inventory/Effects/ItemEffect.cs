using System.Collections;
using System.Collections.Generic;
using UnityEngine;




// An item effect has this as parent. It is created as a scriptable data object.
// Equipment can have item effects.
// Weapon attacks execute the ExecuteEffect. It is executed from the PlayerAnimationTriggers.cs script. 


public class ItemEffect : ScriptableObject
{
    [TextArea]
    public string effectDescription;

    public virtual void ExecuteEffect(Transform _enemyPosition)
    {
        Debug.Log("Effect executed!");
    }
}

