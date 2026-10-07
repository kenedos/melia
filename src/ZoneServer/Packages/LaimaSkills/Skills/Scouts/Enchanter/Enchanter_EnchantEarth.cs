using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Scouts.Enchanter
{
	/// <summary>
	/// Handler for the Enchanter skill Enchant Earth, which raises the block
	/// penetration of the party members around the Enchanter.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Enchanter_EnchantEarth)]
	public class Enchanter_EnchantEarthOverride : ISelfSkillHandler
	{
		private const float Range = 150f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, originPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, originPos, ForceId.GetNew(), null);

			foreach (var ally in EnchanterSkillHelper.GetPartyTargets(caster, Range))
				ally.StartBuff(BuffId.EnchantEarth_Buff, skill.Level, 0, skill.Properties.CaptionTime, caster, skill.Id);
		}
	}
}
