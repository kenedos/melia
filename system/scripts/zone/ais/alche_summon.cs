using System.Collections;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Logging;
using Yggrasil.Ai.BehaviorTree.Leafs;

[Ai("AlcheSummon")]
public class AlcheSummonAiScript : AiScript
{
	protected int MinFollowDistance = 35;

	protected override void Setup()
	{
		During("Idle", CheckEnemies);
		During("Attack", CheckTarget);
		During("ReturnHome", CheckLocation);
	}

	protected override void Root()
	{
		StartRoutine("Idle", Idle());
	}

	protected IEnumerable Idle()
	{
		if (this.Owner == null)
		{
			SetRunning(false);

			yield return Wait(4000, 8000);

			SwitchRandom();
			if (Case(80))
			{
				yield return MoveRandom();
			}
			else
			{
				yield return Animation("IDLE");
			}
		}
		else
		{
			yield return Follow();
		}
	}

	protected IEnumerable StopAndAttack()
	{
		yield return StopMove();
		StartRoutine("Attack", Attack());
	}

	private IEnumerable Follow()
	{
		while (!InRangeOf(this.Owner, MinFollowDistance))
			yield return MoveTo(this.Owner.Position.GetRandomInRange2D(15, MinFollowDistance), wait: false);

		yield return StopMove();
		yield return Wait(1000);
	}

	private void CheckLocation()
	{
		if (this.Entity is IMonster monster && monster.SpawnPosition.Get2DDistance(monster.Position) > MaxRoamDistance)
			StartRoutine("ReturnHome", ReturnHome());
		else
			StartRoutine("Idle", Idle());
	}
}
