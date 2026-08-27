using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Zone;
using Melia.Zone.Buffs;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Maps;

namespace Melia.Test.Balance.Sfr
{
	/// <summary>
	/// What one press of a skill actually landed, per target, in one scenario.
	/// </summary>
	public class SkillPressResult
	{
		public string SkillClassName { get; init; }
		public string ScenarioId { get; init; }
		public int CharacterLevel { get; init; }
		public int SkillLevel { get; init; }
		public int MobsPlaced { get; init; }

		/// <summary>
		/// The reference mob the press was measured against.
		/// </summary>
		public string MobClassName { get; init; }

		/// <summary>
		/// Its move type, which some handlers filter their damage on.
		/// </summary>
		public string MobMoveType { get; init; }

		/// <summary>
		/// Distinct entities that took damage above zero, which is the reach
		/// the pricer charges width on.
		/// </summary>
		public int TargetsDamaged { get; init; }

		/// <summary>
		/// Damage applications that landed on the target the press was aimed
		/// at, which is the hit count the press's budget is divided by.
		/// </summary>
		public int HitsOnPrimary { get; init; }

		/// <summary>
		/// The most any single entity took, which differs from the primary
		/// when a skill re-hits something other than what it was aimed at.
		/// </summary>
		public int MaxHitsOnTarget { get; init; }

		/// <summary>
		/// Applications across every entity.
		/// </summary>
		public int TotalHits { get; init; }

		public float PrimaryDamage { get; init; }
		public float TotalDamage { get; init; }

		/// <summary>
		/// Total HP every mob in the scenario lost across the whole window,
		/// read directly off their HP rather than off the recorder's hook.
		/// </summary>
		/// <remarks>
		/// This is the one number every source of damage shares regardless of
		/// which internal pipeline delivers it: a direct hit, a pad tick, and
		/// a DamageOverTimeBuffHandler tick applying its snapshot through
		/// TakeSimpleHit (which never reaches SCR_Combat_AfterCalc at all)
		/// all show up here identically. MeasureHitsFromDamage is what turns
		/// it into a hit count.
		/// </remarks>
		public float HpLossDamage { get; init; }

		/// <summary>
		/// HP each placed monster lost, in the order the scenario placed them.
		/// </summary>
		/// <remarks>
		/// Per monster rather than summed, because the pull is a mix of races
		/// and a mix of defences: what one hit takes off differs from target
		/// to target, so a hit count read off the scenario total reads that
		/// spread as extra hits.
		/// </remarks>
		public float[] HpLossPerMob { get; init; } = [];

		/// <summary>
		/// What the defence curve leaves of the attack against each placed
		/// monster, in the same order.
		/// </summary>
		public float[] MitigatedPerMob { get; init; } = [];

		/// <summary>
		/// Which placed monsters are the scenario's reference monster, in the
		/// same order.
		/// </summary>
		/// <remarks>
		/// The pull mixes races so that reach, and a race-gated handler, are
		/// measured against a real one. The hit count cannot be: the race and
		/// armour multipliers land after the factor and differ per monster, so
		/// a target that simply takes more per hit reads as extra hits, and
		/// Swordman_Bash - one hit on everything it touches - scored 1.8. The
		/// count is therefore taken over the reference monster alone, which is
		/// the one the S1 reference hit was measured against.
		/// </remarks>
		public bool[] ReferenceMob { get; init; } = [];

		/// <summary>
		/// The factor the press was run at, which is the same for every skill.
		/// </summary>
		public float Factor { get; init; }

		/// <summary>
		/// The basicSp the press was run at, or zero when it ran on the skill's
		/// own cost.
		/// </summary>
		public float SpPinned { get; init; }

		/// <summary>
		/// SP the caster actually spent over the whole window.
		/// </summary>
		/// <remarks>
		/// Read off the bar rather than hooked, which is why the caster's SP
		/// regeneration is suppressed for the press: the number has to be what
		/// the press took, not what it took net of what ticked back.
		/// </remarks>
		public float SpSpent { get; init; }

		/// <summary>
		/// Attack power before the defense curve, averaged over the roll's
		/// range.
		/// </summary>
		public float AttackPower { get; init; }

		/// <summary>
		/// The defense the primary target opposed it with, physical or
		/// magical as the skill's class type decides.
		/// </summary>
		public float TargetDefense { get; init; }

		/// <summary>
		/// What the defense curve leaves of the attack, which is what the
		/// factor multiplies.
		/// </summary>
		public float MitigatedAttack { get; init; }

		/// <summary>
		/// Whether the press was still running when the window closed, which
		/// makes every count here a lower bound rather than the total.
		/// </summary>
		public bool Truncated { get; init; }

		/// <summary>
		/// What was still ticking at window close, if Truncated came from
		/// that rather than skill.IsRunning.
		/// </summary>
		public string TruncationReason { get; init; }

		/// <summary>
		/// Seconds between the press's first and last point of damage.
		/// </summary>
		public float DamageSpanSeconds { get; init; }

		/// <summary>
		/// The largest share of the press's damage landing inside any one
		/// SfrDials.BurstWindowMs window.
		/// </summary>
		public float BurstFraction { get; init; } = 1f;

		/// <summary>
		/// Seconds of delivery before the count window bounded it, which is
		/// what says a press outlives its own cooldown.
		/// </summary>
		public float FullDamageSpanSeconds { get; init; }

		public string Error { get; init; }

		public override string ToString()
			=> this.Error != null
				? $"{this.ScenarioId} {this.SkillClassName}: FAILED ({this.Error})"
				: $"{this.ScenarioId} {this.SkillClassName} sk{this.SkillLevel} @lv{this.CharacterLevel}: " +
				  $"{this.TargetsDamaged}/{this.MobsPlaced} target(s), {this.HitsOnPrimary} hit(s) on the primary, " +
				  $"{this.TotalHits} total, {this.TotalDamage:F0} damage at factor {this.Factor:F0}" +
				  $", vs {this.MobClassName} ({this.MobMoveType})" +
				  (this.Truncated ? $"  TRUNCATED ({this.TruncationReason ?? "still running"})" : "");
	}

	/// <summary>
	/// A skill's measured press across the whole scenario matrix, in the shape
	/// the pricer takes as input.
	/// </summary>
	public class SfrMeasuredPress
	{
		public string Skill { get; init; }
		public int SkillLevel { get; init; }
		public int CharacterLevel { get; init; }

		/// <summary>
		/// One result per scenario, keyed on the scenario id.
		/// </summary>
		public Dictionary<string, SkillPressResult> Scenarios { get; } = [];

		/// <summary>
		/// Damage applications one press makes against the target it is aimed
		/// at, taken from the single-target scenario.
		/// </summary>
		public int DirectHits { get; set; } = 1;

		/// <summary>
		/// The same count unrounded, which is what the price divides by.
		/// </summary>
		/// <remarks>
		/// This is single-hit-equivalents - the slope of HP lost per factor
		/// point over what one reference hit is worth - so it is a real number,
		/// not a tally. Rounding it made a press measuring 1.5 land on 1 or 2
		/// depending on noise, which is a clean 2x swing in the factor and was
		/// the largest remaining source of run-to-run drift.
		/// </remarks>
		public float HitEquivalents { get; set; } = 1f;

