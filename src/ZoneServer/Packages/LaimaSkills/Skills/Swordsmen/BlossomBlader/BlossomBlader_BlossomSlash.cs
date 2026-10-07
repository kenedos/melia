using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Swordsmen.BlossomBlader
{
	/// <summary>
	/// Handler for the Blossom Blader skill Blossom Slash, which dashes to
	/// the target and hacks it 12 times while the Blossom Blader is
	/// invulnerable.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.BlossomBlader_BlossomSlash)]
	public class BlossomBlader_BlossomSlashOverride : IGroundSkillHandler
	{
		private const int HitCount = 12;
		private const float Range = 130f;
		private const float DistanceFromTarget = 20f;
		private static readonly TimeSpan InvulnerableDuration = TimeSpan.FromMilliseconds(1200);
		private static readonly TimeSpan HitDelay = TimeSpan.FromMilliseconds(250);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (target == null || target.IsDead || !caster.Position.InRange2D(target.Position, Range))
			{
				Send.ZC_SKILL_CAST_CANCEL(caster);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);
			caster.StartBuff(BuffId.Skill_NoDamage_Buff, InvulnerableDuration);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, target.Position);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, target.Position, ForceId.GetNew(), null);

			var destination = target.Position.GetRelative(target.Position.GetDirection(caster.Position), DistanceFromTarget);
			if (!caster.Map.Ground.TryGetNearestValidPosition(destination, out destination))
				destination = target.Position;

			caster.SetPosition(destination);
			caster.TurnTowards(target);

			var modifier = SkillModifier.MultiHit(HitCount + BlossomBladerSkillHelper.GetBlossomShowerHits(caster));
			var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
			target.TakeDamage(skillHitResult.Damage, caster);

			Send.ZC_SKILL_HIT_INFO(caster, new SkillHitInfo(caster, target, skill, skillHitResult, HitDelay, TimeSpan.Zero));

			if (skillHitResult.Damage > 0)
				BlossomBladerSkillHelper.ApplyFlowering(caster, target);
		}
	}
}
