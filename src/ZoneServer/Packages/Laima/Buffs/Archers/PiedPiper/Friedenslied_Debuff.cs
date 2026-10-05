using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Yggdrasil.Util;

namespace Melia.Zone.Packages.Laima.Buffs.Archers.PiedPiper
{
	[Package("laima")]
	[BuffHandler(BuffId.Friedenslied_Debuff)]
	public class Friedenslied_DebuffOverride : BuffHandler
	{
		private const int SecondBuffRemovalChance = 10;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var buffComponent = buff.Target.Components.Get<BuffComponent>();

			if (buffComponent == null)
				return;

			var removedBuffId = buffComponent.RemoveRandomBuff();

			if (removedBuffId == 0)
				return;

			var hasCancelBuff = buff.NumArg2 > 0;

			if (!hasCancelBuff)
				return;

			if (RandomProvider.Get().Next(100) >= SecondBuffRemovalChance)
				return;

			buffComponent.RemoveRandomBuff();
		}

		public override void OnEnd(Buff buff)
		{
		}
	}
}
