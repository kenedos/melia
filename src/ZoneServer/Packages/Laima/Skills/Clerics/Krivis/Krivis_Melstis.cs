using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Kriwi
{
	/// <summary>
	/// Handler for the Kriwi skill Melstis.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Kriwi_Melstis)]
	public class Krivis_MelstisOverride : IGroundSkillHandler, IDynamicCasted
	{

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);
			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
			Send.ZC_SKILL_READY(caster, skill, skillHandle, caster.Position, farPos);

			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, caster.Position, caster.Direction, caster.Position);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			SkillCreatePad(caster, skill, caster.Position, 0f, PadName.Cleric_Melstis);

			caster.PlayGroundEffect(caster.Position, "F_cleric_melstis_loop_ground", 2, 25f);
			
		}
	}
}
