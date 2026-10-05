using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Skills.Archers.PiedPiper
{
	[Package("laima")]
	[SkillHandler(SkillId.PiedPiper_Marschierendeslied)]
	public class PiedPiper_Marschierendeslied : IGroundSkillHandler
	{
		private const float SkillRange = 160f;
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 10;
		private const int BaseBlockCount = 10;
		private const int AdditionalBlockCountPerSwordsman = 1;
		private const int MoveSpeedBonus = 10;
		private const int BaseBuffDurationSeconds = 900;
		private const int BaseMoveSpeedDurationSeconds = 900;
		private const int AllegroMoveSpeedDurationSeconds = 900;
		private const int MoraleDurationPerSwordsmanSeconds = 5;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity selectedTarget)
		{
			if (caster is not Character character || character.IsDead)
				return;

			if (!caster.TrySpendSp(skill))
			{
				caster.SetAttackState(false);
				return;
			}

			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, caster.Position);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, caster.Position);

			var skillLevel = Math.Clamp(skill.Level, MinimumSkillLevel, MaximumSkillLevel);
			var hasAllegro = character.IsAbilityActive(AbilityId.PiedPiper7);
			var hasMoraleBoost = character.IsAbilityActive(AbilityId.PiedPiper8);
			var recipients = this.GetRecipients(character);
			var swordsmanCount = recipients.Count(member => member.JobClass == JobClass.Swordsman);
			var blockCount = BaseBlockCount + skillLevel + swordsmanCount * AdditionalBlockCountPerSwordsman;
			var buffDurationSeconds = BaseBuffDurationSeconds;

			if (hasMoraleBoost)
				buffDurationSeconds += swordsmanCount * MoraleDurationPerSwordsmanSeconds;

			var moveSpeedDurationSeconds = hasAllegro ? AllegroMoveSpeedDurationSeconds : BaseMoveSpeedDurationSeconds;
			var buffDuration = TimeSpan.FromSeconds(buffDurationSeconds);
			var moveSpeedDuration = TimeSpan.FromSeconds(moveSpeedDurationSeconds);

			foreach (var recipient in recipients)
			{
				recipient.StartBuff(BuffId.Marschierendeslied_Buff, blockCount, skillLevel, buffDuration, character, skill.Id);
				recipient.StartBuff(BuffId.Allegro_Buff, MoveSpeedBonus, 0, moveSpeedDuration, character, skill.Id);
			}

			skill.IncreaseOverheat();
			PiedPiperHamelnNagetierHelper.TrySummonMouse(character);
			caster.SetAttackState(false);
		}

		private List<Character> GetRecipients(Character caster)
		{
			var recipients = new List<Character> { caster };

			if (caster.Connection?.Party != null)
			{
				var partyMembers = caster.Map.GetPartyMembersInRange(caster, SkillRange, true).Where(member => member != null && !member.IsDead && member.Layer == caster.Layer);

				foreach (var member in partyMembers)
					if (recipients.All(existing => existing.Handle != member.Handle))
						recipients.Add(member);
			}

			return recipients;
		}
	}
}
