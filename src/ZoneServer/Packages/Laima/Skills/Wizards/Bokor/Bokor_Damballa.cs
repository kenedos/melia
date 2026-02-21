using System;
using Melia.Shared.Packages;
using Melia.Shared.L10N;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Maps;
using static Melia.Zone.Skills.SkillUseFunctions;
using Melia.Zone.Skills.SplashAreas;
using System.Collections.Generic;
using System.Linq;

namespace Melia.Zone.Skills.Handlers.Wizards.Bokor
{
	/// <summary>
	/// Handler for the Bokor skill Damballa.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Bokor_Damballa)]
	public class Bokor_DamballaOverride : IMeleeGroundSkillHandler
	{
		/// <summary>
		/// Handles skill behavior
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		/// <param name="targets"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			var maxRange = skill.Properties.GetFloat(PropertyName.MaxR);

			if (caster is not Character character)
				return;

			if (!caster.Position.InRange2D(farPos, maxRange))
			{
				caster.ServerMessage(Localization.Get("Too far away."));
				return;
			}

			if (!caster.TryGetBuff(BuffId.PowerOfDarkness_Buff, out var darkForceBuff) || darkForceBuff.OverbuffCounter < 10)
			{
				caster.ServerMessage(Localization.Get("Requires at least 10 stacks of Dark Force."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			darkForceBuff.OverbuffCounter -= 10;
			darkForceBuff.NotifyUpdate();

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, Position.Zero);

			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			var summons = character.Summons.GetSummons();

			foreach (var summon in summons)
			{
				var enemiesInRange = summon.Map.GetAttackableEntitiesInRangeAroundEntity(character, summon, 150f);
				var targetsToHit = enemiesInRange.Take(5);

				summon.Kill(caster);

				foreach (var target in targetsToHit)
				{
					var skillHitResult = SCR_SkillHit(caster, target, skill);
					target.PlayEffect("F_rize004_dark_damballa", 5f, 0);
					target.TakeDamage(skillHitResult.Damage, caster);

					var hitInfo = new HitInfo(caster, target, skill, skillHitResult.Damage, skillHitResult.Result);
					hitInfo.DamageDelay = TimeSpan.FromMilliseconds(100);

					Send.ZC_HIT_INFO(caster, target, hitInfo);
				}
			}
		}
	}
}
