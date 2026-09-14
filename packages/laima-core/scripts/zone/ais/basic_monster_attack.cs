using System.Collections;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;

/// <summary>
/// Monsters that focus more on dealing damage? Not sure what this is.
/// But it's not the normally aggressive monsters.
/// </summary>
[Ai("BasicMonster_ATK")]
public class BasicMonsterAttackAiScript : AiScript
{

	protected override void Setup()
	{
		this.MaxChaseDistance = 350;
		this.MaxRoamDistance = 1000;

		During("Idle", CheckEnemies);
		During("Attack", CheckTarget);
		During("Attack", CheckMaster);
	}

	protected override void Root()
	{
		StartRoutine("ReturnHome", ReturnHome());
	}
}
