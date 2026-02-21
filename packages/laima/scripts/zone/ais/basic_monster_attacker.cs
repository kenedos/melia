using System.Collections;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;

/// <summary>
/// Aggressive monsters AI script
/// </summary>
[Ai("BasicMonster_Attacker")]
public class BasicMonsterAttackerAiScript : AiScript
{
	protected override void Setup()
	{
		// Aggressive enemies chase further away from their homes
		this.MaxChaseDistance = 350;
		this.MaxRoamDistance = 1500;
		this.SetAggroRange(200f);

		SetTendency(TendencyType.Aggressive);

		During("Idle", CheckEnemies);
		During("Attack", CheckTarget);
		During("Attack", CheckMaster);
	}

	protected override void Root()
	{
		StartRoutine("ReturnHome", ReturnHome());
	}
}
