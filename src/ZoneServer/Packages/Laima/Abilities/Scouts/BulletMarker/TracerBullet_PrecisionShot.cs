using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Scouts.Bulletmarker
{
	/// <summary>
	/// Bulletmarker14 - Tracer Bullet: Precision Shot.
	///
	/// Effect:
	/// - Increases the Critical Rate bonus of Tracer Bullet by 4% per level.
	/// - Maximum level: 5.
	/// - Maximum additional Critical Rate: 20%.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Bulletmarker14)]
	public class BulletMarker_TracerBulletPrecisionShotAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
			this.RefreshCriticalRate(character, ability, true);
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
			this.RefreshCriticalRate(character, ability, false);
		}

		private void RefreshCriticalRate(Character character, Ability ability, bool precisionShotActive)
		{
			if (!character.TryGetBuff(BuffId.TracerBullet_Buff, out var tracerBulletBuff))
				return;

			if (!character.TryGetSkill(SkillId.Bulletmarker_TracerBullet, out var tracerBulletSkill) || tracerBulletSkill.Level <= 0)
				return;

			var oldCriticalRateBonus = tracerBulletBuff.NumArg3;

			if (oldCriticalRateBonus != 0)
				character.Properties.Modify(PropertyName.CRTHR_BM, -oldCriticalRateBonus);

			var tracerBulletLevel = Math.Clamp(tracerBulletSkill.Level, 1, 10);
			var criticalRateBonusRate = tracerBulletLevel * 0.03f;

			if (precisionShotActive)
			{
				var precisionShotLevel = Math.Clamp(ability.Level, 1, 5);
				criticalRateBonusRate += precisionShotLevel * 0.04f;
			}

			var baseCriticalRate = character.Properties.GetFloat(PropertyName.CRTHR);
			var newCriticalRateBonus = baseCriticalRate * criticalRateBonusRate;

			tracerBulletBuff.NumArg3 = newCriticalRateBonus;

			character.Properties.Modify(PropertyName.CRTHR_BM, newCriticalRateBonus);
		}
	}
}
