using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.Handlers
{
	[PadHandler(PadName.Mon_Zaibas)]
	public class Mon_Zaibas : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(30f);
			pad.SetUpdateInterval(750);
			pad.Trigger.MaxActorCount = 1;
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(15000);
			pad.Trigger.MaxUseCount = 10;
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(pad, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			var targets = pad.Trigger.GetAttackableEntities(creator);

			if (targets == null || !targets.Any())
				return;

			var modifier = new SkillModifier();

			var targetCount = pad.Trigger.MaxActorCount;
			foreach (var target in targets)
			{
				if (targetCount <= 0)
					break;

				var skillHitResult = SCR_SkillHit(creator, target, skill, modifier);
				var damage = skillHitResult.Damage;
				target.TakeSimpleHit(damage, creator, skill.Id);

				pad.PlayEffectToGround("F_cleric_zaibas_shot_rize", target.Position, 1f, 3000f, 0, 0);
				pad.PlayEffectToGround("F_cleric_zaibas_shot_ground", target.Position, 0.5f, 500f, 0f, 0);

				targetCount--;
			}

			pad.Trigger.IncreaseUseCount();
		}
	}
}
