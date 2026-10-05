using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.Packages.Laima.Helpers;

namespace Melia.Zone.Buffs.Handlers.Wizards.Necromancer
{
	/// <summary>
	/// Handle for the Gather Corpse,
	/// Collects fragments of defeated enemies affected by the debuff..
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.GatherCorpse_Debuff)]
	public class Necromancer_GatherCorpse_DebuffOverride : BuffHandler
	{
		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			if (target.IsDead && target is Mob && buff.Caster is Character character)
			{
				if (!NecromancerCorpsePartsHelper.TryAdd(character, 1))
					return;

				Send.ZC_NORMAL.PlayGatherCorpseParts(character, buff.Target);
				Send.ZC_PLAY_SOUND(character, "skl_eff_gathercorpse_whoosh");
				Send.ZC_PLAY_SOUND(character, "skl_eff_partscapture_finish");
			}
		}
	}
}