		/// <summary>
		/// Whether DirectHits came from the HP-loss measurement rather than
		/// the fallback of one.
		/// </summary>
		public bool HitsFromDamage { get; set; }

		/// <summary>
		/// Why the HP-loss measurement could not be taken, when it could not.
		/// </summary>
		public string HitsFailure { get; set; }

		/// <summary>
		/// Seconds the press spent delivering its damage.
		/// </summary>
		public float DamageSpanSeconds { get; set; }

		/// <summary>
		/// Largest share of the press's damage inside one burst window.
		/// </summary>
		public float BurstFraction { get; set; } = 1f;

		/// <summary>
		/// Seconds the press kept delivering for, before the count window
		/// bounded it. Larger than CountWindowSeconds means the press outlives
		/// its own cooldown.
		/// </summary>
		public float FullDamageSpanSeconds { get; set; }

		/// <summary>
		/// Seconds of delivery the hit count was read over, which is the
		/// skill's own cycle.
		/// </summary>
		public float CountWindowSeconds { get; set; }

		/// <summary>
		/// Whether the press was still delivering damage when its own cycle
		/// was up, so the count left some of it to a later press.
		/// </summary>
		public bool Overruns => this.FullDamageSpanSeconds > this.CountWindowSeconds + 0.05f;

		/// <summary>
		/// Entities damaged per scenario, which replaces the resolved splash
		/// count in the price.
		/// </summary>
		public Dictionary<string, float> Targets { get; } = [];

		/// <summary>
		/// Whether every priced scenario measured without error.
		/// </summary>
		public bool Complete => this.Scenarios.Values.All(r => r.Error == null);

		/// <summary>
		/// Whether the press was seen to damage anything at all.
		/// </summary>
		/// <remarks>
		/// A press that reached nothing anywhere was not observed, not narrow:
		/// a summon, a pad that outlives its window, or a handler needing state
		/// the probe does not set. Pricing it divides by a reach of zero.
		/// </remarks>
		public bool Delivered => this.Scenarios.Values.Any(r => r.Error == null && r.TargetsDamaged > 0);

		/// <summary>
		/// Whether the damage-slope hit-count inference (MeasureHitsFromDamage)
		/// outran its window on either factor point, so DirectHits may be a
		/// floor rather than the true total.
		/// </summary>
		public bool HitsTruncated { get; set; }

		/// <summary>
		/// Whether any scenario's press, or the hit-count inference, outran
		/// the measurement window.
		/// </summary>
		public bool Truncated => this.HitsTruncated || this.Scenarios.Values.Any(r => r.Truncated);

		/// <summary>
		/// What one press bought in damage the caster avoided taking, in units
		/// of its own basic-attack swing, from SfrDefenseProbe. Zero when the
		/// probe found nothing or could not run.
		/// </summary>
		public float SwingsPrevented { get; set; }

		/// <summary>
		/// Damage the caster took in each of the defensive probe's control
		/// windows, in trial order, so a reading that moved can be read pair
		/// by pair rather than only as a mean.
		/// </summary>
		public float[] DefenseControls { get; set; } = [];

		/// <summary>
		/// Damage the caster took in each treatment window, in the same order.
		/// </summary>
		public float[] DefenseTreatments { get; set; } = [];

		/// <summary>
		/// What one press bought in extra damage on everything else the caster
		/// does, as a fraction of its unbuffed output, from SfrOffenseProbe.
		/// Zero when the probe found nothing or could not run.
		/// </summary>
		public float DamageAmplification { get; set; }

		/// <summary>
		/// How many times one press charges its own SP cost.
		/// </summary>
		/// <remarks>
		/// The slope of SP spent against the cost it was pinned at, taken from
		/// the two points the factor line is already measured at. One for a
		/// press that pays once; a channel or a per-tick pad reads its tick
		/// count, and the priced cost is divided by it so what the press
		/// actually spends is the budget rather than a multiple of it.
		/// </remarks>
		public float SpChargeSlope { get; set; } = 1f;

		/// <summary>
		/// SP the press spent that did not move with the pinned cost, which is
		/// whatever a handler bills at a rate of its own.
		/// </summary>
		public float SpFixedSpend { get; set; }

		/// <summary>
		/// Whether the charge slope came from a measurement rather than the
		/// fallback of one.
		/// </summary>
		public bool SpMeasured { get; set; }
	}

	/// <summary>
	/// Runs one press of a skill through its real handler and records what it
	/// landed, rather than inferring it from the handler's source.
	/// </summary>
	/// <remarks>
	/// SkillProfiler cannot answer this for a Force skill: it approximates the
	/// hit list from the .txt splash fields and, for a projectile, takes the
	/// single nearest target without calling Handle at all. EncounterProbe does
	/// dispatch through the handler, but measures a repeated six-second window
	/// of total damage, which has no per-target breakdown and no way to tell a
	/// second hit on the same mob from a first hit on another.
	///
	/// This is the narrow case both are missing: one press, dispatched for
	/// real, with the damage pipeline instrumented, waited out only for the
	/// work that press itself scheduled.
	/// </remarks>
	public static class SkillPressProbe
	{
		/// <summary>
		/// How often the map is ticked while a press finishes.
		/// </summary>
		private const int TickMs = 25;

		/// <summary>
		/// How long the probe keeps ticking after the handler returns, so
		/// missiles and pads it launched still land.
		/// </summary>
		private const int SettleMs = 1500;

		/// <summary>
		/// Skill variable the packet handler puts the ground aim point in.
		/// </summary>
		public const string GroundPosVariable = "Melia.ToolGroundPos";

		/// <summary>
		/// Ceiling on how long one press is waited out, in milliseconds.
		/// </summary>
		/// <remarks>
		/// A press that is still running past this is a channel or a pad, and
		/// what it does from here is no longer this press. This is the
		/// "~10 s encounter" window (SfrDials.EncounterWindowMs): long enough
		/// that a skill's own pad or DoT finishes its real tick count inside
		/// the measurement instead of that count being guessed from source.
		/// </remarks>
		public const int MaxPressMs = SfrDials.EncounterWindowMs;

		/// <summary>
		/// Extra time a press may run past the window to let its DoT or pad
		/// finish. Only skills that leave something running pay it, since the
		/// drain stops as soon as nothing is.
		/// </summary>
		public const int DrainMs = SfrDials.EncounterWindowMs * 3;

		/// <summary>
		/// Extra max HP so a press never kills what it is being measured on.
		/// </summary>
		private const float SurvivalHp = 100_000_000f;

		/// <summary>
		/// Extra max SP so a press pinned at SpProbeHigh never runs the bar
		/// dry, kept small enough that a float still holds the spend exactly.
		/// </summary>
		private const float SurvivalSp = 100_000f;

		/// <summary>
		/// Seed the press runs under, so a rerun reproduces it.
		/// </summary>
		public const int Seed = 20260807;

