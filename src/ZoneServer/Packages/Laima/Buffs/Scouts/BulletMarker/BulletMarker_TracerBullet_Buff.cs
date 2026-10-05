using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Buffs.Scouts.Bulletmarker
{
	/// <summary>
	/// Handler for Tracer Bullet passive buff.
	/// Accuracy: +3% per skill level.
	/// Critical Rate: +3% per skill level.
	/// Lv10: +30% Accuracy and +30% Critical Rate.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.TracerBullet_Buff)]
	public class BulletMarker_TracerBullet_Buff : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is not Character character)
				return;

			var skillLevel = Math.Clamp((int)buff.NumArg1, 1, 10);
			var accuracyBonusRate = skillLevel * 0.03f;
			var criticalRateBonusRate = skillLevel * 0.03f;

			if (character.Abilities.TryGet(AbilityId.Bulletmarker14, out var precisionShot) && precisionShot.Active)
			{
				var precisionShotLevel = Math.Clamp(precisionShot.Level, 1, 5);
				criticalRateBonusRate += precisionShotLevel * 0.04f;
			}

			var currentAccuracy = character.Properties.GetFloat(PropertyName.HR);
			var currentCriticalRate = character.Properties.GetFloat(PropertyName.CRTHR);
			var accuracyBonus = currentAccuracy * accuracyBonusRate;
			var criticalRateBonus = currentCriticalRate * criticalRateBonusRate;

			buff.NumArg2 = accuracyBonus;
			buff.NumArg3 = criticalRateBonus;

			character.Properties.Modify(PropertyName.HR_BM, accuracyBonus);
			character.Properties.Modify(PropertyName.CRTHR_BM, criticalRateBonus);

			//Send.ZC_OBJECT_PROPERTY(character, PropertyName.HR, PropertyName.HR_BM);
			//Send.ZC_OBJECT_PROPERTY(character, PropertyName.CRTHR, PropertyName.CRTHR_BM);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is not Character character)
				return;

			character.Properties.Modify(PropertyName.HR_BM, -buff.NumArg2);
			character.Properties.Modify(PropertyName.CRTHR_BM, -buff.NumArg3);

			//Send.ZC_OBJECT_PROPERTY(character, PropertyName.HR, PropertyName.HR_BM);
			//Send.ZC_OBJECT_PROPERTY(character, PropertyName.CRTHR, PropertyName.CRTHR_BM);
		}
	}
}
