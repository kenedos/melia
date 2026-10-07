using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Handlers.Clerics.Druid
{
	/// <summary>
	/// Handler for the Druid skill Transform, which turns the Druid back
	/// into the monster they last shape shifted into.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Druid_Transform)]
	public class Druid_TransformOverride : IGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character)
				return;

			var monsterId = DruidSkillHelper.GetShapeMonsterId(character);
			if (monsterId == 0)
			{
				caster.ServerMessage(Localization.Get("You haven't shape shifted into anything yet."));
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

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			character.StopBuff(BuffId.transform);
			character.StartBuff(BuffId.transform, skill.Level, monsterId, skill.Properties.CaptionTime, character, skill.Id);
		}
	}
}
