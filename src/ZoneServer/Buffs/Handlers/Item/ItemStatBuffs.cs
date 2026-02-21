using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for ItemDEFUP, which increases Defense. The bonus amount is passed as NumArg1.
	/// </summary>
	[BuffHandler(BuffId.ItemDEFUP)]
	public class ItemDEFUP : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.DEF_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.DEF_BM, -bonus);
		}
	}

	/// <summary>
	/// Handle for ItemSTRUP, which increases Strength. The bonus amount is passed as NumArg1.
	/// </summary>
	[BuffHandler(BuffId.ItemSTRUP)]
	public class ItemSTRUP : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.STR_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.STR_BM, -bonus);
		}
	}

	/// <summary>
	/// Handle for ItemCONUP, which increases Constitution. The bonus amount is passed as NumArg1.
	/// </summary>
	[BuffHandler(BuffId.ItemCONUP)]
	public class ItemCONUP : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.CON_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.CON_BM, -bonus);
		}
	}

	/// <summary>
	/// Handle for ItemAGIUP, which increases Dexterity. The bonus amount is passed as NumArg1.
	/// </summary>
	[BuffHandler(BuffId.ItemAGIUP)]
	public class ItemAGIUP : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			// Note: The buff is named AGIUP but modifies the DEX property, matching the original script.
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.DEX_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.DEX_BM, -bonus);
		}
	}

	/// <summary>
	/// Handle for ItemINTUP, which increases Intelligence. The bonus amount is passed as NumArg1.
	/// </summary>
	[BuffHandler(BuffId.ItemINTUP)]
	public class ItemINTUP : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.INT_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			var bonus = buff.NumArg1;
			buff.Target.Properties.Modify(PropertyName.INT_BM, -bonus);
		}
	}
}
