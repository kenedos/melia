using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Yggdrasil.Logging;

namespace Melia.Zone.World.Actors.Characters
{
	public static class TeamLevelHelper
	{
		public const int MaxLevel = 100;
		public const float TeamExpGainRate = 0.01f;
		public static int GetLevel(int teamExp)
		{
			if (teamExp <= 13065)
				return 1;

			var level = 1;
			long requiredExp = 13066;

			while (level < MaxLevel && teamExp >= requiredExp)
			{
				level++;

				if (level >= MaxLevel)
					break;

				requiredExp = GetRequiredExp(level + 1);
			}

			return level;
		}

		public static long GetRequiredExp(int level)
		{
			if (level <= 1)
				return 0;

			if (level == 2)
				return 13066;

			if (level == 3)
				return 26133;

			if (level == 4)
				return 39199;

			if (level == 5)
				return 52266;

			// Uses the same progression established by the Team Level EXP table.
			return (long)Math.Ceiling(13066.0 * (level - 1));
		}

		public static float GetExpBonusRate(int teamLevel)
		{
			return Math.Max(0, teamLevel - 1) / 100f;
		}

		public static long CalculateTeamExpGain(long exp)
		{
			if (exp <= 0)
				return 0;

			return (long)Math.Floor(exp * TeamExpGainRate);
		}

		public static void Synchronize(Character character)
		{
			if (character?.Connection?.Account == null)
				return;

			var account = character.Connection.Account;
			var teamLevel = GetLevel(account.TeamExp);

			if (character.TryGetBuff(BuffId.TeamLevel, out var buff))
			{
				if (buff.NumArg1 == teamLevel)
					return;

				Send.ZC_BUFF_REMOVE(character, buff);

				buff.NumArg1 = teamLevel;

				Send.ZC_BUFF_ADD(character, buff);
			}
			else
			{
				character.StartBuff(
					BuffId.TeamLevel,
					teamLevel,
					0,
					TimeSpan.Zero,
					character
				);
			}
		}
	}
}
