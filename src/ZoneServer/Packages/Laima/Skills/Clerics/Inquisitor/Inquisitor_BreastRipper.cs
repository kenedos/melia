using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Packages.Laima.Abilities.Clerics.Inquisitor;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Inquisitor
{
	[Package("laima")]
	[SkillHandler(SkillId.Inquisitor_BreastRipper)]
	public class Inquisitor_BreastRipper : IGroundSkillHandler, ICancelSkillHandler
	{
		private static readonly object ActiveCastsLock = new();
		private static readonly Dictionary<int, int> ActiveCasts = new();
		private const int HitCount = 10;
		private const int MaximumTargets = 9;
		private const float AttackRange = 140f;
		private const float ConeHalfAngle = 30f;
		private const float DamageIncreasePerHit = 0.05f;
		private const float ConditionalDamageMultiplier = 1.50f;
		private static readonly TimeSpan SkillDuration = TimeSpan.FromSeconds(3);
		private static readonly TimeSpan HitInterval = TimeSpan.FromMilliseconds(300);
		private static readonly TimeSpan HitAnimationTime = TimeSpan.Zero;

		public void Handle(Skill skill, ICombatEntity caster)
		{
			this.EndChannel(caster);
		}

		private void EndChannel(ICombatEntity caster, int expectedCastHandle = 0)
		{
			if (caster is not Character character)
				return;

			lock (ActiveCastsLock)
			{
				if (!ActiveCasts.TryGetValue(character.Handle, out var activeCastHandle) || expectedCastHandle != 0 && activeCastHandle != expectedCastHandle)
					return;

				ActiveCasts.Remove(character.Handle);
			}

			character.StopBuff(BuffId.BreastRipper_Buff);
			character.SetAttackState(false);
			Send.ZC_SKILL_CAST_CANCEL(character);
			Send.ZC_SKILL_DISABLE(character);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity selectedTarget)
		{
			if (caster is not Character character || character.IsDead || character.Map == null)
				return;

			if (!character.TrySpendSp(skill))
			{
				character.SetAttackState(false);
				character.ServerMessage(Localization.Get("Not enough SP."));
				Send.ZC_SKILL_DISABLE(character);
				return;
			}

			this.EndChannel(character);
			var directionX = character.Direction.Cos;
			var directionZ = character.Direction.Sin;
			var aimPosition = new Position(originPos.X + directionX * AttackRange, originPos.Y, originPos.Z + directionZ * AttackRange);
			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
			var forceId = ForceId.GetNew();
			lock (ActiveCastsLock)
				ActiveCasts[character.Handle] = skillHandle;

			character.SetAttackState(true);
			var channelStarted = false;
			try
			{
				character.StartBuff(BuffId.BreastRipper_Buff, skill.Level, 0, SkillDuration, character, skill.Id);
				skill.IncreaseOverheat();
				Send.ZC_SKILL_READY(character, skill, skillHandle, originPos, aimPosition);
				Send.ZC_NORMAL.UpdateSkillEffect(character, 0, originPos, character.Direction, aimPosition);
				Send.ZC_SKILL_MELEE_GROUND(character, skill, aimPosition, forceId, null);
				skill.RunFree(this.Channel(skill, character, directionX, directionZ, skillHandle, forceId));
				channelStarted = true;
			}
			finally
			{
				if (!channelStarted)
					this.EndChannel(character, skillHandle);
			}
		}

		private async Task Channel(Skill skill, Character caster, float directionX, float directionZ, int castHandle, int forceId)
		{
			try
			{
				var enhanceMultiplier = Inquisitor_RipperEnhanceAbility.GetDamageMultiplier(caster);
				var hasArmorBreak = Inquisitor_RipperArmorBreakAbility.TryGetLevel(caster, out var armorBreakLevel);

				for (var hitIndex = 0; hitIndex < HitCount; hitIndex++)
				{
					if (!this.IsChannelActive(caster, castHandle))
						break;

					var originPosition = caster.Position;
					var targets = this.GetTargets(caster, originPosition, directionX, directionZ);
					var continuousMultiplier = 1f + (hitIndex + 1) * DamageIncreasePerHit;
					var hits = new List<SkillHitInfo>();

					foreach (var target in targets)
					{
						var modifier = SkillModifier.Default;
						modifier.DamageMultiplier *= enhanceMultiplier;
						modifier.DamageMultiplier *= continuousMultiplier;
						if (this.IsDevil(target) || caster.IsBuffActive(BuffId.Judgment_Buff))
							modifier.DamageMultiplier *= ConditionalDamageMultiplier;

						var result = SCR_SkillHit(caster, target, skill, modifier);

						if (result.Result != HitResultType.Dodge && result.Damage > 0)
						{
							target.TakeDamage(result.Damage, caster);
							if (hasArmorBreak)
								target.StartBuff(BuffId.BreastRipper_Debuff, armorBreakLevel, 0, SkillDuration, caster, skill.Id);
						}

						var hit = new SkillHitInfo(caster, target, skill, result, HitAnimationTime, TimeSpan.Zero);
						hit.ForceId = forceId;
						hits.Add(hit);
					}

					if (hits.Count > 0)
						Send.ZC_SKILL_HIT_INFO(caster, hits);

					if (hitIndex + 1 < HitCount)
					{
						await skill.Wait(HitInterval);

						if (!this.IsChannelActive(caster, castHandle))
							break;
					}
				}
			}
			finally
			{
				this.EndChannel(caster, castHandle);
			}
		}

		private bool IsChannelActive(Character caster, int castHandle)
		{
			if (caster == null || caster.IsDead || caster.Map == null || !caster.IsBuffActive(BuffId.BreastRipper_Buff))
				return false;

			lock (ActiveCastsLock)
				return ActiveCasts.TryGetValue(caster.Handle, out var activeCastHandle) && activeCastHandle == castHandle;
		}

		private IList<ICombatEntity> GetTargets(ICombatEntity caster, Position originPosition, float directionX, float directionZ)
		{
			var area = new Circle(originPosition, AttackRange);

			return caster.Map.GetAttackableEnemiesIn(caster, area)
				.Where(target => target != null && !target.IsDead && target.Map == caster.Map && target.Layer == caster.Layer)
				.Where(target => this.IsInsideCone(target.Position, originPosition, directionX, directionZ))
				.OrderBy(target => originPosition.Get2DDistance(target.Position))
				.Take(MaximumTargets)
				.ToList();
		}

		private bool IsInsideCone(Position targetPosition, Position originPosition, float directionX, float directionZ)
		{
			var offsetX = targetPosition.X - originPosition.X;
			var offsetZ = targetPosition.Z - originPosition.Z;
			var distance = MathF.Sqrt(offsetX * offsetX + offsetZ * offsetZ);

			if (distance <= 0.001f)
				return true;

			var normalizedX = offsetX / distance;
			var normalizedZ = offsetZ / distance;
			var dot = Math.Clamp(normalizedX * directionX + normalizedZ * directionZ, -1f, 1f);
			var angle = MathF.Acos(dot) * 180f / MathF.PI;

			return angle <= ConeHalfAngle;
		}

		private bool IsDevil(ICombatEntity target)
		{
			return target != null && target.Race == RaceType.Velnias;
		}
	}
}