		/// <summary>
		/// Runs every action at once on dedicated threads, and waits.
		/// </summary>
		/// <remarks>
		/// Deliberately not Parallel.For or Task.Run. Every unit of work here
		/// is a press waiting out real wall-clock ticks in Thread.Sleep, and
		/// the thread pool treats a blocked thread as a busy one - past
		/// MinThreads it injects replacements at a rate of one or two a
		/// second, so a fan-out of blocking work runs very nearly serially no
		/// matter how wide it is asked to be. LongRunning gives each window a
		/// thread of its own, which is what makes the wall time the maximum of
		/// the windows rather than their sum.
		/// </remarks>
		/// <param name="work"></param>
		public static void RunAll(params Action[] work)
		{
			if (work.Length == 0)
				return;

			if (work.Length == 1)
			{
				work[0]();
				return;
			}

			var tasks = work
				.Select(w => Task.Factory.StartNew(w, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default))
				.ToArray();

			Task.WaitAll(tasks);
		}

		/// <summary>
		/// Measures one press of one skill in one scenario.
		/// </summary>
		/// <param name="job"></param>
		/// <param name="skillId"></param>
		/// <param name="skillLevel"></param>
		/// <param name="spec"></param>
		/// <param name="characterLevel"></param>
		/// <param name="factor"></param>
		/// <param name="arena">
		/// The map to place the synthetic character and mobs on. Defaults to
		/// the single shared arena; a parallel run passes its own pool arena
		/// so concurrent presses never collide on the same instance.
		/// </param>
		/// <param name="fullWindow">
		/// Waits out the entire window regardless of skill.IsRunning, so a
		/// DoT or pad the press started gets its whole tail counted in
		/// HpLossDamage rather than being cut off the moment the handler's
		/// own tracked task returns.
		/// </param>
		/// <param name="countWindowMs">
		/// Milliseconds of delivery HpLossDamage and the damage shape are read
		/// over, which is the skill's own cycle. The press is still waited out
		/// in full so truncation is still detected; this only bounds what is
		/// counted. Null counts everything.
		/// </param>
		/// <param name="basicSp">
		/// The SP cost to pin the skill at for this press, so what it actually
		/// spends can be read against a known input. Null leaves the skill's
		/// own cost alone.
		/// </param>
		public static SkillPressResult Measure(JobEntry job, SkillId skillId, int skillLevel, ScenarioSpec spec, int characterLevel, float factor = SfrDamageCurve.BaselineFactor, Map arena = null, bool fullWindow = false, int? countWindowMs = null, float? basicSp = null)
		{
			var character = (Character)null;
			var mobs = new List<Mob>();
			var className = skillId.ToString();

			// Installed before anything is created, so every task the press
			// starts inherits it: GameClock.Current is AsyncLocal, and an
			// async flow carries the value it was started with. Started at the
			// clock's fixed epoch, so two presses of the same skill hold the
			// same instants and not merely the same spans.
			GameClock.Use(new VirtualClock());

			// Seeded before anything is built, not just before the press.
			// Creating the character, rolling its reference gear and placing
			// the pull all draw on the RNG, so seeding after them left the
			// actors themselves varying between runs - and a mob that rolled
			// different stats takes a different number of hits.
			DeterministicRandom.Seed(Seed);

			try
			{
				var map = arena ?? SyntheticActors.GetArena();
				var stat = JobCatalog.GetPrimaryStat(job);

				character = SyntheticActors.CreateCharacter(job.JobId, characterLevel, StatSpread.AllIn(stat, characterLevel), arena: map);
				ReferenceGear.Equip(character, job);

				var skill = SyntheticActors.GiveSkill(character, skillId, skillLevel);
				className = skill.Data.ClassName;

				var trigger = TriggerFor(character, className);

				var cycle = CastCycleModel.Measure(character, skill);

				var tolerance = spec.Rank == MonsterRank.Normal ? 8 : 30;
				var mobData = SpawnCensus.FindReferenceMob(Math.Max(1, characterLevel + spec.LevelOffset), spec.Rank, tolerance, out var mobLevel);
				var offsets = ScenarioMatrix.GetOffsets(spec, spec.MobCount, cycle.CastTimeMs, mobData.RunSpeed,
					Math.Max(1, mobLevel - 9), mobLevel + 9, out var aimDistance);

				var spread = SpawnCensus.FindRaceSpread(mobData, mobLevel, spec.Rank, tolerance);
				var isReference = new bool[offsets.Length];

				for (var i = 0; i < offsets.Length; ++i)
				{
					var placed = spread[i % spread.Length];

					isReference[i] = placed.Id == mobData.Id;
					mobs.Add(SyntheticActors.CreateMob(placed.Id, offsets[i], map));
				}

				foreach (var mob in mobs)
					Fortify(mob);

				Fortify(character);
				Refill(character);

				var aimPos = new Position(character.Position.X + aimDistance, character.Position.Y, character.Position.Z);
				character.Direction = character.Position.GetDirection(aimPos);

				var primary = mobs.OrderBy(m => m.Position.Get2DDistance(aimPos)).FirstOrDefault();

				DeterministicRandom.Seed(Seed);

				using (new SfrFactorScope(skill, factor))
				using (var spScope = basicSp != null ? new SfrSpScope(skill, basicSp.Value) : null)
				using (var recorder = new SfrPressRecorder(character))
				{
					var timeline = fullWindow ? new List<(int ElapsedMs, float Loss, float[] PerMob)>() : null;
					var spBefore = character.Properties.GetFloat(PropertyName.SP);
					var truncated = Press(skill, character, aimPos, mobs, map, recorder, fullWindow, timeline, trigger);
					var spSpent = Math.Max(0f, spBefore - character.Properties.GetFloat(PropertyName.SP));

					// The pricer's budget is one cycle, so the delivery it
					// divides by is read over one cycle too, per mob as well as
					// in total. The press itself is still waited out in full.
					var counted = Bound(timeline, countWindowMs);
					var shape = Shape(counted);
					var fullShape = Shape(timeline);

					var damaged = recorder.Damaged();
					var hitsOnPrimary = primary != null ? recorder.HitsOn(primary) : 0;
					var hpLoss = counted != null && counted.Count > 0
						? counted[counted.Count - 1].Loss
						: HpLost(mobs);
					var hpLossPerMob = counted != null && counted.Count > 0
						? counted[counted.Count - 1].PerMob
						: HpLostPerMob(mobs);

					// A trap or DoT still alive past the window is only
					// undercounted if the count window itself was not reached:
					// what it does after one cycle belongs to the next press,
					// which is what Overruns already says.
					var covered = countWindowMs != null && timeline != null && timeline.Count > 0
						&& timeline[timeline.Count - 1].ElapsedMs >= countWindowMs.Value;

					// Only the hit-count pass is undercounted by a tail still
					// running; a reach pass has already counted the target.
					var outlivingReason = fullWindow && !covered ? OutlivingTickReason(character, mobs) ?? OutlivingPadReason(map, character) : null;

					return new SkillPressResult
					{
						SkillClassName = className,
						ScenarioId = spec.Id,
						CharacterLevel = characterLevel,
						SkillLevel = skillLevel,
						MobsPlaced = mobs.Count,
						MobClassName = mobData.ClassName,
						MobMoveType = primary?.MoveType.ToString(),
						TargetsDamaged = damaged.Length,
						HitsOnPrimary = hitsOnPrimary,
						HpLossDamage = hpLoss,
						HpLossPerMob = hpLossPerMob,
						MitigatedPerMob = mobs.Select(m => SfrDamageCurve.MitigatedAttack(AttackPower(character, skill), Defense(m, skill))).ToArray(),
						ReferenceMob = isReference,
						MaxHitsOnTarget = damaged.Length > 0 ? damaged.Max(recorder.HitsOn) : 0,
						TotalHits = damaged.Sum(recorder.HitsOn),
						PrimaryDamage = primary != null ? recorder.DamageOn(primary) : 0,
						TotalDamage = recorder.TotalDamage(),
						Factor = factor,
						SpPinned = basicSp ?? 0f,
						SpSpent = spSpent,
						AttackPower = AttackPower(character, skill),
						TargetDefense = Defense(primary, skill),
						MitigatedAttack = SfrDamageCurve.MitigatedAttack(AttackPower(character, skill), Defense(primary, skill)),
						Truncated = (truncated && !covered) || outlivingReason != null,
						TruncationReason = outlivingReason,
						DamageSpanSeconds = shape.SpanSeconds,
						BurstFraction = shape.BurstFraction,
						FullDamageSpanSeconds = fullShape.SpanSeconds,
					};
				}
			}
			catch (Exception ex)
			{
				return new SkillPressResult
				{
					SkillClassName = className,
					ScenarioId = spec.Id,
					CharacterLevel = characterLevel,
					SkillLevel = skillLevel,
					Error = ex.GetType().Name + ": " + ex.Message,
				};
			}
			finally
			{
				DeterministicRandom.Reset();
				GameClock.Use(null);
				SyntheticActors.Cleanup(character, mobs.ToArray());
			}
		}

