using System;
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
	/// Handler for the Zealot skill Immolation, which sets the Zealot and
	/// the enemies around them on fire for 5 seconds.
	/// </summary>
	/// <remarks>
	/// With Immolation: Melt Armor both the Zealot and the burning enemies
	/// lose defense for the duration.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Zealot_Immolation)]
	public class Zealot_ImmolationOverride : IGroundSkillHandler
	{
		private const float FireRange = 80f;
		private static readonly TimeSpan Duration = TimeSpan.FromSeconds(5);

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

			caster.StartBuff(BuffId.Immolation_Buff, skill.Level, 0, Duration, caster, skill.Id);

			var hasMeltArmor = caster.TryGetActiveAbilityLevel(AbilityId.Zealot9, out var meltArmorLevel);
			if (hasMeltArmor)
				caster.StartBuff(BuffId.ImmolationMeltArmor_Debuff, meltArmorLevel, 0, Duration, caster, skill.Id);

			foreach (var enemy in caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, FireRange))
			{
				enemy.StartBuff(BuffId.Immolation_Debuff, skill.Level, 0, Duration, caster, skill.Id);

				if (hasMeltArmor)
					enemy.StartBuff(BuffId.ImmolationMeltArmor_Debuff, meltArmorLevel, 0, Duration, caster, skill.Id);
			}
		}
	}
}
