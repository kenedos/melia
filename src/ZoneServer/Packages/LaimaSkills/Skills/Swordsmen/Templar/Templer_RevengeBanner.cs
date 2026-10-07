using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Swordsmen.Templar
{
	/// <summary>
	/// Handler for the Templar skill Flag of Revenge.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Templer_RevengeBanner)]
	public class Templer_RevengeBannerOverride : IGroundSkillHandler
	{
		private static readonly TimeSpan PlantDelay = TimeSpan.FromMilliseconds(500);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var flagPos))
				flagPos = farPos;

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, flagPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, flagPos, ForceId.GetNew(), null);

			TemplarSkillHelper.RemoveFlags(caster);

			skill.Run(this.PlantFlag(skill, caster, flagPos));
		}

		/// <summary>
		/// Plants the flag once the cast animation reaches the ground.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="flagPos"></param>
		private async Task PlantFlag(Skill skill, ICombatEntity caster, Position flagPos)
		{
			await skill.Wait(PlantDelay);

			SkillCreatePad(caster, skill, flagPos, 0f, PadName.Templer_RevengeBanner);
		}
	}
}
