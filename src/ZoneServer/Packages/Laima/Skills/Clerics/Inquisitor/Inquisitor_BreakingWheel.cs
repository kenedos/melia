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
using Melia.Zone.Scripting;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Spawning;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Inquisitor
{
	[Package("laima")]
	[SkillHandler(SkillId.Inquisitor_BreakingWheel)]
	public class Inquisitor_BreakingWheel : IGroundSkillHandler, ICancelSkillHandler
	{
		private static readonly object ActiveWheelsLock = new();
		private static readonly Dictionary<int, Mob> ActiveWheels = new();
		private static readonly Dictionary<int, int> PendingCasts = new();
		private const int BaseDurationSeconds = 10;
		private const int MaximumReinforceLevel = 5;
		private const int MaximumPeriodicTargets = 10;
		private const float DamageRange = 60f;
		private static readonly TimeSpan SpawnDelay = TimeSpan.FromMilliseconds(800);
		private static readonly TimeSpan DamageInterval = TimeSpan.FromSeconds(1);
		private static readonly TimeSpan HitAnimationTime = TimeSpan.Zero;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity selectedTarget)
		{
			if (caster is not Character character || character.IsDead)
				return;

			if (!character.TrySpendSp(skill))
			{
				character.SetAttackState(false);
				character.ServerMessage(Localization.Get("Not enough SP."));
				Send.ZC_SKILL_DISABLE(character);
				return;
			}

			this.RemoveActiveWheel(character);
			character.TurnTowards(farPos);
			character.SetAttackState(true);
			skill.IncreaseOverheat();
			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
			this.SetPendingCast(character, skillHandle);
			Send.ZC_SKILL_READY(character, skill, skillHandle, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(character, 0, originPos, character.Direction, farPos);
			Send.ZC_SKILL_RANGE_SQUARE(character, originPos, farPos, 21, false);
			skill.Run(this.CreateWheel(skill, character, farPos, skillHandle));
		}

		public void Handle(Skill skill, ICombatEntity caster)
		{
			if (caster is not Character character)
				return;

			this.ClearPendingCast(character);
			this.RemoveActiveWheel(character);
			character.SetAttackState(false);
			Send.ZC_SKILL_DISABLE(character);
		}

		private async Task CreateWheel(Skill skill, Character caster, Position position, int skillHandle)
		{
			try
			{
				await skill.Wait(SpawnDelay);

				if (caster.IsDead || caster.Map == null || !this.IsPendingCast(caster, skillHandle))
					return;

				var durationSeconds = BaseDurationSeconds;

				if (caster.TryGetActiveAbilityLevel(AbilityId.Inquisitor20, out var reinforceLevel))
					durationSeconds += Math.Clamp(reinforceLevel, 1, MaximumReinforceLevel);

				var wheel = MonsterSkillCreateMob(skill, caster, "pcskill_Breaking_wheel", position, 0, "", "None", 0, durationSeconds, "None", "Faction#Monster");

				if (wheel == null)
				{
					caster.ServerMessage(Localization.Get("Failed to summon Breaking Wheel."));
					return;
				}

				if (!this.IsPendingCast(caster, skillHandle))
				{
					wheel.Map?.RemoveMonster(wheel);
					return;
				}

				// Use the torch's one-damage mechanic while preserving the wheel's maximum HP.
				wheel.Properties.InvalidateAll();
				var maximumHp = Math.Max(1f, wheel.Properties.GetFloat(PropertyName.MHP));
				var propertyOverrides = new PropertyOverrides();
				propertyOverrides.Add(PropertyName.HPCount, maximumHp);
				wheel.ApplyOverrides(propertyOverrides);
				wheel.Properties.InvalidateAll();
				wheel.HealToFull();

				this.ClearPendingCast(caster, skillHandle);
				this.RegisterActiveWheel(caster, wheel);
				wheel.StartBuff(BuffId.Inquisitor_BreakingWheel_Internal_Buff, skill.Level, 0, TimeSpan.FromSeconds(durationSeconds), caster, skill.Id);
				skill.RunFree(this.DamageLoop(skill, caster, wheel, durationSeconds));
			}
			finally
			{
				this.ClearPendingCast(caster, skillHandle);
				caster.SetAttackState(false);
				Send.ZC_SKILL_DISABLE(caster);
			}
		}

		private async Task DamageLoop(Skill skill, Character caster, Mob wheel, int durationSeconds)
		{
			try
			{
				var enhanceMultiplier = Inquisitor_BreakingWheelEnhanceAbility.GetDamageMultiplier(caster);
				var expiresAt = DateTime.UtcNow.AddSeconds(durationSeconds);
				while (DateTime.UtcNow < expiresAt)
				{
					await skill.Wait(DamageInterval);
					if (!this.IsWheelValid(caster, wheel))
						break;

					var area = new Circle(wheel.Position, DamageRange);
					var targets = caster.Map.GetAttackableEnemiesIn(caster, area)
						.Where(target => target != null && !target.IsDead && target != wheel)
						.OrderBy(target => wheel.Position.Get2DDistance(target.Position))
						.Take(MaximumPeriodicTargets)
						.ToList();
					var hits = new List<SkillHitInfo>();
					foreach (var target in targets)
					{
						var modifier = SkillModifier.Default;
						modifier.DamageMultiplier *= enhanceMultiplier;
						var result = SCR_SkillHit(caster, target, skill, modifier);
						if (result.Result != HitResultType.Dodge && result.Damage > 0)
							target.TakeDamage(result.Damage, caster);
						hits.Add(new SkillHitInfo(caster, target, skill, result, HitAnimationTime, TimeSpan.Zero));
					}
					if (hits.Count > 0)
						Send.ZC_SKILL_HIT_INFO(caster, hits);
				}
			}
			finally
			{
				this.RemoveActiveWheel(caster, wheel);
			}
		}

		private bool IsWheelValid(Character caster, Mob wheel)
		{
			return caster != null && !caster.IsDead && caster.Map != null && wheel != null && !wheel.IsDead && wheel.Map != null && wheel.Map == caster.Map;
		}

		private void RegisterActiveWheel(Character caster, Mob wheel)
		{
			Mob previousWheel = null;
			lock (ActiveWheelsLock)
			{
				ActiveWheels.TryGetValue(caster.Handle, out previousWheel);
				ActiveWheels[caster.Handle] = wheel;
			}

			if (previousWheel != null && previousWheel != wheel)
				previousWheel.Map?.RemoveMonster(previousWheel);
		}

		private void SetPendingCast(Character caster, int skillHandle)
		{
			lock (ActiveWheelsLock)
				PendingCasts[caster.Handle] = skillHandle;
		}

		private bool IsPendingCast(Character caster, int skillHandle)
		{
			lock (ActiveWheelsLock)
				return PendingCasts.TryGetValue(caster.Handle, out var currentHandle) && currentHandle == skillHandle;
		}

		private void ClearPendingCast(Character caster, int expectedHandle = 0)
		{
			if (caster == null)
				return;

			lock (ActiveWheelsLock)
			{
				if (!PendingCasts.TryGetValue(caster.Handle, out var currentHandle) || expectedHandle != 0 && currentHandle != expectedHandle)
					return;

				PendingCasts.Remove(caster.Handle);
			}
		}

		private void RemoveActiveWheel(Character caster, Mob expectedWheel = null)
		{
			if (caster == null)
				return;

			Mob wheel = null;
			lock (ActiveWheelsLock)
			{
				if (!ActiveWheels.TryGetValue(caster.Handle, out wheel) || expectedWheel != null && wheel != expectedWheel)
					return;

				ActiveWheels.Remove(caster.Handle);
			}

			wheel.Map?.RemoveMonster(wheel);
		}
	}
}
