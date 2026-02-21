using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Buffs.Handlers.Scouts.Corsair
{
	/// <summary>
	/// Handler for the Looting Buff.
	/// Party members with this buff have a chance to steal silver from enemies on hit.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Looting_Buff)]
	public class Looting_BuffOverride : BuffHandler, IBuffCombatAttackAfterCalcHandler
	{
		private const int BaseSilverAmount = 5;
		private const int SilverPerLevel = 3;
		private const float DropChance = 15f;

		private const string LootingCounterKey = "Melia.Buff.Looting.DropCount";
		private const int MaxDropsPerMob = 10;

		public void OnAttackAfterCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skillHitResult.Damage == 0)
				return;

			if (attacker is not Character character)
				return;

			if (target is not Mob mob)
				return;

			if (mob.Rank == MonsterRank.NPC || mob.Rank == MonsterRank.Material || mob.Rank == MonsterRank.MISC)
				return;

			var dropCount = mob.Vars.GetInt(LootingCounterKey);
			if (dropCount >= MaxDropsPerMob)
				return;

			var rnd = RandomProvider.Get();
			if (rnd.NextDouble() * 100 < DropChance)
			{
				mob.Vars.SetInt(LootingCounterKey, dropCount + 1);

				var silverAmount = BaseSilverAmount + (buff.NumArg1 * SilverPerLevel);
				var mobLevelMultiplier = 1f + (mob.Level * 0.005f);
				mob.DropItem(character, (int)ItemId.Silver, (int)(silverAmount * mobLevelMultiplier), 100);
			}
		}
	}
}
