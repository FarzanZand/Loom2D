using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{

    // 1. Add this script to any entity to have access to FXs
    // 2. Any new FX materials as Material typeMat;
    // 3. call the function from wherever (i.e. Entity method Damage(); ) 

    private SpriteRenderer sr;

    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    private Material originalMat;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material; 
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;                    // Change material in the sprite renderer to the hitMat, added in inspector
        yield return new WaitForSeconds(.2f);

        sr.material = originalMat;               // Reset back to original material
    }

}
