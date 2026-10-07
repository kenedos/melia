using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Zone.Buffs;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Skills.Handlers.Clerics.PlagueDoctor
{
	/// <summary>
	/// Handler for the Plague Doctor skill Pandemic, which spreads the
	/// debuffs of the enemies around the caster among them and throws them
	/// into a Panic.
	/// </summary>
	/// <remarks>
	/// Incineration only spreads with Pandemic: Spread Incineration, at a 5%
	/// chance per ability level for each enemy.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.PlagueDoctor_Pandemic)]
	public class PlagueDoctor_PandemicOverride : IGroundSkillHandler
	{
		private const float PandemicRange = 90f;
		private const int SpreadIncinerationChancePerLevel = 5;
		private static readonly TimeSpan PanicDuration = TimeSpan.FromSeconds(15);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			var maxTargets = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio);
			var enemies = caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, PandemicRange).Take(maxTargets).ToList();
			var debuffs = this.GetSpreadableDebuffs(enemies);

			caster.TryGetActiveAbilityLevel(AbilityId.PlagueDoctor14, out var spreadIncinerationLevel);

			foreach (var enemy in enemies)
			{
				foreach (var debuff in debuffs)
				{
					if (debuff.Id == BuffId.Incineration_Debuff && GameRandom.Get().Next(100) >= spreadIncinerationLevel * SpreadIncinerationChancePerLevel)
						continue;

					PlagueDoctorSkillHelper.SpreadBuff(debuff, enemy);
				}

				enemy.StartBuff(BuffId.Panic_Pandemic_Debuff, skill.Level, 0, PanicDuration, caster, skill.Id);
			}
		}

		/// <summary>
		/// Returns one of each debuff the enemies carry that Pandemic can
		/// spread.
		/// </summary>
		/// <param name="enemies"></param>
		/// <returns></returns>
		private List<Buff> GetSpreadableDebuffs(List<ICombatEntity> enemies)
		{
			var debuffs = new Dictionary<BuffId, Buff>();

			foreach (var enemy in enemies)
			{
				if (!enemy.Components.TryGet<BuffComponent>(out var buffComponent))
					continue;

				foreach (var buff in buffComponent.GetList())
				{
					if (buff.Data.Type != BuffType.Debuff || !buff.Data.RemoveBySkill || buff.Id == BuffId.Panic_Pandemic_Debuff)
						continue;

					debuffs.TryAdd(buff.Id, buff);
				}
			}

			return debuffs.Values.ToList();
		}
	}
}
