using System;
using System.Collections;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.Packages.Laima.Skills.Wizards.Sage;

[Ai("SageBlink")]
public class SageBlinkAiScript : AiScript
{
	protected override void Setup()
	{
		MaxChaseDistance = 250;
		MaxMasterDistance = 250;
		EnableReturnHome = false;
	}
	protected override void Root() { StartRoutine("Idle", Idle()); }
	protected override void CheckEnemies() { }
	protected override IEnumerable Idle()
	{
		while (!Entity.IsDead)
		{
			var master = GetMaster();
			if (master == null || master.IsDead || EntityGone(master) || master.Map != Entity.Map)
			{
				if (Entity is DummyCharacter clone) clone.Despawn();
				yield break;
			}
			SetRunning(true);
			SetFixedMoveSpeed(Math.Max(1f, master.Properties.GetFloat(PropertyName.MSPD) * 2f));
			var destination = master.Position.GetRelative(master.Direction.Backwards, 40f);
			if (Entity.Position.Get2DDistance(destination) > 8f)
				yield return MoveTo(destination, wait: false);
			else
				yield return StopMove();
			yield return Wait(100);
		}
	}
}
