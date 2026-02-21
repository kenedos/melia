using System;

namespace Melia.Zone.Skills.Helpers
{
	/// <summary>
	/// Contains constants used across skill and pad helper functions.
	/// Extracted from magic numbers to improve maintainability and readability.
	/// </summary>
	public static class SkillConstants
	{
		#region AOE Parameters

		/// <summary>
		/// Default AOE range for skills without explicit range.
		/// </summary>
		public const float DefaultAoeRange = 20f;

		/// <summary>
		/// Default target count for single-target skills.
		/// </summary>
		public const int DefaultTargetCount = 1;

		/// <summary>
		/// Default hit delay in milliseconds.
		/// </summary>
		public const float DefaultHitDelayMs = 500f;

		/// <summary>
		/// Default attack ratio multiplier.
		/// </summary>
		public const float DefaultAttackRatio = 1.0f;

		#endregion

		#region Knockback/Knockdown

		/// <summary>
		/// Default knockback power.
		/// </summary>
		public const float DefaultKnockbackPower = 150f;

		/// <summary>
		/// Default knockdown vertical angle in degrees.
		/// </summary>
		public const float DefaultKnockdownVerticalAngle = 45f;

		/// <summary>
		/// Default knockdown horizontal angle in degrees.
		/// </summary>
		public const float DefaultKnockdownHorizontalAngle = 0f;

		#endregion

		#region Debug Preview

		/// <summary>
		/// Maximum shoot time threshold for debug preview (5 seconds).
		/// </summary>
		public static readonly TimeSpan MaxShootTimeForPreview = TimeSpan.FromSeconds(5);

		/// <summary>
		/// Default debug shape duration when shoot time exceeds threshold (3 seconds).
		/// </summary>
		public static readonly TimeSpan DefaultDebugShapeDuration = TimeSpan.FromSeconds(3);

		/// <summary>
		/// Short debug shape duration for pad previews (1 second).
		/// </summary>
		public static readonly TimeSpan ShortDebugShapeDuration = TimeSpan.FromSeconds(1);

		/// <summary>
		/// Maximum pad shoot time threshold for debug preview (3 seconds).
		/// </summary>
		public static readonly TimeSpan MaxPadShootTimeForPreview = TimeSpan.FromSeconds(3);

		#endregion

		#region Link System

		/// <summary>
		/// Vertical distance limit for standard links.
		/// </summary>
		public const float LinkVerticalDistanceLimit = 25f;

		/// <summary>
		/// Vertical distance limit for party links.
		/// </summary>
		public const float PartyLinkVerticalLimit = 35f;

		#endregion

		#region Casting

		/// <summary>
		/// Default maximum charge time in seconds.
		/// </summary>
		public const float DefaultMaxChargeTimeSeconds = 3.0f;

		/// <summary>
		/// Default casting animation time in seconds.
		/// </summary>
		public const float DefaultCastAnimationSeconds = 1.0f;

		#endregion

		#region Pad Defaults

		/// <summary>
		/// Default concurrent use count for pads.
		/// </summary>
		public const int DefaultConcurrentUseCount = 1;

		/// <summary>
		/// Default pad lifetime in milliseconds.
		/// </summary>
		public const float DefaultPadLifetimeMs = 10000f;

		/// <summary>
		/// Threshold for treating lifetime as seconds vs milliseconds (300ms).
		/// Values above this are treated as milliseconds, below as seconds.
		/// </summary>
		public const float PadLifetimeThresholdMs = 300f;

		/// <summary>
		/// Duration for invisible effect on pad monsters (30 seconds).
		/// </summary>
		public const float InvisibleEffectDurationSeconds = 30f;

		#endregion

		#region Damage Over Time

		/// <summary>
		/// Minimum sleep delay for damage over time in milliseconds.
		/// </summary>
		public const int MinDotSleepDelayMs = 100;

		/// <summary>
		/// Default dot effect scale.
		/// </summary>
		public const float DefaultDotEffectScale = 1.0f;

		#endregion

		#region Diminishing Returns

		/// <summary>
		/// Window duration for diminishing returns tracking in milliseconds.
		/// </summary>
		public const float DiminishingReturnsWindowMs = 15000f;

		/// <summary>
		/// Reduction percentage per stack for diminishing returns (25%).
		/// </summary>
		public const float DiminishingReturnsReductionPerStack = 0.25f;

		/// <summary>
		/// Maximum stacks before immunity is granted.
		/// </summary>
		public const int DiminishingReturnsMaxStacks = 3;

		#endregion

		#region Summon/Monster Defaults

		/// <summary>
		/// Default summon lifetime in minutes.
		/// </summary>
		public const float DefaultSummonLifetimeMinutes = 3f;

		/// <summary>
		/// Default spawn range for created monsters.
		/// </summary>
		public const int DefaultMonsterSpawnRange = 1;

		#endregion

		#region Skill Effect

		/// <summary>
		/// Default effect scale multiplier.
		/// </summary>
		public const float DefaultEffectScale = 1.0f;

		/// <summary>
		/// Default ground effect lifetime in seconds.
		/// </summary>
		public const float DefaultGroundEffectLifetimeSeconds = 1.0f;

		#endregion
	}
}
