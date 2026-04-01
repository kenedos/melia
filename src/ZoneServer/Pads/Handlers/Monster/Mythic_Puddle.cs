using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.Handlers
{
	/// <summary>
	/// Handler for the Mythic_Puddle pad.
	/// Deals periodic damage to enemies standing in the puddle
	/// based on the creating monster's ATK via SCR_SkillHit.
	/// </summary>
	[PadHandler(PadName.Mythic_Puddle)]
	public class Mythic_Puddle : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const float PuddleRadius = 75f;
		private const int PuddleLifetimeMs = 10000;
		private const int UpdateIntervalMs = 1000;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(PuddleRadius);
			pad.SetUpdateInterval(UpdateIntervalMs);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(PuddleLifetimeMs);
			pad.Trigger.MaxActorCount = 20;
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			if (creator is not ICombatEntity caster || caster.IsDead)
				return;

			var targets = pad.Map.GetAttackableEnemiesIn(caster, pad.Area);
			foreach (var target in targets)
			{
				if (target is not ICombatEntity combatTarget || combatTarget.IsDead)
					continue;

				if (!caster.IsEnemy(combatTarget))
					continue;

				var skillHitResult = SCR_SkillHit(caster, combatTarget, skill);
				var damage = skillHitResult.Damage * 0.05f;

				combatTarget.TakeDamage(damage, caster);

				var hitInfo = new HitInfo(caster, combatTarget, skill, damage, skillHitResult.Result);
				Send.ZC_HIT_INFO(caster, combatTarget, hitInfo);
			}
		}
	}
}
