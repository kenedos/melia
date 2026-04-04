using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Util;

namespace Melia.Zone.Skills.Handlers.Wizards.Chronomancer
{
	[Package("laima")]
	[SkillHandler(SkillId.Chronomancer_Samsara)]
	public class Chronomancer_SamsaraOverride : IGroundSkillHandler
	{
		private const string VarReincarnated = "Melia.Skill.Samsara.Reincarnated";
		private const string VarCreatedBySamsara = "Melia.Skill.Samsara.Created";

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(farPos);
			caster.SetAttackState(true);

			var deadTargets = GetDeadEnemies(caster, farPos, 15);

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();

			Send.ZC_SKILL_READY(caster, skill, skillHandle, caster.Position, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, caster.Position, caster.Direction, caster.Position);

			var reincarnateChance = Math.Min(100f, 30f + 3f * skill.Level);
			var doubleCloneChance = 0f;

			if (caster is Character character)
			{
				var SCR_Get_AbilityReinforceRate = ScriptableFunctions.Skill.Get("SCR_Get_AbilityReinforceRate");
				reincarnateChance = Math.Min(100f, reincarnateChance * (1f + SCR_Get_AbilityReinforceRate(skill)));

				if (character.TryGetActiveAbilityLevel(AbilityId.Chronomancer3, out var doubleLevel))
					doubleCloneChance = doubleLevel * 0.5f;
			}

			Send.ZC_SYNC_START(caster, skillHandle, 1);
			foreach (var deadMob in deadTargets)
			{
				deadMob.Vars.SetBool(VarReincarnated, true);

				if (RandomProvider.Next(1, 101) > reincarnateChance)
					continue;

				var cloneCount = 1;
				if (doubleCloneChance > 0 && RandomProvider.Next(1, 101) <= doubleCloneChance)
					cloneCount = 2;

				for (var i = 0; i < cloneCount; i++)
				{
					if (!deadMob.Map.TryGetRandomPositionInRange(deadMob.Position, 10, out var position))
						position = deadMob.Position.GetRandomInRange2D(10);

					var clone = Mob.CopyFrom(deadMob, position);
					if (clone == null)
						continue;

					var hpRate = Math.Max(0.50f, 1f - 0.03f * skill.Level);
					clone.Properties.SetFloat(PropertyName.HP, (int)(clone.Properties.GetFloat(PropertyName.MHP) * hpRate));
					clone.Vars.SetBool(VarCreatedBySamsara, true);

					clone.SpawnPosition = clone.Position;
					clone.Tendency = TendencyType.Aggressive;
					clone.Components.Add(new MovementComponent(clone));

					var aiName = deadMob.Data?.AiName;
					if (!string.IsNullOrEmpty(aiName) && AiScript.Exists(aiName))
						clone.Components.Add(new AiComponent(clone, aiName));
					else
						clone.Components.Add(new AiComponent(clone, "BasicMonster"));

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
				&& mob.Rank != MonsterRank.Boss
				&& mob.Rank != MonsterRank.MISC
				&& mob.Rank != MonsterRank.Material
				&& mob.Rank != MonsterRank.NPC
			);

			return deadMobs.Take(maxTargets).ToList();
		}
	}
}
