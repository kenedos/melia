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

namespace Melia.Zone.Skills.Handlers.Wizards.Onmyoji
{
	/// <summary>
	/// Handler for the Onmyoji skill Yin Yang Harmony, a circle in front of
	/// the Onmyoji held for up to 3 seconds that strikes the enemies in it
	/// every 0.1 seconds while the Onmyoji can't be knocked back or down.
	/// </summary>
	/// <remarks>
	/// [Arts] Yin Yang Harmony: Heaven and Earth doubles the hits and makes
	/// the circle shrink.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Onmyoji_YinYangConsonance)]
	public class Onmyoji_YinYangConsonanceOverride : IGroundSkillHandler, IDynamicCasted
	{
		private const float Range = 120f;
		private const float Distance = 150f;
		private static readonly TimeSpan MaxDuration = TimeSpan.FromSeconds(3);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);
			caster.StartBuff(BuffId.Skill_SuperArmor_Buff, skill.Level, 0, MaxDuration, caster, skill.Id);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, caster.Position);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			var padName = caster.IsAbilityActive(AbilityId.Onmyoji21) ? PadName.YinYangConsonance_Hidden_Pad : PadName.YinYangConsonance_Pad;

			var center = caster.Map.Ground.GetLastValidPosition(caster.Position, caster.Position.GetRelative(caster.Direction, Distance));

			var pad = new Pad(padName, caster, skill, new Circle(center, Range));
			pad.Position = center;
			pad.Direction = caster.Direction;
			pad.Trigger.LifeTime = MaxDuration;
			caster.Map.AddPad(pad);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.StopBuff(BuffId.Skill_SuperArmor_Buff);

			Send.ZC_SKILL_DISABLE(caster);
			Send.ZC_NORMAL.SkillCancel(caster, skill.Id);
			Send.ZC_NORMAL.SkillCancelCancel(caster, skill.Id);
		}
	}
}
