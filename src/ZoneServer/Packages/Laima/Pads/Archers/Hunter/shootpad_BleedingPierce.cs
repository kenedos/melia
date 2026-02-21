using System;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.Handlers
{
	[Package("laima")]
	[PadHandler(PadName.shootpad_BleedingPierce)]
	public class shootpad_BleedingPierceOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetUpdateInterval(750);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(2000);
			pad.Trigger.MaxUseCount = 8;
			pad.Trigger.MaxConcurrentUseCount = 8;
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;
			if (!creator.IsEnemy(initiator)) return;

			PadTargetDamage(pad, initiator, RelationType.Enemy, 1f, 0, 0);

			var skillHitResult = SCR_SkillHit(creator, initiator, skill);
			var damage = (int)skillHitResult.Damage;

			var duration = (int)((7 + 0.3f * skill.Level) * 1000);

			var buffId = BuffId.BleedingPierce_Debuff;
			if (creator.IsAbilityActive(AbilityId.Hunter25))
				buffId = BuffId.BleedingPierce_Abil_Debuff;
			PadTargetBuff(pad, initiator, RelationType.Enemy, 0, -1, buffId, 1, damage, duration, 1, 100, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

		}
	}
}
