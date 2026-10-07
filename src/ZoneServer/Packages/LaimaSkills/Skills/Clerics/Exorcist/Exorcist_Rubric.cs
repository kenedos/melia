using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Pads;

namespace Melia.Zone.Skills.Handlers.Clerics.Exorcist
{
	/// <summary>
	/// Handler for the Exorcist skill Rubric, which reads commandments for
	/// as long as the skill is held, striking and slowing the enemies in
	/// front of the Exorcist.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Exorcist_Rubric)]
	public class Exorcist_RubricOverride : IDynamicCasted
	{
		private const float Length = 100f;
		private const float Width = 40f;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var padName = caster.IsAbilityActive(AbilityId.Exorcist3) ? PadName.Exorcist_Rubric_abil : PadName.Exorcist_Rubric;

			var pad = new Pad(padName, caster, skill, new Square(caster.Position, caster.Direction, Length, Width));
			pad.Position = caster.Position;
			pad.Trigger.LifeTime = TimeSpan.FromSeconds(skill.Properties.GetFloat(PropertyName.CaptionRatio3));
			pad.FollowsTarget(caster);

			caster.Map.AddPad(pad);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
		}
	}
}
