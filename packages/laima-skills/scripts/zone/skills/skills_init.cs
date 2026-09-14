//--- Melia Script ----------------------------------------------------------
// Skill Initialization
//--- Description -----------------------------------------------------------
// Grants skills specific buffs on learning a skill.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Scripting;
using Melia.Shared.Game.Const;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;

public class SkillInitializationScript : GeneralScript
{
	[On("PlayerSkillLevelChanged")]
	public void OnPlayerSkillLevelChanged(object sender, PlayerSkillLevelChangedEventArgs args)
	{
		var skill = args.Skill;

		switch (skill.Id)
		{
			case SkillId.Fletcher_BarbedArrow:
				if (!skill.IsOnCooldown && !skill.Owner.IsBuffActive(BuffId.Fletcher_BarbedArrow_Buff))
					skill.Owner.StartBuff(BuffId.Fletcher_BarbedArrow_Buff, TimeSpan.Zero);
				break;
			case SkillId.Fletcher_BodkinPoint:
				if (!skill.IsOnCooldown && !skill.Owner.IsBuffActive(BuffId.Fletcher_BodkinPoint_Buff))
					skill.Owner.StartBuff(BuffId.Fletcher_BodkinPoint_Buff, TimeSpan.Zero);
				break;
			case SkillId.Fletcher_CrossFire:
				if (!skill.IsOnCooldown && !skill.Owner.IsBuffActive(BuffId.Fletcher_CrossFire_Buff))
					skill.Owner.StartBuff(BuffId.Fletcher_CrossFire_Buff, TimeSpan.Zero);
				break;
			case SkillId.Fletcher_Singijeon:
				if (!skill.IsOnCooldown && !skill.Owner.IsBuffActive(BuffId.Fletcher_Singijeon_Buff))
					skill.Owner.StartBuff(BuffId.Fletcher_Singijeon_Buff, TimeSpan.Zero);
				break;
		}
	}
}
