using System;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Pads.Helpers.PadHelper;
using Melia.Shared.Data.Database;
using Melia.Zone.Skills;

namespace Melia.Zone.Pads.Handlers
{
	[Package("laima")]
	[PadHandler(PadName.QuarrelShooter_BlockAndShoot)]
	public class QuarrelShooter_BlockAndShootOverride : ICreatePadHandler, IEnterPadHandler, IUpdatePadHandler
	{
		/// <summary>
		/// Initializes the pad when created.
		/// </summary>
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = args.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, pad.Name, -2.356194f, 0, 30, true);
			pad.SetRange(30f);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(1000);
			pad.Trigger.Area = new Circle(pad.Position, 30f);
			var value = (int)(skill.Data.SplashRate + creator.Properties.GetFloat(PropertyName.SR));
			pad.Trigger.MaxUseCount = value;
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
		}

		/// <summary>
		/// Handles an entity entering the pad area.
		/// </summary>
		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			this.Attack(pad, skill, creator, initiator);
		}

		/// <summary>
		/// Handles periodic updates of the pad.
		/// </summary>
		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;
			var sr = creator.Properties.GetFloat(PropertyName.SR);
		}

		private void Attack(Pad pad, Skill skill, ICombatEntity creator, ICombatEntity target)
		{
			if (pad.Trigger.AtCapacity)
				return;

			if (!creator.CanAttack(target))
				return;

			if (target.IsDead)
				return;

			var skillHitResult = SCR_SkillHit(creator, target, skill);
			target.TakeDamage(skillHitResult.Damage, creator);

			var hit = new HitInfo(creator, target, skill, skillHitResult);
			Send.ZC_HIT_INFO(creator, target, hit);

			pad.Trigger.IncreaseUseCount();
		}
	}
}
