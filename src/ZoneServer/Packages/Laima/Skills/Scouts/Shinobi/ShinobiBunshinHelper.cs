using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Skills.Scouts.Shinobi
{
	public static class ShinobiBunshinHelper
	{
		public static void ReplicateSkill(Character character, Skill skill, Position originPos, Position farPos, ICombatEntity target)
		{
			if (character is DummyCharacter || character.Map == null || !CanBeReplicated(skill.Id))
				return;

			var clones = character.Map.GetCharacters(actor => actor is DummyCharacter dummy && dummy.Owner == character && dummy.IsBuffActive(BuffId.Bunshin_Buff)).OfType<DummyCharacter>().ToArray();

			if (clones.Length == 0)
				return;

			var directionX = farPos.X - originPos.X;
			var directionZ = farPos.Z - originPos.Z;

			foreach (var clone in clones)
			{
				if (clone == null || clone.IsDead || clone.Map != character.Map)
					continue;

				if (!clone.Skills.TryGet(skill.Id, out var cloneSkill))
					continue;

				var cloneOriginPos = clone.Position;
				var cloneFarPos = new Position(cloneOriginPos.X + directionX, cloneOriginPos.Y, cloneOriginPos.Z + directionZ);

				clone.Direction = character.Direction;

				ExecuteSkill(cloneSkill, clone, cloneOriginPos, cloneFarPos, target);
			}
		}

		private static bool CanBeReplicated(SkillId skillId)
		{
			return skillId == SkillId.Shinobi_Kunai || skillId == SkillId.Shinobi_Katon_no_jutsu || skillId == SkillId.Shinobi_Mijin_no_jutsu || skillId == SkillId.Shinobi_Raiton_no_Jutsu;
		}

		private static void ExecuteSkill(Skill skill, DummyCharacter clone, Position originPos, Position farPos, ICombatEntity target)
		{
			switch (skill.Id)
			{
				case SkillId.Shinobi_Kunai:
					new Shinobi_Kunai().Handle(skill, clone, originPos, farPos, target);
					break;
				case SkillId.Shinobi_Katon_no_jutsu:
					new Shinobi_KatonNoJutsu().Handle(skill, clone, originPos, farPos, target);
					break;
				case SkillId.Shinobi_Mijin_no_jutsu:
					new Shinobi_MijinNoJutsu().Handle(skill, clone, originPos, farPos, target);
					break;
				case SkillId.Shinobi_Raiton_no_Jutsu:
					new Shinobi_RaitonNoJutsu().Handle(skill, clone, originPos, farPos, target);
					break;
			}
		}
	}
}
