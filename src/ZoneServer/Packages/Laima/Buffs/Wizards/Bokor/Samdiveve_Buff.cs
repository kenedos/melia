using System;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Buffs.Handlers.Wizards.Bokor
{
	/// <summary>
	/// Handler for the Samdiveve Buff, which increases summon movement speed
	/// and causes summons to apply Decomposition debuff to nearby enemies.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Samdiveve_Buff)]
	public class Samdiveve_BuffOverride : BuffHandler
	{
		private const float BaseBonus = 20;
		private const float BonusPerLevel = 3;
		private const float DecompositionRange = 100;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is not Summon)
				return;

			buff.SetUpdateTime(1000);

			var mspdBonus = this.GetMspdBonus(buff);
			AddPropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, mspdBonus);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is not Summon)
				return;

			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target is not Summon summon)
				return;

			var caster = buff.Caster as ICombatEntity;
			if (caster == null)
				return;

			var enemies = summon.Map.GetAttackableEnemiesInPosition(summon, summon.Position, DecompositionRange)
				.Where(e => !e.IsDead)
				.ToList();

			foreach (var enemy in enemies)
			{
				if (!enemy.IsBuffActive(BuffId.Decomposition_Debuff))
			{
				enemy.StartBuff(BuffId.Decomposition_Debuff, buff.NumArg1, 0f, TimeSpan.FromSeconds(10), summon);
			}
			}
		}

		private float GetMspdBonus(Buff buff)
		{
			var skillLevel = buff.NumArg1;
			return BaseBonus + skillLevel * BonusPerLevel;
		}
	}
}
