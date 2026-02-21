using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Scouts.Corsair
{
	/// <summary>
	/// Handler for the Jolly Roger Buff.
	/// Combo activated by attacking enemies. Fever buff initiated after 100 combos.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.JollyRoger_Buff)]
	public class JollyRoger_BuffOverride : BuffHandler
	{
		public override void OnEnd(Buff buff)
		{
			if (buff.Caster is not Character caster)
				return;

			caster.Variables.Temp.Remove("Melia.Buff.JollyRoger");
			caster.Variables.Temp.Remove("Melia.Buff.JollyRoger.FeverStartTime");
		}
	}
}
