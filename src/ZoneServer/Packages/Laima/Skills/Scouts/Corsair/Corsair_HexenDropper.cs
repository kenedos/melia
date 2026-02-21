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
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Scouts.Corsair
{
	/// <summary>
	/// Handler for the Corsair skill Hexen Dropper.
	/// Multi-hit forward dash attack with 7 hits.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Corsair_HexenDropper)]
	public class Corsair_HexenDropperOverride : IMeleeGroundSkillHandler
	{
		private const float SplashLength = 70f;
		private const float SplashWidth = 20f;
		private const float SplashAngle = 10f;
		private const int HitCount = 9;
		private const float JollyRogerDamageBonus = 0.2f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(skill, caster, originPos, farPos));
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			skill.Run(this.PlayDashEffects(skill, caster));

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: SplashLength, width: SplashWidth, angle: SplashAngle);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);

			var baseDelay = 200;
			for (var i = 0; i < HitCount; i++)
			{
				var hitDelay = i == 0 ? 0 : 50;
				var damageDelay = baseDelay + (i * 50);
				await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, null, this.ModifyDamage);
			}
		}

		private SkillHitResult ModifyDamage(Skill skill, ICombatEntity caster, ICombatEntity target, SkillHitResult skillHitResult)
		{
			if (target.IsBuffActive(BuffId.JollyRoger_Enemy_Debuff))
				skillHitResult.Damage *= 1 + JollyRogerDamageBonus;

			return skillHitResult;
		}

		private async Task PlayDashEffects(Skill skill, ICombatEntity caster)
		{
			var position = GetRelativePosition(PosType.Self, caster, caster, distance: 15, height: 20);
			caster.PlayGroundEffect(position, "I_warrior_florysh_shot_dash_ride_short", 0.6f, 0f, 0f, caster.Direction.DegreeAngle);
			await skill.Wait(TimeSpan.FromMilliseconds(100));

			position = GetRelativePosition(PosType.Self, caster, caster, distance: 20, angle: 0f, height: 15);
			caster.PlayGroundEffect(position, "I_warrior_florysh_shot_dash_ride_short", 0.5f, 0f, 0f, caster.Direction.DegreeAngle);
			await skill.Wait(TimeSpan.FromMilliseconds(100));

			position = GetRelativePosition(PosType.Self, caster, caster, distance: 20, angle: 572f, height: 20);
			caster.PlayGroundEffect(position, "I_warrior_florysh_shot_dash_ride_short", 0.6f, 0f, 0f, caster.Direction.DegreeAngle);
			await skill.Wait(TimeSpan.FromMilliseconds(100));

			position = GetRelativePosition(PosType.Self, caster, caster, distance: 15, angle: 0f, height: 15);
			caster.PlayGroundEffect(position, "I_warrior_florysh_shot_dash_ride_short", 0.7f, 0f, 0f, caster.Direction.DegreeAngle);
			await skill.Wait(TimeSpan.FromMilliseconds(100));

			position = GetRelativePosition(PosType.Self, caster, caster, distance: 15, angle: 859f, height: 15);
			caster.PlayGroundEffect(position, "I_warrior_florysh_shot_dash_ride_short", 0.6f, 0f, 0f, caster.Direction.DegreeAngle);
			await skill.Wait(TimeSpan.FromMilliseconds(100));

			position = GetRelativePosition(PosType.Self, caster, caster, distance: 15, height: 20);
			caster.PlayGroundEffect(position, "I_warrior_florysh_shot_dash_ride_short", 0.7f, 0f, 0f, caster.Direction.DegreeAngle);

			position = GetRelativePosition(PosType.Self, caster, caster, distance: 20, angle: 0f, height: 15);
			caster.PlayGroundEffect(position, "I_warrior_florysh_shot_dash_ride_short", 0.5f, 0f, 0f, caster.Direction.DegreeAngle);
			await skill.Wait(TimeSpan.FromMilliseconds(100));

			position = GetRelativePosition(PosType.Self, caster, caster, distance: 20, angle: 572f, height: 20);
			caster.PlayGroundEffect(position, "I_warrior_florysh_shot_dash_ride_short", 0.6f, 0f, 0f, caster.Direction.DegreeAngle);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
		}
	}
}
