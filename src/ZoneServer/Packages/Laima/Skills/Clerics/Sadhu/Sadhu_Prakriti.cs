using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Clerics.Sadhu
{
	/// <summary>
	/// Handler for the Sadhu skill Prakriti.
	/// Teleports the caster's body to the spirit's location, ends spirit
	/// form, and partially recovers SP lost during spirit form.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Sadhu_Prakriti)]
	public class Sadhu_PrakritiOverride : IMeleeGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.IsBuffActive(BuffId.OOBE_Soulmaster_Buff))
				return;

			if (caster is not Character casterCharacter)
				return;

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, caster.Position, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, caster.Position, caster.Direction, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, caster.Position, ForceId.GetNew(), null);

			if (!caster.TryGetBuff(BuffId.OOBE_Soulmaster_Buff, out var oobeBuff))
				return;

			var dummyHandle = (int)oobeBuff.NumArg2;
			if (casterCharacter.Map.TryGetCharacter(dummyHandle, out var dummyCharacter))
				dummyCharacter.Position = casterCharacter.Position;

			caster.StopBuff(BuffId.OOBE_Soulmaster_Buff);
		}
	}
}
