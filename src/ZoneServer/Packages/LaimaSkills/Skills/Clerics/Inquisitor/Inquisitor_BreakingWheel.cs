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

namespace Melia.Zone.Skills.Handlers.Clerics.Inquisitor
{
	/// <summary>
	/// Handler for the Inquisitor skill Breaking Wheel, which summons a
	/// spinning wheel in front of the Inquisitor that strikes the enemies
	/// around it.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Inquisitor_BreakingWheel)]
	public class Inquisitor_BreakingWheelOverride : IGroundSkillHandler
	{
		private const float Range = 45f;
		private const float Distance = 50f;
		private static readonly TimeSpan SummonDelay = TimeSpan.FromMilliseconds(700);

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

			skill.Run(this.Summon(skill, caster));
		}

		/// <summary>
		/// Summons the wheel in front of the caster.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <returns></returns>
		private async Task Summon(Skill skill, ICombatEntity caster)
		{
			await skill.Wait(SummonDelay);

			if (caster.IsDead)
				return;

			var position = caster.Map.Ground.GetLastValidPosition(caster.Position, caster.Position.GetRelative(caster.Direction, Distance));

			var pad = new Pad(PadName.Inquisitor_BreakingWheel, caster, skill, new Circle(position, Range));
			pad.Position = position;
			caster.Map.AddPad(pad);
		}
	}
}
