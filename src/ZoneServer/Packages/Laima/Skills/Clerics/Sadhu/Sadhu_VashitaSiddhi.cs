using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Clerics.Sadhu
{
	/// <summary>
	/// Handler for the Sadhu skill Vashita Siddhi.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Sadhu_VashitaSiddhi)]
	public class Sadhu_VashitaSiddhiOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(500);
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPos))
			{
				caster.ServerMessage(Localization.Get("No target location specified."));
				return;
			}
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

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos, targetPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos, Position targetPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 30, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 300;
			var damageDelay = 500;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			caster.StartBuff(BuffId.Sadhu_Soul_Pre_Buff, 1f, 0f, TimeSpan.Zero, caster);
			await skill.Wait(TimeSpan.FromMilliseconds(600));
			//var position = GetRelativePosition(PosType.TargetFront, caster, target);
			await EffectAndHit(skill, caster, targetPos, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_explosion098_dark_mint", 1.2f),
				Range = 100f,
				KnockdownPower = 0f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			}, hits);
			var time = 0;
			var over = 0;
			if (caster.TryGetBuff(BuffId.Sadhu_Soul_Buff, out var buff))
				over = buff.OverbuffCounter;
			if (!caster.IsBuffActive(BuffId.ITEM_BUFF_VIBORA_THMACE_SADHU))
			{
				if (over > 0)
					time = over * 500;
			}
			else if (caster.IsBuffActive(BuffId.ITEM_BUFF_VIBORA_THMACE_SADHU))
			{
				if (over > 2)
					over = 2;
				time = over * 1250;
			}
			if (caster.Map.IsPVP)
				time /= 2;
			SkillResultTargetBuff(caster, skill, BuffId.Stun, 1, 0f, time, 0, 100, -1, hits);
		}
	}
}
