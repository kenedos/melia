using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers
{
	[PadHandler(PadName.Monster_FireBall, PadName.Mon_Fireball_santan)]
	public class Mon_Fireball : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			pad.Trigger.MaxUseCount = 1;
			pad.SetRange(15f);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(30000);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;


			if (!creator.IsEnemy(initiator))
				return;

			if (initiator.IsDead)
				return;

			var damage = creator.Properties.GetFloat(PropertyName.MINMATK) + creator.Properties.GetFloat(PropertyName.MAXMATK) / 2;

			var forceId = ForceId.GetNew();

			initiator.TakeDamage(damage, creator);

			var hitInfo = new HitInfo(creator, initiator, damage, HitResultType.Hit);
			hitInfo.ForceId = forceId;
			hitInfo.Type = HitType.Fire;

			Send.ZC_HIT_INFO(creator, initiator, hitInfo);

			pad.Trigger.IncreaseUseCount();
		}
	}
}
