using System;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Util;

namespace Melia.Zone.Buffs.HandlersOverrides.Wizards.Chronomancer
{
	[Package("laima")]
	[BuffHandler(BuffId.TimeForward_Debuff)]
	public class TimeForward_DebuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var caster = buff.Caster;

			var skillLevel = Math.Max(1, (int)buff.NumArg1);
			var cooldownIncrease = TimeSpan.FromSeconds(10 + (skillLevel * 2));

			if (!target.Components.TryGet<CooldownComponent>(out var cooldownComponent))
				return;

			if (target is Mob mob && mob.Data.Skills.Count > 0)
			{
				var randomSkillData = mob.Data.Skills[RandomProvider.Next(mob.Data.Skills.Count)];
				if (randomSkillData != null)
				{
					if (target.Components.TryGet<BaseSkillComponent>(out var skills))
					{
						if (!skills.TryGet(randomSkillData.SkillId, out var targetSkill))
						{
							targetSkill = new Skills.Skill(target, randomSkillData.SkillId, 1);
							skills.AddSilent(targetSkill);
						}

						var currentRemaining = cooldownComponent.GetRemain(targetSkill.CooldownData.Id);
						var newDuration = currentRemaining + cooldownIncrease;
						cooldownComponent.Start(targetSkill.CooldownData.Id, newDuration);
					}
				}
			}
			else if (target is Character character)
			{
				var allSkills = character.Skills.GetList();
				if (allSkills.Length > 0)
				{
					var randomSkill = allSkills[RandomProvider.Next(allSkills.Length)];
					var currentRemaining = cooldownComponent.GetRemain(randomSkill.CooldownData.Id);
					var newDuration = currentRemaining + cooldownIncrease;
					cooldownComponent.Start(randomSkill.CooldownData.Id, newDuration);
				}
			}
		}
	}
}
