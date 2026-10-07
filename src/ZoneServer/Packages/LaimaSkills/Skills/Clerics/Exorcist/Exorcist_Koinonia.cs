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

namespace Melia.Zone.Skills.Handlers.Clerics.Exorcist
{
	/// <summary>
	/// Handler for the Exorcist skill Grand Cross, which engraves a holy
	/// pattern on the ground that strikes the enemies on it every second.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Exorcist_Koinonia)]
	public class Exorcist_KoinoniaOverride : IGroundSkillHandler, IDynamicCasted
	{
		private const float Range = 70f;
		private static readonly TimeSpan EngraveDelay = TimeSpan.FromMilliseconds(600);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPos))
			{
				caster.ServerMessage(Localization.Get("No target location specified."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.Engrave(skill, caster, targetPos));
		}

		/// <summary>
		/// Engraves the Grand Cross at the target position.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="targetPos"></param>
		/// <returns></returns>
		private async Task Engrave(Skill skill, ICombatEntity caster, Position targetPos)
		{
			await skill.Wait(EngraveDelay);

			if (caster.IsDead)
				return;

			var pad = new Pad(PadName.Exorcist_Koinonia, caster, skill, new Circle(targetPos, Range));
			pad.Position = targetPos;
			caster.Map.AddPad(pad);
		}
	}
}
