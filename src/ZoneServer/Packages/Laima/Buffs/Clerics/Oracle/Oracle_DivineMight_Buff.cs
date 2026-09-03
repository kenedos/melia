using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Clerics.Oracle
{
	/// <summary>
	/// Handle for the Divine Might buff, which temporarily increases
	/// all non-common skill levels by 1, and ends once the target has
	/// used its share of skills.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.DivineMight_Buff)]
	public class Oracle_DivineMight_BuffOverride : BuffHandler, IBuffOnSkillUseHandler
	{
		private const int SkillUsesPerLevel = 3;
		private const string RemainingVar = "Melia.DivineMight.Remaining";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Vars.SetInt(RemainingVar, (int)buff.NumArg1 * SkillUsesPerLevel);

			if (buff.Target is not Character character)
				return;

			var skills = character.Skills.GetList(s => (int)s.Id > 10000 && s.Id != SkillId.Oracle_DivineMight);
			foreach (var skill in skills)
			{
				skill.Properties.Modify(PropertyName.Level_BM, 1);
				skill.Properties.InvalidateAll();
				Send.ZC_NORMAL.SkillProperties(character.Connection, 0, skill);
			}
		}

		public void OnSkillUse(Buff buff, ICombatEntity caster, Skill skill)
		{
			if (skill.IsNormalAttack)
				return;

			var remaining = buff.Vars.GetInt(RemainingVar) - 1;
			buff.Vars.SetInt(RemainingVar, remaining);

			if (remaining <= 0)
				caster.StopBuff(BuffId.DivineMight_Buff);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is not Character character)
				return;

			var skills = character.Skills.GetList(s => (int)s.Id > 10000 && s.Id != SkillId.Oracle_DivineMight);
			foreach (var skill in skills)
			{
				skill.Properties.Modify(PropertyName.Level_BM, -1);
				skill.Properties.InvalidateAll();
				Send.ZC_NORMAL.SkillProperties(character.Connection, 0, skill);
			}
		}
	}
}
