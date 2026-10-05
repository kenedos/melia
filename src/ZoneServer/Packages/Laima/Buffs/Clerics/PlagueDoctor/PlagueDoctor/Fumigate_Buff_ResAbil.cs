using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Yggdrasil.Util;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.PlagueDoctor
{
	/// <summary>
	/// Fumigate: Purification.
	/// Increases Poison Resistance and grants 50% resistance against removable debuffs.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Fumigate_Buff_ResAbil)]
	public class Fumigate_Buff_ResAbilOverride : BuffHandler
	{
		private const float PoisonResistanceRatePerLevel = 0.10f;
		private const int RemovableDebuffResistanceChance = 50;
		private const string PoisonResistanceVariable = "PlagueDoctor.Fumigate.Purification.PoisonResistance";
		private bool _removingDebuff;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (activationType == ActivationType.Start)
			{
				var poisonResistance = buff.Target.Properties.GetFloat(PropertyName.ResPoison);
				var resistanceBonus = poisonResistance * buff.NumArg1 * PoisonResistanceRatePerLevel;

				buff.Vars.Set(PoisonResistanceVariable, resistanceBonus);
				buff.Target.Properties.Modify(PropertyName.ResPoison_BM, resistanceBonus);

				var buffComponent = buff.Target.Components.Get<BuffComponent>();

				if (buffComponent != null)
					buffComponent.BuffStarted += this.OnBuffStarted;
			}
		}

		public override void OnEnd(Buff buff)
		{
			var buffComponent = buff.Target.Components.Get<BuffComponent>();

			if (buffComponent != null)
				buffComponent.BuffStarted -= this.OnBuffStarted;

			var resistanceBonus = buff.Vars.GetFloat(PoisonResistanceVariable);

			if (resistanceBonus != 0)
				buff.Target.Properties.Modify(PropertyName.ResPoison_BM, -resistanceBonus);
		}

		private void OnBuffStarted(ICombatEntity entity, Buff appliedBuff)
		{
			if (_removingDebuff || appliedBuff == null)
				return;

			if (!entity.TryGetBuff(BuffId.Fumigate_Buff_ResAbil, out _))
				return;

			if (appliedBuff.Data.Type != BuffType.Debuff || !appliedBuff.Data.RemoveBySkill)
				return;

			var buffComponent = entity.Components.Get<BuffComponent>();

			if (buffComponent == null || !entity.IsBuffActive(appliedBuff.Id))
				return;

			if (RandomProvider.Get().Next(100) >= RemovableDebuffResistanceChance)
				return;

			_removingDebuff = true;

			try
			{
				buffComponent.Remove(appliedBuff.Id);
			}
			finally
			{
				_removingDebuff = false;
			}
		}
	}
}
