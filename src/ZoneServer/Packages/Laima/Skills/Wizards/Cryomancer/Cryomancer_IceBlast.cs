using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Pads;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using Melia.Zone.Skills.SplashAreas;

namespace Melia.Zone.Skills.Handlers.Cryomancer
{
	/// <summary>
	/// Handler for the Cryomancer skill Ice Blast.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Cryomancer_IceBlast)]
	public class Cryomancer_IceBlastOverride : IGroundSkillHandler, IDynamicCasted
	{
		private const int DebuffDurationSeconds = 20;
		private const int DebuffUpdateTimeMilliseconds = 1000;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.PlaySound("voice_atk_long_cast_f", "voice_war_atk_long_cast");
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			if (caster.TryGetActiveAbilityLevel(AbilityId.Cryomancer24, out _))
				skill.Run(this.HandleAbilitySkill(caster, skill, originPos, farPos));
			else
				skill.Run(this.HandleSkill(caster, skill, originPos, farPos));
		}

		/// <summary>
		/// Handles the ability version of Ice Blast, dealing 4x skill factor
		/// as direct damage to frozen enemies.
		/// </summary>
		private async Task HandleAbilitySkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(450));

			var targetList = SkillSelectEnemiesInCircle(caster, caster.Position, 250);
			foreach (var currentTarget in targetList)
			{
				if (!currentTarget.IsBuffActiveByKeyword(BuffTag.Freeze))
					continue;

				var multihitCount = 4;
				var modifier = SkillModifier.MultiHit(multihitCount);

				var skillHitResult = SCR_SkillHit(caster, currentTarget, skill, modifier);
				currentTarget.TakeDamage(skillHitResult.Damage, caster);
				var hit = new HitInfo(caster, currentTarget, skill, skillHitResult);
				Send.ZC_HIT_INFO(caster, currentTarget, hit);

				// Don't stack same debuff
				if (currentTarget.TryGetBuff(BuffId.IceBlast_Debuff, out var iceBlastBuff))
					continue;

				var debuff = currentTarget.StartBuff(BuffId.IceBlast_Debuff, skill.Level, skillHitResult.Damage / multihitCount, TimeSpan.FromSeconds(DebuffDurationSeconds), caster);
				debuff?.SetUpdateTime(DebuffUpdateTimeMilliseconds);
			}
		}

		/// <summary>
		/// Handles the default version of Ice Blast, applying the IceBlast
		/// debuff to frozen enemies.
		/// </summary>
		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(450));

			var targetList = SkillSelectEnemiesInCircle(caster, caster.Position, 250);
			foreach (var currentTarget in targetList)
			{
				if (!currentTarget.IsBuffActiveByKeyword(BuffTag.Freeze))
					continue;

				// Don't stack same debuff
				if (currentTarget.TryGetBuff(BuffId.IceBlast_Debuff, out var iceBlastBuff))
					continue;

				var skillHitResult = SCR_SkillHit(caster, currentTarget, skill);

				var debuff = currentTarget.StartBuff(BuffId.IceBlast_Debuff, skill.Level, skillHitResult.Damage, TimeSpan.FromSeconds(DebuffDurationSeconds), caster);
				debuff?.SetUpdateTime(DebuffUpdateTimeMilliseconds);
			}
		}
	}
}
