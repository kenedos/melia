using System.Collections.Generic;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Clerics.Oracle
{
	/// <summary>
	/// Handle for the Divine Might: Ill Omen debuff, which reduces
	/// all skill levels to Lv. 1.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.DivineMight_Hidden_Debuff)]
	public class Oracle_DivineMight_Hidden_DebuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is not Character character)
				return;

			var reductions = new Dictionary<int, float>();
			var skills = character.Skills.GetList(s => (int)s.Id > 10000);

			foreach (var skill in skills)
			{
				var currentLevel = skill.Level;
				if (currentLevel <= 1)
					continue;

				var reduction = -(currentLevel - 1);
				reductions[(int)skill.Id] = reduction;

				skill.Properties.Modify(PropertyName.Level_BM, reduction);
				skill.Properties.InvalidateAll();
				Send.ZC_NORMAL.SkillProperties(character.Connection, 0, skill);
			}

			foreach (var kvp in reductions)
				buff.Vars.SetFloat($"SkillReduce_{kvp.Key}", kvp.Value);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is not Character character)
				return;

			var skills = character.Skills.GetList(s => (int)s.Id > 10000);
			foreach (var skill in skills)
			{
				var varName = $"SkillReduce_{(int)skill.Id}";
				if (buff.Vars.TryGetFloat(varName, out var reduction))
				{
					skill.Properties.Modify(PropertyName.Level_BM, -reduction);
					skill.Properties.InvalidateAll();
					Send.ZC_NORMAL.SkillProperties(character.Connection, 0, skill);
				}
			}
		}
	}
}
