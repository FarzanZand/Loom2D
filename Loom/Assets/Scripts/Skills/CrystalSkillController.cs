using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{
    private float crystalExistTimer;


    public void SetupCrystal(float _crystalDuration)
    {
        crystalExistTimer = _crystalDuration;
    }

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;

        if (crystalExistTimer < 0) 
            SelfDestroy();
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    } 

}
