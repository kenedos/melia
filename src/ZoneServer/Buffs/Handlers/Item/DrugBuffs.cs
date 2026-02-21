using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.Buffs.Handlers.Common;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for Drug_Haste, which increases Movement Speed.
	/// The bonus amount is passed as NumArg1.
	/// </summary>
	[BuffHandler(BuffId.Drug_Haste)]
	public class Drug_Haste : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.MSPD_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.MSPD_BM, -bonus);
		}
	}

	/// <summary>
	/// Handle for Drug_RedOx, which rapidly regenerates Stamina.
	/// </summary>
	[BuffHandler(BuffId.Drug_RedOx)]
	public class Drug_RedOx : BuffHandler
	{
		private const float StaminaRegen = 99999f;

		public override void WhileActive(Buff buff)
		{
			if (buff.Target is Character character)
			{
				character.Properties.Modify(PropertyName.Stamina, StaminaRegen);
			}
		}
	}

	/// <summary>
	/// Base class for HP/SP healing drugs to share calculation logic.
	/// </summary>
	public abstract class Drug_Heal_Base : BuffHandler
	{
		protected float CalculateHpHeal(Buff buff, float baseHeal)
		{
			var target = buff.Target;
			var healAmount = baseHeal;

			if (target.IsBuffActive(BuffId.Restoration_Buff))
			{
				healAmount = (float)Math.Floor(healAmount * 1.3f);
			}

			var hpPotionBonus = target.Properties.GetFloat(PropertyName.HPPotion_BM);
			if (hpPotionBonus > 0)
			{
				healAmount = (float)Math.Floor(healAmount * (1 + hpPotionBonus / 100f));
			}

			return healAmount;
		}

		protected float CalculateSpHeal(Buff buff, float baseHeal)
		{
			var target = buff.Target;
			var healAmount = baseHeal;

			var spPotionBonus = target.Properties.GetFloat(PropertyName.SPPotion_BM);
			if (spPotionBonus > 0)
			{
				healAmount = (float)Math.Floor(healAmount * (1 + spPotionBonus / 100f));
			}

			return healAmount;
		}
	}

	/// <summary>
	/// Handle for Drug_HealHP_Dot, which has identical logic to Drug_HealHP.
	/// </summary>
	[BuffHandler(BuffId.Drug_HealHP_Dot)]
	public class Drug_HealHP_Dot : Drug_HealHP { }

	/// <summary>
	/// Handle for Drug_HealSP_Dot, which has identical logic to Drug_HealSP.
	/// </summary>
	[BuffHandler(BuffId.Drug_HealSP_Dot)]
	public class Drug_HealSP_Dot : Drug_HealSP { }

	/// <summary>
	/// Handle for Drug_HealHPSP_Dot, which heals both HP and SP over time.
	/// </summary>
	[BuffHandler(BuffId.Drug_HealHPSP_Dot)]
	public class Drug_HealHPSP_Dot : Drug_Heal_Base
	{
		private void HealTick(Buff buff)
		{
			var target = buff.Target;
			var baseHpHeal = buff.NumArg1;
			var baseSpHeal = (buff.NumArg2 > 0) ? buff.NumArg2 : buff.NumArg1;

			var hpHeal = CalculateHpHeal(buff, baseHpHeal);
			var spHeal = CalculateSpHeal(buff, baseSpHeal);

			target.Heal(hpHeal, 0);
			if (target is Character character)
			{
				character.Heal(0, spHeal);
			}
		}

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			HealTick(buff);
		}

		public override void WhileActive(Buff buff)
		{
			HealTick(buff);
		}
	}

	/// <summary>
	/// Handle for Drug_BLK, which increases Block. The bonus amount is passed as NumArg1.
	/// </summary>
	[BuffHandler(BuffId.Drug_BLK)]
	public class Drug_BLK : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.BLK_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.BLK_BM, -bonus);
		}
	}

	/// <summary>
	/// Handle for Drug_CRTATK, which increases Critical Attack. The bonus amount is passed as NumArg1.
	/// </summary>
	[BuffHandler(BuffId.Drug_CRTATK)]
	public class Drug_CRTATK : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.CRTATK_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.CRTATK_BM, -bonus);
		}
	}

	/// <summary>
	/// Handle for Drug_MHR, which increases Magic Hit Rate. The bonus amount is passed as NumArg1.
	/// </summary>
	[BuffHandler(BuffId.Drug_MHR)]
	public class Drug_MHR : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.MHR_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.MHR_BM, -bonus);
		}
	}
}
