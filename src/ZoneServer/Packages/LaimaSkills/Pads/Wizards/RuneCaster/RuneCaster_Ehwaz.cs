using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Wizards.RuneCaster;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.Handlers.Wizards.RuneCaster
{
	/// <summary>
	/// Handler for Rune of Gravity's spheres, which circle the Rune Caster for
	/// 5 seconds and strike the enemies they run into.
	/// </summary>
	[Package("laima-skills")]
	[PadHandler(PadName.RuneCaster_Ehwaz_Pad1, PadName.RuneCaster_Ehwaz_Pad2, PadName.RuneCaster_Ehwaz_Pad3)]
	public class RuneCaster_EhwazPadOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, IUpdatePadHandler
	{
		private const int UpdateInterval = 200;
		private const float OrbitStep = 90f;
		private static readonly TimeSpan LifeTime = TimeSpan.FromSeconds(5);

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetUpdateInterval(UpdateInterval);
			pad.Trigger.LifeTime = LifeTime;
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			Send.ZC_NORMAL.PadUpdate(args.Trigger, false);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var caster = args.Creator;
			var target = args.Initiator;
			var skill = pad.Skill;

			if (caster.IsDead || target.IsDead || !caster.IsEnemy(target))
				return;

			var skillHitResult = SCR_SkillHit(caster, target, skill);
			target.TakeDamage(skillHitResult.Damage, caster);

			Send.ZC_SKILL_HIT_INFO(caster, new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var caster = args.Creator;

			if (caster.IsDead || caster.Map != pad.Map)
			{
				pad.Destroy();
				return;
			}

			var direction = caster.Direction.AddDegreeAngle(pad.FollowTargetOffsetAngle + OrbitStep);
			pad.FollowsTarget(caster, RuneCaster_EhwazOverride.OrbitRadius, direction);
		}
	}
}
