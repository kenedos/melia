
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Scouts.Enchanter
{
	[Package("laima")]
	[BuffHandler(BuffId.OverReinforce_Buff)]
	public class Enchanter_OverReinforce_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is Character character)
			{
				var bonus = this.GetAttackBonus(buff);

				buff.NumArg2 = bonus;

				character.Properties.Modify(PropertyName.PATK_BM, bonus);
				character.Properties.Modify(PropertyName.MATK_BM, bonus);

				Send.ZC_OBJECT_PROPERTY(
					character,
					PropertyName.PATK,
					PropertyName.PATK_BM,
					PropertyName.MATK,
					PropertyName.MATK_BM);
			}
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is Character character)
			{
				var bonus = buff.NumArg2;

				character.Properties.Modify(PropertyName.PATK_BM, -bonus);
				character.Properties.Modify(PropertyName.MATK_BM, -bonus);

				Send.ZC_OBJECT_PROPERTY(
					character,
					PropertyName.PATK,
					PropertyName.PATK_BM,
					PropertyName.MATK,
					PropertyName.MATK_BM);
			}
		}

		private float GetAttackBonus(Buff buff)
		{
			var skillLevel = buff.NumArg1;

			// Lv1  = 50
			// Lv2  = 60
			// Lv3  = 70
			// Lv4  = 80
			// Lv5  = 90
			// Lv6  = 100
			// Lv7  = 110
			// Lv8  = 120
			// Lv9  = 130
			// Lv10 = 140
			return 50f + (10f * (skillLevel - 1));
		}
	}
}
