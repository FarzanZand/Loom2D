using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    // Make this a singleton, so it is solo and also make it so it can be reached from anywhere in the game

    public static PlayerManager instance;
    public Player player; 
    private void Awake()
    {
        if(instance != null)
            Destroy(instance.gameObject);
        else
        instance = this;
    }

}
