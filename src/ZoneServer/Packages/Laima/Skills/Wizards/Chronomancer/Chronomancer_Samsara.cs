using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Util;

namespace Melia.Zone.Skills.Handlers.Wizards.Chronomancer
{
	[Package("laima")]
	[SkillHandler(SkillId.Chronomancer_Samsara)]
	public class Chronomancer_SamsaraOverride : IMeleeGroundSkillHandler
	{
		private const string VarReincarnated = "Melia.Skill.Samsara.Reincarnated";
		private const string VarCreatedBySamsara = "Melia.Skill.Samsara.Created";

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(farPos);
			caster.SetAttackState(true);

			var maxTargets = 3 + skill.Level / 2;
			var deadTargets = GetDeadEnemies(caster, farPos, maxTargets);

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();

			Send.ZC_SKILL_READY(caster, skill, skillHandle, caster.Position, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, caster.Position, caster.Direction, caster.Position);

			Send.ZC_SYNC_START(caster, skillHandle, 1);
			foreach (var deadMob in deadTargets)
			{
				deadMob.Vars.SetBool(VarReincarnated, true);

				var createCount = 1;
				if (caster is Character character
					&& character.TryGetActiveAbilityLevel(AbilityId.Chronomancer3, out var abilityLevel)
					&& RandomProvider.Next(1, 101) <= abilityLevel)
				{
					createCount++;
				}

				for (var i = 0; i < createCount; i++)
				{
					if (!deadMob.Map.TryGetRandomPositionInRange(deadMob.Position, 10, out var position))
						position = deadMob.Position.GetRandomInRange2D(10);

					var clone = Mob.CopyFrom(deadMob, position);
					if (clone == null)
						continue;

					var hpRate = Math.Max(0.50f, 1f - 0.03f * skill.Level);
				clone.Properties.SetFloat(PropertyName.HP, (int)(clone.Properties.GetFloat(PropertyName.MHP) * hpRate));
					clone.Vars.SetBool(VarCreatedBySamsara, true);

					deadMob.Map.AddMonster(clone);

					clone.StartBuff(BuffId.Samsara_Buff, TimeSpan.FromSeconds(1), caster);
				}
			}
			Send.ZC_SYNC_END(caster, skillHandle, 0);
			Send.ZC_SYNC_EXEC_BY_SKILL_TIME(caster, skillHandle, TimeSpan.FromMilliseconds(300));

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);
		}

		private static List<Mob> GetDeadEnemies(ICombatEntity caster, Position targetPos, int maxTargets)
		{
			const float Range = 300;

			var deadMobs = caster.Map.GetActorsInRange<Mob>(targetPos, Range, mob =>
				mob.IsDead
				&& caster.CheckRelation(mob, RelationType.Enemy)
				&& !mob.Vars.GetBool(VarReincarnated)
				&& !mob.Vars.GetBool(VarCreatedBySamsara)
			);

			return deadMobs.Take(maxTargets).ToList();
		}
	}
}
