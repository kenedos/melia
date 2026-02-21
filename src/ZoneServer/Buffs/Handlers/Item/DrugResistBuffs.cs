using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handler for Drug_ResFire, which increases Fire Resistance.
	/// The bonus amount is passed as NumArg1.
	/// </summary>
	[BuffHandler(BuffId.Drug_ResFire)]
	public class Drug_ResFire : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.ResFire_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.ResFire_BM, -bonus);
		}
	}

	/// <summary>
	/// Handler for Drug_ResIce, which increases Ice Resistance.
	/// The bonus amount is passed as NumArg1.
	/// </summary>
	[BuffHandler(BuffId.Drug_ResIce)]
	public class Drug_ResIce : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.ResIce_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.ResIce_BM, -bonus);
		}
	}

	/// <summary>
	/// Handler for Drug_ResPoison, which increases Poison Resistance.
	/// The bonus amount is passed as NumArg1.
	/// </summary>
	[BuffHandler(BuffId.Drug_ResPoison)]
	public class Drug_ResPoison : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.ResPoison_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.ResPoison_BM, -bonus);
		}
	}

	/// <summary>
	/// Handler for Drug_ResEarth, which increases Earth Resistance.
	/// The bonus amount is passed as NumArg1.
	/// </summary>
	[BuffHandler(BuffId.Drug_ResEarth)]
	public class Drug_ResEarth : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.ResEarth_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.ResEarth_BM, -bonus);
		}
	}

	/// <summary>
	/// Handler for Drug_ResLightning, which increases Lightning Resistance.
	/// The bonus amount is passed as NumArg1.
	/// </summary>
	[BuffHandler(BuffId.Drug_ResLightning)]
	public class Drug_ResLightning : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.ResLightning_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.ResLightning_BM, -bonus);
		}
	}
}
