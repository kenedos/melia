using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Pads;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Rodelero
{
	/// <summary>
	/// Handler for the Rodelero skill Shield Charge.
	/// Channeled charge with shield that knocks down nearby enemies.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Rodelero_ShieldCharge)]
	public class Rodelero_ShieldChargeOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		/// <summary>
		/// Called when the skill begins channeling.
		/// </summary>
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.RemoveBuff(BuffId.ShieldCharge_Buff);
			caster.StartBuff(BuffId.ShieldCharge_Buff, skill.Level, 0f, TimeSpan.Zero, caster);
			var targetPos = caster.Position.GetRelative(caster.Direction, distance: 20f);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Rodelero_ShieldCharge);
			caster.PlaySound("voice_archer_camouflage_shot", "voice_archer_m_camouflage_shot");
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		/// <summary>
		/// Called when the skill channeling ends.
		/// </summary>
		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.RemoveBuff(BuffId.ShieldCharge_Buff);
			SkillRemovePad(caster, skill);
			caster.StopSound("voice_archer_camouflage_shot", "voice_archer_m_camouflage_shot");
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		/// <summary>
		/// Handles the Shield Charge skill execution.
		/// </summary>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
			Send.ZC_SKILL_READY(caster, skill, skillHandle, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);
		}
	}
}
