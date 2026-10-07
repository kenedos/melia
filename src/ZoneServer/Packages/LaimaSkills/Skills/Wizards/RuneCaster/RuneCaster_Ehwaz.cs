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
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Pads;

namespace Melia.Zone.Skills.Handlers.Wizards.RuneCaster
{
	/// <summary>
	/// Handler for the Rune Caster skill Rune of Gravity, which pulls three
	/// Psychokinesis spheres that circle the Rune Caster for 5 seconds.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.RuneCaster_Ehwaz)]
	public class RuneCaster_EhwazOverride : IGroundSkillHandler, IDynamicCasted
	{
		public const float OrbitRadius = 60f;
		private const float SphereRange = 30f;
		private static readonly TimeSpan SummonDelay = TimeSpan.FromMilliseconds(500);
		private static readonly (string PadName, float Angle)[] Spheres = [(PadName.RuneCaster_Ehwaz_Pad1, 0f), (PadName.RuneCaster_Ehwaz_Pad2, 120f), (PadName.RuneCaster_Ehwaz_Pad3, 240f)];

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);
			RuneCasterSkillHelper.ApplySkilledCasting(caster);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, caster.Position);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.Summon(skill, caster));
		}

		/// <summary>
		/// Pulls the three spheres around the caster.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <returns></returns>
		private async Task Summon(Skill skill, ICombatEntity caster)
		{
			await skill.Wait(SummonDelay);

			if (caster.IsDead)
				return;

			foreach (var sphere in Spheres)
			{
				var direction = caster.Direction.AddDegreeAngle(sphere.Angle);

				var pad = new Pad(sphere.PadName, caster, skill, new Circle(caster.Position, SphereRange));
				pad.Position = caster.Position.GetRelative(direction, OrbitRadius);
				pad.FollowsTarget(caster, OrbitRadius, direction);
				caster.Map.AddPad(pad);
			}
		}
	}
}
