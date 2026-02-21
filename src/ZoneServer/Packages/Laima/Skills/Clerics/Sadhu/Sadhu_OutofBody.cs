using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Clerics.Sadhu
{
	/// <summary>
	/// Handler for the Sadhu skill Out of Body.
	/// Toggle skill: enter spirit form or return to body.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Sadhu_OutofBody)]
	public class Sadhu_OutofBodyOverride : IMeleeGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (caster is not Character casterCharacter)
				return;

			if (caster.IsBuffActive(BuffId.OOBE_Soulmaster_Buff))
			{
				this.ReturnToBody(skill, casterCharacter);
				return;
			}

			this.EnterSpiritForm(skill, casterCharacter, farPos);
		}

		/// <summary>
		/// Enters spirit form: creates dummy body, teleports spirit forward,
		/// applies visual effects and enables all Sadhu skills.
		/// </summary>
		private void EnterSpiritForm(Skill skill, Character casterCharacter, Position farPos)
		{
			if (!casterCharacter.TrySpendSp(skill))
			{
				casterCharacter.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			casterCharacter.SetAttackState(true);

			farPos = casterCharacter.Position.GetRelative(casterCharacter.Direction, 30);

			Send.ZC_SKILL_READY(casterCharacter, skill, casterCharacter.Position, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(casterCharacter, casterCharacter.Handle, casterCharacter.Position, casterCharacter.Direction, farPos);
			Send.ZC_SKILL_MELEE_GROUND(casterCharacter, skill, casterCharacter.Position, ForceId.GetNew(), null);

			var dummyCharacter = casterCharacter.Clone(casterCharacter.Position);

			this.SendDummyRelationToDuelOpponents(casterCharacter, dummyCharacter);

			Send.ZC_PLAY_ANI(dummyCharacter, "BORN", false);
			Send.ZC_PLAY_ANI(dummyCharacter, "skl_OOBE_loop", true);
			Send.ZC_NORMAL.Skill_DynamicCastStart(dummyCharacter, SkillId.None);

			Send.ZC_PLAY_SOUND(casterCharacter, "skl_eff_yuchae_start_2");
			Send.ZC_GROUND_EFFECT(casterCharacter, farPos, "I_only_quest_smoke013_blue_smoke", 1, 0.7f, 0, 0, 0);
			Send.ZC_GROUND_EFFECT(casterCharacter, casterCharacter.Position, "I_only_quest_smoke013_blue_smoke", 1.5f, 0.3f, 0, 0, 0);
			Send.ZC_GROUND_EFFECT(casterCharacter, farPos, "I_only_quest_smoke058_blue", 3f, 0.5f, 0, 0, 0);
			Send.ZC_GROUND_EFFECT(casterCharacter, casterCharacter.Position, "I_only_quest_smoke058_blue", 3, 0.5f, 0, 0, 0);

			casterCharacter.Position = farPos;
			Send.ZC_SET_POS(casterCharacter, farPos);

			Send.ZC_NORMAL.SetActorColor(casterCharacter, 255, 200, 100, 150, 0.01f);
			Send.ZC_NORMAL.Skill_DynamicCastStart(casterCharacter, SkillId.None);

			casterCharacter.StartBuff(BuffId.OOBE_Soulmaster_Buff, skill.Level, dummyCharacter.Handle, TimeSpan.FromMinutes(5), casterCharacter);

			this.SendAvailableSkills(casterCharacter);
		}

		/// <summary>
		/// Returns the spirit to the body, removing spirit form.
		/// </summary>
		private void ReturnToBody(Skill skill, Character casterCharacter)
		{
			if (!casterCharacter.TrySpendSp(skill))
			{
				casterCharacter.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();

			Send.ZC_SKILL_READY(casterCharacter, skill, casterCharacter.Position, casterCharacter.Position);
			//Send.ZC_NORMAL.UpdateSkillEffect(casterCharacter, 1, casterCharacter.Handle, casterCharacter.Position, casterCharacter.Direction, casterCharacter.Position);
			Send.ZC_SKILL_MELEE_GROUND(casterCharacter, skill, casterCharacter.Position, ForceId.GetNew(), null);

			casterCharacter.StopBuff(BuffId.OOBE_Soulmaster_Buff);
		}

		/// <summary>
		/// Sends the list of ALL skills as available during spirit form.
		/// Unlike the old system, ALL Sadhu skills are enabled simultaneously.
		/// </summary>
		private void SendAvailableSkills(Character casterCharacter)
		{
			if (!ZoneServer.Instance.Data.BuffDb.TryFind(BuffId.OOBE_Soulmaster_Buff, out var buffData))
				return;

			foreach (var availableSkill in casterCharacter.Skills.GetList())
			{
				Send.ZC_NORMAL.ApplyBuff(casterCharacter, buffData.ClassName, availableSkill.Id, true);
			}
		}

		/// <summary>
		/// Notifies duel opponents that the dummy body is an enemy,
		/// so their client allows targeting it.
		/// </summary>
		private void SendDummyRelationToDuelOpponents(Character casterCharacter, Character dummyCharacter)
		{
			var duel = casterCharacter.Connection?.ActiveDuel;
			if (duel == null)
				return;

			foreach (var participant in duel.GetParticipants())
			{
				if (participant == casterCharacter || participant.Connection == null)
					continue;

				if (duel.AreDueling(casterCharacter, participant))
					Send.ZC_CHANGE_RELATION(participant.Connection, dummyCharacter.Handle, RelationType.Enemy);
			}
		}
	}
}
