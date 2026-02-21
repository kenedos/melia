using System.Collections;
using System.Diagnostics;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;

/// <summary>
/// AI for Magical casters monsters
/// </summary>
[Ai("BasicMonster_Magical")]
public class BasicMonsterMagicalAiScript : AiScript
{
	protected override void Setup()
	{
		// Keep distance when attacking target
		this.RangeType = AttackerRangeType.Ranged;
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
