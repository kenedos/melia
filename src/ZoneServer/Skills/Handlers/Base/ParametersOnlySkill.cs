using System;
using Melia.Shared.Data.Database;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Base
{
	/// <summary>
	/// A base class for skills that can be handled entirely by their
	/// delays and splash parameters.
	/// </summary>
	public abstract class ParametersOnlySkill : SimpleMonsterAttackSkill
	{
		protected abstract TimeSpan AniTime { get; }
		protected abstract TimeSpan HitDelay { get; }
		protected abstract SplashType SplashType { get; }
		protected abstract float Length { get; }
		protected abstract float Width { get; }
		protected abstract float Angle { get; }

		protected override TimeSpan GetAniTime(Skill skill)
			=> this.AniTime;

		protected override TimeSpan GetHitDelay(Skill skill)
			=> this.HitDelay;

		protected override ISplashArea GetSplashArea(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			var skillParam = skill.GetSplashParameters(caster, caster.Position, target.Position, this.Length, this.Width, this.Angle);
			var splashArea = skill.GetSplashArea(this.SplashType, skillParam);

			return splashArea;
		}
	}
}
