using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Clerics.Exorcist
{
	/// <summary>
	/// Handler for the Exorcist skill Gregorate, a wave of divine energy
	/// that strikes the enemies around the Exorcist 5 times and marks them
	/// with Gregorate: Magic for a minute.
	/// </summary>
	/// <remarks>
	/// A summoned zombie caught in the wave is destroyed outright.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Exorcist_Gregorate)]
	public class Exorcist_GregorateOverride : ISelfSkillHandler
	{
		private const float Range = 100f;
		private const int HitCount = 5;
		private static readonly TimeSpan SelfBuffDuration = TimeSpan.FromMilliseconds(500);
		private static readonly TimeSpan MarkDuration = TimeSpan.FromMinutes(1);
		private static readonly TimeSpan AniTime = TimeSpan.FromMilliseconds(300);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			caster.StartBuff(BuffId.Gregorate_Buff, skill.Level, 0, SelfBuffDuration, caster, skill.Id);

			var maxTargets = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio3);
			var hits = new List<SkillHitInfo>();

			foreach (var target in caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, Range).Take(maxTargets))
			{
				if (this.IsSummonedZombie(target))
				{
					target.Kill(caster);
					continue;
				}

				var skillHitResult = SCR_SkillHit(caster, target, skill, SkillModifier.MultiHit(HitCount));
				target.TakeDamage(skillHitResult.Damage, caster);

				hits.Add(new SkillHitInfo(caster, target, skill, skillHitResult, AniTime, TimeSpan.Zero));

				target.StartBuff(BuffId.GregorateATK_Buff, skill.Level, 0, MarkDuration, caster, skill.Id);
			}

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, originPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, originPos, hits);
		}

		/// <summary>
		/// Returns true if the target is a zombie summoned by another
		/// character.
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		private bool IsSummonedZombie(ICombatEntity target)
		{
			return target is Mob mob && mob.OwnerHandle != 0 && mob.Data.ClassName.Contains("zombie", StringComparison.OrdinalIgnoreCase);
		}
	}
}
