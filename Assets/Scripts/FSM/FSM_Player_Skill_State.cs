using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class FSM_Player_Skill_State : PlayerState
{
    public override void FSMEnter(PlayerController unitController)
    {
        unitController.curSkillData.SkillEnter(unitController, unitController.target, unitController.targetNode);
    }

    public override void FSMExit(PlayerController unitController)
    {
        unitController.curSkillData.SkillExit(unitController, unitController.target, unitController.targetNode);
    }

    public override void FSMUpdate(PlayerController unitController)
    {
        unitController.curSkillData.SkillUpdate(unitController, unitController.target, unitController.targetNode);
    }
}