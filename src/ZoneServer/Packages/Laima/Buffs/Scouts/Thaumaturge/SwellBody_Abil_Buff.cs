using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Buffs.Handlers.Scouts.Thaumaturge
{
	[Package("laima")]
	[BuffHandler(BuffId.SwellBody_Abil_Buff)]
	public class SwellBody_Abil_BuffOverride : BuffHandler
	{
		private const float SwellScale = 1.15f;
		private const string VarOriginalSize = "Melia.SwellBody.OriginalSize";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			if (buff.Target.IsBuffActive(BuffId.ShrinkBody_Debuff))
				buff.Target.RemoveBuff(BuffId.ShrinkBody_Debuff);

			if (!buff.Vars.Has(VarOriginalSize))
				buff.Vars.SetString(VarOriginalSize, target.EffectiveSize.ToString());

			switch (target.EffectiveSize)
			{
				case SizeType.M:
					target.Properties.SetString(PropertyName.Size, SizeType.L);
					(target as Mob)?.InvalidateSizeCache();
					target.Properties.SetFloat(PropertyName.Scale, SwellScale);
					Send.ZC_NORMAL.SetScale(target, 33, SwellScale, 0, 1, 1, 0);
					break;
				case SizeType.S:
					target.Properties.SetString(PropertyName.Size, SizeType.M);
					(target as Mob)?.InvalidateSizeCache();
					target.Properties.SetFloat(PropertyName.Scale, SwellScale);
					Send.ZC_NORMAL.SetScale(target, 33, SwellScale, 0, 1, 1, 0);
					break;
			}

			var skillLevel = buff.NumArg1;
			var flatMhpFromInt = buff.NumArg2;

			var mhpRateBonus = Math.Min(0.30f, (10f + skillLevel) / 100f);

			AddPropertyModifier(buff, target, PropertyName.MHP_RATE_BM, mhpRateBonus);
			AddPropertyModifier(buff, target, PropertyName.MHP_BM, flatMhpFromInt);
			target.Properties.InvalidateAll();
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			if (buff.Vars.TryGetString(VarOriginalSize, out var originalSizeStr) && Enum.TryParse<SizeType>(originalSizeStr, out var originalSize))
				target.Properties.SetString(PropertyName.Size, originalSize);
			else
				target.Properties.SetString(PropertyName.Size, target.EffectiveSize);

			(target as Mob)?.InvalidateSizeCache();
			target.Properties.SetFloat(PropertyName.Scale, 1f);
			Send.ZC_NORMAL.SetScale(target, 33, 1f, 0, 1, 1, 0);

			RemovePropertyModifier(buff, target, PropertyName.MHP_RATE_BM);
			RemovePropertyModifier(buff, target, PropertyName.MHP_BM);
			target.Properties.InvalidateAll();
		}
	}
}
