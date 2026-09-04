using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Melia.Test.Balance.Sfr
{
	/// <summary>
	/// Prices a skill's SFR from its data and handler alone, with no prior
	/// factor.
	/// </summary>
	/// <remarks>
	/// Implements the model in SFR_OVERHAUL.md: a press is worth the basic
	/// attacks its whole cycle costs you, times how much better than swinging
	/// it is, divided by how wide it reaches. Nothing here reads the skill's
	/// current factor, so the pass is idempotent and does not compound with
	/// itself.
	/// </remarks>
	public static class SfrPricer
	{
		/// <summary>
		/// How long the last roster run spent building its arenas, which is
		/// CPU rather than the waiting the rest of a run is.
		/// </summary>
		public static TimeSpan LastPoolBuildTime { get; private set; }

		/// <summary>
		/// How long the last roster run spent actually measuring presses.
		/// </summary>
		public static TimeSpan LastMeasureTime { get; private set; }

		private static float? _measuredCalibration;
		private static SfrMeasuredPress _anchorMeasurement;
		private static readonly object _syncLock = new();

		/// <summary>
		/// The measured press for the anchor skill, once one has been taken.
		/// </summary>
		public static SfrMeasuredPress AnchorMeasurement
		{
			get
			{
				lock (_syncLock)
					return _anchorMeasurement;
			}
		}

		/// <summary>
		/// Registers a measured press for the anchor skill, so measured prices
		/// calibrate against a measured anchor.
		/// </summary>
		/// <param name="measured"></param>
		public static void SetAnchorMeasurement(SfrMeasuredPress measured)
		{
			if (measured != null && measured.Skill != SfrDials.AnchorSkill)
				throw new ArgumentException($"The anchor is '{SfrDials.AnchorSkill}', not '{measured.Skill}'.", nameof(measured));

			// The anchor is priced without its rider, and that is what makes
			// the roster's level reproducible. Every other term in a press is
			// exact now; the defensive probe is the one that still moves,
			// because the mob acts through its AI. Through the anchor that
			// wobble is not one skill's price - the scale multiplies all 106,
			// so a knockdown on Swordman_Bash reading 0.33 swings on one run
			// and 0.53 on the next moved the whole roster by 15%. It also
			// restores what the model always intended: the anchor measured
			// 0.10 swings and took no rider at all, until a longer defence
			// window revealed the real value. A rider on the anchor is a level
			// shift, not a discount, since Calibration pins it to AnchorFactor
			// either way.
			if (measured != null)
			{
				measured.SwingsPrevented = 0f;
				measured.DamageAmplification = 0f;
			}

			lock (_syncLock)
			{
				_anchorMeasurement = measured;
				_measuredCalibration = null;
			}
		}

		/// <summary>
		/// Returns the scale that puts the anchor skill back on its factor.
		/// </summary>
		/// <remarks>
		/// Every term in the model is a ratio, so one anchor sets the level for
		/// the roster. Charging for width divides every price by a number above
		/// one, which would drop the whole roster rather than redistribute it.
		/// There is one calibration now, against the measured anchor - nothing
		/// prices without a measured press any more, so there is no second
		/// scale to keep in sync with it.
		/// </remarks>
		public static float Calibration()
		{
			lock (_syncLock)
			{
				if (_measuredCalibration != null)
					return _measuredCalibration.Value;

				if (_anchorMeasurement == null)
					throw new InvalidOperationException("Calibration: the anchor has not been measured yet (SetAnchorMeasurement).");

				// Priced uncalibrated first, which is what the scale is measured against.
				_measuredCalibration = 1f;

				var scale = 1f;

				try
				{
					// Against RawFactor, not Factor: the scale is what puts the
					// anchor back on its factor, and dividing by the rounded
					// int leaves the anchor landing a point either side of it.
					var raw = Price(SfrDials.AnchorSkill, null, _anchorMeasurement);
					scale = SfrDials.AnchorFactor / Math.Max(raw.RawFactor, 1e-6f);
				}
				catch (Exception)
				{
					scale = 1f;
				}

				_measuredCalibration = scale;

				return scale;
			}
		}

		/// <summary>
		/// Picks the anchor measurement that prices in the middle, and
		/// calibrates on it.
		/// </summary>
		/// <remarks>
		/// The anchor's price is pinned to AnchorFactor whatever it measures,
		/// so its own noise is invisible in its own number and lands instead in
		/// the scale every other skill is multiplied by. Taking the median of
		/// several presses is what stops one unlucky anchor window moving the
		/// whole roster the other way.
		/// </remarks>
		/// <param name="candidates"></param>
		public static void CalibrateOnMedian(IEnumerable<SfrMeasuredPress> candidates)
		{
			var scored = new List<(SfrMeasuredPress Press, float Raw)>();

			foreach (var candidate in candidates)
			{
				if (candidate == null)
					continue;

				try
				{
					// Priced against a scale of one, which is what
					// Calibration() measures its own scale against.
					lock (_syncLock)
					{
						_anchorMeasurement = candidate;
						_measuredCalibration = 1f;
					}

					scored.Add((candidate, Price(SfrDials.AnchorSkill, null, candidate).RawFactor));
				}
				catch (Exception)
				{
					// An anchor press that cannot be priced is no anchor.
				}
			}

			if (scored.Count == 0)
				throw new InvalidOperationException($"{SfrDials.AnchorSkill}: no anchor press could be priced.");

			scored.Sort((a, b) => a.Raw.CompareTo(b.Raw));

			SetAnchorMeasurement(scored[scored.Count / 2].Press);
		}

		/// <summary>
		/// Returns the absolute SFR for a skill, from its data and a measured
		/// press only. Nothing here reads the handler's source.
		/// </summary>
		/// <remarks>
		/// Every input that used to come from regex-scanning the handler -
		/// hit count, pad/buff/DoT ticks, reach, rider value, whether a cast
		/// is really a channel - is measured instead: hit count and reach
		/// from SkillPressProbe, the defensive rider from SfrDefenseProbe's
		/// live control/treatment pair and the amplifier rider from
		/// SfrOffenseProbe's. What is left is the policy layer alone -
		/// the cooldown curve, cast-time premiums, circle premium, width
		/// exponent - applied to what the press actually did.
		/// </remarks>
		/// <param name="skillName"></param>
		/// <param name="maxLevel"></param>
		/// <param name="measured"></param>
		public static SfrPrice Price(string skillName, int? maxLevel, SfrMeasuredPress measured)
		{
			if (measured == null)
				throw new ArgumentNullException(nameof(measured), $"{skillName}: nothing prices without a measured press.");

			if (!SfrData.Skills.TryGetValue(skillName, out var entry))
				throw new KeyNotFoundException($"No skill data for '{skillName}'.");

			var cls = SfrData.ClassOf(skillName);

			if (!SfrData.BaseJob.ContainsKey(cls))
				throw new KeyNotFoundException($"{skillName}: '{cls}' is out of scope.");

			// A heal pad's factor is a heal factor, not a damage factor, so
			// whatever chip damage it deals to undead is not what the number
			// means. Pricing it as a damage skill reads that chip damage
			// against a basic swing and returns nonsense.
			if (IsHealSkill(skillName))
				throw new InvalidOperationException($"{skillName}: heals, this model does not price healing.");

			// A measurement that landed nothing anywhere is an unobserved
			// press, not a narrow one, and its reach of zero would divide the
			// price into the millions.
			if (!measured.Delivered)
				throw new InvalidOperationException($"{skillName}: the measured press damaged nothing in any scenario.");

			// The hit count has to come from the HP the mobs actually lost;
			// the fallback of 1 is a guess, and a guess divided by a near-zero
			// reach is what prices a press into the thousands.
			if (!measured.HitsFromDamage)
				throw new InvalidOperationException($"{skillName}: no factor-scaled HP loss was observed, so the press could not be measured.");

			// A press still delivering when the window closed without its own
			// cycle having elapsed delivered more than was counted, so its hit
			// count is a floor and the price a ceiling. One that outlives only
			// its cycle is counted over the cycle and priced.
			if (measured.Truncated)
				throw new InvalidOperationException($"{skillName}: the press was still delivering when the {SkillPressProbe.MaxPressMs} ms window closed and its cycle had not elapsed, so its hit count is truncated.");

			var levels = maxLevel ?? SfrData.SkillMaxLevel(skillName);
			var cast = entry.Num("basicCast") / 1000f;

			// A channel delivers damage for as long as it runs rather than
			// landing one hit at the end of a wind-up, decided purely from
			// how long the data says the press is held - shootTime past the
			// channel threshold, with no cast committing to it first.
			var channel = IsChannel(entry);

			var (t, rawOccupancy, cycle) = SfrData.PressWindow(entry);

			var hits = Math.Max(1f, measured.HitEquivalents);

			// The defensive rider, measured rather than named: what a
			// live, hostile mob's own output dropped by after the press,
			// in units of the caster's own basic swing. Silent - a
			// multiplier of 1 - for any press SfrDefenseProbe found nothing
			// on.
			//
			// Floored, because DefenseValueScale was calibrated at a quarter
			// of a swing prevented and the hyperbola is being read at ten:
			// nothing measured says what a press that locks a mob down for a
			// whole window is worth, and unfloored it takes the price to zero.
			var riderMultiplier = RiderMultiplier(measured.SwingsPrevented);

			// The same trade on the attacking side: what the press added to
			// every other press is budget it did not spend on its own number.
			var amplifierMultiplier = AmplifierMultiplier(measured.DamageAmplification);

			var riderKinds = new List<string>();

			if (measured.SwingsPrevented > 0)
				riderKinds.Add($"defensive (measured, {measured.SwingsPrevented:0.00} swings prevented)");

			if (measured.DamageAmplification > 0)
				riderKinds.Add($"amplifier (measured, +{measured.DamageAmplification:P0} on other damage, x{amplifierMultiplier:0.00})");

			var (castPremium, premiumKinds) = CastPremium(entry, cast, channel);

			// Nothing here reads the occupancy on its own, so it pays for itself
			// only through the cycle and a cast lands at DPS parity with an
			// instant press. The ceiling bounds efficiency, not the cast.
			var efficiency = Math.Min(SfrDials.MaxEfficiency,
				SfrDials.BaseInstantEfficiency * CycleGate(cycle - t) * riderMultiplier * amplifierMultiplier);

			var cappedEfficiency = efficiency * castPremium;

			var reach = new Dictionary<string, float>();
			var targets = new Dictionary<string, (float Mine, float Theirs)>();
			var fieldArea = new Dictionary<string, float>();

			foreach (var spec in SfrGeometry.PricedScenarios)
			{
				var offsets = SfrGeometry.Placement(spec, cast, out var aim);

				// Ground one monster has to itself in this placement, which is
				// what turns a target count on a spread field into an area.
				fieldArea[spec.Id] = SfrGeometry.FieldAreaPerMob(offsets);

				// The yardstick stays resolved rather than measured: it is an
				// average over the five base-job swings, a property of the
				// model rather than of this skill.
				var mine = measured.Targets.TryGetValue(spec.Id, out var reached) ? reached : 0f;
				var theirs = SfrGeometry.GenericBasicReach(offsets, aim);

				targets[spec.Id] = (mine, theirs);
				reach[spec.Id] = mine / Math.Max(theirs, SfrDials.MinYardstick);
			}

			// What one press is worth across the scenarios a player actually
			// meets, rather than on the single target of S1. Dividing by it is
			// what pays a narrow skill for being narrow.
			var weightSum = reach.Keys.Sum(s => SfrDials.ScenarioWeights[s]);
			var weighted = reach.Sum(r => SfrDials.ScenarioWeights[r.Key] * r.Value) / weightSum;
			var widest = SfrDials.SpreadScenarios.Where(reach.ContainsKey).Select(s => reach[s]).DefaultIfEmpty(0f).Max();

			// Mostly the gathered pull, because that is the one a player sets
			// up before pressing. Floored at the average so a skill that reaches
			// nothing in the gathered scenarios still pays for the range it has.
			var peakest = SfrDials.PeakScenarios.Where(reach.ContainsKey).Select(s => reach[s]).DefaultIfEmpty(0f).Max();
			var peak = Math.Max(peakest, weighted);
			var charged = SfrDials.WidthPeakShare * peak + (1f - SfrDials.WidthPeakShare) * weighted;
			var width = MathF.Pow(Math.Max(charged, 1e-6f), SfrDials.AoeExponent);

			// Area and count are separate axes and width only charges the first.
			// The anchor is the zero point of this one, the same way it is
			// priced without its own rider: it is a demanding press itself, so
			// leaving it in would have calibration hand the premium straight
			// back and turn a raise for packed AoE into a cut for everything
			// else.
			var capacity = TargetCapacity(measured);
			var area = AreaCovered(measured, fieldArea);
			var demand = area > 0 ? SfrGeometry.NaturalMobArea * capacity / area : 0f;
			var gathering = skillName == SfrDials.AnchorSkill || area <= 0 ? 1f : GatheringPremium(demand);

			var basicRate = SfrData.GenericBasicRate();
			var baseSfr = Calibration() * cappedEfficiency * basicRate * cycle * 100f * gathering / width;

			// The spread gate is the multi-target ceiling: whatever the skill
			// does when everything is gathered may not exceed the cap times what
			// it does in the weighted-typical case.
			var gateSfr = widest > 0 ? baseSfr * SfrDials.SpreadCap * weighted / widest : baseSfr;
			var sfr = Math.Min(baseSfr, gateSfr);

			sfr /= SfrDials.CritAllowance;

			if (!SfrDials.SkillSfrMultipliers.TryGetValue(skillName, out var hardMultiplier))
				hardMultiplier = 1f;

			sfr *= hardMultiplier;

			// What an advanced class's circle buys over the base pool. The
			// anchor is a base-job skill, so this raises the advanced roster
			// against it rather than the roster's level.
			var circlePremium = SfrData.CirclePremium(skillName);
			sfr *= circlePremium;

			// A channel pays its SP over the hold rather than up front, and is
			// charged SpChannelMultiplier for it; this is the other half of
			// that trade.
			var channelPremium = channel ? SfrDials.ChannelSfrPremium : 1f;
			sfr *= channelPremium;

			// The press budget is spread over everything the press delivers,
			// so total damage lands back on the budget exactly. The retired
			// blend divided by hits x k with k <= 1, which handed a spread
			// press up to four times its own budget in total damage - that is
			// what priced Swordman_Thrust's 33-tick bleed at 102.
			var divisor = hits;

			// The ceiling is what one hit reads at the skill's own cap; the
			// slope share splits it between what unlocking the skill gives and
			// what levelling it gives.
			var ceiling = sfr / Math.Max(divisor, 1f);
			var slopeShare = SfrData.SlopeShare(skillName);
			var factor = ceiling * (1f - slopeShare);
			var factorByLevel = ceiling * slopeShare / Math.Max(1, levels);

			var sp = PriceSp(skillName, measured);

			return new SfrPrice
			{
				Skill = skillName,
				Class = cls,
				Levels = levels,
				Circle = SfrData.SkillCircle(skillName),
				Measured = true,
				CirclePremium = circlePremium,
				ChannelPremium = channelPremium,
				Sp = sp,
				Occupancy = t,
				RawOccupancy = rawOccupancy,
				Cycle = cycle,
				Utilization = Math.Min(1f, t / cycle),
				DirectHits = measured.DirectHits,
				PadHits = 0,
				PadDetail = [],
				PadUnknown = [],
				BuffHits = 0,
				BuffDetail = [],
				Hits = hits,
				DamageSpan = measured.DamageSpanSeconds,
				FullDamageSpan = measured.FullDamageSpanSeconds,
				CountWindow = measured.CountWindowSeconds,
				Overruns = measured.Overruns,
				BurstFraction = measured.BurstFraction,
				Divisor = divisor,
				Dot = 0,
				DotBuff = null,
				DotShare = 0,
				Efficiency = cappedEfficiency,
				GateEfficiency = efficiency,
				Cast = cast,
				CastPremium = castPremium,
				CastPremiumKinds = premiumKinds,
				IsChannel = channel,
				RiderMultiplier = riderMultiplier,
				RiderKinds = riderKinds.ToArray(),
				AmplifierMultiplier = amplifierMultiplier,
				DamageAmplification = measured.DamageAmplification,
				GatheringPremium = gathering,
				GatheringDemand = demand,
				TargetCapacity = capacity,
				AreaCovered = area,
				BasicRate = basicRate,
				Reach = reach,
				Targets = targets,
				WeightedReach = weighted,
				ChargedReach = charged,
				Sfr = sfr,
				SpreadCapped = gateSfr < baseSfr,
				HardMultiplier = hardMultiplier,
				RawFactor = factor,
				Factor = (int)Math.Round(factor),
				FactorByLevel = MathF.Round(factorByLevel, 1),
			};
		}

		/// <summary>
		/// Returns whether the data says a press is held rather than wound up.
		/// </summary>
		/// <remarks>
		/// A channel delivers damage for as long as it runs instead of landing
		/// one hit at the end of a wind-up, so a skill with a real cast is
		/// never one whatever its shoot time is.
		/// </remarks>
		/// <param name="entry"></param>
		public static bool IsChannel(SkillEntryData entry)
			=> entry.Num("basicCast") <= 0 && entry.Num("shootTime") >= SfrDials.ChannelShootMs;

		/// <summary>
		/// Returns what one press should cost at the bar, and what a level of
		/// it adds.
		/// </summary>
		/// <remarks>
		/// SP is the throttle the model has that does not lengthen a cooldown
		/// (BALANCE.md §7), so it is priced on what a press takes from the
		/// player rather than on what it delivers: the slice of the rotation it
		/// gates, the wind-up it commits to, and the class it belongs to.
		///
		/// The cycle term is sublinear, so a long cooldown costs more without
		/// costing proportionally more - the cooldown is already that press's
		/// throttle. The cast term takes the same (1 + cast)^k shape the SFR
		/// cast premium does, so what a wind-up earns in damage it pays for at
		/// the bar.
		///
		/// The written cost is divided by the measured charge count, so what a
		/// press actually spends lands on the budget: a channel billing every
		/// tick writes a per-tick cost, not a per-press one. Nothing is
		/// measured for a skill the damage pass never pressed - a factor-0
		/// buff, a heal - and those take the fallback of one charge, which is
		/// what all but a handful of presses measure anyway.
		///
		/// Both numbers are integers with a floor of one: SCR_Get_SpendSP
		/// floors its own result, so a fractional cost is a cost the engine
		/// rounds away.
		/// </remarks>
		/// <param name="skillName"></param>
		/// <param name="measured"></param>
		public static SfrSpPrice PriceSp(string skillName, SfrMeasuredPress measured)
		{
			if (!SfrData.Skills.TryGetValue(skillName, out var entry))
				throw new KeyNotFoundException($"No skill data for '{skillName}'.");

			var cast = entry.Num("basicCast") / 1000f;
			var cycle = SfrData.PressWindow(entry).Cycle;
			var anchorCycle = SfrData.CycleFor(SfrDials.AnchorSkill) ?? 1f;

			var cycleTerm = MathF.Pow(Math.Min(cycle, SfrDials.SpMaxCycle) / Math.Max(anchorCycle, 1e-6f), SfrDials.SpCycleExponent);
			var castTerm = MathF.Pow(1f + cast, SfrDials.SpCastExponent);

			var target = SfrDials.SpAnchorCost * cycleTerm * castTerm;
			var kinds = new List<string>();

			// factor 0 is the data's marker for a press that deals no damage,
			// so nothing in the damage model is holding it and SP is the only
			// thing that is.
			if (!SfrData.DealsDamage(skillName))
			{
				target *= SfrDials.SpBuffMultiplier;
				kinds.Add("buff");
			}

			var jobMultiplier = SfrData.SpJobMultiplier(skillName);

			if (jobMultiplier != 1f)
			{
				target *= jobMultiplier;
				kinds.Add("arcane");
			}

			// SP rides the same circle multiplier the SFR does, so a skill that
			// hits harder for its circle costs proportionally more to press.
			var circleMultiplier = SfrData.CirclePremium(skillName);

			if (circleMultiplier != 1f)
			{
				target *= circleMultiplier;
				kinds.Add($"circle {SfrData.SkillCircle(skillName)}");
			}

			var channel = IsChannel(entry);

			if (channel)
			{
				target *= SfrDials.SpChannelMultiplier;
				kinds.Add("channel");
			}

			if (SfrDials.SkillSpMultipliers.TryGetValue(skillName, out var hardMultiplier))
			{
				target *= hardMultiplier;
				kinds.Add($"override x{hardMultiplier:0.##}");
			}

			var charges = measured is { SpMeasured: true } ? measured.SpChargeSlope : 1f;
			var levels = SfrData.SkillMaxLevel(skillName);

			// The floor holds a priced press above zero, but an override to
			// zero is a free press by design and passes under it.
			var cost = target <= 0f ? 0 : Math.Max(SfrDials.MinSpCost, (int)Math.Round(target / Math.Max(charges, SfrDials.MinSpChargeSlope)));

			// SCR_Get_SpendSP reads level minus one where the factor reads the
			// level itself, so this share is what holds SP proportional to SFR
			// at every level rather than only at the cap.
			var share = SfrData.SlopeShare(skillName);
			var spGrowth = share / Math.Max(levels * (1f - share) + share, 1e-6f);

			return new SfrSpPrice
			{
				Skill = skillName,
				Target = target,
				Charges = charges,
				Measured = measured is { SpMeasured: true },
				Kinds = kinds.ToArray(),
				Cost = cost,
				// Mirrors the factor split: SP tracks the same slope share, so
				// damage per SP is flat across levels and circles. Fractional
				// because SCR_Get_SpendSP floors the total, not the slope, and
				// a floor of 1 here would overcharge a 15-level skill fourfold.
				CostByLevel = MathF.Round(cost * spGrowth, 2),
			};
		}

		/// <summary>
		/// Returns the seconds between presses: a full overheat volley plus its
		/// cooldown, per press.
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="occupancy"></param>
		public static float CycleSeconds(SkillEntryData entry, float occupancy)
		{
			var overheat = Math.Max(1f, entry.Num("overheatCount", 1f));
			var cooldown = entry.Num("cooldownTime") / 1000f;

			return Math.Max(occupancy, (overheat * occupancy + cooldown) / overheat);
		}

		/// <summary>
		/// Returns the rider multiplier a measured press earns.
		/// </summary>
		/// <remarks>
		/// Continuous, with no deadband in front of it. A deadband is a step,
		/// and a step turns a measurement landing either side of it into a jump
		/// in the price - Archer_Multishot measured 0.07 swings on one run and
		/// 0.61 on the next and moved 33% for it, which is the whole lever. The
		/// deadband existed to stop that flicker when the floor was 0.5 and the
		/// lever was 2x; at a floor of 0.75 the curve is gentle enough that
		/// being continuous is strictly better than being gated.
		/// </remarks>
		/// <param name="swingsPrevented"></param>
		public static float RiderMultiplier(float swingsPrevented)
			=> Math.Max(SfrDials.RiderFloor, 1f / (1f + SfrDials.DefenseValueScale * Math.Max(0f, swingsPrevented)));

		/// <summary>
		/// Returns the discount a press takes for the damage it adds to
		/// everything else the caster does.
		/// </summary>
		/// <remarks>
		/// The same hyperbola the defensive rider uses, for the same reason: a
		/// press that spends part of its budget on a stacking self-buff, a
		/// damage-taken debuff or an attack-speed gain is not paid for in its
		/// own number alone. Scout_ObliqueFire's 4% per application and the
		/// debuffs Barbarian_Cleave and Hoplite_SpearLunge leave behind are all
		/// measured the same way - by running the caster's own swings twice -
		/// so nothing here has to know which of them a skill carries.
		/// </remarks>
		/// <param name="amplification"></param>
		public static float AmplifierMultiplier(float amplification)
			=> Math.Max(SfrDials.AmplifierFloor, 1f / (1f + SfrDials.AmplifierValueScale * Math.Max(0f, amplification)));

		/// <summary>
		/// Returns the most targets a press was seen reaching anywhere in the
		/// matrix, which is the count its geometry and splash budget allow.
		/// </summary>
		/// <remarks>
		/// The best case across every scenario rather than one of them: what
		/// caps a press's count is its own splash rate, target cap or projectile
		/// budget, and which placement exposes that cap differs by skill - a
		/// stacked pile for a box, a field for a long line.
		/// </remarks>
		/// <param name="measured"></param>
		public static float TargetCapacity(SfrMeasuredPress measured)
			=> measured.Targets.Values.DefaultIfEmpty(0f).Max();

		/// <summary>
		/// Returns the ground a press covers, in square world units.
		/// </summary>
		/// <remarks>
		/// Targets reached on a field of known density is an area: a press that
		/// catches a third of a spread field covered a third of its ground, so
		/// the count times the ground one monster has to itself there is what
		/// the press's own shape covers. Read only on the spread scenarios,
		/// since a stacked pile puts every monster on one point and tells you
		/// nothing about how much ground a press covers.
		///
		/// This is the axis width cannot see. Two presses reaching six targets
		/// are charged the same width, and this is what separates the one whose
		/// area found them from the one that needed them stacked.
		/// </remarks>
		/// <param name="measured"></param>
		/// <param name="fieldArea"></param>
		/// <returns>
		/// Zero when no spread field was reached at all, which is an area the
		/// probe could not resolve rather than a small one.
		/// </returns>
		public static float AreaCovered(SfrMeasuredPress measured, Dictionary<string, float> fieldArea)
		{
			var areas = SfrDials.GatheringAreaScenarios
				.Where(s => measured.Targets.ContainsKey(s) && fieldArea.ContainsKey(s))
				.Select(s => measured.Targets[s] * fieldArea[s])
				.DefaultIfEmpty(0f)
				.ToArray();

			// A press that damaged nothing on either spread field says nothing
			// about how much ground it covers, and floored it would divide its
			// whole target count by the smallest area the model can express and
			// take the largest premium in the roster for it - which is how a
			// plain dagger swing came out the most gathering-dependent skill in
			// the game.
			if (areas.Max() <= 0f)
				return 0f;

			return Math.Max(areas.Average(), SfrDials.GatheringMinArea);
		}

		/// <summary>
		/// Returns what a press earns for the density it demands.
		/// </summary>
		/// <remarks>
		/// One at natural spawn density and below - a press that pays out on
		/// monsters as they already stand needs no pull built for it, whatever
		/// its area or its count.
		///
		/// Convex above that, because the marginal target is not equally easy to
		/// gather: pulling a second monster onto the first is most of a step
		/// from one to two, and pulling an eleventh onto ten is a pull that has
		/// to be built.
		///
		/// Keyed on the density ratio alone, so splash rate, a hard target cap
		/// and a bounce loop all earn it the same way - what is read is how
		/// tightly the count has to be packed, never how a handler got there.
		/// </remarks>
		/// <param name="demand"></param>
		public static float GatheringPremium(float demand)
		{
			if (demand <= 1f)
				return 1f;

			var scaled = (demand - 1f) / Math.Max(SfrDials.GatheringReference - 1f, 1e-6f);

			return Math.Min(SfrDials.GatheringMax, 1f + SfrDials.GatheringPremium * MathF.Pow(scaled, SfrDials.GatheringExponent));
		}

		/// <summary>
		/// Returns what waiting out a cooldown is worth, keyed on the wait
		/// alone.
		/// </summary>
		/// <remarks>
		/// The numerator is the SFR a wait is meant to buy and the denominator
		/// cancels the cycle the price already multiplies by, leaving the
		/// sub-parity discount that makes cooldown-gated burst worth less DPS
		/// than sustain. Normalized so an idle of zero scores one, which is what
		/// makes the base efficiency a spam skill's ceiling.
		/// </remarks>
		/// <param name="idle"></param>
		public static float CycleGate(float idle)
			=> SfrDials.ReferenceOccupancy * CooldownSfr(idle) / (SfrDials.ReferenceOccupancy + idle);

		/// <summary>
		/// Returns the SFR multiple a wait of the given seconds buys, against a
		/// zero-cooldown press.
		/// </summary>
		/// <remarks>
		/// Linear, so every second of cooldown is worth the same fixed slice of
		/// SFR. Below the ramp the line is discounted, because a button with no
		/// cooldown and no overheat is not gated by anything but SP.
		/// </remarks>
		/// <param name="idle"></param>
		public static float CooldownSfr(float idle)
			=> (1f + idle * SfrDials.CooldownSfrPerSecond) * NoGateDiscount(idle);

		/// <summary>
		/// Returns the discount on an ungated button, fading out as soon as a
		/// real cooldown exists.
		/// </summary>
		/// <param name="idle"></param>
		public static float NoGateDiscount(float idle)
		{
			if (idle >= SfrDials.NoGateRamp)
				return 1f;

			return SfrDials.NoGatePenalty + (1f - SfrDials.NoGatePenalty) * (idle / SfrDials.NoGateRamp);
		}

		/// <summary>
		/// Returns what a wind-up risks that its duration alone does not
		/// describe.
		/// </summary>
		/// <remarks>
		/// The cycle already pays a cast back to DPS parity with an instant
		/// press, so everything here is the premium above parity. A channel
		/// earns none of it: it is already delivering damage while it runs.
		/// </remarks>
		/// <param name="entry"></param>
		/// <param name="castSeconds"></param>
		/// <param name="channel"></param>
		public static (float Premium, string[] Kinds) CastPremium(SkillEntryData entry, float castSeconds, bool channel)
		{
			if (channel || castSeconds <= 0)
				return (1f, []);

			var premium = MathF.Pow(1f + castSeconds, 0.5f + SfrDials.CastLengthSlope * castSeconds);
			var kinds = new List<string> { $"cast {castSeconds:0.0}s x{premium:0.00}" };

			if (entry.Flag("castInterruptible"))
			{
				premium *= SfrDials.InterruptiblePremium;
				kinds.Add("interruptible");
			}

			if (!entry.Flag("enableCastMove"))
			{
				premium *= SfrDials.NoCastMovePremium;
				kinds.Add("rooted");
			}

			if (!entry.Flag("speedRateAffectedByDex"))
			{
				premium *= SfrDials.NoDexScalingPremium;
				kinds.Add("no-dex");
			}

			return (premium, kinds.ToArray());
		}

		/// <summary>
		/// Returns whether a skill's own handler, or a pad it creates, heals
		/// through SCR_CalculateHeal.
		/// </summary>
		/// <remarks>
		/// This is a plain text check against the handler source, not a
		/// measurement - there is nothing to measure a heal skill's damage
		/// against, since the whole point is that it is not one. It stands
		/// alone rather than through SfrHandlerAnalysis, which the measured
		/// pipeline no longer calls into for anything else.
		/// </remarks>
		/// <param name="skillName"></param>
		public static bool IsHealSkill(string skillName)
		{
			var text = SfrSources.SkillHandler(skillName);

			if (text != null && text.Contains("SCR_CalculateHeal"))
				return true;

			foreach (var pad in SfrHandlerAnalysis.PadNames(skillName))
			{
				if (SfrSources.Pads.TryGetValue(pad, out var info) && info.Text.Contains("SCR_CalculateHeal"))
					return true;
			}

			return false;
		}

		/// <summary>
		/// Measures every named skill in parallel, across a fixed pool of
		/// independent arenas, and returns whatever came back clean.
		/// </summary>
		/// <remarks>
		/// A skill the probe throws on - no handler, an unreadable ground
		/// target, anything SkillPressProbe.Measure was not built to dispatch -
		/// is simply left out, and Price() falls back to the handler scan for
		/// it. This is the parallel path the roster run's time budget depends
		/// on: each arena is a separate Map instance (ArenaPool), and
		/// SfrPressRecorder's hook flows via AsyncLocal, so the pool's workers
		/// never see each other's presses even when a handler's own damage
		/// lands after an await hops to a different pool thread.
		///
		/// The work is almost entirely Thread.Sleep, not CPU - a press mostly
		/// waits out real wall-clock ticks. Concurrency ramps in
		/// SfrDials.RosterWaveCount waves of poolSize workers each, rather
		/// than starting at full width: every worker's first arena touch runs
		/// GetArenaCenter's clearance search, which is real CPU work, so
		/// starting the whole pool at once turns into a spike of every worker
		/// doing that search simultaneously. Each wave starts
		/// SfrDials.RosterRampUpMs after the last, once the previous wave has
		/// settled into steady-state waiting. ThreadPool.SetMinThreads is
		/// raised to the full width up front so no wave is throttled by the
		/// pool growing into it on its own.
		/// </remarks>
		/// <param name="keys">
		/// One entry per press to run. A key may carry a "#n" suffix to queue
		/// the same skill more than once, which is how the anchor's repeat
		/// trials ride along with the roster instead of costing time after it.
		/// </param>
		/// <param name="poolSize"></param>
		private static Dictionary<string, SfrMeasuredPress> MeasureRoster(List<string> keys, int poolSize)
		{
			var results = new ConcurrentDictionary<string, SfrMeasuredPress>();
			var queue = new ConcurrentQueue<string>(keys);
			var workersPerWave = Math.Max(1, SfrDials.SkillWorkers / SfrDials.RosterWaveCount);

			// Every window is a sleep rather than work, and a skill now fans
			// its own windows out too, so the pool has to cover the product of
			// the two rather than just the skill count.
			ThreadPool.GetMinThreads(out _, out var minIocp);
			ThreadPool.SetMinThreads(poolSize + 8, minIocp);

			using var pool = new ArenaPool(poolSize);

			LastPoolBuildTime = pool.BuildTime;

			var measureStarted = DateTime.UtcNow;

			void Worker()
			{
				while (queue.TryDequeue(out var key))
				{
					var at = key.IndexOf('#');
					var skillName = at < 0 ? key : key[..at];

					try
					{
						results[key] = SkillPressProbe.MeasureAll(skillName, pool: pool);
					}
					catch (Exception)
					{
						// Left unmeasured; the skill keeps its existing factor.
					}
				}
			}

			var waves = new List<Task>();

			for (var wave = 0; wave < SfrDials.RosterWaveCount; ++wave)
			{
				// LongRunning for the same reason the windows inside a skill
				// are: these threads spend their lives blocked, and the pool
				// will not grow into that on its own.
				waves.AddRange(Enumerable.Range(0, workersPerWave)
					.Select(_ => Task.Factory.StartNew(Worker, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default)));

				if (wave < SfrDials.RosterWaveCount - 1)
					Thread.Sleep(SfrDials.RosterRampUpMs);
			}

			Task.WaitAll(waves.ToArray());

			LastMeasureTime = DateTime.UtcNow - measureStarted;

			return new Dictionary<string, SfrMeasuredPress>(results);
		}

		/// <summary>
		/// Prices every skill the model can account for and optionally writes
		/// the result.
		/// </summary>
		/// <remarks>
		/// A factor of zero is the data's marker for a skill that deals no
		/// damage, not a value to price against. Nothing else here reads the
		/// current factor.
		///
		/// This is the generation procedure proper. Every in-scope skill is run
		/// through <see cref="SkillPressProbe"/> and <see cref="SfrDefenseProbe"/>
		/// before it is priced - hit count, pad/DoT/buff ticks, reach and
		/// rider value all come from watching the handler actually run, never
		/// from reading its source. A skill the probes cannot measure at all
		/// (a summon, a press that damages nothing, one that outruns the
		/// window) is held back rather than guessed at.
		/// </remarks>
		/// <param name="write"></param>
		/// <param name="arenaPoolSize">
		/// Concurrent arenas the measurement pass runs on. Defaults to
		/// SfrDials.ArenaPoolSize; raise or lower it to trade memory for time.
		/// </param>
		public static SfrApplyResult ApplyAll(bool write, int? arenaPoolSize = null)
		{
			var result = new SfrApplyResult();
			var lines = File.ReadAllLines(SfrData.OverridesPath);
			var priced = new Dictionary<string, (int Factor, float FactorByLevel)>();
			var spPriced = new Dictionary<string, SfrSpPrice>();
			var inScope = new List<string>();
			var spScope = new List<string>();

			foreach (var line in lines)
			{
				var name = Regex.Match(line, @"className: ""([^""]+)""");
				if (!name.Success)
					continue;

				var skillName = name.Groups[1].Value;
				var cls = SfrData.ClassOf(skillName);

				if (!SfrData.BaseJob.ContainsKey(cls) || !SfrData.Scope.Contains(cls))
					continue;

				// SP is priced for every in-scope press, damage or not: a buff
				// costs SP, a heal costs SP, and a press the damage model
				// cannot measure still charges the bar. Only the charge count
				// needs a measurement, and a press without one charges once.
				spScope.Add(skillName);

				// The field has to exist for the rewrite to have something to
				// replace, but its value is never read - the "deals no damage"
				// marker is taken from the base data instead, which this pass
				// never writes.
				var currentFactor = Regex.Match(line, @"\bfactor: ([0-9.]+)");

				if (!currentFactor.Success || !SfrData.DealsDamage(skillName))
					continue;

				// Not worth a measurement window: Price() rejects these
				// unconditionally, and there is nothing a live press would
				// tell us that the handler source does not already say.
				if (IsHealSkill(skillName))
				{
					result.NotPriceable++;
					result.Unmeasured.Add((skillName, SfrData.ParseFloat(currentFactor.Groups[1].Value),
						"heals, this model does not price healing"));
					continue;
				}

				inScope.Add(skillName);
			}

			// The anchor's repeat trials are queued with everything else, so
			// they run inside the same fan-out and add no wall time.
			var keys = new List<string>(inScope);

			for (var trial = 1; trial < SfrDials.AnchorTrials; ++trial)
				keys.Add($"{SfrDials.AnchorSkill}#{trial}");

			var measured = MeasureRoster(keys, arenaPoolSize ?? SfrDials.ArenaPoolSize);

			// The anchor calibrates the whole roster (Calibration()); losing
			// it loses the whole pass rather than pricing against no scale
			// at all.
			if (!measured.ContainsKey(SfrDials.AnchorSkill))
			{
				result.NotPriceable = inScope.Count;
				return result;
			}

			try
			{
				CalibrateOnMedian(keys.Where(k => k == SfrDials.AnchorSkill || k.StartsWith(SfrDials.AnchorSkill + "#", StringComparison.Ordinal))
					.Select(k => measured.GetValueOrDefault(k)));
			}
			catch (Exception)
			{
				result.NotPriceable = inScope.Count;
				return result;
			}

			foreach (var line in lines)
			{
				var name = Regex.Match(line, @"className: ""([^""]+)""");
				if (!name.Success)
					continue;

				var skillName = name.Groups[1].Value;
				var currentFactor = Regex.Match(line, @"\bfactor: ([0-9.]+)");

				if (!currentFactor.Success || !inScope.Contains(skillName))
					continue;

				var oldFactor = SfrData.ParseFloat(currentFactor.Groups[1].Value);

				if (!measured.TryGetValue(skillName, out var press))
				{
					result.NotPriceable++;
					result.Unmeasured.Add((skillName, oldFactor, "the probe could not dispatch the press"));
					continue;
				}

				SfrPrice price;

				try
				{
					price = Price(skillName, null, press);
				}
				catch (Exception ex)
				{
					result.NotPriceable++;
					result.Unmeasured.Add((skillName, oldFactor, ex.Message.Replace(skillName + ": ", "")));
					continue;
				}

				if (price.RawOccupancy > SfrDials.MaxOccupancy)
				{
					result.NotPriceable++;
					result.Held.Add((skillName, SfrData.ParseFloat(currentFactor.Groups[1].Value), price.Factor,
						$"occupies {price.RawOccupancy:0}s per press - a duration, not an animation"));
					continue;
				}

				priced[skillName] = (price.Factor, price.FactorByLevel);

				if (price.HardMultiplier != 1f)
					result.HardOverrides.Add((skillName, price.HardMultiplier));

				// A ratio against zero says nothing, so a skill coming off the
				// zero marker is reported on its own rather than topping the
				// movers list with an arbitrary number.
				if (oldFactor == 0)
					result.NewlyPriced.Add((skillName, price.Factor));

				result.Changes[skillName] = (price.Factor, price.FactorByLevel,
					oldFactor > 0 ? price.Factor / oldFactor : 1f);

				if (price.Overruns)
					result.Overrunning.Add((skillName, price.FullDamageSpan, price.CountWindow, price.Hits));

				if (price.DamageAmplification > 0)
					result.Amplifiers.Add((skillName, price.DamageAmplification, price.AmplifierMultiplier));

				if (price.GatheringPremium > 1f)
					result.Gathered.Add((skillName, price.TargetCapacity, price.AreaCovered, price.GatheringDemand, price.GatheringPremium));
			}

			foreach (var skillName in spScope)
			{
				SfrSpPrice sp;

				try
				{
					sp = PriceSp(skillName, measured.GetValueOrDefault(skillName));
				}
				catch (Exception)
				{
					continue;
				}

				var oldSp = SfrData.Skills.TryGetValue(skillName, out var entry) ? entry.Num("basicSp") : 0f;

				// A channel bills its cost every tick, so writing the whole
				// press budget as the per-tick cost over-charges it by however
				// many ticks it runs. The charge count is the one thing the
				// fallback of one cannot stand in for.
				if (sp.Charges <= 1f && IsChannel(entry))
				{
					result.SpUnmeasuredChannels.Add((skillName, oldSp));
					continue;
				}

				spPriced[skillName] = sp;
				result.SpChanges[skillName] = (sp.Cost, sp.CostByLevel, oldSp > 0 ? sp.Cost / oldSp : 1f);

				if (sp.Measured && sp.Charges > 1.5f)
					result.SpRepeatCharges.Add((skillName, sp.Charges, sp.Cost));
			}

			if (write)
				ApplyPrices(priced, spPriced);

			return result;
		}

		/// <summary>
		/// Writes the given factors and SP costs into the overrides file and
		/// returns how many skill lines were rewritten.
		/// </summary>
		/// <remarks>
		/// Every line the two sets have no price for is left exactly as it is,
		/// so this is as much the single-skill write as it is the roster's.
		/// </remarks>
		/// <param name="priced"></param>
		/// <param name="spPriced"></param>
		public static int ApplyPrices(IReadOnlyDictionary<string, (int Factor, float FactorByLevel)> priced, IReadOnlyDictionary<string, SfrSpPrice> spPriced)
		{
			var lines = File.ReadAllLines(SfrData.OverridesPath);
			var written = 0;

			for (var i = 0; i < lines.Length; ++i)
			{
				var line = lines[i];
				var name = Regex.Match(line, @"className: ""([^""]+)""");

				if (!name.Success)
					continue;

				var hasSp = spPriced.TryGetValue(name.Groups[1].Value, out var sp);
				var hasFactor = priced.TryGetValue(name.Groups[1].Value, out var price);

				if (!hasSp && !hasFactor)
					continue;

				if (hasSp)
				{
					line = SetField(line, "basicSp", Settled(line, "basicSp", sp.Cost).ToString(CultureInfo.InvariantCulture));
					line = SetField(line, "lvUpSpendSp", Settled(line, "lvUpSpendSp", sp.CostByLevel).ToString("0.##", CultureInfo.InvariantCulture), after: "basicSp");
				}

				if (hasFactor)
				{
					line = Regex.Replace(line, @"\bfactor: [0-9.]+", "factor: " + Settled(line, "factor", price.Factor), RegexOptions.None);
					line = Regex.Replace(line, @"\bfactorByLevel: [0-9.]+",
						"factorByLevel: " + Settled(line, "factorByLevel", price.FactorByLevel).ToString("0.0", CultureInfo.InvariantCulture));
				}

				lines[i] = line;
				written++;
			}

			File.WriteAllLines(SfrData.OverridesPath, lines);

			return written;
		}

		/// <summary>
		/// Returns the priced value, or the one the line already carries when
		/// the two are within RewriteTolerance of each other.
		/// </summary>
		/// <remarks>
		/// Rounding is what makes this necessary rather than cosmetic: two
		/// readings a thousandth apart become different integers when they
		/// straddle a boundary, and the file then changes on every run without
		/// anything having been learned.
		/// </remarks>
		/// <param name="line"></param>
		/// <param name="field"></param>
		/// <param name="priced"></param>
		private static float Settled(string line, string field, float priced)
		{
			var match = Regex.Match(line, $@"{field}: (-?[0-9.]+)");

			if (!match.Success || !float.TryParse(match.Groups[1].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var current))
				return priced;

			var span = Math.Max(Math.Abs(current), 1f);

			return Math.Abs(priced - current) <= span * SfrDials.RewriteTolerance ? current : priced;
		}

		/// <summary>
		/// Sets a field on an override line, adding it when the line does not
		/// already carry one.
		/// </summary>
		/// <remarks>
		/// The factor rewrite can assume its field is there, because a line
		/// without one is skipped. SP cannot: two thirds of the override lines
		/// carry no lvUpSpendSp at all, and a replace that matched nothing
		/// would silently leave them on the base data's growth. Inserted after
		/// the name, which is where the file's own field order puts it.
		/// </remarks>
		/// <param name="line"></param>
		/// <param name="field"></param>
		/// <param name="value"></param>
		/// <param name="after">
		/// Field the new one is inserted behind when the line carries it. Two
		/// fields inserted at the same anchor come out in reverse order, since
		/// the second lands in front of the first.
		/// </param>
		public static string SetField(string line, string field, string value, string after = null)
		{
			var existing = new Regex($@"\b{field}: [0-9.]+");

			if (existing.IsMatch(line))
				return existing.Replace(line, $"{field}: {value}");

			var anchor = after != null ? Regex.Match(line, $@"\b{after}: [0-9.]+, ") : Match.Empty;

			if (!anchor.Success)
				anchor = Regex.Match(line, @"name: ""[^""]*"", ");

			if (!anchor.Success)
				return line;

			return line.Insert(anchor.Index + anchor.Length, $"{field}: {value}, ");
		}
	}

	/// <summary>
	/// One skill's priced SFR, with every term that produced it.
	/// </summary>
	public class SfrPrice
	{
		public string Skill { get; init; }
		public string Class { get; init; }
		public int Levels { get; init; }
		public int Circle { get; init; }

		/// <summary>
		/// Whether the hit count and the reach came from a measured press
		/// rather than from the handler scan.
		/// </summary>
		public bool Measured { get; init; }

		public float CirclePremium { get; init; }

		/// <summary>
		/// What being held rather than wound up bought this skill.
		/// </summary>
		public float ChannelPremium { get; init; }

		/// <summary>
		/// What one press costs at the bar.
		/// </summary>
		public SfrSpPrice Sp { get; init; }

		public float Occupancy { get; init; }
		public float RawOccupancy { get; init; }
		public float Cycle { get; init; }
		public float Utilization { get; init; }
		public int DirectHits { get; init; }
		public float PadHits { get; init; }
		public List<(string Pad, int Ticks, float PerTick)> PadDetail { get; init; }
		public List<(string Pad, string Reason)> PadUnknown { get; init; }
		public float BuffHits { get; init; }
		public List<(string Buff, int Ticks, float PerTick)> BuffDetail { get; init; }
		public float Hits { get; init; }

		/// <summary>
		/// Seconds the press spent delivering the damage that was counted.
		/// </summary>
		public float DamageSpan { get; init; }

		/// <summary>
		/// Seconds it kept delivering for in total, counted or not.
		/// </summary>
		public float FullDamageSpan { get; init; }

		/// <summary>
		/// Seconds of delivery the count was bounded to, which is the cycle.
		/// </summary>
		public float CountWindow { get; init; }

		/// <summary>
		/// Whether delivery outlasted the cycle, so some of it was left to a
		/// later press.
		/// </summary>
		public bool Overruns { get; init; }

		/// <summary>
		/// Share of the damage landing inside one burst window.
		/// </summary>
		public float BurstFraction { get; init; }

		/// <summary>
		/// The blended divisor the press budget is spread over.
		/// </summary>
		public float Divisor { get; init; }
		public float Dot { get; init; }
		public string DotBuff { get; init; }
		public float DotShare { get; init; }
		public float Efficiency { get; init; }
		public float GateEfficiency { get; init; }
		public float Cast { get; init; }
		public float CastPremium { get; init; }
		public string[] CastPremiumKinds { get; init; }
		public bool IsChannel { get; init; }
		public float RiderMultiplier { get; init; }
		public string[] RiderKinds { get; init; }

		/// <summary>
		/// What the press gave up for the damage it adds to everything else.
		/// </summary>
		public float AmplifierMultiplier { get; init; }

		/// <summary>
		/// The measured gain itself, as a fraction of the caster's unbuffed
		/// output.
		/// </summary>
		public float DamageAmplification { get; init; }

		/// <summary>
		/// What the press earned for needing its targets stacked, 1.0 for one
		/// whose area delivers its count on its own.
		/// </summary>
		public float GatheringPremium { get; init; }

		/// <summary>
		/// How many times natural spawn density the press has to be handed
		/// before it reaches its own target count.
		/// </summary>
		public float GatheringDemand { get; init; }

		/// <summary>
		/// The most targets the press was seen reaching anywhere.
		/// </summary>
		public float TargetCapacity { get; init; }

		/// <summary>
		/// The ground it covers, in square world units.
		/// </summary>
		public float AreaCovered { get; init; }
		public float BasicRate { get; init; }
		public Dictionary<string, float> Reach { get; init; }
		public Dictionary<string, (float Mine, float Theirs)> Targets { get; init; }
		public float WeightedReach { get; init; }

		/// <summary>
		/// The reach the price actually divides by, blending the best gathered
		/// scenario with the weighted average per WidthPeakShare.
		/// </summary>
		public float ChargedReach { get; init; }
		public float Sfr { get; init; }
		public bool SpreadCapped { get; init; }

		/// <summary>
		/// The design-fiat multiplier SfrDials.SkillSfrMultipliers applied to
		/// this skill, 1.0 when none did.
		/// </summary>
		public float HardMultiplier { get; init; }

		/// <summary>
		/// The factor before it is rounded to what the file carries, which is
		/// what the anchor's calibration has to be taken against.
		/// </summary>
		public float RawFactor { get; init; }

		public int Factor { get; init; }
		public float FactorByLevel { get; init; }
	}

	/// <summary>
	/// One skill's priced SP cost, with the terms behind it.
	/// </summary>
	public class SfrSpPrice
	{
		public string Skill { get; init; }

		/// <summary>
		/// What one press should spend in total, before the charge count is
		/// divided out.
		/// </summary>
		public float Target { get; init; }

		/// <summary>
		/// How many times the press was measured charging its own cost.
		/// </summary>
		public float Charges { get; init; }

		/// <summary>
		/// Whether the charge count was measured rather than assumed to be one.
		/// </summary>
		public bool Measured { get; init; }

		/// <summary>
		/// Which multipliers the cost picked up.
		/// </summary>
		public string[] Kinds { get; init; } = [];

		/// <summary>
		/// The basicSp written to the data.
		/// </summary>
		public int Cost { get; init; }

		/// <summary>
		/// The lvUpSpendSp written alongside it.
		/// </summary>
		public float CostByLevel { get; init; }
	}

	/// <summary>
	/// What a full pricing pass changed, held back, or could not price.
	/// </summary>
	public class SfrApplyResult
	{
		/// <summary>
		/// Skill name to its new factor, factorByLevel and the ratio against
		/// the value it replaced.
		/// </summary>
		public Dictionary<string, (int Factor, float FactorByLevel, float Ratio)> Changes { get; } = [];

		/// <summary>
		/// Skills the model priced but refused to write, with the reason.
		/// </summary>
		public List<(string Skill, float OldFactor, int NewFactor, string Reason)> Held { get; } = [];

		/// <summary>
		/// Skills that could not be measured at all, with the reason. These
		/// keep whatever factor the file already carried.
		/// </summary>
		public List<(string Skill, float OldFactor, string Reason)> Unmeasured { get; } = [];

		/// <summary>
		/// Skills whose delivery outlasted their own cycle, with the span, the
		/// cycle it was counted over, and the hits that survived the bound.
		/// </summary>
		/// <remarks>
		/// Priced, not held - the bound is what makes them priceable. This is
		/// the diagnostic that says which skills the bound is load-bearing for.
		/// </remarks>
		public List<(string Skill, float Span, float Cycle, float Hits)> Overrunning { get; } = [];

		/// <summary>
		/// Skills whose press was measured making the caster's other damage
		/// land harder, with the gain and what it cost them in factor.
		/// </summary>
		public List<(string Skill, float Amplification, float Multiplier)> Amplifiers { get; } = [];

		/// <summary>
		/// Skills paid a gathering premium for the density they demand, with the
		/// count, the ground it has to fit into, the ratio and the premium.
		/// </summary>
		public List<(string Skill, float Capacity, float Area, float Demand, float Premium)> Gathered { get; } = [];

		/// <summary>
		/// Skills that carried a factor of zero and now price at something,
		/// with the value they landed on.
		/// </summary>
		public List<(string Skill, int Factor)> NewlyPriced { get; } = [];

		/// <summary>
		/// Skill name to its new SP cost, its per-level growth and the ratio
		/// against the cost it replaced.
		/// </summary>
		/// <remarks>
		/// Wider than Changes: SP is priced for every in-scope skill, including
		/// the buffs, heals and unmeasurable presses the damage model holds
		/// back, since a press with no charge measurement simply charges once.
		/// </remarks>
		public Dictionary<string, (int Sp, float SpByLevel, float Ratio)> SpChanges { get; } = [];

		/// <summary>
		/// Skills whose press was measured charging its cost more than once,
		/// with the count the written cost was divided by.
		/// </summary>
		public List<(string Skill, float Charges, int Sp)> SpRepeatCharges { get; } = [];

		/// <summary>
		/// Channels whose charge count was never measured, so they keep the SP
		/// cost the file already carried.
		/// </summary>
		/// <remarks>
		/// The fallback of one charge is what every unmeasured press takes, and
		/// for a channel it is the one assumption that cannot hold: a press
		/// billing every tick would be written the whole budget as its per-tick
		/// cost.
		/// </remarks>
		public List<(string Skill, float OldSp)> SpUnmeasuredChannels { get; } = [];

		/// <summary>
		/// How many in-scope skills could not be priced at all.
		/// </summary>
		public int NotPriceable { get; set; }

		/// <summary>
		/// Skills SfrDials.SkillSfrMultipliers cut or boosted by design fiat,
		/// with the multiplier applied.
		/// </summary>
		public List<(string Skill, float Multiplier)> HardOverrides { get; } = [];
	}
}
