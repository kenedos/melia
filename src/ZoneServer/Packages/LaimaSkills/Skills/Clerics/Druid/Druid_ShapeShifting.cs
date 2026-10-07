using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Skills.Handlers.Clerics.Druid
{
	/// <summary>
	/// Handler for the Druid skill Shape Shifting, which turns the Druid
	/// into the beast, plant or insect in front of them, with its skills.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Druid_ShapeShifting)]
	public class Druid_ShapeShiftingOverride : IGroundSkillHandler
	{
		private const float Length = 120f;
		private const float Width = 30f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character)
				return;

			var area = new Square(caster.Position, caster.Direction, Length, Width);
			var monster = caster.Map.GetAttackableEnemiesIn(caster, area).OfType<Mob>().FirstOrDefault(IsShapeable);

			if (monster == null)
			{
				caster.ServerMessage(Localization.Get("There is no beast, plant or insect in front of you."));
				Send.ZC_SKILL_CAST_CANCEL(caster);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, monster.Position);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, monster.Position, ForceId.GetNew(), null);

			DruidSkillHelper.SaveShape(character, monster);

			character.StopBuff(BuffId.transform);
			character.StartBuff(BuffId.transform, skill.Level, monster.Id, skill.Properties.CaptionTime, character, skill.Id);
		}

		/// <summary>
		/// Returns true if the Druid can take the monster's shape.
		/// </summary>
		/// <param name="monster"></param>
		/// <returns></returns>
		private static bool IsShapeable(Mob monster)
		{
			if (monster.Rank == MonsterRank.Boss)
				return false;

			return monster.Race == RaceType.Widling || monster.Race == RaceType.Forester || monster.Race == RaceType.Klaida;
		}
	}
}
