using Melia.Zone.Skills.Combat;

namespace Melia.Zone.Skills.Helpers
{
	/// <summary>
	/// Groups visual effect parameters used across skill helper methods.
	/// </summary>
	public struct EffectConfig
	{
		/// <summary>
		/// The effect name.
		/// </summary>
		public string Name;

		/// <summary>
		/// The effect scale.
		/// </summary>
		public float Scale;

		/// <summary>
		/// No effect.
		/// </summary>
		public static readonly EffectConfig None = new() { Name = "None", Scale = 1f };

		public EffectConfig(string name, float scale = 1f)
		{
			Name = name;
			Scale = scale;
		}
	}

	/// <summary>
	/// Configuration for missile-type skills (MissileFall, MissileThrow, etc).
	/// </summary>
	public struct MissileConfig
	{
		/// <summary>
		/// Main projectile effect.
		/// </summary>
		public EffectConfig Effect;

		/// <summary>
		/// Effect displayed on impact.
		/// </summary>
		public EffectConfig EndEffect;

		/// <summary>
		/// Effect displayed during damage-over-time ticks.
		/// </summary>
		public EffectConfig DotEffect;

		/// <summary>
		/// Ground/decal effect (displayed at the landing position).
		/// </summary>
		public EffectConfig GroundEffect;

		/// <summary>
		/// Additional effect played at the target position before the
		/// projectile launches.
		/// </summary>
		public EffectConfig TargetEffect;

		/// <summary>
		/// Duration for the target effect display.
		/// </summary>
		public float TargetEffectDuration;

		/// <summary>
		/// Splash radius of the impact area.
		/// </summary>
		public float Range;

		/// <summary>
		/// Time in seconds for the projectile to travel.
		/// </summary>
		public float FlyTime;

		/// <summary>
		/// Delay in seconds before the projectile launches.
		/// </summary>
		public float DelayTime;

		/// <summary>
		/// Gravity applied to the projectile arc.
		/// </summary>
		public float Gravity;

		/// <summary>
		/// Projectile travel speed.
		/// </summary>
		public float Speed;

		/// <summary>
		/// Maximum height of the projectile arc.
		/// </summary>
		public float Height;

		/// <summary>
		/// Easing factor for the projectile trajectory.
		/// </summary>
		public float Easing;

		/// <summary>
		/// Starting easing factor.
		/// </summary>
		public float StartEasing;

		/// <summary>
		/// Offset applied to the start of hit timing.
		/// </summary>
		public float HitStartFix;

		/// <summary>
		/// Number of damage ticks on impact.
		/// </summary>
		public int HitCount;

		/// <summary>
		/// Duration of each hit tick in milliseconds.
		/// </summary>
		public float HitTime;

		/// <summary>
		/// Delay before the effect starts moving.
		/// </summary>
		public float EffectMoveDelay;

		/// <summary>
		/// Delay before the ground effect triggers, in seconds.
		/// </summary>
		public float GroundDelay;

		/// <summary>
		/// Knockdown/knockback power.
		/// </summary>
		public float KnockdownPower;

		/// <summary>
		/// Type of knockback effect.
		/// </summary>
		public KnockType KnockType;

		/// <summary>
		/// Vertical angle for knockback direction.
		/// </summary>
		public float VerticalAngle;

		/// <summary>
		/// Inner range for donut-shaped splash areas.
		/// </summary>
		public float InnerRange;
	}

	/// <summary>
	/// Configuration for arrow-type line effects (EffectHitArrow).
	/// </summary>
	public struct ArrowConfig
	{
		/// <summary>
		/// Arrow trail effect.
		/// </summary>
		public EffectConfig ArrowEffect;

		/// <summary>
		/// Effect displayed at each hit point.
		/// </summary>
		public EffectConfig HitEffect;

		/// <summary>
		/// Spacing between arrow segments.
		/// </summary>
		public float ArrowSpacing;

		/// <summary>
		/// Time spacing between arrow segments.
		/// </summary>
		public float ArrowSpacingTime;

		/// <summary>
		/// How long the arrow effect persists.
		/// </summary>
		public float ArrowLifeTime;

		/// <summary>
		/// Delay before hit processing begins.
		/// </summary>
		public float PositionDelay;

		/// <summary>
		/// Splash radius at each hit point.
		/// </summary>
		public float Range;

		/// <summary>
		/// Knockdown/knockback power.
		/// </summary>
		public float KnockdownPower;

		/// <summary>
		/// Delay before damage application.
		/// </summary>
		public float Delay;

		/// <summary>
		/// Spacing between hit effect displays.
		/// </summary>
		public float HitEffectSpacing;

		/// <summary>
		/// Time spacing between hit ticks.
		/// </summary>
		public float HitTimeSpacing;

		/// <summary>
		/// Number of damage ticks per hit point.
		/// </summary>
		public int HitCount;

		/// <summary>
		/// Duration of hit processing.
		/// </summary>
		public float HitDuration;

		/// <summary>
		/// Type of knockback effect.
		/// </summary>
		public KnockType KnockType;

		/// <summary>
		/// Vertical angle for knockback direction.
		/// </summary>
		public float VerticalAngle;
	}

	/// <summary>
	/// Configuration for ground effect + hit skills (EffectAndHit).
	/// </summary>
	public struct EffectHitConfig
	{
		/// <summary>
		/// Ground/decal effect shown at the target position.
		/// </summary>
		public EffectConfig GroundEffect;

		/// <summary>
		/// Delay in milliseconds before the main effect plays.
		/// </summary>
		public int PositionDelay;

		/// <summary>
		/// Main visual effect at the hit position.
		/// </summary>
		public EffectConfig Effect;

		/// <summary>
		/// Splash radius of the hit area.
		/// </summary>
		public float Range;

		/// <summary>
		/// Knockdown/knockback power.
		/// </summary>
		public float KnockdownPower;

		/// <summary>
		/// Delay in milliseconds before damage starts.
		/// </summary>
		public float Delay;

		/// <summary>
		/// Number of damage ticks.
		/// </summary>
		public int HitCount;

		/// <summary>
		/// Duration of each hit tick in milliseconds.
		/// </summary>
		public float HitDuration;

		/// <summary>
		/// Effect played on the caster.
		/// </summary>
		public EffectConfig CasterEffect;

		/// <summary>
		/// Bone/node name for the caster effect attachment.
		/// </summary>
		public string CasterNodeName;

		/// <summary>
		/// Type of knockback effect.
		/// </summary>
		public int KnockType;

		/// <summary>
		/// Vertical angle for knockback direction.
		/// </summary>
		public float VerticalAngle;

		/// <summary>
		/// Inner range for donut-shaped splash areas.
		/// </summary>
		public float InnerRange;
	}
}
