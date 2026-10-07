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

namespace Melia.Zone.Skills.Handlers.Clerics.Druid
{
	/// <summary>
	/// Handler for the Druid skill Chortasmata, which grows grass on the
	/// target area that gives enemies a rash and heals the party.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Druid_Chortasmata)]
	public class Druid_ChortasmataOverride : IGroundSkillHandler, IDynamicCasted
	{
		private const float Range = 100f;
		private static readonly TimeSpan BornDelay = TimeSpan.FromMilliseconds(600);
		private static readonly TimeSpan GrowDelay = TimeSpan.FromMilliseconds(200);
		private static readonly TimeSpan BornLifeTime = TimeSpan.FromMilliseconds(400);

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

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, targetPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, targetPos, ForceId.GetNew(), null);

			skill.Run(this.Grow(skill, caster, targetPos));
		}

		/// <summary>
		/// Grows the grass on the target position.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		private async Task Grow(Skill skill, ICombatEntity caster, Position position)
		{
			await skill.Wait(BornDelay);

			var born = new Pad(PadName.plant_pad_born, caster, skill, new Circle(position, Range));
			born.Position = position;
			born.Trigger.LifeTime = BornLifeTime;
			caster.Map.AddPad(born);

			await skill.Wait(GrowDelay);

			var grass = new Pad(PadName.plant_pad, caster, skill, new Circle(position, Range));
			grass.Position = position;
			caster.Map.AddPad(grass);
		}
	}
}
