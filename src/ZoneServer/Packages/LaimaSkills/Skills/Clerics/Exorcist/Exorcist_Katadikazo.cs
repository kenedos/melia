using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Clerics.Exorcist
{
	/// <summary>
	/// Handler for the Exorcist skill Katadikazo, a spear of divine light
	/// that strikes the target area 3 times, harder against enemies in
	/// Aqua Benedicta.
	/// </summary>
	/// <remarks>
	/// [Arts] Katadikazo: Flame Spear turns it into Fire and leaves a fire
	/// on the ground.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Exorcist_Katadikazo)]
	public class Exorcist_KatadikazoOverride : IGroundSkillHandler, IDynamicCasted
	{
		private const float Range = 70f;
		private const float AquaBenedictaDamageBonus = 0.50f;
		private static readonly TimeSpan[] HitDelays = [TimeSpan.FromMilliseconds(700), TimeSpan.FromMilliseconds(300), TimeSpan.FromMilliseconds(300)];

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPos))
			{
				caster.ServerMessage(Localization.Get("No target location specified."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, targetPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, targetPos, caster.Direction, targetPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, targetPos, ForceId.GetNew(), null);

			skill.Run(this.Strike(skill, caster, targetPos));
		}

		/// <summary>
		/// Strikes the target area three times.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="targetPos"></param>
		/// <returns></returns>
		private async Task Strike(Skill skill, ICombatEntity caster, Position targetPos)
		{
			var flameSpear = caster.IsAbilityActive(AbilityId.Exorcist19);
			var maxTargets = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio2);

			for (var i = 0; i < HitDelays.Length; i++)
			{
				await skill.Wait(HitDelays[i]);

				if (caster.IsDead)
					return;

				if (i == 0 && flameSpear)
				{
					var pad = new Pad(PadName.Exorcist_Katadikazo, caster, skill, new Circle(targetPos, Range));
					pad.Position = targetPos;
					caster.Map.AddPad(pad);
				}

				var hits = new List<SkillHitInfo>();

				foreach (var target in caster.Map.GetAttackableEnemiesInPosition(caster, targetPos, Range).Take(maxTargets))
				{
					var modifier = new SkillModifier();

					if (flameSpear)
						modifier.AttackAttribute = AttributeType.Fire;

					if (target.IsBuffActive(BuffId.AquaBenedictaHIT_Debuff))
						modifier.DamageMultiplier += AquaBenedictaDamageBonus;

					var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
					target.TakeDamage(skillHitResult.Damage, caster);

					hits.Add(new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));
				}

				if (hits.Count > 0)
					Send.ZC_SKILL_HIT_INFO(caster, hits);
			}
		}
	}
}
