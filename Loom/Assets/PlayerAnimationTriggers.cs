using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{

    // Set the animation clip to call this as an event
    // This will be passed to the player, which will call that function in current playerState
    // Setting triggerCalled to true.
    // This will for instance in playerAttack1 return the player to idle when triggerCalled is true. A check done in update.
    // A means to control trigger-timings in animator.
    // triggerCalled is reset to false in enter-method when you enter a state. 
    // Basically, with this, you can via animator trigger an event via the update method in state

    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }
}
