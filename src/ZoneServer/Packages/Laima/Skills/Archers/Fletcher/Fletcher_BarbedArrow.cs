using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors;
using Melia.Zone.Skills.Handlers.Base;

namespace Melia.Zone.Skills.Handlers.Archers.Fletcher
{
	/// <summary>
	/// Handler for the Fletcher skill Barbed Arrow.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Fletcher_BarbedArrow)]
	public class Fletcher_BarbedArrowOverride : IPassiveSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster)
		{
			if (!skill.IsOnCooldown && !caster.IsBuffActive(BuffId.Fletcher_BarbedArrow_Buff))
				caster.StartBuff(BuffId.Fletcher_BarbedArrow_Buff, TimeSpan.Zero);
			skill.OnCooldownChanged += () => caster.StartBuff(BuffId.Fletcher_BarbedArrow_Buff, TimeSpan.Zero);
		}
	}
}
