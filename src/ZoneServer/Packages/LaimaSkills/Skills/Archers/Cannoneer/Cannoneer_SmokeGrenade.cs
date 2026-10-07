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

namespace Melia.Zone.Skills.Handlers.Archers.Cannoneer
{
	/// <summary>
	/// Handler for the Cannoneer skill Smoke Grenade, which throws a
	/// grenade whose smoke blinds the enemies in it and reveals the hidden
	/// ones.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Cannoneer_SmokeGrenade)]
	public class Cannoneer_SmokeGrenadeOverride : IGroundSkillHandler, IDynamicCasted
	{
		private static readonly TimeSpan ThrowDelay = TimeSpan.FromMilliseconds(450);

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
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, targetPos, ForceId.GetNew(), null);

			skill.Run(this.Throw(skill, caster, targetPos));
		}

		/// <summary>
		/// Throws the grenade at the target position.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="targetPos"></param>
		/// <returns></returns>
		private async Task Throw(Skill skill, ICombatEntity caster, Position targetPos)
		{
			await skill.Wait(ThrowDelay);

			await MissilePadThrow(skill, caster, targetPos, new MissileConfig
			{
				Effect = new EffectConfig("I_archer_Lachrymator_force_mash#Dummy_effect_canon", 0.6f),
				EndEffect = new EffectConfig("I_bomb003_dark", 1f),
				DotEffect = EffectConfig.None,
				Range = 0f,
				FlyTime = 0.3f,
				DelayTime = 0f,
				Gravity = 200f,
				Speed = 1f,
				HitTime = 0f,
				HitCount = 0,
				GroundEffect = EffectConfig.None,
				GroundDelay = 0f,
				EffectMoveDelay = 0f,
			}, 0f, PadName.Cannoneer_SmokeGrenade);
		}
	}
}
