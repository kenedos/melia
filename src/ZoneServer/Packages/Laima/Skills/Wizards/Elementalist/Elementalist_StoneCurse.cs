using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.Game.Const.Web;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Pads.Helpers;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Pads;
using Yggdrasil.Geometry.Shapes;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Wizards.Elementalist
{
	/// <summary>
	/// Handler override for the Elementalist skill Lightning Orb.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Elementalist_StoneCurse)]
	public class Elementalist_StoneCurseOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const int OrbCount = 14;

		private const string VarPads = "Melia.StoneCurse.Pads";
		private const string VarIsCasting = "Melia.StoneCurse.IsCasting";
		private const string VarCastTimeMs = "Melia.StoneCurse.CastTimeMs";

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			var oldPads = caster.Map.GetPads(p => p.Name == PadName.Elementalist_ChainReaction_Pad && p.Creator == caster);
			foreach (var pad in oldPads)
				pad.Destroy();

			skill.Vars.Set(VarPads, new List<Pad>());
			skill.Vars.Set(VarIsCasting, true);

			caster.SetCastingState(true, skill);
			caster.StartBuff(BuffId.Chainreaction_Runpad_Buff, 1f, 0f, TimeSpan.Zero, caster);
			caster.PlaySound("voice_atk_long_cast_f", "voice_war_atk_long_cast");
			caster.PlaySound("skl_eff_lightningsphere_cast", "skl_eff_lightningsphere_cast");
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);

			var castingSpeed = caster.Properties.GetFloat(PropertyName.CastingSpeed);
			var castTimeMs = skill.Data.BasicCast * castingSpeed / 100f;
			skill.Vars.Set(VarCastTimeMs, castTimeMs);

			skill.Run(this.HandleCasting(caster, skill));
		}

		private async Task HandleCasting(ICombatEntity caster, Skill skill)
		{
			var pads = skill.Vars.Get<List<Pad>>(VarPads);
			var castTimeMs = skill.Vars.Get<float>(VarCastTimeMs);

			var delayBetweenOrbs = TimeSpan.FromMilliseconds(castTimeMs / OrbCount);

			for (var i = 0; i < OrbCount; i++)
			{
				if (!skill.Vars.Get<bool>(VarIsCasting))
					break;

				var position = caster.Position.GetRandomInRange2D(20, 45);
				pads.Add(SkillCreatePad(caster, skill, position, 0f, PadName.Elementalist_ChainReaction_Pad));
				await skill.Wait(delayBetweenOrbs);
			}
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			skill.Vars.Set(VarIsCasting, false);
			caster.RemoveBuff(BuffId.Chainreaction_Runpad_Buff);
			caster.StopSound("skl_eff_lightningsphere_cast", "skl_eff_lightningsphere_cast");
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, Position.Zero);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = targets.FirstOrDefault()?.Handle ?? 0;

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			var padsSnapshot = skill.Vars.Get<List<Pad>>(VarPads);
			var padsToLaunch = padsSnapshot != null ? new List<Pad>(padsSnapshot) : new List<Pad>();
			skill.Run(this.HandleSkill(caster, skill, padsToLaunch));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, List<Pad> pads)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(700));

			foreach (var pad in pads)
			{
				if (pad.IsDead)
					continue;

				var target = pad.Map.GetNearestAttackableEntity(caster, pad.Position, 200);
				if (target != null)
					pad.SetDestPos(target.Position, 350, 100, false);
			}
		}
	}
}
