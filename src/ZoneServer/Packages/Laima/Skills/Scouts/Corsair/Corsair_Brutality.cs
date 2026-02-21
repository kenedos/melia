using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Handlers.Scouts.Corsair
{
	/// <summary>
	/// Handler for the passive Corsair skill Brutality.
	/// When attacking enemies under JollyRoger debuff, applies Brutality_Buff
	/// to the corsair and party members.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Corsair_Brutality)]
	public class Corsair_BrutalityOverride : ISkillHandler, ISkillCombatAttackAfterCalcHandler
	{
		private const int BuffDurationMs = 10000;

		public void OnAttackAfterCalc(Skill skill, ICombatEntity attacker, ICombatEntity target, Skill attackerSkill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skillHitResult.Result == HitResultType.Dodge)
				return;

			if (!target.IsBuffActive(BuffId.JollyRoger_Enemy_Debuff))
				return;

			if (attacker is not Character character)
				return;

			if (character.TryGetBuff(BuffId.Brutality_Buff, out var buff) && buff.RemainingDuration >= TimeSpan.FromSeconds(6))
				return;

			var buffDuration = TimeSpan.FromMilliseconds(BuffDurationMs);

			if (character.HasParty)
			{
				foreach (var member in character.Map.GetPartyMembers(character))
					member.StartBuff(BuffId.Brutality_Buff, skill.Level, 0, buffDuration, character);
			}
			else
			{
				character.StartBuff(BuffId.Brutality_Buff, skill.Level, 0, buffDuration, character);
			}
		}
	}
}
