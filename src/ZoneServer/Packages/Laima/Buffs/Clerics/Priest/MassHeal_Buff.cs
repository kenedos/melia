using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Mass Heal, HP Recovery.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.MassHeal_Buff)]
	public class MassHeal_BuffOverride : BuffHandler
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
