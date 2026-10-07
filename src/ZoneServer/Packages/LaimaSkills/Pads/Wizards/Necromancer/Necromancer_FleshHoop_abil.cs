using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers.Wizards.Necromancer
{
	/// <summary>
	/// Handler for the Flesh Hoop pad, which damages enemies around it
	/// once per second.
	/// </summary>
	[Package("laima-skills")]
	[PadHandler(PadName.Necromancer_FleshHoop_abil)]
	public class Necromancer_FleshHoop_abilOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(24f);
			pad.SetUpdateInterval(1000);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(16000);
			pad.Trigger.MaxActorCount = 5;
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			Send.ZC_NORMAL.PadUpdate(args.Trigger, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			PadDamageEnemy(pad);

			if (args.Creator is not ICombatEntity caster)
				return;

			foreach (var target in pad.Trigger.GetAttackableEntities(caster))
				NecromancerSkillHelper.ApplyDemoralize(caster, target, pad.Skill);
		}
	}
}
