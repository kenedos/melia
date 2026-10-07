using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Pads;

namespace Melia.Zone.Skills.Handlers.Scouts.Enchanter
{
	/// <summary>
	/// Handler for the Enchanter skill Enchant Aura, which toggles an area
	/// left where the Enchanter stood that damages enemies in it while
	/// draining the Enchanter's SP.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Enchanter_EnchantAura)]
	public class Enchanter_EnchantAuraOverride : IGroundSkillHandler
	{
		private const float Range = 75f;
		private static readonly TimeSpan LifeTime = TimeSpan.FromMinutes(30);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			var wasActive = caster.IsBuffActive(BuffId.EnchantAura_Buff);

			if (skill.Vars.TryGet<Pad>(EnchanterSkillHelper.AuraPadVar, out var oldPad))
			{
				skill.Vars.Remove(EnchanterSkillHelper.AuraPadVar);
				oldPad.Destroy();
			}

			if (wasActive)
			{
				caster.StopBuff(BuffId.EnchantAura_Buff);
				return;
			}

			caster.StartBuff(BuffId.EnchantAura_Buff, skill.Level, 0, TimeSpan.Zero, caster, skill.Id);

			var pad = new Pad(PadName.Enchanter_EnchantAura, caster, skill, new Circle(caster.Position, Range));
			pad.Position = caster.Position;
			pad.Trigger.LifeTime = LifeTime;
			caster.Map.AddPad(pad);
			skill.Vars.Set(EnchanterSkillHelper.AuraPadVar, pad);
		}
	}
}
