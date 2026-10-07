using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Clerics.Miko
{
	/// <summary>
	/// Handler for the Miko skill Hamaya, which throws a sacred arrow whose
	/// circle burns the enemies around it with Holy damage every second and
	/// lowers their critical resistance.
	/// </summary>
	/// <remarks>
	/// [Arts] Hamaya: Heal makes the circle heal the party instead.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Miko_Hamaya)]
	public class Miko_HamayaOverride : IGroundSkillHandler, IDynamicCasted
	{
		private static readonly TimeSpan ThrowDelay = TimeSpan.FromMilliseconds(650);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
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

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, targetPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, targetPos, caster.Direction, targetPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, targetPos, ForceId.GetNew(), null);

			skill.Run(this.Throw(skill, caster, targetPos));
		}

		/// <summary>
		/// Throws the arrow at the target position.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="targetPos"></param>
		/// <returns></returns>
		private async Task Throw(Skill skill, ICombatEntity caster, Position targetPos)
		{
			await skill.Wait(ThrowDelay);

			var padName = caster.IsAbilityActive(AbilityId.Miko13) ? PadName.Miko_Hamaya_Abil : PadName.Miko_Hamaya;

			await MissilePadThrow(skill, caster, targetPos, new MissileConfig
			{
				Effect = new EffectConfig("I_cleric_hamaya_arrow#Dummy_force_hamaya", 1f),
				EndEffect = EffectConfig.None,
				DotEffect = EffectConfig.None,
				Range = 10f,
				FlyTime = 0.1f,
				DelayTime = 0f,
				Gravity = 400f,
				Speed = 1f,
				HitTime = 0f,
				HitCount = 0,
				GroundEffect = EffectConfig.None,
				GroundDelay = 0f,
				EffectMoveDelay = 0f,
			}, 0f, padName);
		}
	}
}
