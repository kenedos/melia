using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Pads;

namespace Melia.Zone.Skills.Handlers.Wizards.Sage
{
	/// <summary>
	/// Handler for the Sage skill Hole of Darkness, which opens a dark hole
	/// on the ground around the Sage for 3 seconds.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Sage_HoleOfDarkness)]
	public class Sage_HoleOfDarknessOverride : IGroundSkillHandler, IDynamicCasted
	{
		private const float Range = 80f;
		private static readonly TimeSpan OpenDelay = TimeSpan.FromMilliseconds(800);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.Open(skill, caster));
		}

		/// <summary>
		/// Opens the hole around the caster.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <returns></returns>
		private async Task Open(Skill skill, ICombatEntity caster)
		{
			await skill.Wait(OpenDelay);

			if (caster.IsDead)
				return;

			var pad = new Pad(PadName.Sage_HoleOfDarkness, caster, skill, new Circle(caster.Position, Range));
			pad.Position = caster.Position;
			caster.Map.AddPad(pad);
		}
	}
}
