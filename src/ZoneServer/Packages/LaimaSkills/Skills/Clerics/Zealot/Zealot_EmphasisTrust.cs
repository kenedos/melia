using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Clerics.Zealot
{
	/// <summary>
	/// Handler for the Zealot skill Emphatic Trust, which marks the enemies
	/// around the Zealot so their next 10 hits taken carry a Holy strike.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Zealot_EmphasisTrust)]
	public class Zealot_EmphasisTrustOverride : IGroundSkillHandler
	{
		private const float TrustRange = 100f;

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

			var duration = TimeSpan.FromSeconds(skill.Properties.GetFloat(PropertyName.CaptionRatio));
			var maxTargets = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio2);

			foreach (var enemy in caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, TrustRange).Take(maxTargets))
				enemy.StartBuff(BuffId.EmphasisTrust_Debuff, skill.Level, 0, duration, caster, skill.Id);
		}
	}
}
