using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{

    [Header("Dash")]
    public bool dashUnlocked { get; private set; }
    [SerializeField] private UI_SkillTreeSlot dashUnlockButton;             // Drag object into this from inspector

    [Header("Clone on dash")]
    public bool cloneOnDashUnlocked { get; private set; }
    [SerializeField] private UI_SkillTreeSlot cloneOnDashUnlockButton;


    [Header("Clone on arrival")]
    public bool cloneOnArrivalUnlocked { get; private set; }
    [SerializeField] private UI_SkillTreeSlot cloneOnArrivalUnlockButton;

    protected override void Start()
    {
        base.Start();

        // Add listeners to each button, so when they are clicked, these methods are called
        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);        // When you click the dashUnlockButton, run UnlockDash()
        cloneOnDashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        cloneOnArrivalUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);
    }

    public override void UseSkill()
    {
        base.UseSkill();
    }

    private void UnlockDash()
    {
        if(dashUnlockButton.unlocked)
            dashUnlocked = true;
    }

    private void UnlockCloneOnDash()
    {
        if(cloneOnDashUnlockButton.unlocked)
            cloneOnDashUnlocked = true;
    }

    private void UnlockCloneOnArrival()
    {

        if (cloneOnArrivalUnlockButton.unlocked)
            cloneOnArrivalUnlocked = true;
    }

    public void CloneOnDash() // Called in PlayerDashState.cs
    {
        if (cloneOnDashUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
        }
    }

    public void CloneOnArrival() // Called in PlayerDashState.cs
    {
        if (cloneOnArrivalUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
        }
    }
}
