using System;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Scouts.Rogue
{
	/// <summary>
	/// Handler for the Rogue skill Lachrymator.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Rogue_Lachrymator)]
	public class Rogue_LachrymatorOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
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
			var isEscape = caster.IsAbilityActive(AbilityId.Rogue27);
			var targetPos = caster.Position;

			if (!isEscape)
			{
				if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out targetPos))
				{
					caster.ServerMessage(Localization.Get("No target location specified."));
					return;
				}
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, targetPos);

			skill.Run(this.HandleSkill(skill, caster, targetPos, isEscape));
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster, Position targetPos, bool isEscape)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(250));
			var value = PadName.lachrymator_pad;
			if (caster.IsAbilityActive(AbilityId.Rogue9))
				value = PadName.Rogue_Lachrymator_abil;
			if (caster.IsAbilityActive(AbilityId.Rogue26))
				value = PadName.lachrymator_pad_Rogue26;

			if (isEscape)
			{
				SkillCreatePad(caster, skill, caster.Position, 0f, value);
				caster.StartBuff(BuffId.Sprint_Buff, 10, 0f, TimeSpan.FromSeconds(1), caster);
			}
			else
			{
				await MissilePadThrow(skill, caster, targetPos, new MissileConfig
				{
					Effect = new EffectConfig("I_archer_Lachrymator_force_mash#Bip01 R Hand", 0.6f),
					EndEffect = new EffectConfig("I_bomb003_dark", 1f),
					DotEffect = EffectConfig.None,
					Range = 0f,
					FlyTime = 0.5f,
					DelayTime = 0f,
					Gravity = 300f,
					Speed = 1f,
					HitTime = 200f,
					HitCount = 0,
					GroundEffect = EffectConfig.None,
					GroundDelay = 0f,
					EffectMoveDelay = 0f,
				}, 0f, value);
			}
		}
	}
}
