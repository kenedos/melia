using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handler for Heal_Dot_Buff, which heals over time
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Heal_Dot_Buff)]
	public class Heal_Dot_BuffOverride : BuffHandler
	{
		/// <summary>
		/// Healing the target on tick.
		/// </summary>
		/// <param name="buff"></param>
		public override void WhileActive(Buff buff)
		{
			var caster = buff.Caster;
			var target = buff.Target;
			var skillId = buff.SkillId;
			var healAmount = buff.NumArg2;

			target.Heal(healAmount, 0);
		}
	}
}
