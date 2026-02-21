using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers
{
	/// <summary>
	/// Pad handler for Bwa Kayiman follower pads that circle around the caster.
	/// Each pad follows a summon, dealing trampling damage to enemies.
	/// </summary>
	[Package("laima")]
	[PadHandler(PadName.Bokor_BwaKayiman_Fluting)]
	public class Bokor_BwaKayiman_FlutingOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetUpdateInterval(200);
		}
		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			if (!creator.IsEnemy(initiator))
				return;

			initiator.StartBuff(BuffId.Pollution_Debuff, skill.Level, 0, TimeSpan.FromSeconds(3), creator);
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

			if (!creator.IsCasting())
			{
				pad.Destroy();
				return;
			}

			if (creator is not Character character)
			{
				pad.Destroy();
				return;
			}

			var summonHandle = pad.Variables.Get<int>("BwaKayiman_SummonHandle");
			var monster = character.Map.GetMonster(summonHandle);

			if (monster is not Summon summon || summon.IsDead)
			{
				pad.Destroy();
				return;
			}

			pad.Movement.MoveTo(summon.Position);
			pad.Position = summon.Position;

			PadDamageEnemy(pad, 1f, 0, 0, "None", 1, 0f, 0f);
		}
	}
}
