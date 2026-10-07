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
	/// Handler for the Sage skill Ultimate Dimension, which distorts a wide
	/// space around the Sage for 1.5 seconds, striking the enemies in it.
	/// </summary>
	/// <remarks>
	/// With Ultimate Dimension: Enlarged Magic Circle, allied circles in its
	/// range grow by 10%.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Sage_UltimateDimension)]
	public class Sage_UltimateDimensionOverride : IGroundSkillHandler
	{
		private const float Range = 50f;
		private const float EnlargeRate = 1.1f;
		private const string EnlargedVar = "Melia.Sage.Enlarged";
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
		/// Opens the distortion around the caster.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <returns></returns>
		private async Task Open(Skill skill, ICombatEntity caster)
		{
			await skill.Wait(OpenDelay);

			if (caster.IsDead)
				return;

			var pad = new Pad(PadName.Sage_UltimateDimension, caster, skill, new Circle(caster.Position, Range));
			pad.Position = caster.Position;
			caster.Map.AddPad(pad);

			if (caster.IsAbilityActive(AbilityId.Sage9))
				this.EnlargeAllyCircles(caster, pad);
		}

		/// <summary>
		/// Grows the circular pads of the caster and their allies within the
		/// distortion by 10%, once per pad.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="dimension"></param>
		private void EnlargeAllyCircles(ICombatEntity caster, Pad dimension)
		{
			foreach (var pad in caster.Map.GetPadsAt(dimension.Position, Range))
			{
				if (pad == dimension || pad.Variables.GetBool(EnlargedVar))
					continue;

				if (pad.Creator is not ICombatEntity creator || (creator != caster && !creator.IsAlly(caster)))
					continue;

				if (pad.Area is not Circle circle)
					continue;

				pad.Variables.SetBool(EnlargedVar, true);
				pad.SetRange(circle.Radius * EnlargeRate);
			}
		}
	}
}
