using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Wizards.Sorcerer
{
	[Package("laima")]
	[BuffHandler(BuffId.ServantSP_Buff)]
	public class ServantSP_BuffOverride : BuffHandler
	{
		private const int UpdateInterval = 2000;
		private const float RecoveryRatePerLevel = 0.001f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.SetUpdateTime(UpdateInterval);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target.IsDead)
				return;

			var level = Math.Clamp((int)buff.NumArg1, 1, 10);
			var maxSp = buff.Target.Properties.GetFloat(PropertyName.MSP);
			var recovery = Math.Max(1f, maxSp * RecoveryRatePerLevel * level);
			buff.Target.Heal(0, recovery);
		}

		public override void OnEnd(Buff buff)
		{
		}
	}

	[Package("laima")]
	[BuffHandler(BuffId.ServantSR_Buff)]
	public class ServantSR_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var level = Math.Clamp((int)buff.NumArg1, 1, 10);
			AddPropertyModifier(buff, buff.Target, PropertyName.SR_BM, level);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.SR_BM);
		}
	}

	[Package("laima")]
	[BuffHandler(BuffId.ServantSTA_Buff)]
	public class ServantSTA_BuffOverride : BuffHandler
	{
		private const int UpdateInterval = 1500;
		private const int StaminaPerLevel = 1000;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.SetUpdateTime(UpdateInterval);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target is not Character character || character.IsDead)
				return;

			var level = Math.Clamp((int)buff.NumArg1, 1, 10);
			character.ModifyStamina(StaminaPerLevel * level);
		}

		public override void OnEnd(Buff buff)
		{
		}
	}

	[Package("laima")]
	[BuffHandler(BuffId.ServantMDEF_Buff)]
	public class ServantMDEF_BuffOverride : BuffHandler
	{
		private const float BonusRatePerLevel = 0.10f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var level = Math.Clamp((int)buff.NumArg1, 1, 10);
			var currentBonus = buff.Target.Properties.GetFloat(PropertyName.MDEF_BM);
			var baseMdef = Math.Max(0f, buff.Target.Properties.GetFloat(PropertyName.MDEF) - currentBonus);
			var bonus = baseMdef * BonusRatePerLevel * level;

			AddPropertyModifier(buff, buff.Target, PropertyName.MDEF_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MDEF_BM);
		}
	}

	[Package("laima")]
	[BuffHandler(BuffId.ServantDARKATK_Buff)]
	public class ServantDARKATK_BuffOverride : BuffHandler
	{
		private const float FlatDarkDamagePerLevel = 50f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var level = Math.Clamp((int)buff.NumArg1, 1, 10);
			var darkDamageBonus = FlatDarkDamagePerLevel * level;
			var propertyName = buff.Target is Character ? PropertyName.Dark_Atk_BM : PropertyName.ADD_DARK_BM;
			AddPropertyModifier(buff, buff.Target, propertyName, darkDamageBonus);
		}

		public override void OnEnd(Buff buff)
		{
			var propertyName = buff.Target is Character ? PropertyName.Dark_Atk_BM : PropertyName.ADD_DARK_BM;
			RemovePropertyModifier(buff, buff.Target, propertyName);
		}
	}
}
