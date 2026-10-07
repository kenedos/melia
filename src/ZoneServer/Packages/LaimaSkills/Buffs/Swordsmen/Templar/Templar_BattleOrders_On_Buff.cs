using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.Templar
{
	/// <summary>
	/// Handler for the Battle Orders toggle on the Templar, which drains
	/// SP every second and keeps nearby party members under the orders.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.BattleOrders_On_Buff)]
	public class Templar_BattleOrders_On_BuffOverride : BuffHandler
	{
		private const float AuraRange = 100f;
		private const float SpRatePerSecond = 0.01f;
		private static readonly TimeSpan OrdersDuration = TimeSpan.FromSeconds(2);

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Target.StopBuff(BuffId.AdvancedOrders_On_Buff);
			buff.SetUpdateTime(1000);
		}

		public override void WhileActive(Buff buff)
		{
			var templar = buff.Target;
			var spCost = templar.Properties.GetFloat(PropertyName.MSP) * SpRatePerSecond;

			if (templar.IsDead || !templar.TrySpendSp(spCost))
			{
				templar.StopBuff(BuffId.BattleOrders_On_Buff);
				return;
			}

			foreach (var ally in PartySkillHelper.GetAlliesInRange(templar, templar.Position, AuraRange))
			{
				ally.StopBuff(BuffId.AdvancedOrders_Buff);
				ally.StartBuff(BuffId.BattleOrders_Buff, buff.NumArg1, 0, OrdersDuration, templar, buff.SkillId);
			}

			TemplarSkillHelper.ProgressUplift(templar);
		}
	}
}
