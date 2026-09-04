using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Items;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Clerics.Pardoner
{
	/// <summary>
	/// Handler for the Pardoner skill Dekatos.
	/// Attacks enemies in front of the caster with money collected as
	/// offerings, spending silver and drawing most of its damage from the
	/// silver the caster is carrying.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Pardoner_Dekatos)]
	public class Pardoner_DekatosOverride : IGroundSkillHandler
	{
		private const int HitDelay = 500;
		private const int AniTime = 300;
		private const int MaxTargets = 3;
		private const float SplashLength = 15;
		private const float SplashRadius = 45;
		private const int SilverCostPerLevel = 1000;
		private const int MinSilverStacks = 7;
		private const int MaxSilverStacks = 10;

		/// <summary>
		/// Silver amount the attack bonus saturates towards, so carrying
		/// several times as much barely moves the number.
		/// </summary>
		private const float SilverSoftCap = 1_000_000f;

		/// <summary>
		/// Magic attack granted per cube root of the caster's effective
		/// silver.
		/// </summary>
		private const float SilverAttackScale = 30f;

		/// <summary>
		/// Share of the caster's own magic attack the skill keeps, so gear
		/// and INT barely move it and the purse is what the hit is made of.
		/// </summary>
		private const float RetainedAttackShare = 0.25f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!TryGetSilverCost(caster, skill, out var silverCost))
			{
				caster.ServerMessage(Localization.Get("Not enough silver."));
				Send.ZC_SKILL_DISABLE(caster);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var spentSilver = SpendSilver(caster, silverCost);

			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.Attack(skill, caster, originPos, farPos, spentSilver));
		}

		/// <summary>
		/// Strikes the enemies in front of the caster and returns their
		/// share of the spent silver.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		/// <param name="spentSilver"></param>
		private async Task Attack(Skill skill, ICombatEntity caster, Position originPos, Position farPos, int spentSilver)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: SplashLength, width: SplashRadius);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);

			await skill.Wait(TimeSpan.FromMilliseconds(AniTime));

			if (caster.IsDead)
				return;

			var modifier = new SkillModifier();
			modifier.BonusMAtk += GetSilverAttack(caster) - GetWithheldAttack(caster);

			var refundPerTarget = spentSilver / MaxTargets;
			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea, MaxTargets);
			var hits = new List<SkillHitInfo>();

			foreach (var target in targets)
			{
				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.FromMilliseconds(HitDelay), TimeSpan.Zero);
				skillHit.HitEffect = HitEffect.Impact;
				hits.Add(skillHit);

				DropSilver(caster, target, refundPerTarget);
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}

		/// <summary>
		/// Returns whether the caster can pay the skill's silver cost, and
		/// what that cost comes to.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="skill"></param>
		/// <param name="cost"></param>
		private static bool TryGetSilverCost(ICombatEntity caster, Skill skill, out int cost)
		{
			cost = 0;

			if (caster is not Character character)
				return true;

			cost = SilverCostPerLevel * skill.Level;

			return character.Inventory.CountItem(ItemId.Silver) >= cost;
		}

		/// <summary>
		/// Removes the skill's silver cost from the caster and returns how
		/// much was actually removed.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="cost"></param>
		private static int SpendSilver(ICombatEntity caster, int cost)
		{
			if (cost <= 0 || caster is not Character character)
				return 0;

			return character.Inventory.Remove(ItemId.Silver, cost, InventoryItemRemoveMsg.Given);
		}

		/// <summary>
		/// Returns the flat magic attack the silver in the caster's
		/// inventory is worth.
		/// </summary>
		/// <param name="caster"></param>
		private static float GetSilverAttack(ICombatEntity caster)
		{
			if (caster is not Character character)
				return 0;

			var silver = character.Inventory.CountItem(ItemId.Silver);
			if (silver <= 0)
				return 0;

			var effective = SilverSoftCap * (1 - (float)Math.Exp(-silver / SilverSoftCap));

			return SilverAttackScale * (float)Math.Cbrt(effective);
		}

		/// <summary>
		/// Returns the part of the caster's own magic attack the skill does
		/// not use.
		/// </summary>
		/// <param name="caster"></param>
		private static float GetWithheldAttack(ICombatEntity caster)
		{
			return caster.Properties.GetFloat(PropertyName.MINMATK) * (1 - RetainedAttackShare);
		}

		/// <summary>
		/// Drops a hit target's share of the spent silver on the ground for
		/// the caster alone, if the caster is a Pardoner.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <param name="amount"></param>
		private static void DropSilver(ICombatEntity caster, ICombatEntity target, int amount)
		{
			if (amount <= 0 || caster is not Character character)
				return;

			if (!character.Components.TryGet<JobComponent>(out var jobs) || !jobs.Has(JobId.Pardoner))
				return;

			var stackCount = GameRandom.Get().Next(MinSilverStacks, MaxSilverStacks + 1);
			var stackAmount = Math.Max(1, amount / stackCount);

			for (var i = 0; i < stackCount; ++i)
			{
				var left = amount - stackAmount * i;
				if (left <= 0)
					break;

				var dropAmount = i == stackCount - 1 ? left : Math.Min(stackAmount, left);
				var item = new Item(ItemId.Silver, dropAmount);
				item.SetLootProtection(character, TimeSpan.FromSeconds(ZoneServer.Instance.Conf.World.LootPrectionSeconds));

				var direction = new Direction(GameRandom.Get().Next(0, 360));
				var dropRadius = ZoneServer.Instance.Conf.World.DropRadius;
				var distance = GameRandom.Get().Next(dropRadius / 2, dropRadius + 1);

				item.Drop(target.Map, target.Position, direction, distance, character.AccountObjectId, character.Layer);
			}
		}
	}
}
