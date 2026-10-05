using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Abilities.Handlers.Archers.Cannoneer;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Handlers.Archers.Cannoneer
{
	/// <summary>

	/// Bazooka

	/// Toggle que fortalece Cannon Shot e Cannon Barrage.

	/// </summary>

	[Package("laima")]
	[SkillHandler(SkillId.Cannoneer_Bazooka)]
	public class Cannoneer_BazookaOverride : ISelfSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction direction)
		{
			if (caster is not Character character)
				return;

			if (character.IsBuffActive(BuffId.Bazooka_Buff))
			{
				character.RemoveBuff(BuffId.Bazooka_Buff);
				Send.ZC_SKILL_MELEE_TARGET(character, skill, character);
				return;
			}

			if (!character.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			character.SetAttackState(true);

			Send.ZC_SKILL_READY(character, skill, 1, originPos, Position.Zero);
			Send.ZC_NORMAL.UpdateSkillEffect(character, character.Handle, originPos, direction, Position.Zero);
			Send.ZC_SKILL_MELEE_TARGET(character, skill, character);

			character.StartBuff(BuffId.Bazooka_Buff, Math.Clamp(skill.Level, 1, 10), 0f, TimeSpan.Zero, character, skill.Id);
			character.SetAttackState(false);
		}
	}

	/// <summary>

	/// Centraliza os modificadores da Bazooka usados por Cannon Shot

	/// e Cannon Barrage.

	/// </summary>

	public static class CannoneerBazookaHelper
	{
		public const float EnhancedSkillRange = 200f;
		public const float MinimumAttackDistance = 40f;
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 10;
		private const int MinimumAoeBonus = 4;
		private const float MinimumFinalDamageBonus = 1f;
		private const float FinalDamageBonusPerLevel = 0.10f;

		public static bool IsActive(Character character)
		{
			return character != null && character.IsBuffActive(BuffId.Bazooka_Buff);
		}

		public static int GetSkillLevel(Character character)
		{
			if (character == null || !character.TryGetBuff(BuffId.Bazooka_Buff, out var buff))
				return 0;

			return Math.Clamp((int)buff.NumArg1, MinimumSkillLevel, MaximumSkillLevel);
		}

		public static float GetMaximumRange(Character character, float normalRange)
		{
			return IsActive(character) ? EnhancedSkillRange : normalRange;
		}

		public static int GetAdditionalAoeAttackRatio(Character character)
		{
			var skillLevel = GetSkillLevel(character);
			return skillLevel <= 0 ? 0 : MinimumAoeBonus + skillLevel - 1;
		}

		public static float GetFinalDamageMultiplier(Character character)
		{
			var skillLevel = GetSkillLevel(character);

			if (skillLevel <= 0)
				return 1f;

			var finalDamageBonus = MinimumFinalDamageBonus + (skillLevel - 1) * FinalDamageBonusPerLevel;
			return 1f + finalDamageBonus;
		}

		public static bool IsInsideAllowedDistance(Character character, Position originPosition, Position targetPosition, float normalMaximumRange)
		{
			var distance = originPosition.Get2DDistance(targetPosition);
			var maximumRange = GetMaximumRange(character, normalMaximumRange);

			if (distance > maximumRange)
				return false;

			if (!IsActive(character))
				return true;

			if (Cannoneer_BazookaVeteranMercenaryAbility.IsActive(character))
				return true;

			return distance >= MinimumAttackDistance;
		}

		public static void ApplyCooldownMultiplier(Character character, Skill skill)
		{
			if (character == null || skill == null || !IsActive(character) || !skill.IsOnCooldown)
				return;

			skill.StartCooldown(skill.Properties.CoolDown * 2);
		}
	}
}
