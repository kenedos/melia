using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Helpers
{
	public static class BulletMarkerOverheatingHelper
	{
		public const int MaxStacks = 40;
		public const int BasicAttackStacks = 1;
		public const int SkillStacks = 2;

		private static readonly TimeSpan Duration = TimeSpan.FromSeconds(35);

		public static void AddBasicAttackStack(Character character, Skill skill)
		{
			AddStacks(character, BasicAttackStacks, skill);
		}

		public static void AddSkillStacks(Character character, Skill skill)
		{
			AddStacks(character, SkillStacks, skill);
		}

		public static bool TryConsumeOutrageStack(Character character)
		{
			if (character == null)
				return false;

			if (!character.TryGetBuff(BuffId.Overheating_outrage_Buff, out _))
				return false;

			if (!character.TryGetBuff(BuffId.Outrage_Buff, out var outrageBuff))
				return false;

			if (outrageBuff.OverbuffCounter <= 0)
				return false;

			outrageBuff.DecreaseOverbuff();

			if (outrageBuff.OverbuffCounter <= 0)
				character.StopBuff(BuffId.Outrage_Buff);
			else
				outrageBuff.NotifyUpdate();

			return true;
		}

		private static void AddStacks(Character character, int amount, Skill skill)
		{
			if (character == null || skill == null)
				return;

			if (!character.TryGetBuff(BuffId.DoubleGunStance_Buff, out _))
				return;

			if (character.TryGetBuff(BuffId.Outrage_Buff, out _))
				return;

			if (character.TryGetBuff(BuffId.Overheating_outrage_Buff, out _))
				return;

			var currentStacks = character.Buffs.GetOverbuffCount(BuffId.Overheating_Buff);
			var stacksToAdd = Math.Min(amount, MaxStacks - currentStacks);

			for (var i = 0; i < stacksToAdd; i++)
				character.StartBuff(BuffId.Overheating_Buff, skill.Level, 0f, Duration, character, skill.Id);
		}
	}
}
