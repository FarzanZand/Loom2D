using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    // Make this a singleton, so it is solo and also make it so it can be reached from anywhere in the game

    public static PlayerManager instance;
    public Player player;
 
    public int skillPoints;           // Called "currency" in tutorial. HaveEnoughSkillPoints is HaveEnoughMoney there. 
    private void Awake()
    {
        if(instance != null)
            Destroy(instance.gameObject);
        else
        instance = this;
    }

    public bool HaveEnoughSkillPoints(int _skillPointsCost)
    {
        if (_skillPointsCost > skillPoints)
        {
            Debug.Log("Not enough skill points");
            return false;
        }

        skillPoints = skillPoints - _skillPointsCost;
        return true;
    }

    public int GetCurrentSkillPoints()
    {
        return skillPoints;
    }

}
