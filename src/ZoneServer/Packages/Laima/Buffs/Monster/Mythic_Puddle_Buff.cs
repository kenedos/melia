using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Buffs.Handlers.Monster;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;

namespace Melia.Zone.Buffs.Handlers.Laima.Monster
{
	/// <summary>
	/// Handler for Mythic_Puddle_Buff.
	/// Monster periodically creates blood puddle pads at its position
	/// that damage and slow players standing in them.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Mythic_Puddle_Buff)]
	public class Mythic_Puddle_BuffOverride : BuffHandler
	{
		private const int PuddleSpawnRange = 50;
		private const int PuddleIntervalMs = 5000;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is not Mob monster)
				return;

			MythicBuffHelper.ApplyMythicStats(monster);
			buff.SetUpdateTime(PuddleIntervalMs);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target is not Mob monster || monster.Map == null || monster.IsDead)
				return;

			var pos = monster.Position.GetRandomInRange2D(0, PuddleSpawnRange);
			if (!monster.Map.Ground.TryGetNearestValidPosition(pos, out var validPos))
				validPos = monster.Position;

			var dummySkill = new Skill(monster, SkillId.Normal_Attack);
			var pad = new Pad(monster, dummySkill, PadName.Mythic_Puddle, validPos, monster.Direction);
			monster.Map.AddPad(pad);
		}

		public override void OnEnd(Buff buff)
		{
		}
	}
}
