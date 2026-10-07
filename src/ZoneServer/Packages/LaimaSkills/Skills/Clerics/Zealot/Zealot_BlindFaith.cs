using System;
using System.Collections.Generic;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Clerics.Zealot
{
	/// <summary>
	/// Handler for the Zealot skill Blind Faith, which spends 5% of the
	/// Zealot's max SP to shock the enemies around them, draining twice
	/// that SP from each.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Zealot_BlindFaith)]
	public class Zealot_BlindFaithOverride : ISelfSkillHandler
	{
		private const int HitCount = 10;
		private const float ShockRange = 60f;
		private const float SpCostRate = 0.05f;
		private const float SpDrainRate = 2f;
		private static readonly TimeSpan AniTime = TimeSpan.FromMilliseconds(200);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			var spCost = caster.Properties.GetFloat(PropertyName.MSP) * SpCostRate;

			if (!caster.TrySpendSp(spCost))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var hits = new List<SkillHitInfo>();

			foreach (var target in caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, ShockRange))
			{
				var skillHitResult = SCR_SkillHit(caster, target, skill, SkillModifier.MultiHit(HitCount));
				target.TakeDamage(skillHitResult.Damage, caster);

				hits.Add(new SkillHitInfo(caster, target, skill, skillHitResult, AniTime, TimeSpan.Zero));

				this.DrainSp(target, spCost * SpDrainRate);
			}

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, originPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, caster.Position);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, originPos, hits);
		}

		/// <summary>
		/// Drains the given amount of SP from the target.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="amount"></param>
		private void DrainSp(ICombatEntity target, float amount)
		{
			if (target is Character character)
				character.ModifySp(-Math.Min(amount, character.Sp));
			else
				target.Properties.Modify(PropertyName.SP, -amount);
		}
	}
}