		/// <summary>
		/// Measures a skill across every scenario the pricer weights, and
		/// returns it in the shape the pricer reads.
		/// </summary>
		/// <param name="skillName"></param>
		/// <param name="skillLevel"></param>
		/// <param name="characterLevel"></param>
		/// <param name="arena">The map to run every scenario's press on.</param>
		/// <param name="measureDefense">
		/// Whether to also run SfrDefenseProbe. It costs
		/// SfrDials.DefenseProbeTrials full encounter windows on its own, so
		/// the single-skill diagnostic loop (SfrPricingTests.Explain) leaves
		/// it off by default; the roster pass always wants it on.
		/// </param>
		/// <param name="pool">
		/// Arenas the skill's own windows fan out across. Every window here is
		/// independent - nine scenarios, two factor points, the defence
		/// trials - and each is mostly Thread.Sleep, so running them serially
		/// made a skill's wall time their sum rather than their maximum. With
		/// a pool the three groups run at once. Null keeps the serial path,
		/// which the single-skill diagnostic uses.
		/// </param>
		/// <param name="measureOffense">
		/// Whether to also run SfrOffenseProbe, which reads what the press adds
		/// to the caster's other damage. Off in the single-skill loop for the
		/// same reason the defence probe is; the roster pass wants it on.
		/// </param>
		public static SfrMeasuredPress MeasureAll(string skillName, int? skillLevel = null, int characterLevel = 50, Map arena = null, bool measureDefense = true, ArenaPool pool = null, bool measureOffense = true)
		{
			if (!ZoneServer.Instance.Data.SkillDb.TryFind(skillName, out var data))
				throw new ArgumentException($"Unknown skill '{skillName}'.", nameof(skillName));

			var prefix = SfrData.ClassOf(skillName);

			if (!JobCatalog.TryGet(prefix, out var job))
				throw new ArgumentException($"'{skillName}' belongs to '{prefix}', which the job catalog does not carry.", nameof(skillName));

			var level = skillLevel ?? SfrData.SkillMaxLevel(skillName);
			var charLevel = ScenarioMatrix.CharacterLevelsFor(job, [characterLevel]).FirstOrDefault(characterLevel);

			var measured = new SfrMeasuredPress
			{
				Skill = skillName,
				SkillLevel = level,
				CharacterLevel = charLevel,
			};

			var fallbackHits = 0;

			void Scenarios()
			{
				var specs = SfrGeometry.PricedScenarios.ToArray();
				var trials = Math.Max(1, SfrDials.ScenarioTrials);
				var low = new SkillPressResult[trials][];
				var high = new SkillPressResult[trials][];

				// Every scenario is run at both factor points, because how many
				// times a press hits each target is a property of the encounter
				// rather than of the skill: Wizard_MagicMissile ricochets off
				// every target it lands on, so it hits once in S1 and four times
				// per target in a pile. Read from S1 alone that was 1.0, and the
				// crowd damage it was actually delivering went unpriced.
				//
				// The whole pair is then repeated ScenarioTrials times and every
				// reading taken as a median across the repeats. A press is a
				// live wall-clock simulation, so how many of a volley's arrows
				// land inside the window is not the same twice, and reach feeds
				// both the width term and the spread cap - which divides by a
				// max across scenarios, the least stable statistic in the model.
				//
				// fullWindow throughout, so a DoT or pad tail is inside the count
				// the same way the old single-scenario pass had it.
				//
				// Counted over the skill's own cycle, since that is the budget
				// the price divides: a trap that keeps ticking for two minutes
				// is delivering for later presses too, and counting all of it
				// against one press reads as a press that hits a hundred times.
				var countWindowMs = (int?)((SfrData.CycleFor(skillName) ?? 0f) * 1000f);

				if (countWindowMs <= 0)
					countWindowMs = null;

				var work = new List<Action>();

				for (var trial = 0; trial < trials; ++trial)
				{
					low[trial] = new SkillPressResult[specs.Length];
					high[trial] = new SkillPressResult[specs.Length];

					for (var i = 0; i < specs.Length; ++i)
					{
						var t = trial;
						var at = i;

						work.Add(() => low[t][at] = Run(specs[at], SfrDamageCurve.BaselineFactor, SfrDials.SpProbeLow));
						work.Add(() => high[t][at] = Run(specs[at], SfrDamageCurve.BaselineFactor * 2f, SfrDials.SpProbeHigh));
					}
				}

				// The two factor points carry the two SP points as well, so
				// what a press charges costs no window of its own: the pair
				// gives the factor line and the SP line at once.
				SkillPressResult Run(ScenarioSpec spec, float factor, float basicSp)
					=> pool == null
						? Measure(job, data.Id, level, spec, charLevel, factor, arena, fullWindow: true, countWindowMs: countWindowMs, basicSp: basicSp)
						: pool.Use(a => Measure(job, data.Id, level, spec, charLevel, factor, a, fullWindow: true, countWindowMs: countWindowMs, basicSp: basicSp));

				if (pool == null)
				{
					foreach (var one in work)
						one();
				}
				else
				{
					RunAll(work.ToArray());
				}

				for (var i = 0; i < specs.Length; ++i)
				{
					var at = i;

					measured.Scenarios[specs[i].Id] = low[0][i];
					measured.Targets[specs[i].Id] = Median(Enumerable.Range(0, trials)
						.SelectMany(t => new[] { low[t][at], high[t][at] })
						.Where(r => r != null && r.Error == null)
						.Select(r => (float)r.TargetsDamaged));
				}

				if (measured.Scenarios.TryGetValue("S1", out var single) && single.Error == null)
				{
					fallbackHits = Math.Max(1, single.HitsOnPrimary);

					measured.DamageSpanSeconds = single.DamageSpanSeconds;
					measured.BurstFraction = single.BurstFraction;
					measured.FullDamageSpanSeconds = single.FullDamageSpanSeconds;
					measured.CountWindowSeconds = SfrData.CycleFor(skillName) ?? 0f;
				}

				MeasureSpCharges(specs, low, high, measured);

				var counts = new List<float>();
				var anyTruncated = false;

				for (var trial = 0; trial < trials; ++trial)
				{
					try
					{
						counts.Add(HitsPerTarget(specs, low[trial], high[trial], out var truncated));
						anyTruncated |= truncated;
					}
					catch (Exception ex)
					{
						measured.HitsFailure = ex.Message;
					}
				}

				if (counts.Count > 0)
				{
					var hits = Median(counts);

					measured.DirectHits = Math.Max(1, (int)Math.Round(hits));
					measured.HitEquivalents = hits;
					measured.HitsTruncated = anyTruncated;
					measured.HitsFromDamage = true;
					measured.HitsFailure = null;
				}
			}

			// Runs for every skill: nothing short of actually watching a live,
			// hostile mob's own output drop can say whether a press bought
			// defensive/CC value, and there is no regex classification left
			// standing in for that triage.
			void Defense()
			{
				if (!measureDefense)
					return;

				var defense = SfrDefenseProbe.Measure(job, data.Id, level, charLevel, arena: arena, pool: pool);

				if (defense.Error == null)
				{
					measured.SwingsPrevented = defense.SwingsPrevented;
					measured.DefenseControls = defense.Controls;
					measured.DefenseTreatments = defense.Treatments;
				}
			}

			// The other half of the same question the defence probe asks: a
			// press that leaves the rest of the rotation hitting harder spent
			// part of its budget there, and nothing but running the caster's own
			// swings twice can say how much.
			void Offense()
			{
				if (!measureOffense)
					return;

				var offense = SfrOffenseProbe.Measure(job, data.Id, level, charLevel, arena: arena, pool: pool);

				if (offense.Error == null)
					measured.DamageAmplification = offense.Amplification;
			}

			if (pool == null)
			{
				Scenarios();
				Defense();
				Offense();
			}
			else
			{
				RunAll(Scenarios, Defense, Offense);
			}

			// Applied last rather than seeded by Scenarios, so the three groups
			// have no ordering between them and can run at once.
			if (!measured.HitsFromDamage && fallbackHits > 0)
				measured.DirectHits = fallbackHits;

			return measured;
		}

