using System;
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

namespace Melia.Zone.Skills.Handlers.Swordsmen.BlossomBlader
{
	/// <summary>
	/// Handler for the Blossom Blader skill Control Blade, which sends the
	/// summoned swords at the target for 3 seconds.
	/// </summary>
	/// <remarks>
	/// [Arts] Control Blade: Hiten Blade has them slash the area ahead
	/// instead, for half the damage.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.BlossomBlader_ControlBlade)]
	public class BlossomBlader_ControlBladeOverride : IGroundSkillHandler
	{
		private const float Range = 130f;
		private const float HitenDistance = 50f;
		private const float HitenRange = 100f;
		private static readonly TimeSpan Duration = TimeSpan.FromSeconds(3);
		private static readonly TimeSpan BlossomShowerDuration = TimeSpan.FromMilliseconds(3250);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			var hiten = caster.IsAbilityActive(AbilityId.Blossomblader18);

			if (!hiten && (target == null || target.IsDead || !caster.Position.InRange2D(target.Position, Range)))
			{
				Send.ZC_SKILL_CAST_CANCEL(caster);
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

			var duration = caster.IsBuffActive(BuffId.StartUp_Abil_Buff) ? BlossomShowerDuration : Duration;

			if (!hiten)
			{
				target.StartBuff(BuffId.ControlBlade_Debuff, skill.Level, 0, duration, caster, skill.Id);
				return;
			}

			var position = caster.Position.GetRelative(caster.Direction, HitenDistance);

			var pad = new Pad(PadName.BlossomBlader_ControlBlade, caster, skill, new Circle(position, HitenRange));
			pad.Position = position;
			pad.Trigger.LifeTime = duration;
			caster.Map.AddPad(pad);
		}
	}
}
