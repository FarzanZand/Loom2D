using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_UI : MonoBehaviour
{
    // Flip-functionality
    // 1. Create a System.Action (event holder) in Entity.cs called onFlipped. 
    // 2. add the function FlipUI() to the event holder onFlipped.
    // 3. FlipUI will be called every time onFlipped is called. 
    // 4. onFlipped() is called in Entity Flip(), as long as it is not null (shouldn't be null, as HealthBar_UI.cs adds it.

    // Updating health
    // 1. Add UpdateHealthUI on the event onHealthChanged, which is called in CharacterStats.DecreaseHealthBy(_damage). 
    // 2. DecreaseHealthBy() lowers current health, and also updates health-bar via UpdateHealthUI(). Every time health is decreased, update UI. 
    // 3. Likely, we'll add same for increasing health later.

    private Entity entity;
    private CharacterStats myStats; 
    private RectTransform myTransform;
    private Slider slider; 

    private void Start()
    {
        myTransform = GetComponent<RectTransform>();
        entity = GetComponentInParent<Entity>();
        slider = GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<CharacterStats>();


        entity.onFlipped += FlipUI;                  // adds FlipUI to the event to be called on each onFlipped-call.
        myStats.onHealthChanged += UpdateHealthUI;   // adds UpdateHealthUI to the event to be called on each onHealthChanged-call.

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHealth; 
    }
    
    private void FlipUI()
    {
        myTransform.Rotate(0, 180, 0);
    }

    private void OnDisable()
    {
        entity.onFlipped -= FlipUI;                 // unsubscribe FlipUI from onFlipped.
        myStats.onHealthChanged -= UpdateHealthUI;  // unsubscribe UpdateHealthUI from onHealthChanged
    }
}
