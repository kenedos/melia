using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Extensions;
using Yggdrasil.Logging;

/// <summary>
/// AI for Boss monsters
/// </summary>
[Ai("BasicBoss")]
public class BasicBossAi : AiScript
{
	protected override void Setup()
	{
		// Configure boss-specific parameters
		this.MaxChaseDistance = 400;
		this.MaxRoamDistance = 1500;
		this.SetAggroRange(400f);
		this.SetHatePerSecond(30, 5);

		// Set up behavior checks
		During("Idle", CheckEnemies);
		During("Attack", CheckTarget);
		During("Attack", CheckMaster);
	}

	protected override void Root()
	{
		StartRoutine("ReturnHome", ReturnHome());
	}
}
