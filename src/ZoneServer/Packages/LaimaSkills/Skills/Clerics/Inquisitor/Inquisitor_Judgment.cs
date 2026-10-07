using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Clerics.Inquisitor
{
	/// <summary>
	/// Handler for the Inquisitor skill Judgment, which raises the
	/// Inquisitor's critical chance and unlocks their skills' devil bonuses
	/// against every enemy.
	/// </summary>
	/// <remarks>
	/// With Judgment: Provoke, the enemies around the Inquisitor are
	/// provoked for 5 seconds.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Inquisitor_Judgment)]
	public class Inquisitor_JudgmentOverride : IGroundSkillHandler
	{
		private const float ProvokeRange = 100f;
		private static readonly TimeSpan ProvokeDuration = TimeSpan.FromSeconds(5);

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

			caster.StartBuff(BuffId.Judgment_Buff, skill.Level, 0, skill.Properties.CaptionTime, caster, skill.Id);

			if (!caster.IsAbilityActive(AbilityId.Inquisitor15))
				return;

			foreach (var enemy in caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, ProvokeRange))
			{
				enemy.InsertHate(caster);
				enemy.StartBuff(BuffId.Judgment_Provoke_Debuff, skill.Level, 0, ProvokeDuration, caster, skill.Id);
			}
		}
	}
}
