using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Clerics.PlagueDoctor
{
	/// <summary>
	/// Handler for Beak Mask, which blocks removable debuffs other than
	/// stun, bind, immobility and slow, and comes off after 5 blocks.
	/// </summary>
	/// <remarks>
	/// The blocking itself happens in BuffComponent.TryResistDebuff.
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.BeakMask_Buff)]
	public class PlagueDoctor_BeakMask_BuffOverride : BuffHandler, IBuffOnDebuffResistedHandler
	{
		private const int MaxBlocks = 5;
		private const string BlocksVar = "Melia.PlagueDoctor.BeakMaskBlocks";

		public void OnDebuffResisted(Buff buff, BuffId buffId, IActor caster)
		{
			var blocks = buff.Vars.GetInt(BlocksVar) + 1;
			buff.Vars.SetInt(BlocksVar, blocks);

			if (blocks >= MaxBlocks)
				buff.Target.StopBuff(BuffId.BeakMask_Buff);
		}

		public override void OnEnd(Buff buff)
		{
			PlagueDoctorSkillHelper.StartBeakMaskCooldown(buff.Target);
		}
	}

	/// <summary>
	/// Handler for the White Mask, which trades Beak Mask's protection for
	/// a poison puff on every Black Death Steam.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.WhiteBeakMask_Buff)]
	public class PlagueDoctor_WhiteBeakMask_BuffOverride : BuffHandler
	{
		public override void OnEnd(Buff buff)
		{
			PlagueDoctorSkillHelper.StartBeakMaskCooldown(buff.Target);
		}
	}
}
