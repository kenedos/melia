using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Miko
{
	[Package("laima")]
	[SkillHandler(SkillId.Miko_KaguraDance)]
	public class Miko_KaguraDance : IGroundSkillHandler, IDynamicCasted
	{
		private const int MaximumChannelSeconds = 15;
		private const int MaximumTargets = 10;
		private const float EffectRange = 100f;
		private const float InitialDamageBonus = 0.42f;
		private const float FinalDamageBonus = 0.46f;
		private const float EnhancePerLevel = 0.005f;
		private static readonly TimeSpan TickInterval = TimeSpan.FromSeconds(1);
		private static readonly TimeSpan BuffDuration = TimeSpan.FromMinutes(15);
		private readonly ConcurrentDictionary<int, DateTime> _channelStartTimes = new();
		private readonly ConcurrentDictionary<int, byte> _runningChannels = new();

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			_channelStartTimes[caster.Handle] = DateTime.UtcNow;
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character || caster.IsDead)
				return;

			if (!caster.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				_channelStartTimes.TryRemove(caster.Handle, out _);
				caster.SetAttackState(false);
				return;
			}

			_channelStartTimes[caster.Handle] = DateTime.UtcNow;
			_runningChannels[caster.Handle] = 0;
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
			Send.ZC_SKILL_READY(caster, skill, skillHandle, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, originPos, caster.Direction, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);
			skill.Run(this.RunChannel(skill, character));
		}

		private async Task RunChannel(Skill skill, Character caster)
		{
			var elapsedTicks = 0;

			while (_runningChannels.ContainsKey(caster.Handle) && elapsedTicks < MaximumChannelSeconds)
			{
				await skill.Wait(TickInterval);

				if (!_runningChannels.ContainsKey(caster.Handle) || caster.IsDead)
					break;

				if (caster.IsAbilityActive(AbilityId.Miko18))
					this.ExecuteKenHit(skill, caster);

				elapsedTicks++;
			}

			if (caster.IsDead)
			{
				this.ClearChannel(caster);
				return;
			}

			if (elapsedTicks >= MaximumChannelSeconds)
			{
				_runningChannels.TryRemove(caster.Handle, out _);

				if (_channelStartTimes.ContainsKey(caster.Handle))
					this.FinishChannel(skill, caster);
			}
		}

		private void FinishChannel(Skill skill, ICombatEntity caster)
		{
			if (!_channelStartTimes.TryRemove(caster.Handle, out var startedAt))
			{
				caster.SetAttackState(false);
				return;
			}

			caster.SetAttackState(false);

			if (caster is not Character character || character.IsDead || character.IsAbilityActive(AbilityId.Miko18))
				return;

			var elapsedSeconds = Math.Clamp((float)(DateTime.UtcNow - startedAt).TotalSeconds, 0f, MaximumChannelSeconds);
			var progress = elapsedSeconds / MaximumChannelSeconds;
			var damageBonus = InitialDamageBonus + (FinalDamageBonus - InitialDamageBonus) * progress;
			this.ApplyKaguraBuffs(skill, character, damageBonus);
		}

		private void ClearChannel(ICombatEntity caster)
		{
			_runningChannels.TryRemove(caster.Handle, out _);
			_channelStartTimes.TryRemove(caster.Handle, out _);
			caster.SetAttackState(false);
		}

		private void ApplyKaguraBuffs(Skill skill, Character caster, float damageBonus)
		{
			var allies = caster.Map.GetCharacters(character => character != null && !character.IsDead && character.Layer == caster.Layer && character.IsAlly(caster) && caster.Position.Get2DDistance(character.Position) <= EffectRange).ToList();

			if (!allies.Contains(caster))
				allies.Add(caster);

			foreach (var ally in allies)
				ally.StartBuff(BuffId.KaguraDance_Buff, skill.Level, damageBonus, BuffDuration, caster, skill.Id);

			if (!caster.TryGetActiveAbilityLevel(AbilityId.Miko8, out var nightingaleLevel) || nightingaleLevel <= 0)
				return;

			nightingaleLevel = Math.Min(nightingaleLevel, 5);
			var area = new Circle(caster.Position, EffectRange);
			var enemies = caster.Map.GetAttackableEnemiesIn(caster, area).Where(enemy => enemy != null && !enemy.IsDead).ToList();

			foreach (var enemy in enemies)
				enemy.StartBuff(BuffId.Kagura_Debuff, nightingaleLevel, 0f, BuffDuration, caster, skill.Id);
		}

		private void ExecuteKenHit(Skill skill, Character caster)
		{
			var area = new Circle(caster.Position, EffectRange);
			var targets = caster.Map.GetAttackableEnemiesIn(caster, area).Where(target => target != null && !target.IsDead).OrderBy(target => caster.Position.Get2DDistance(target.Position)).Take(MaximumTargets).ToList();

			foreach (var target in targets)
			{
				var skillHitResult = SCR_SkillHit(caster, target, skill);

				if (skillHitResult.Result == HitResultType.Dodge)
					continue;

				this.ApplyKenEnhance(caster, skillHitResult);
				target.TakeDamage(skillHitResult.Damage, caster);
			}
		}

		private void ApplyKenEnhance(Character character, SkillHitResult skillHitResult)
		{
			if (!character.Abilities.TryGet(AbilityId.Miko15, out var ability) || !ability.Active)
				return;

			var abilityLevel = Math.Min(ability.Level, 100);
			var enhanceRate = abilityLevel * EnhancePerLevel;

			if (abilityLevel >= 100)
				enhanceRate += 0.10f;

			skillHitResult.Damage *= 1f + enhanceRate;
		}
	}
}
