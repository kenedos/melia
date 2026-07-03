using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Scouts.Enchanter
{
	/// <summary>
	/// Handler for the Enchant Aura buff.
	/// The actual periodic SP drain and damage loop is handled by the skill handler.
	/// This buff works as the toggle state.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.EnchantAura_Buff)]
	public class Enchanter_EnchantAura_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
		}

		public override void OnEnd(Buff buff)
		{
		}
	}
}