		/// <summary>
		/// Returns the median of a set of readings, or zero when there are none.
		/// </summary>
		/// <remarks>
		/// The median rather than the mean throughout, because the readings this
		/// stabilizes are counts taken over a fixed wall-clock window: a press
		/// that happened to have its last arrow land a tick past the boundary is
		/// an outlier, not a sample of the same distribution, and a mean carries
		/// it into the price.
		/// </remarks>
		/// <param name="values"></param>
		private static float Median(IEnumerable<float> values)
		{
			var ordered = values.OrderBy(v => v).ToArray();

			if (ordered.Length == 0)
				return 0f;

			var middle = ordered.Length / 2;

			return ordered.Length % 2 == 1
				? ordered[middle]
				: (ordered[middle - 1] + ordered[middle]) / 2f;
		}

		/// <summary>
		/// Reads how many times one press charges its own SP cost, from the two
		/// pinned costs the scenarios already ran at.
		/// </summary>
		/// <remarks>
		/// SP spent is affine in the cost the skill is pinned at - a press
		/// charges it a fixed number of times, whatever it is - so the slope
		/// between the two points is the charge count and the intercept is
		/// whatever the handler bills at a rate of its own. Most presses read a
		/// slope of one; a channel, or a pad that bills per tick, reads its
		/// tick count, which is the whole reason the pin exists.
		///
		/// Taken from the single-target scenario, so a wide press is not read
		/// as charging once per target it reached. Averaged over nothing: the
		/// two points are exact, since the pin removes the level term.
		/// </remarks>
		/// <param name="specs"></param>
		/// <param name="low"></param>
		/// <param name="high"></param>
		/// <param name="measured"></param>
		private static void MeasureSpCharges(ScenarioSpec[] specs, SkillPressResult[][] low, SkillPressResult[][] high, SfrMeasuredPress measured)
		{
			var at = Array.FindIndex(specs, s => s.Id == "S1");

			if (at < 0)
				return;

			var slopes = new List<float>();
			var fixedSpends = new List<float>();

			for (var trial = 0; trial < low.Length; ++trial)
			{
				var a = low[trial][at];
				var b = high[trial][at];

				if (a == null || b == null || a.Error != null || b.Error != null)
					continue;

				var span = b.SpPinned - a.SpPinned;

				if (span <= 0)
					continue;

				var slope = (b.SpSpent - a.SpSpent) / span;

				if (slope < SfrDials.MinSpChargeSlope)
					continue;

				slopes.Add(slope);
				fixedSpends.Add(Math.Max(0f, a.SpSpent - slope * a.SpPinned));
			}

			if (slopes.Count == 0)
				return;

			measured.SpChargeSlope = Median(slopes);
			measured.SpFixedSpend = Median(fixedSpends);
			measured.SpMeasured = true;
		}

		/// <summary>
		/// Measures the same press at two factors and returns the line through
		/// them, which inverts to a factor in one step.
		/// </summary>
		/// <remarks>
		/// Damage is affine in the factor - SCR_CalculateDamage mitigates the
		/// attack on a ratio the factor is not part of, then multiplies the
		/// factor in - so two points determine it exactly and there is nothing
		/// to converge on.
		/// </remarks>
		/// <param name="job"></param>
		/// <param name="skillId"></param>
		/// <param name="skillLevel"></param>
		/// <param name="spec"></param>
		/// <param name="characterLevel"></param>
		public static SfrDamageCurve.FactorLine MeasureFactorLine(JobEntry job, SkillId skillId, int skillLevel, ScenarioSpec spec, int characterLevel)
		{
			const float low = SfrDamageCurve.BaselineFactor;
			const float high = SfrDamageCurve.BaselineFactor * 2f;

			var a = Measure(job, skillId, skillLevel, spec, characterLevel, low);
			var b = Measure(job, skillId, skillLevel, spec, characterLevel, high);

			if (a.Error != null)
				throw new InvalidOperationException(a.Error);

			if (b.Error != null)
				throw new InvalidOperationException(b.Error);

			return SfrDamageCurve.Solve(low, a.PrimaryDamage, high, b.PrimaryDamage);
		}

