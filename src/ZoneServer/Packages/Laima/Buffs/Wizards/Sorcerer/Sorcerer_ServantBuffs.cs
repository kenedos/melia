using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Wizards.Sorcerer
{
	/// <summary>
	/// Handler for the servant's AoE Attack Ratio buff.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.ServantSR_Buff)]
	public class Sorcerer_ServantSR_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.SR_BM, buff.NumArg1);
			buff.Target.InvalidateProperties();
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.SR_BM);
			buff.Target.InvalidateProperties();
		}
	}

	/// <summary>
	/// Handler for the servant's SP recovery buff, which shortens the
	/// target's SP recovery interval.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.ServantSP_Buff)]
	public class Sorcerer_ServantSP_BuffOverride : BuffHandler
	{
		private const float ReductionBase = 18f;
		private const float ReductionPerLevel = 3f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			var rate = (ReductionBase + ReductionPerLevel * buff.NumArg1) / 100f;
			var recoveryTime = target.Properties.GetFloat(PropertyName.RSPTIME);

			AddPropertyModifier(buff, target, PropertyName.RSPTIME_BM, -MathF.Floor(recoveryTime * rate));
			target.InvalidateProperties();
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.RSPTIME_BM);
			buff.Target.InvalidateProperties();
		}
	}

	/// <summary>
	/// Handler for the servant's stamina recovery buff.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.ServantSTA_Buff)]
	public class Sorcerer_ServantSTA_BuffOverride : BuffHandler
	{
		private const float StaminaPerLevel = 0.1f;

		public override void WhileActive(Buff buff)
		{
			if (buff.Target is not Character character)
				return;

			var amount = (int)MathF.Ceiling(buff.NumArg1 * StaminaPerLevel);
			if (amount > 0)
				character.ModifyStamina(amount);
		}
	}

	/// <summary>
	/// Handler for the servant's magic defense buff.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.ServantMDEF_Buff)]
	public class Sorcerer_ServantMDEF_BuffOverride : BuffHandler
	{
		private const float DefensePerLevel = 10f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.MDEF_BM, buff.NumArg1 * DefensePerLevel);
			buff.Target.InvalidateProperties();
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MDEF_BM);
			buff.Target.InvalidateProperties();
		}
	}

	/// <summary>
	/// Handler for the servant's additional damage buff.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.ServantDARKATK_Buff)]
	public class Sorcerer_ServantDARKATK_BuffOverride : BuffHandler
	{
		private const float DamagePerLevel = 400f;
		private const float DamagePerSpr = 1.5f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			var spr = target.Properties.GetFloat(PropertyName.MNA);
			var bonus = MathF.Floor(buff.NumArg1 * DamagePerLevel + spr * DamagePerSpr);

			AddPropertyModifier(buff, target, PropertyName.Add_Damage_Atk_BM, bonus);
			target.InvalidateProperties();
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.Add_Damage_Atk_BM);
			buff.Target.InvalidateProperties();
		}
	}
}
