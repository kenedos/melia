using System;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Scouts.Squire
{
	/// <summary>
	/// Handler for the Squire skill Deadly Combo.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Squire_DeadlyCombo)]
	public class Squire_DeadlyComboOverride : IGroundSkillHandler
	{
		private const int HitDelay = 250;
		private const int AniTime = 50;
		private const int HitCount = 3;

		// Durability is stored times 100, so this is the one point the
		// tooltip means.
		private const int DurabilityCost = 100;
		private const float DurabilityThreshold = 0.25f;
		private const float WornEdgeDamageBonus = 0.5f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			var modifier = SkillModifier.MultiHit(HitCount);

			if (this.TrySpendEdge(caster))
				modifier.DamageMultiplier += WornEdgeDamageBonus;

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos, modifier));
		}

		/// <summary>
		/// Spends a point of the caster's weapon on the swing, and returns
		/// whether it had enough left to give.
		/// </summary>
		/// <param name="caster"></param>
		private bool TrySpendEdge(ICombatEntity caster)
		{
			if (caster is not Character character)
				return false;

			if (!caster.TryGetEquipItem(EquipSlot.RightHand, out var weapon) || weapon.MaxDurability <= 0)
				return false;

			if (weapon.Durability < weapon.MaxDurability * DurabilityThreshold)
				return false;

			weapon.ModifyDurability(character, -DurabilityCost);

			return true;
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos, SkillModifier modifier)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 50, width: 50, angle: 170f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			await SkillAttack(caster, skill, splashArea, HitDelay, AniTime, skillModifier: modifier);
		}
	}
}