		/// <summary>
		/// Returns how many single-hit-equivalents one press lands on each
		/// target it reaches, averaged over every scenario it reached one in.
		/// </summary>
		/// <remarks>
		/// Damage is affine in the factor, so the slope of HP lost per factor
		/// point, over what one hit of the same skill is worth per factor
		/// point, is what the press delivered. Every source counts the same
		/// way - direct hits, pad ticks, DoT ticks - because HP loss is all
		/// this reads.
		///
		/// Two things this is not. It is not read from S1 alone: a press that
		/// hits each target more often the more of them there are - a ricochet,
		/// a bounce, a pierce - lands once in a single-target scenario and its
		/// crowd delivery went entirely unpriced. And it is per target rather
		/// than in total, because the target count is already charged once, as
		/// reach; multiplying the two back together is the press's whole
		/// delivery, which is exactly the budget the pricer spreads.
		///
		/// The reference is one application of this same skill, taken as S1's
		/// slope over the applications the recorder counted there. It was
		/// MitigatedAttack, the attack left after the defence curve - which is
		/// what the factor multiplies, but not what the target finally loses,
		/// since the attribute and armour multipliers land after it. Those are
		/// the same for every hit of one skill and cancel here; against
		/// MitigatedAttack they did not, so every magic press read 1.25 hits
		/// where a physical one read 1.00 and the whole magic roster was
		/// divided down against a physical anchor.
		///
		/// Rescaled per scenario by that scenario's own MitigatedAttack, since
		/// the mobs are not the same from one to the next: S7 stands a boss
		/// where S1 stands a trash mob, and the pulls now rotate races. Left
		/// unscaled, a target that simply takes less per hit read as fewer
		/// hits - Pyromancer_FireBall hits twice in both S1 and S7 and scored
		/// 2.00 against 1.53.
		/// </remarks>
		/// <param name="specs"></param>
		/// <param name="low"></param>
		/// <param name="high"></param>
		/// <param name="truncated"></param>
		public static float HitsPerTarget(ScenarioSpec[] specs, SkillPressResult[] low, SkillPressResult[] high, out bool truncated)
		{
			const float span = SfrDamageCurve.BaselineFactor;

			truncated = false;

			bool Usable(int i)
			{
				var a = low[i];
				var b = high[i];

				return a != null && b != null && a.Error == null && b.Error == null
					&& a.HpLossPerMob.Length == b.HpLossPerMob.Length
					&& a.HpLossPerMob.Length == a.MitigatedPerMob.Length
					&& a.HpLossPerMob.Length == a.ReferenceMob.Length;
			}

			// Slope per placed monster, over what one hit against that same
			// monster is worth per factor point. The bonus below is what turns
			// MitigatedAttack into real HP lost.
			float Slope(int i, int m)
				=> (high[i].HpLossPerMob[m] - low[i].HpLossPerMob[m]) / span;

			// The post-factor multiplier: what one application really takes off
			// against what MitigatedAttack says it should. Read from the
			// recorder, not from the HP-loss slope. The slope carries the whole
			// press - DoT ticks included, which is the point of reading it - so
			// dividing it by the applications the recorder counted folds a DoT
			// into its own reference and prices it as a single hit:
			// Wugushi_LatentVenom's 26 s bleed read 1.0 and rose x24.
			//
			// A pad tick is recorded (PadHelper attacks as the creator), so a
			// pad skill's reference is one tick and its count is its ticks -
			// Pyromancer_FlameGround is unaffected either way. A
			// DamageOverTimeBuffHandler tick is not, and here that blindness is
			// exactly what is wanted.
			var bonus = 0f;

			for (var pass = 0; pass < 2 && bonus <= 0; ++pass)
			{
				for (var i = 0; i < specs.Length && bonus <= 0; ++i)
				{
					if (pass == 0 && specs[i].Id != "S1")
						continue;

					if (!Usable(i) || low[i].HitsOnPrimary <= 0)
						continue;

					var application = low[i].PrimaryDamage / low[i].HitsOnPrimary;

					if (application > 0)
						bonus = application / Math.Max(1f, low[i].MitigatedAttack);
				}
			}

			// Nothing the recorder could see anywhere, which means the damage
			// was not dealt by the caster at all - a statue, a companion, a
			// summon, whose attacks carry their own attacker and never reach
			// the hook, though the HP they take still shows in the slope.
			// Falls back to MitigatedAttack unscaled, which is what the whole
			// model used before, rather than refusing to price the skill.
			if (bonus <= 0)
				bonus = 1f;

			var perTarget = new List<float>();

			for (var i = 0; i < specs.Length; ++i)
			{
				if (!Usable(i) || low[i].TargetsDamaged <= 0)
					continue;

				var equivalents = 0f;
				var counted = 0;

				for (var m = 0; m < low[i].HpLossPerMob.Length; ++m)
				{
					if (!low[i].ReferenceMob[m])
						continue;

					var slope = Slope(i, m);

					if (slope <= 0)
						continue;

					equivalents += slope * span / (Math.Max(1f, low[i].MitigatedPerMob[m]) * bonus);
					counted++;
				}

				if (counted == 0)
					continue;

				truncated |= low[i].Truncated || high[i].Truncated;

				perTarget.Add(equivalents / counted);
			}

			if (perTarget.Count == 0)
				throw new InvalidOperationException("HP lost did not grow with the factor in any scenario.");

			return Math.Max(1f, perTarget.Average());
		}

		/// <summary>
		/// Dispatches the handler once and waits out only the work that press
		/// scheduled for itself.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="aimPos"></param>
		/// <param name="mobs"></param>
		/// <param name="map"></param>
		/// <param name="recorder"></param>
		/// <param name="fullWindow">
		/// Holds the window open for anything the press left running - a DoT
		/// or a pad - so its whole tail has landed before the HP it took is
		/// read, rather than exiting the moment the handler returns.
		/// </param>
		/// <param name="timeline"></param>
		/// <param name="trigger"></param>
		private static bool Press(Skill skill, Character caster, Position aimPos, List<Mob> mobs, Map map, SfrPressRecorder recorder, bool fullWindow = false, List<(int ElapsedMs, float Loss, float[] PerMob)> timeline = null, Skill trigger = null)
		{
			if (!Dispatch(skill, caster, aimPos, mobs))
				return false;

			var tick = TimeSpan.FromMilliseconds(TickMs);
			var clock = GameClock.Current;
			var triggered = trigger == null;

			for (var elapsed = 0; elapsed < MaxPressMs; elapsed += TickMs)
			{
				Step(clock, tick);
				map.Update(tick);

				if (timeline != null)
				{
					var losses = HpLostPerMob(mobs);
					timeline.Add((elapsed, losses.Sum(), losses));
				}

				// A trap sits armed until something sets it off, so the press
				// that plants it lands nothing on its own.
				if (!triggered && elapsed >= SfrDials.TriggerPressDelayMs)
				{
					Dispatch(trigger, caster, aimPos, mobs);
					triggered = true;
				}

				// A handler that paces itself with skill.Wait is still this
				// press until its runners return; one that scheduled nothing
				// only needs the settle window.
				//
				// A press that has not landed anything yet is held longer than
				// one that has. skill.IsRunning goes false between the handler
				// returning and its own continuation being scheduled, so under
				// a wide roster run - where a continuation can wait behind
				// hundreds of other threads - the short settle was letting a
				// real press be recorded as having damaged nothing.
				// fullWindow does not mean "sleep the whole window", it means
				// "do not read the HP until the tail has landed" - and what is
				// still delivering is the drain loop's own test, not the clock.
				// Sleeping it out unconditionally cost a clean burst 10 s where
				// it needed 1.5, on every one of the 18 windows a skill now
				// runs, and is what took the roster pass from 3.5 min to 12.
				if (triggered && !skill.IsRunning && elapsed >= SettleMs
					&& (recorder.TotalDamage() > 0 || elapsed >= SfrDials.EmptyPressSettleMs)
					&& !HasLiveSummon(map, caster)
					&& (!fullWindow || (OutlivingTickReason(caster, mobs) == null && OutlivingPadReason(map, caster) == null)))
				{
					return false;
				}
			}

			// Let a short DoT or pad finish rather than reading a partial tail.
			for (var drained = 0; drained < DrainMs; drained += TickMs)
			{
				if (!skill.IsRunning && OutlivingTickReason(caster, mobs) == null && OutlivingPadReason(map, caster) == null)
					return false;

				Step(clock, tick);
				map.Update(tick);

				if (timeline != null)
				{
					var drainLosses = HpLostPerMob(mobs);
					timeline.Add((MaxPressMs + drained, drainLosses.Sum(), drainLosses));
				}
			}

			return skill.IsRunning || OutlivingTickReason(caster, mobs) != null || OutlivingPadReason(map, caster) != null;
		}

