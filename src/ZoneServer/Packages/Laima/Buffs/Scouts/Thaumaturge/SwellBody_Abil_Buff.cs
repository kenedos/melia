using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Scouts.Thaumaturge
{
	[Package("laima")]
	[BuffHandler(BuffId.SwellBody_Abil_Buff)]
	public class SwellBody_Abil_BuffOverride : BuffHandler
	{
		private const float SwellScale = 1.15f;
		private const string VarMhpRateBonus = "Melia.SwellBody.MhpRate";
		private const string VarMhpFlatBonus = "Melia.SwellBody.MhpFlat";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			if (buff.Target.IsBuffActive(BuffId.ShrinkBody_Debuff))
				buff.Target.RemoveBuff(BuffId.ShrinkBody_Debuff);

			if (!buff.Vars.Has("Melia.SwellBody.Size"))
				buff.Vars.Set("Melia.SwellBody.Size", target.EffectiveSize);

			switch (target.EffectiveSize)
			{
				case SizeType.M:
					target.Properties.SetString(PropertyName.Size, SizeType.L);
					target.Properties.SetFloat(PropertyName.Scale, SwellScale);
					Send.ZC_NORMAL.SetScale(target, 33, SwellScale, 0, 1, 1, 0);
					break;
				case SizeType.S:
					target.Properties.SetString(PropertyName.Size, SizeType.M);
					target.Properties.SetFloat(PropertyName.Scale, SwellScale);
					Send.ZC_NORMAL.SetScale(target, 33, SwellScale, 0, 1, 1, 0);
					break;
			}

			var skillLevel = buff.NumArg1;
			var flatMhpFromInt = buff.NumArg2;

			var mhpRateBonus = Math.Min(0.30f, (10f + skillLevel) / 100f);

			buff.Vars.SetFloat(VarMhpRateBonus, mhpRateBonus);
			buff.Vars.SetFloat(VarMhpFlatBonus, flatMhpFromInt);

			target.Properties.Modify(PropertyName.MHP_RATE_BM, mhpRateBonus);
			target.Properties.Modify(PropertyName.MHP_BM, flatMhpFromInt);
			target.Properties.InvalidateAll();
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			var originalSize = buff.Vars.Get("Melia.SwellBody.Size", target.EffectiveSize);

			target.Properties.SetString(PropertyName.Size, originalSize);
			target.Properties.SetFloat(PropertyName.Scale, 1f);
			Send.ZC_NORMAL.SetScale(target, 33, 1f, 0, 1, 1, 0);

			if (buff.Vars.TryGetFloat(VarMhpRateBonus, out var mhpRateBonus))
				target.Properties.Modify(PropertyName.MHP_RATE_BM, -mhpRateBonus);

			if (buff.Vars.TryGetFloat(VarMhpFlatBonus, out var flatMhpFromInt))
				target.Properties.Modify(PropertyName.MHP_BM, -flatMhpFromInt);

			target.Properties.InvalidateAll();
		}
	}
}
