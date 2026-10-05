using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.Packages.Laima.Abilities.Clerics.Inquisitor;
using Melia.Zone.Pads;
using Melia.Zone.Pads.Handlers;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Pads.Clerics.Inquisitor
{
	[Package("laima")]
	[PadHandler("F_smoke008_pearofanguish##1")]
	public class Inquisitor_PearofAnguish_Pad : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, IUpdatePadHandler
	{
		private const float TriggerRange = 100f;
		private const float MagicDetectionRange = 150f;
		private const int UpdateIntervalMilliseconds = 200;
		private static readonly TimeSpan InstallationDuration = TimeSpan.FromSeconds(30);
		private static readonly TimeSpan HitAnimationTime = TimeSpan.Zero;
		private static readonly HashSet<Pad> ActivatingPads = new();
		private static readonly object ActivationLock = new();

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			pad.SetRange(TriggerRange);
			pad.SetUpdateInterval(UpdateIntervalMilliseconds);
			pad.Trigger.LifeTime = InstallationDuration;
			pad.Trigger.MaxUseCount = 1;
			pad.Trigger.MaxActorCount = 1;

			Send.ZC_NORMAL.PadUpdate(pad, true);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			lock (ActivationLock)
				ActivatingPads.Remove(pad);

			Send.ZC_NORMAL.PadUpdate(pad, false);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var caster = args.Creator as Character;
			var target = args.Initiator;

			if (caster == null || target == null || target.IsDead)
				return;

			if (!caster.IsEnemy(target))
				return;

			if (!this.TryReserve(pad))
				return;

			this.Attack(pad, caster, target);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var caster = args.Creator as Character;

			if (caster == null || caster.IsDead || caster.Map == null || pad.IsDead)
				return;

			if (this.IsReserved(pad))
				return;

			var area = new Circle(pad.Position, MagicDetectionRange);
			var target = caster.Map
				.GetAttackableEnemiesIn(caster, area)
				.Where(candidate => candidate != null && !candidate.IsDead)
				.Where(this.IsUsingMagicSkill)
				.OrderBy(candidate => pad.Position.Get2DDistance(candidate.Position))
				.FirstOrDefault();

			if (target == null || !this.TryReserve(pad))
				return;

			pad.Skill.Run(this.FlyAndAttack(pad, caster, target));
		}

		private async Task FlyAndAttack(Pad pad, Character caster, ICombatEntity target)
		{
			try
			{
				if (target == null || target.IsDead)
					return;

				var movementTime = pad.Movement.MoveTo(target.Position);

				if (movementTime > TimeSpan.Zero)
					await pad.Skill.Wait(movementTime);

				if (target.IsDead || caster.IsDead)
					return;

				this.DealDamage(pad, caster, target);
			}
			finally
			{
				this.ConsumePad(pad);
			}
		}

		private void Attack(Pad pad, Character caster, ICombatEntity target)
		{
			try
			{
				this.DealDamage(pad, caster, target);
			}
			finally
			{
				this.ConsumePad(pad);
			}
		}

		private void DealDamage(Pad pad, Character caster, ICombatEntity target)
		{
			if (pad == null || caster == null || target == null || target.IsDead)
				return;

			var skill = pad.Skill;
			var modifier = SkillModifier.Default;
			modifier.DamageMultiplier *= Inquisitor_PearofAnguishEnhanceAbility.GetDamageMultiplier(caster);

			var result = SCR_SkillHit(caster, target, skill, modifier);

			if (result.Result != HitResultType.Dodge && result.Damage > 0)
			{
				target.TakeDamage(result.Damage, caster);
				Inquisitor_PearofAnguishSilenceAbility.TryApplySilence(caster, target, skill);
			}

			var hit = new SkillHitInfo(caster, target, skill, result, HitAnimationTime, TimeSpan.Zero);
			Send.ZC_SKILL_HIT_INFO(caster, new[] { hit });
		}

		private bool IsUsingMagicSkill(ICombatEntity target)
		{
			var currentSkillId = target.GetCurrentSkill();

			if (currentSkillId == SkillId.None)
				return false;

			if (!target.TryGetSkill(currentSkillId, out var currentSkill))
				return false;

			return string.Equals(currentSkill.Data.ClassType.ToString(), "Magic", StringComparison.OrdinalIgnoreCase);
		}

		private bool TryReserve(Pad pad)
		{
			if (pad == null || pad.IsDead)
				return false;

			lock (ActivationLock)
				return ActivatingPads.Add(pad);
		}

		private bool IsReserved(Pad pad)
		{
			lock (ActivationLock)
				return ActivatingPads.Contains(pad);
		}

		private void ConsumePad(Pad pad)
		{
			if (pad == null)
				return;

			pad.Trigger.MaxUseCount = 0;

			lock (ActivationLock)
				ActivatingPads.Remove(pad);

			Send.ZC_NORMAL.PadUpdate(pad, false);

			if (pad.Map != null)
				pad.Map.RemovePad(pad);
		}
	}
}
