using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Pads;

namespace Melia.Zone.Skills.Handlers.Clerics.Miko
{
	/// <summary>
	/// Handler for the Miko skill Sweeping, which sweeps the ground for as
	/// long as the skill is held, leaving purified patches behind while the
	/// Miko can't be afflicted by removable debuffs.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Miko_HoukiBroom)]
	public class Miko_HoukiBroomOverride : IDynamicCasted
	{
		private const float PatchRange = 45f;
		private static readonly TimeSpan PatchInterval = TimeSpan.FromMilliseconds(500);

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);
			caster.StartBuff(BuffId.HoukiBroom_Buff, skill.Level, 0, skill.Properties.CaptionTime, caster, skill.Id);

			skill.Run(this.Sweep(skill, caster));
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.StopBuff(BuffId.HoukiBroom_Buff);
		}

		/// <summary>
		/// Leaves a purified patch where the caster stands every half second
		/// until they stop sweeping.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <returns></returns>
		private async Task Sweep(Skill skill, ICombatEntity caster)
		{
			while (!caster.IsDead && caster.IsBuffActive(BuffId.HoukiBroom_Buff))
			{
				var pad = new Pad(PadName.Miko_HoukiBroom, caster, skill, new Circle(caster.Position, PatchRange));
				pad.Position = caster.Position;
				pad.Trigger.LifeTime = TimeSpan.FromSeconds(skill.Properties.GetFloat(PropertyName.CaptionRatio));
				caster.Map.AddPad(pad);

				await skill.Wait(PatchInterval);
			}
		}
	}
}
