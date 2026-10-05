using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handler for Heal_Buff, which is primarily triggered by Cleric_Heal.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Heal_Buff)]
	public class Heal_BuffOverride : BuffHandler
	{
		/// <summary>
		/// Starts the buff, healing the target.
		/// </summary>
		/// <param name="buff"></param>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var caster = buff.Caster;
			var target = buff.Target;
			var skillId = buff.SkillId;
			var healAmount = buff.NumArg2;

			target.Heal(healAmount, 0);
		}
	}
}
