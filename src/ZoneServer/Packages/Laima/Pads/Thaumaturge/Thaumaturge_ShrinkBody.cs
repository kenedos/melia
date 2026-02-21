using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.Handlers
{
	[Package("laima")]
	[PadHandler(PadName.Thaumaturge_ShrinkBody)]
	public class Thaumaturge_ShrinkBodyOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(50f);
			pad.SetUpdateInterval(100);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(1000);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			if (pad.Variables.GetBool("Melia.Applied"))
				return;

			pad.Variables.SetBool("Melia.Applied", true);

			var maxTargets = (int)(3 + skill.Level * 0.5);
			var enemies = pad.Trigger.GetAttackableEntities(creator);
			var sorted = enemies.OrderBy(e => e.IsBuffActive(BuffId.ShrinkBody_Debuff) ? 1 : 0);

			var count = 0;
			var casterInt = (int)creator.Properties.GetFloat(PropertyName.INT);
			var hasThaumaturge4 = creator.IsAbilityActive(AbilityId.Thaumaturge4);

			foreach (var target in sorted)
			{
				if (count >= maxTargets)
					break;

				AddPadBuff(creator, target, pad, BuffId.ShrinkBody_Debuff, skill.Level, casterInt, 15000, 1, 100);
				count++;

				if (hasThaumaturge4)
				{
					var skillHitResult = SCR_SkillHit(creator, target, skill);
					target.TakeDamage(skillHitResult.Damage, creator);

					var hitInfo = new HitInfo(creator, target, skillHitResult.Damage, HitResultType.Hit);
					Send.ZC_HIT_INFO(creator, target, hitInfo);
				}
			}
		}
	}
}
