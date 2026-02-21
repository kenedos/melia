using System;
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

namespace Melia.Zone.Skills.Handlers.Wizards.Elementalist
{
	/// <summary>
	/// Handler override for the Elementalist skill Storm Dust (Blizzard Storm).
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Elementalist_StormDust)]
	public class Elementalist_StormDustOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(300);

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
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

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
			var forceId = ForceId.GetNew();

			Send.ZC_SKILL_READY(caster, skill, skillHandle, caster.Position, targetPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, targetPos, caster.Position.GetDirection(targetPos), targetPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, targetPos, forceId, null);

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos, targetPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos, Position targetPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 30, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 0;
			var damageDelay = 300;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var value = PadName.StormDust_Pad;
			if (caster.IsAbilityActive(AbilityId.Elementalist40))
				value = PadName.StormDust_Abil_Pad;
			SkillCreatePad(caster, skill, targetPos, 0f, value);
		}
	}
}
