using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSkill : Skill
{
    // Todo perhaps separate DashSkill from CloneSkill.
    public override void UseSkill()
    {
        base.UseSkill();
        Debug.Log("Left Clone Behind");
    }

}