		/// <summary>
		/// Moves one tick of the press's clock, virtual or real.
		/// </summary>
		/// <remarks>
		/// With a virtual clock the tick costs nothing and everything paced
		/// against it - a handler's own Wait, a buff's expiry, a pad's
		/// lifetime - resolves on this thread before the call returns, so the
		/// press replays identically however loaded the machine is. Without
		/// one it falls back to sleeping, which is what the whole measurement
		/// used to do.
		/// </remarks>
		/// <param name="clock"></param>
		/// <param name="tick"></param>
		internal static void Step(VirtualClock clock, TimeSpan tick)
		{
			if (clock == null)
			{
				Thread.Sleep(tick);
				return;
			}

			clock.Advance(tick);
		}

		/// <summary>
		/// Invokes the handler for the skill's use type, mirroring what the
		/// packet handlers do.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="aimPos"></param>
		/// <param name="mobs"></param>
		private static bool Dispatch(Skill skill, Character caster, Position aimPos, List<Mob> mobs)
		{
			var handlers = ZoneServer.Instance.SkillHandlers;
			var origin = caster.Position;
			var maxRange = skill.Data.MaxRange;

			// The packet handlers reject a cast the client could not have
			// made, and nothing below re-checks it: Nearest() handed a
			// 150-range projectile S5's column at 200, so a short-ranged skill
			// scored the reach of a long-ranged one - amplified by MinYardstick,
			// since S5's own yardstick floors at 0.5.
			if (maxRange > 0 && origin.Get2DDistance(aimPos) > maxRange)
				return false;

			var targets = mobs.Cast<ICombatEntity>()
				.Where(caster.CanDamage)
				.Where(t => maxRange <= 0 || origin.Get2DDistance(t.Position) <= maxRange)
				.ToList();

			// CZ_SKILL_TOOL_GROUND_POS carries the aim point for a
			// ground-targeted skill, and 47 handlers bail out when it is
			// missing rather than falling back to farPos.
			skill.Vars.Set(GroundPosVariable, aimPos);

			switch (skill.Data.UseType)
			{
				case SkillUseType.MeleeGround when handlers.TryGetHandler<IMeleeGroundSkillHandler>(skill.Id, out var melee):
					melee.Handle(skill, caster, origin, aimPos, targets);
					return true;

				case SkillUseType.Self when handlers.TryGetHandler<ISelfSkillHandler>(skill.Id, out var self):
					self.Handle(skill, caster, origin, caster.Direction);
					return true;

				case SkillUseType.Force when handlers.TryGetHandler<IForceSkillHandler>(skill.Id, out var force):
					force.Handle(skill, caster, origin, aimPos, Nearest(targets, aimPos));
					return true;

				case SkillUseType.ForceGround when handlers.TryGetHandler<IForceGroundSkillHandler>(skill.Id, out var forceGround):
					forceGround.Handle(skill, caster, origin, aimPos, Nearest(targets, aimPos));
					return true;
			}

			if (handlers.TryGetHandler<IGroundSkillHandler>(skill.Id, out var ground))
			{
				ground.Handle(skill, caster, origin, aimPos, Nearest(targets, aimPos));
				return true;
			}

			if (handlers.TryGetHandler<ITargetSkillHandler>(skill.Id, out var target))
			{
				target.Handle(skill, caster, Nearest(targets, aimPos));
				return true;
			}

			// A passive is registered on load, and its damage lands through the skill that spends what it set up.
			if (handlers.TryGetPassiveSkillHandler<IPassiveSkillHandler>(skill.Id, out var passive))
			{
				passive.Handle(skill, caster);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Returns the target closest to the aim point, which is what the
		/// client would have locked.
		/// </summary>
		/// <param name="targets"></param>
		/// <param name="aimPos"></param>
		private static ICombatEntity Nearest(List<ICombatEntity> targets, Position aimPos)
			=> targets.OrderBy(t => t.Position.Get2DDistance(aimPos)).FirstOrDefault();

		/// <summary>
		/// Returns the attack the skill rolls against, averaged over its
		/// range, mirroring SCR_GetRandomAtk's choice of stat.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="skill"></param>
		private static float AttackPower(ICombatEntity caster, Skill skill)
		{
			var patk = (caster.Properties.GetFloat(PropertyName.MINPATK) + caster.Properties.GetFloat(PropertyName.MAXPATK)) / 2f;
			var matk = (caster.Properties.GetFloat(PropertyName.MINMATK) + caster.Properties.GetFloat(PropertyName.MAXMATK)) / 2f;

			return skill.Data.ClassType switch
			{
				SkillClassType.Responsive => Math.Max(patk, matk),
				<= SkillClassType.Missile => patk,
				_ => matk,
			};
		}

		/// <summary>
		/// Returns the defense the skill is opposed by.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		private static float Defense(ICombatEntity target, Skill skill)
		{
			if (target == null)
				return 0f;

			var name = skill.Data.ClassType != SkillClassType.Magic ? PropertyName.DEF : PropertyName.MDEF;

			return target.Properties.GetFloat(name);
		}

		/// <summary>
		/// How much time a ticking buff must still have left to count as
		/// outliving the press.
		/// </summary>
		private static readonly TimeSpan OutlivingTickTolerance = TimeSpan.FromMilliseconds(TickMs * 4);

		/// <summary>
		/// Returns the buff this press applied that is still ticking, if any.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="mobs"></param>
		private static string OutlivingTickReason(Character caster, List<Mob> mobs)
		{
			foreach (var mob in mobs)
			{
				var buffs = mob.Components.Get<BuffComponent>()?.GetList();

				if (buffs == null)
					continue;

				foreach (var buff in buffs)
				{
					if (buff.Caster == caster && buff.HasUpdateTime && buff.RemainingDuration > OutlivingTickTolerance)
						return $"{buff.Id} ({buff.RemainingDuration.TotalMilliseconds:0} ms left)";
				}
			}

			return null;
		}

		/// <summary>
		/// Returns the pad this press created that is still alive, if any.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="caster"></param>
		private static string OutlivingPadReason(Map map, Character caster)
		{
			var pad = map.GetPads(p => p.Creator == caster).FirstOrDefault(p => p.Trigger.RemainingLifeTime > OutlivingTickTolerance);

			return pad != null ? $"{pad.Name} ({pad.Trigger.RemainingLifeTime.TotalMilliseconds:0} ms left)" : null;
		}

		/// <summary>
		/// Returns whether the caster still has something summoned that is
		/// alive and dealing damage on its behalf.
		/// </summary>
		/// <remarks>
		/// Read only by the press window, never by the drain loop or the
		/// truncation verdict. A summon's damage is the skill's damage and the
		/// window has to stay open to see any of it - Dievdirbys_CarveOwl's
		/// owl attacks every 2.8 s, so a press exiting on the 1.5 s settle saw
		/// none of it. But an owl lives 15 + 2/level seconds, past the 10 s
		/// window and its 30 s drain together, so counting it as still
		/// delivering would reject every summon as Truncated instead. The
		/// window is a sample of what the summon does, which is what the pricer
		/// wants; the tail beyond it is the same limitation a pet has.
		/// </remarks>
		/// <param name="map"></param>
		/// <param name="caster"></param>
		private static bool HasLiveSummon(Map map, Character caster)
			=> map.GetMonsters(m => m.OwnerHandle == caster.Handle).Any(m => m is not ICombatEntity live || !live.IsDead);

		/// <summary>
		/// Returns the HP every mob has lost so far.
		/// </summary>
		/// <param name="mobs"></param>
		/// <remarks>
		/// A dead mob is left out. Nothing dies of damage at SurvivalHp, so a
		/// corpse means an execute - Priest_TurnUndead rolls one on a chance
		/// scaled by the target's max HP - and counting its whole bar as HP
		/// this press took reads as a hundred thousand hits. What the factor
		/// is worth is the damage, and the execute rides on top of it.
		/// </remarks>
		private static float HpLost(List<Mob> mobs)
			=> mobs.Where(m => !m.IsDead).Sum(m => Math.Max(0f, m.Properties.GetFloat(PropertyName.MHP) - m.Properties.GetFloat(PropertyName.HP)));

		/// <summary>
		/// Returns the HP each mob has lost so far, in the order they were
		/// placed, so the count can be read at the cycle boundary rather than
		/// only at the end of the press.
		/// </summary>
		/// <param name="mobs"></param>
		private static float[] HpLostPerMob(List<Mob> mobs)
			=> mobs.Select(m => m.IsDead ? 0f : Math.Max(0f, m.Properties.GetFloat(PropertyName.MHP) - m.Properties.GetFloat(PropertyName.HP))).ToArray();

		/// <summary>
		/// Returns the skill that sets off what this press only plants, given
		/// to the caster so the press can be measured at all.
		/// </summary>
		/// <remarks>
		/// A trap does no damage until something detonates it, and the probe
		/// has nothing walking onto it. The partner is pressed at level one so
		/// what is measured is the trap's own damage and not the riders the
		/// partner's own levels add on top.
		/// </remarks>
		/// <param name="character"></param>
		/// <param name="skillName"></param>
		private static Skill TriggerFor(Character character, string skillName)
		{
			if (!SfrDials.PressTriggers.TryGetValue(skillName, out var triggerName))
				return null;

			if (!ZoneServer.Instance.Data.SkillDb.TryFind(triggerName, out var triggerData))
				return null;

			return SyntheticActors.GiveSkill(character, triggerData.Id, 1);
		}

		/// <summary>
		/// Returns the timeline cut off at the count window.
		/// </summary>
		/// <param name="timeline"></param>
		/// <param name="windowMs"></param>
		private static List<(int ElapsedMs, float Loss, float[] PerMob)> Bound(List<(int ElapsedMs, float Loss, float[] PerMob)> timeline, int? windowMs)
		{
			if (timeline == null || windowMs == null)
				return timeline;

			var past = timeline.FindIndex(s => s.ElapsedMs > windowMs.Value);

			return past < 0 ? timeline : timeline.GetRange(0, Math.Max(1, past));
		}

		/// <summary>
		/// Returns how long the press spent delivering damage, and the largest
		/// share of it landing inside one burst window.
		/// </summary>
		/// <param name="timeline"></param>
		private static (float SpanSeconds, float BurstFraction) Shape(List<(int ElapsedMs, float Loss, float[] PerMob)> timeline)
		{
			if (timeline == null || timeline.Count == 0)
				return (0f, 1f);

			var total = timeline[timeline.Count - 1].Loss;

			if (total <= 0)
				return (0f, 1f);

			var first = timeline.FindIndex(s => s.Loss > 0);
			var last = timeline.FindLastIndex(s => s.Loss < total);

			var startMs = first >= 0 ? timeline[first].ElapsedMs : 0;
			var endMs = last >= 0 && last + 1 < timeline.Count ? timeline[last + 1].ElapsedMs : startMs;

			var peak = 0f;

			for (var i = 0; i < timeline.Count; ++i)
			{
				var windowStart = timeline[i].ElapsedMs - SfrDials.BurstWindowMs;
				var before = 0f;

				for (var j = i; j >= 0; --j)
				{
					if (timeline[j].ElapsedMs <= windowStart)
					{
						before = timeline[j].Loss;
						break;
					}
				}

				peak = Math.Max(peak, timeline[i].Loss - before);
			}

			return (Math.Max(0f, endMs - startMs) / 1000f, Math.Min(1f, peak / total));
		}

		/// <summary>
		/// Gives an entity enough max HP that one press cannot kill it.
		/// </summary>
		/// <param name="entity"></param>
		private static void Fortify(ICombatEntity entity)
		{
			entity.Properties.SetFloat(PropertyName.MHP_BM, SurvivalHp);
			entity.Properties.Invalidate(PropertyName.MHP);
			entity.Properties.SetFloat(PropertyName.HP, entity.Properties.GetFloat(PropertyName.MHP));
		}

		/// <summary>
		/// Tops the caster up so an empty SP pool is not what the press
		/// measures.
		/// </summary>
		/// <remarks>
		/// The bar is widened and its regeneration switched off, because the SP
		/// a press spends is read off it. A channel pinned at SpProbeHigh bills
		/// its cost every tick and would otherwise run the bar dry - which
		/// stops the channel, so the damage measurement would move with the SP
		/// pin too. SCR_Get_Character_RSP floors at zero, so a large enough
		/// negative RSP_BM is what stops RecoverSp handing SP back mid-window.
		/// SurvivalSp stays inside the range a float holds exactly, so a spend
		/// of a few SP is not lost in the rounding.
		/// </remarks>
		/// <param name="character"></param>
		private static void Refill(Character character)
		{
			character.Properties.SetFloat(PropertyName.MSP_BM, SurvivalSp);
			character.Properties.SetFloat(PropertyName.RSP_BM, -SurvivalSp);
			character.Properties.Invalidate(PropertyName.MSP, PropertyName.RSP);

			character.Properties.SetFloat(PropertyName.HP, character.Properties.GetFloat(PropertyName.MHP));
			character.Properties.SetFloat(PropertyName.SP, character.Properties.GetFloat(PropertyName.MSP));
		}
	}
}
