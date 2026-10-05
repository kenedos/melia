using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Swordsmen.Templar
{
	[Package("laima")]
	[SkillHandler(SkillId.Templer_MoraleBanner)]
	public class Templer_MoraleBannerOverride : ITargetSkillHandler, IGroundSkillHandler, IMeleeGroundSkillHandler, IDynamicCasted, ICancelSkillHandler
	{
		private const string BannerClassName = "pcskill_morale_banner";
		private const float BannerDurationSeconds = 30f;
		private const float BannerEffectRange = 150f;
		private const float MinimumCriticalRateBonus = 0.05f;
		private const float MaximumCriticalRateBonus = 0.10f;
		private const float EnhancePerLevel = 0.005f;
		private const float MaximumEnhanceLevelBonus = 0.10f;
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 10;
		private const int MaximumEnhanceLevel = 100;
		private static readonly TimeSpan InstallationDelay = TimeSpan.FromMilliseconds(200);
		private static readonly TimeSpan AuraUpdateInterval = TimeSpan.FromSeconds(1);
		private static readonly TimeSpan AuraBuffDuration = TimeSpan.FromSeconds(30);
		private static readonly object BannerLock = new();
		private static readonly Dictionary<int, BannerState> ActiveBanners = new();

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			StopSkill(caster);
		}

		public void Handle(Skill skill, ICombatEntity caster)
		{
			StopSkill(caster);
		}

		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (caster == null)
				return;

			this.Cast(skill, caster, caster.Position, target?.Position ?? caster.Position);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			this.Cast(skill, caster, originPos, farPos);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IList<ICombatEntity> targets)
		{
			this.Cast(skill, caster, originPos, farPos);
		}

		private void Cast(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			if (caster is not Character character || character.IsDead || character.Map == null)
			{
				StopSkill(caster);
				return;
			}

			if (!character.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				StopSkill(character);
				return;
			}

			try
			{
				var targetPosition = this.GetTargetPosition(skill, farPos);
				var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
				var forceId = ForceId.GetNew();

				skill.IncreaseOverheat();
				character.SetAttackState(true);
				character.TurnTowards(targetPosition);

				Send.ZC_SKILL_READY(character, skill, skillHandle, originPos, targetPosition);
				Send.ZC_NORMAL.UpdateSkillEffect(character, 0, originPos, character.Direction, Position.Zero);
				Send.ZC_SKILL_MELEE_GROUND(character, skill, targetPosition, forceId, null);

				skill.Run(this.InstallBanner(character, skill, targetPosition));
			}
			catch
			{
				StopSkill(character);
				throw;
			}
		}

		private async Task InstallBanner(Character character, Skill skill, Position position)
		{
			try
			{
				await skill.Wait(InstallationDelay);

				if (character.IsDead || character.Map == null)
					return;

				var banner = MonsterSkillCreateMob(skill, character, BannerClassName, position, 0f, "Flag of Morale", "None", 0, BannerDurationSeconds, "None", "WlkMSPD#0#RunMSPD#0");

				if (banner == null)
					return;

				var criticalRateBonus = this.GetCriticalRateBonus(character, skill);
				var state = new BannerState(banner);
				BannerState previousState = null;

				lock (BannerLock)
				{
					if (ActiveBanners.TryGetValue(character.Handle, out previousState))
						ActiveBanners.Remove(character.Handle);

					ActiveBanners[character.Handle] = state;
				}

				if (previousState != null)
					this.RemoveBanner(previousState);

				this.ApplyAura(character, skill.Level, skill.Id, banner.Position, criticalRateBonus);

				_ = this.RunBannerAura(character, skill.Level, skill.Id, criticalRateBonus, state);
			}
			finally
			{
				StopSkill(character);
			}
		}

		private async Task RunBannerAura(Character character, int skillLevel, SkillId skillId, float criticalRateBonus, BannerState state)
		{
			try
			{
				var expirationTime = DateTime.UtcNow.AddSeconds(BannerDurationSeconds);

				while (DateTime.UtcNow < expirationTime && !state.Cancellation.IsCancellationRequested)
				{
					if (character.IsDead || character.Map == null || state.Banner == null || state.Banner.IsDead || state.Banner.Map != character.Map)
						break;

					this.ApplyAura(character, skillLevel, skillId, state.Banner.Position, criticalRateBonus);
					await Task.Delay(AuraUpdateInterval, state.Cancellation.Token);
				}
			}
			catch (OperationCanceledException)
			{
			}
			finally
			{
				var ownsActiveSlot = false;

				lock (BannerLock)
				{
					if (ActiveBanners.TryGetValue(character.Handle, out var activeState) && ReferenceEquals(activeState, state))
					{
						ActiveBanners.Remove(character.Handle);
						ownsActiveSlot = true;
					}
				}

				if (ownsActiveSlot)
					this.RemoveBanner(state);
			}
		}

		private void ApplyAura(Character character, int skillLevel, SkillId skillId, Position bannerPosition, float criticalRateBonus)
		{
			var recipients = new List<Character> { character };
			var party = character.Connection?.Party;

			if (party != null)
			{
				recipients.AddRange(party.GetPartyMembers()
					.Where(member => member != null && member != character));
			}

			foreach (var ally in recipients
				.Where(ally => !ally.IsDead)
				.Where(ally => ally.Map == character.Map)
				.Where(ally => ally.Layer == character.Layer)
				.Where(ally => bannerPosition.Get2DDistance(ally.Position) <= BannerEffectRange)
				.Distinct())
			{
				ally.StartBuff(BuffId.Templar_Enhancement_Buff, skillLevel, criticalRateBonus, AuraBuffDuration, character, skillId);
			}
		}

		private float GetCriticalRateBonus(Character character, Skill skill)
		{
			var skillLevel = Math.Clamp(skill.Level, MinimumSkillLevel, MaximumSkillLevel);
			var bonusPerLevel = (MaximumCriticalRateBonus - MinimumCriticalRateBonus) / (MaximumSkillLevel - MinimumSkillLevel);
			var criticalRateBonus = MinimumCriticalRateBonus + (skillLevel - MinimumSkillLevel) * bonusPerLevel;

			if (!character.Abilities.TryGet(AbilityId.Templar15, out var ability) || !ability.Active)
				return criticalRateBonus;

			var abilityLevel = Math.Clamp(ability.Level, 0, MaximumEnhanceLevel);
			var enhanceRate = abilityLevel * EnhancePerLevel;

			if (abilityLevel >= MaximumEnhanceLevel)
				enhanceRate += MaximumEnhanceLevelBonus;

			return criticalRateBonus * (1f + enhanceRate);
		}

		private Position GetTargetPosition(Skill skill, Position farPos)
		{
			if (skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPosition))
				return targetPosition;

			return farPos;
		}

		private void RemoveBanner(BannerState state)
		{
			if (state == null)
				return;

			state.Cancellation.Cancel();

			if (state.Banner?.Map != null)
				state.Banner.Map.RemoveMonster(state.Banner);
		}

		private static void StopSkill(ICombatEntity caster)
		{
			if (caster == null)
				return;

			caster.SetAttackState(false);
			Send.ZC_SKILL_DISABLE(caster);
		}

		private sealed class BannerState
		{
			public Mob Banner { get; }
			public CancellationTokenSource Cancellation { get; } = new();

			public BannerState(Mob banner)
			{
				this.Banner = banner;
			}
		}
	}
}
