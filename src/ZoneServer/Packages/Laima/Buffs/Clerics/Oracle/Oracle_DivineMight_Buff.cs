using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Clerics.Oracle
{
	/// <summary>
	/// Handle for the Divine Might buff, which temporarily increases
	/// all non-common skill levels by 1.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.DivineMight_Buff)]
	public class Oracle_DivineMight_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
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
