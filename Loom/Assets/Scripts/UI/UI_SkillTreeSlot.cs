using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Every skillslot gameobject in the skilltree has this script attached to it, and it is a button. 
    // This button calls UnlockSkillSlot() when clicked which does checks to see if skill is unlockable or not
    // These checks are based on if pre-requisite skills are unlocked or not. If all checks pass, skill gets unlocked. 
    // Tooltip is shown when mouse hovers over, and details is in UI_SkillTooltip.cs script. It activates tooltip when mouse hovers over it

    // Unlocking skills: Look at DashSkill.cs for simple example. SkillUnlock management is in respective <SkillName>Skill.cs skill. 
    // In the skillnameSkill.cs You need a bool for <skillname>Unlocked, and a reference to UI_SkillTreeSlot so you can reach its button, you can drag a skillslot via inspector. 
    // You also need a function SkillnameUnlock() making the skillUnlocked bool true when it runs. 
    // To run SkillNameUnlock(), in the Start() add a listener to the UI_SkillSlot button, like, dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
    // This means that when you click on the skillslot (AKA talent in the talent tree), UnlockSkillname() is called, setting <skillname>unlocked as true, making it active. 

    private UI ui;

    [SerializeField] private int skillPointsCost; 
    [SerializeField] private string skillName;                               // Filled in inspector per skillslot
    [TextArea]
    [SerializeField] private string skillDescription;                        // Filled in inspector per skillslot
    [SerializeField] private Color lockedSkillColor;

    public bool unlocked;

    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;            // This decides which skills need to be unlocked before you can open this skill. The pre-requisites 
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;              // For instance, if you can only select one skill in a row. If anyone here is unlocked, this doesn't click.
    private Image skillImage;

    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_UI - " + skillName;
    }

    private void Awake()                                                     // Called before start
    {
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot()); // So we don't need to assign the function in the inspector every time.
    }

    private void Start()
    {
        skillImage = GetComponent<Image>();
        ui = GetComponentInParent<UI>();

        skillImage.color = lockedSkillColor;

    }

    public void UnlockSkillSlot()                                            // When you click on button, this runs and unlocks skillslot if all checks pass
    {
        if (PlayerManager.instance.HaveEnoughSkillPoints(skillPointsCost) == false) // If you don't have enough skillpoints, return and don't unlock.
            return;

        for (int i = 0; i < shouldBeUnlocked.Length; i++)                    // Drag in inspector. All these should be unlocked for this to pass
        {
            if (shouldBeUnlocked[i].unlocked == false)
            {
                Debug.Log("Cannot unlock skill");
                return;
            }
        }

        for (int i = 0; i < shouldBeLocked.Length; i++)                      // Drag in inspector. All these should be locked for this to pass
        {
            if (shouldBeLocked[i].unlocked == true)
            {
                Debug.Log("Cannot unlock skill");
                return;
            }
        }

        unlocked = true;                                                     // If checks above pass, the skillslot unlocks.                    
        skillImage.color = Color.white;
    }
    public void OnPointerEnter(PointerEventData eventData)                   // x = 0, y = 0 when mouse is at the bottom left corner. 1920, 1080 is top right. 
    {
        ui.skillTooltip.ShowTooltip(skillDescription, skillName);            // Show the tooltip with a description
        Vector2 mousePosition = Input.mousePosition;

        // Place the Tooltip near the mouse
        float xOffset = 0;
        float yOffset = 0;

        if (mousePosition.x > 600)                                            // Mouse is on the right side of screen
            xOffset = -150;
        else
            xOffset = 150;

        if (mousePosition.y > 320)
            yOffset = -150;
        else
            yOffset = 150;

        ui.skillTooltip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
    }

    public void OnPointerExit(PointerEventData eventData)       
    {
        ui.skillTooltip.HideTooltip();                                       // Hide the tooltip
    }


}
