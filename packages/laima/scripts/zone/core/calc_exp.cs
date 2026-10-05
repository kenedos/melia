//--- Melia Script ----------------------------------------------------------
// Exp Calculation Functions
//--- Description -----------------------------------------------------------
// Functions that primarily calculate exp given by monsters.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Dungeons;

public class ExpCalculationsFunctionsScript : GeneralScript
{

	/// <summary>
	/// Official exp ratio formula
	/// </summary>
	/// <param name="monster"></param>
	/// <param name="character"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float GET_EXP_RATIO(Mob monster, Character character)
	{
		var characterLv = character.Level;
		var monsterLv = monster.Level;
		var value = 1.0f;

		if (!ZoneServer.Instance.Conf.World.LevelExpPenalty)
			return value;

		var penaltyRate = 0.25f; // Base value to hit at a 10-level gap.
		var levelGap = Math.Abs(characterLv - monsterLv);

		if (levelGap > 5)
		{
			// Use an exponential decay that quickly reduces the experience ratio.
			var factor = (levelGap - 5) / 5.0f; // Normalize the gap over 5 levels.

			// Exponential decay to hit 0.01 at 10 levels difference.
			value *= (float)Math.Pow(penaltyRate, factor);
		}

		// Ensure the value remains within a reasonable range.
		return Math.Min(Math.Max(penaltyRate, value), 1);
	}

	/// <summary>
	/// Official instance dungeon exp ratio formula
	/// </summary>
	/// <param name="instanceDungeon"></param>
	/// <param name="character"></param>
	/// <returns></returns>
	/// <remarks>Disabled Attributed because Instance Dungeons aren't implemented.</remarks>
	///[ScriptableFunction]
	public float GET_EXP_RATIO_INDUN(InstanceDungeon instanceDungeon, Character character)
	{
		var characterLv = character.Level;
		var instanceDungeonLv = instanceDungeon.Level;
		var value = 1.0f;

		var standardLevel = 80;
		var levelGap = Math.Abs(characterLv - instanceDungeonLv);

		if (levelGap > standardLevel)
		{
			var penaltyRatio = characterLv < instanceDungeonLv ? 0.05f : 0.05f;
			var lvRatio = 1.0f - ((levelGap - standardLevel) * penaltyRatio);
			value *= lvRatio;
		}

		return Math.Min(0, value);
	}
}
