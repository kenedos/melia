using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Skills.Handlers.Archers.Fletcher
{
	/// <summary>
	/// Handler for the Fletcher skill Cross Fire.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Fletcher_CrossFire)]
	public class Fletcher_CrossFireOverride : IPassiveSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster)
		{
			skill.OnCooldownChanged = () => ChargeArrow(skill, caster);

			if (skill.IsOnCooldown)
			{
				if (caster.Components.TryGet<CooldownComponent>(out var cc) && cc.TryGet(skill.Data.CooldownGroup, out var cooldown))
					cooldown.OnCooldownChanged = () => skill.OnCooldownChanged?.Invoke();
			}
			else
			{
				ChargeArrow(skill, caster);
			}
		}

		private static void ChargeArrow(Skill skill, ICombatEntity caster)
		{
			if (caster.TryGetActiveAbility(AbilityId.Fletcher45, out _))
				return;

			if (!Fletcher_FletcherArrowShotOverride.HasQuiverSpace(caster, Fletcher_FletcherArrowShotOverride.CrossFireCost))
				return;

			var cooldown = caster.StartCooldown(skill.Data.CooldownGroup, skill.Properties.CoolDown);
			if (cooldown != null)
				cooldown.OnCooldownChanged = () => skill.OnCooldownChanged?.Invoke();

			caster.StartBuff(BuffId.Fletcher_CrossFire_Buff, TimeSpan.Zero);
		}
	}
}
