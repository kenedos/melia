using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Pads.Handlers.Scouts.Bulletmarker
{
	/// <summary>
	/// Handler for the [Arts] Freeze Bullet: Fog pad, which chills up to 5
	/// enemies in it every second and freezes them for 2 seconds once they
	/// reach 4 stacks of Chill.
	/// </summary>
	[Package("laima-skills")]
	[PadHandler(PadName.Bulletmarker_FreezeBullet)]
	public class Bulletmarker_FreezeBulletOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const int MaxTargets = 5;
		private const int FreezeStacks = 4;
		private const int ChillInterval = 1000;
		private static readonly TimeSpan LifeTime = TimeSpan.FromSeconds(10);
		private static readonly TimeSpan ChillDuration = TimeSpan.FromSeconds(2);
		private static readonly TimeSpan FreezeDuration = TimeSpan.FromSeconds(2);

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetUpdateInterval(ChillInterval);
			pad.Trigger.LifeTime = LifeTime;
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			Send.ZC_NORMAL.PadUpdate(args.Trigger, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var caster = args.Creator;

			foreach (var target in pad.Trigger.GetAttackableEntities(caster).Take(MaxTargets))
			{
				if (target.GetOverbuffCount(BuffId.FreezeBullet_Cold_Debuff) >= FreezeStacks - 1)
				{
					target.StopBuff(BuffId.FreezeBullet_Cold_Debuff);
					target.StartBuff(BuffId.Freeze, 1, 0, FreezeDuration, caster, pad.Skill.Id);
					continue;
				}

				target.StartBuff(BuffId.FreezeBullet_Cold_Debuff, pad.Skill.Level, 0, ChillDuration, caster, pad.Skill.Id);
			}
		}
	}
}
