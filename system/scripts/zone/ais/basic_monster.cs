using System.Collections;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;

/// <summary>
/// Basic AI for most monsters.
/// </summary>
[Ai("BasicMonster")]
public class BasicMonsterAiScript : AiScript
{
	protected override void Setup()
	{
		this.MaxChaseDistance = 350;
		this.MaxRoamDistance = 1000;

		During("Idle", CheckEnemies);
		During("Idle", CheckFear);
		During("Attack", CheckTarget);
		During("Attack", CheckMaster);
		During("Attack", CheckFear);
	}

	protected override void Root()
	{
		StartRoutine("ReturnHome", ReturnHome());
	}
}
