using System;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.L10N;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Kriwi
{
	/// <summary>
	/// Handler for the Kriwi skill Zaibas.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Kriwi_Zaibas)]
	public class Krivis_ZaibasOverride : IGroundSkillHandler, IDynamicCasted
	{

		/// <summary>
		/// Handles skill behavior
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		/// <param name="targets"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, skill));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(500));

			var spBonus = 0f;
			var currentSp = caster.Properties.GetFloat(PropertyName.SP);
			var spCost = currentSp * 0.10f;
			if (caster.TrySpendSp(spCost))
				spBonus = spCost;

			var targetPos = caster.Position.GetRelative(caster.Direction, 50);
			var pad = SkillCreatePad(caster, skill, targetPos, 0f, PadName.Cleric_Zaibas);
			if (pad != null)
				pad.NumArg2 = spBonus;
		}
	}
}
