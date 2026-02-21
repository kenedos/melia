using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Pads.Helpers.PadHelper;
using Melia.Zone.World.Actors;
using Melia.Zone.Scripting;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills;

namespace Melia.Zone.Pads.Handlers.Clerics.Paladin
{
	[Package("laima")]
	[PadHandler(PadName.Cleric_Restoration)]
	public class Paladin_RestorationOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const int PadLifeTimeSeconds = 300;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(70f);
			pad.Trigger.LifeTime = TimeSpan.FromSeconds(PadLifeTimeSeconds);
			pad.SetUpdateInterval(300);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
			PadRemoveBuff(pad, RelationType.All, 0, 0, BuffId.Restoration_Buff);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			if (!initiator.IsAlly(creator))
				return;

			if (initiator.IsBuffActive(BuffId.Restoration_Buff))
				return;

			var healAmount = this.GetHealAmount(creator, initiator, skill);
			initiator.StartBuff(BuffId.Restoration_Buff, skill.Level, healAmount, TimeSpan.FromMilliseconds(3000f), creator);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			pad.Position = creator.Position;

			if (creator.IsDead)
			{
				pad.Destroy();
				return;
			}

			var targets = pad.Trigger.GetAlliedEntities(creator);
			if (pad.Trigger.Area.IsInside(creator.Position))
				targets.Add(creator);

			foreach (var target in targets)
			{
				if (target.IsDead)
					continue;

				if (target.IsBuffActive(BuffId.Restoration_Buff))
					continue;

				var healAmount = this.GetHealAmount(creator, target, skill);
				target.StartBuff(BuffId.Restoration_Buff, skill.Level, healAmount, TimeSpan.FromMilliseconds(3000f), creator);
			}
		}

		/// <summary>
		/// Returns the healing power of the skill.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <returns></returns>
		private float GetHealAmount(ICombatEntity caster, ICombatEntity target, Skill skill)
		{
			var SCR_CalculateHeal = ScriptableFunctions.Combat.Get("SCR_CalculateHeal");
			var modifier = new SkillModifier();
			var skillHitResult = new SkillHitResult();
			var healAmount = SCR_CalculateHeal(caster, target, skill, modifier, skillHitResult);

			// Apply Paladin7 ability bonus (SP restoration)
			if (caster.TryGetActiveAbilityLevel(AbilityId.Paladin7, out var abilityLevel))
			{
				healAmount *= 1f + abilityLevel * 0.005f;
			}

			return healAmount;
		}
	}
}
