using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.Templar
{
	/// <summary>
	/// Handler for the Advanced Orders toggle on the Templar, which drains
	/// SP every second and keeps nearby party members under the orders.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.AdvancedOrders_On_Buff)]
	public class Templar_AdvancedOrders_On_BuffOverride : BuffHandler
	{
		private const float AuraRange = 100f;
		private const float SpRatePerSecond = 0.01f;
		private const float BindSpRate = 0.50f;
		private static readonly TimeSpan OrdersDuration = TimeSpan.FromSeconds(2);
		private static readonly TimeSpan BindDuration = TimeSpan.FromSeconds(3);

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Target.StopBuff(BuffId.BattleOrders_On_Buff);
			buff.SetUpdateTime(1000);
		}

		public override void WhileActive(Buff buff)
		{
			var templar = buff.Target;

			if (templar.IsDead || !this.TrySpendUpkeep(templar))
			{
				templar.StopBuff(BuffId.AdvancedOrders_On_Buff);
				return;
			}

			foreach (var ally in PartySkillHelper.GetAlliesInRange(templar, templar.Position, AuraRange))
			{
				ally.StopBuff(BuffId.BattleOrders_Buff);
				ally.StartBuff(BuffId.AdvancedOrders_Buff, buff.NumArg1, 0, OrdersDuration, templar, buff.SkillId);
			}

			if (templar.IsAbilityActive(AbilityId.Templar12))
			{
				var maxTargets = (int)GetCaptionRatio(buff, 3);
				var enemies = templar.Map.GetAttackableEnemiesInPosition(templar, templar.Position, AuraRange).Take(maxTargets);

				foreach (var enemy in enemies)
					enemy.StartBuff(BuffId.AdvancedOrders_Debuff, buff.NumArg1, 0, BindDuration, templar, buff.SkillId);
			}

			TemplarSkillHelper.ProgressUplift(templar);
		}

		/// <summary>
		/// Spends the toggle's SP upkeep, returning false if the Templar
		/// can't pay it.
		/// </summary>
		/// <param name="templar"></param>
		/// <returns></returns>
		private bool TrySpendUpkeep(ICombatEntity templar)
		{
			if (templar is not Character character)
				return true;

			var spCost = character.Properties.GetFloat(PropertyName.MSP) * SpRatePerSecond;
			if (character.IsAbilityActive(AbilityId.Templar12))
				spCost *= 1f + BindSpRate;

			return character.TrySpendSp(spCost);
		}
	}
}
