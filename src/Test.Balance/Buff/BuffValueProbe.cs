using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Test.Balance.Sfr;
using Melia.Zone;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Maps;

namespace Melia.Test.Balance.Buff
{
	/// <summary>
	/// What one buff was worth, measured by running the same fight twice.
	/// </summary>
	public class BuffValueResult
	{
		public string SkillClassName { get; init; }
		public string ScenarioId { get; init; }
		public int CharacterLevel { get; init; }
		public int BuffLevel { get; init; }

		/// <summary>
		/// The scale the caption ratios were held at for the reading, where 1
		/// is what the data currently carries.
		/// </summary>
		public float SlotScale { get; init; }

		/// <summary>
		/// Buffs that were actually up when the window ran.
		/// </summary>
		public BuffId[] Applied { get; init; } = [];

		/// <summary>
		/// Whether the press put its effect on the enemy rather than on the
		/// caster.
		/// </summary>
		/// <remarks>
		/// A debuff's gains are the same measurements read from the other side:
		/// cutting an enemy's block raises what the caster deals, and cutting
		/// its attack lowers what the caster takes. Both already fall out of the
		/// window - what changes is only which of the two is which, so nothing
		/// here needs a second kind of measurement.
		/// </remarks>
		public bool OnEnemy { get; init; }

		/// <summary>
		/// Whether the side was seen rather than assumed. False for a press the
		/// probe could not dispatch, whose effect the fallback puts on the
		/// caster.
		/// </summary>
		public bool SideObserved { get; init; }

		public float ControlDealt { get; init; }
		public float TreatmentDealt { get; init; }
		public float ControlTaken { get; init; }
		public float TreatmentTaken { get; init; }

		/// <summary>
		/// Swings each side landed in the control window, which is what says
		/// whether a reading has any signal behind it at all.
		/// </summary>
		public float ControlDealtHits { get; init; }
		public float ControlTakenHits { get; init; }

		/// <summary>
		/// Damage dealt with the buff over damage dealt without it.
		/// </summary>
		public float GainOffense { get; init; }

		/// <summary>
		/// Damage taken without the buff over damage taken with it, inverted so
		/// that above 1 always means the buff helped.
		/// </summary>
		public float GainDefense { get; init; }

		/// <summary>
		/// The two gains folded into one number, per BuffDials.DefenseWeight.
		/// </summary>
		public float Value
			=> this.GainOffense <= 0 || this.GainDefense <= 0
				? 0f
				: this.GainOffense * (float)Math.Pow(this.GainDefense, BuffDials.DefenseWeight);

		/// <summary>
		/// Whether the buff moved either side of the fight at all.
		/// </summary>
		public bool HasEffect
			=> this.Error == null && Math.Abs(this.Value - 1f) > BuffDials.EffectTolerance;

		public string Error { get; init; }

		public override string ToString()
			=> this.Error != null
				? $"{this.SkillClassName}: FAILED ({this.Error})"
				: $"{this.SkillClassName} sk{this.BuffLevel} x{this.SlotScale:0.00} " +
				  $"[{string.Join(", ", this.Applied)}]: " +
				  $"off {this.GainOffense:0.000}x def {this.GainDefense:0.000}x -> value {this.Value:0.000}";
	}

	/// <summary>
	/// Measures what a buff is worth by fighting the same mob twice, once with
	/// the buff up and once without, and reading both directions of damage.
	/// </summary>
	/// <remarks>
	/// This is the buff half of SfrDefenseProbe and shares its shape: paired
	/// control and treatment windows on a common seed, a live hostile mob, and
	/// a trimmed mean of the per-pair readings. Two things differ, and both are
	/// forced by what a buff is.
	///
	/// The character attacks throughout rather than standing idle, because a
	/// buff's offensive half only exists while damage is going out. The swing
	/// interval is re-read from the skill's own ShootTime after every swing, so
	/// an attack-speed buff shortens it exactly as it would in play - a per-cast
	/// sample cannot see that at all, and reads an ASPD buff as doing nothing.
	///
	/// Dodge, block and crit are left live. The damage probe pins all three off
	/// so a hit count cannot be a coin flip, but Finestra and High Guard consist
	/// of nothing except those rolls, and measure exactly zero with them pinned.
	/// The cost is that a reading is a sample rather than a replayable count,
	/// which is what the trial count and the long window pay for.
	/// </remarks>
	public static class BuffValueProbe
	{
		private const int TickMs = 25;

		private static readonly ConcurrentDictionary<string, WindowReading> _controls = new();

		/// <summary>
		/// Runs one body on BuffDials.RosterWorkers threads and waits for all of
		/// them.
		/// </summary>
		/// <remarks>
		/// The body is expected to drain a shared queue, so a worker that
		/// finishes early picks up the next subject rather than idling to the end
		/// of a partition - buffs differ by several times in how many scales
		/// their solve takes, which is what makes a fixed split the wrong shape.
		///
		/// LongRunning for the reason SkillPressProbe.RunAll gives: a window
		/// never yields, and the thread pool treats a busy thread as a reason to
		/// inject replacements a couple a second rather than starting the fan-out
		/// at full width.
		/// </remarks>
		/// <param name="body"></param>
		public static void RunWorkers(Action body)
			=> Task.WaitAll(Enumerable.Range(0, BuffDials.RosterWorkers)
				.Select(_ => Task.Factory.StartNew(body, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default))
				.ToArray());

		/// <summary>
		/// Measures one buff-granting press.
		/// </summary>
		/// <param name="subject"></param>
		/// <param name="job"></param>
		/// <param name="slotScale"></param>
		/// <param name="buffLevel"></param>
		/// <param name="characterLevel"></param>
		/// <param name="pool"></param>
		/// <param name="applyBuff">
		/// False runs the buff-free window on both sides of the pair, which
		/// measures the probe's own noise floor rather than the buff. Every
		/// gain it reports should then be 1.
		/// </param>
		/// <param name="scenario">
		/// Conditions to measure under. Defaults to the first, which is the solo
		/// all-in build against a plain enemy.
		/// </param>
		/// <param name="slotsOverride">
		/// Slot seeds to install instead of subject.Slots, with any slot it
		/// omits hard-zeroed rather than left at the subject's own. Isolates one
		/// axis of a multi-slot buff for an independent solve.
		/// </param>
		public static BuffValueResult Measure(BuffSubject subject, JobEntry job = null, float slotScale = 1f,
			int buffLevel = BuffDials.ProbeBuffLevel, int characterLevel = BuffDials.ProbeLevel, ArenaPool pool = null,
			bool applyBuff = true, BuffScenario scenario = null, BuffSubject[] alsoHeld = null,
			IReadOnlyDictionary<int, float> slotsOverride = null)
		{
			scenario ??= BuffScenarios.All[0];
			job ??= JobCatalog.Entries.FirstOrDefault(e => e.SkillPrefix == subject.ClassName);

			if (job == null)
			{
				return new BuffValueResult
				{
					SkillClassName = subject.SkillClassName,
					Error = $"no job entry for class '{subject.ClassName}'",
				};
			}

			var controlDealt = new float[BuffDials.Trials];
			var controlTaken = new float[BuffDials.Trials];
			var controlDealtHits = new float[BuffDials.Trials];
			var controlTakenHits = new float[BuffDials.Trials];
			var treatmentDealt = new float[BuffDials.Trials];
			var treatmentTaken = new float[BuffDials.Trials];
			var applied = new BuffId[BuffDials.Trials][];
			var onEnemy = new bool[BuffDials.Trials];
			var sideObserved = new bool[BuffDials.Trials];
			var errors = new string[BuffDials.Trials];

			SkillPressProbe.RunAll(Enumerable.Range(0, BuffDials.Trials).Select(trial => (Action)(() =>
			{
				try
				{
					// Control and treatment stay on one flow so the seed they
					// share is the same GameRandom instance.
					DeterministicRandom.Seed(SkillPressProbe.Seed + trial);

					var control = _controls.GetOrAdd(ControlKey(subject, job, buffLevel, characterLevel, scenario, alsoHeld, trial),
						_ => RunOn(pool, m => RunWindow(job, subject, buffLevel, characterLevel, slotScale, false, m, scenario, alsoHeld, slotsOverride)));

					DeterministicRandom.Seed(SkillPressProbe.Seed + trial);
					var treatment = RunOn(pool, m => RunWindow(job, subject, buffLevel, characterLevel, slotScale, applyBuff, m, scenario, alsoHeld, slotsOverride));

					controlDealt[trial] = control.Dealt;
					controlTaken[trial] = control.Taken;
					controlDealtHits[trial] = control.DealtHits;
					controlTakenHits[trial] = control.TakenHits;

					treatmentDealt[trial] = treatment.Dealt;
					treatmentTaken[trial] = treatment.Taken;
					applied[trial] = treatment.Applied;
					onEnemy[trial] = treatment.OnEnemy;
					sideObserved[trial] = treatment.SideObserved;
				}
				catch (Exception ex)
				{
					errors[trial] = ex.GetType().Name + ": " + ex.Message;
				}
				finally
				{
					DeterministicRandom.Reset();
				}
			})).ToArray());

			var error = errors.FirstOrDefault(e => e != null);

			if (error != null)
			{
				return new BuffValueResult
				{
					SkillClassName = subject.SkillClassName,
					ScenarioId = scenario.Id,
				CharacterLevel = characterLevel,
					BuffLevel = buffLevel,
					SlotScale = slotScale,
					Error = error,
				};
			}

			var landed = applied.FirstOrDefault(a => a is { Length: > 0 }) ?? [];

			if (landed.Length == 0 && applyBuff)
			{
				return new BuffValueResult
				{
					SkillClassName = subject.SkillClassName,
					ScenarioId = scenario.Id,
				CharacterLevel = characterLevel,
					BuffLevel = buffLevel,
					SlotScale = slotScale,
					Error = "the press applied no buff and none could be applied directly",
				};
			}

			return new BuffValueResult
			{
				SkillClassName = subject.SkillClassName,
				ScenarioId = scenario.Id,
				CharacterLevel = characterLevel,
				BuffLevel = buffLevel,
				SlotScale = slotScale,
				Applied = landed,
				OnEnemy = onEnemy.Any(v => v),
				SideObserved = sideObserved.Any(v => v),
				ControlDealt = controlDealt.Average(),
				TreatmentDealt = treatmentDealt.Average(),
				ControlTaken = controlTaken.Average(),
				TreatmentTaken = treatmentTaken.Average(),
				ControlDealtHits = controlDealtHits.Average(),
				ControlTakenHits = controlTakenHits.Average(),
				GainOffense = PairedTrimmedRatio(treatmentDealt, controlDealt),
				GainDefense = PairedTrimmedRatio(controlTaken, treatmentTaken),
			};
		}

		/// <summary>
		/// Runs one window on a pooled arena, or on the shared one when there
		/// is no pool.
		/// </summary>
		/// <param name="pool"></param>
		/// <param name="work"></param>
		private static WindowReading RunOn(ArenaPool pool, Func<Map, WindowReading> work)
			=> pool != null ? pool.Use(work) : work(SyntheticActors.GetArena());

		/// <summary>
		/// Identifies a control window by everything that can change what it
		/// reads.
		/// </summary>
		/// <remarks>
		/// The scale and the slot seeds are deliberately absent. Both reach a
		/// window through BuffCaptionScope alone, and a control window applies
		/// no buff, so nothing in it ever reads a caption ratio: the same seed
		/// over the same fight returns the same numbers at every scale the
		/// solve tries. A solve costs one control per scenario and level
		/// instead of one per scale, and the pair is still measured against a
		/// control taken under exactly its own conditions.
		/// </remarks>
		/// <param name="subject"></param>
		/// <param name="job"></param>
		/// <param name="buffLevel"></param>
		/// <param name="characterLevel"></param>
		/// <param name="scenario"></param>
		/// <param name="alsoHeld"></param>
		/// <param name="trial"></param>
		private static string ControlKey(BuffSubject subject, JobEntry job, int buffLevel, int characterLevel,
			BuffScenario scenario, BuffSubject[] alsoHeld, int trial)
		{
			var held = alsoHeld == null ? "" : string.Join(',', alsoHeld.Select(s => s.SkillClassName));

			return string.Join('|', subject.SkillClassName, job.SkillPrefix, buffLevel, characterLevel, scenario.Id, trial, held);
		}

		/// <summary>
		/// Returns the trimmed mean of the per-pair ratios between two matched
		/// sets of windows.
		/// </summary>
		/// <remarks>
		/// A ratio rather than a difference, because what a buff is worth is
		/// multiplicative and the two windows it is read across have no reason
		/// to hold the same absolute damage. Per pair rather than between two
		/// means, for the reason SfrDefenseProbe gives: control and treatment
		/// share a seed, so the comparison belongs inside the pair.
		/// </remarks>
		/// <param name="numerators"></param>
		/// <param name="denominators"></param>
		private static float PairedTrimmedRatio(float[] numerators, float[] denominators)
		{
			var ratios = numerators
				.Zip(denominators, (n, d) => d <= 0 ? 1f : n / d)
				.OrderBy(r => r)
				.ToArray();

			if (ratios.Length == 0)
				return 1f;

			var trim = (int)(ratios.Length * BuffDials.TrimShare);

			return ratios.Skip(trim).Take(Math.Max(1, ratios.Length - trim * 2)).Average();
		}

		/// <summary>
		/// What one window read.
		/// </summary>
		/// <param name="Applied"></param>
		/// <param name="OnEnemy"></param>
		/// <param name="SideObserved"></param>
		/// <param name="Dealt"></param>
		/// <param name="Taken"></param>
		/// <param name="DealtHits"></param>
		/// <param name="TakenHits"></param>
		private readonly record struct WindowReading(BuffId[] Applied, bool OnEnemy, bool SideObserved,
			float Dealt, float Taken, int DealtHits, int TakenHits);

		/// <summary>
		/// Where a press put its effect, and whether that was seen or assumed.
		/// </summary>
		/// <param name="Landed"></param>
		/// <param name="OnEnemy"></param>
		/// <param name="Observed"></param>
		private readonly record struct Application(BuffId[] Landed, bool OnEnemy, bool Observed);

		/// <summary>
		/// Returns the damage skill a skill-rotation scenario presses, or null
		/// when the class has none the harness can profile.
		/// </summary>
		/// <remarks>
		/// The class's own first damage skill by name, so the choice is the
		/// same on every run and every scale. It is pressed on its own shoot
		/// time rather than on its cooldown - the scenario is there to ask what
		/// a buff does to skill damage, and a rotation that spends most of its
		/// time on basic attacks would answer the question the other scenarios
		/// already answer.
		/// </remarks>
		/// <param name="character"></param>
		/// <param name="job"></param>
		/// <param name="skillLevel"></param>
		private static Skill DamageSkillOf(Character character, JobEntry job, int skillLevel)
		{
			var damage = JobCatalog.GetDamageSkills(job)
				.OrderBy(s => s.Data.ClassName, StringComparer.Ordinal)
				.FirstOrDefault();

			return damage == null ? null : SyntheticActors.GiveSkill(character, damage.Id, skillLevel);
		}

		/// <summary>
		/// Measures several buffs held at once, as one reading.
		/// </summary>
		/// <remarks>
		/// The control window holds none of them and the treatment window holds
		/// all of them, so the value that comes back is the whole stack's. Every
		/// buff keeps the magnitude its own row carries; nothing is scaled here,
		/// because the question is what the priced roster does when it is worn
		/// together.
		/// </remarks>
		/// <param name="subjects"></param>
		/// <param name="buffLevel"></param>
		/// <param name="characterLevel"></param>
		/// <param name="pool"></param>
		public static BuffValueResult MeasureStack(BuffSubject[] subjects, int buffLevel = BuffDials.ProbeBuffLevel,
			int characterLevel = BuffDials.ProbeLevel, ArenaPool pool = null)
		{
			if (subjects == null || subjects.Length == 0)
				throw new ArgumentException("A stack needs at least one buff.", nameof(subjects));

			return Measure(subjects[0], slotScale: 1f, buffLevel: buffLevel, characterLevel: characterLevel,
				pool: pool, alsoHeld: subjects.Skip(1).ToArray(), slotsOverride: subjects[0].WrittenMagnitudes);
		}

		/// <summary>
		/// Runs one half of a pair and returns what both sides of the fight
		/// landed.
		/// </summary>
		/// <param name="job"></param>
		/// <param name="subject"></param>
		/// <param name="buffLevel"></param>
		/// <param name="characterLevel"></param>
		/// <param name="slotScale"></param>
		/// <param name="applyBuff"></param>
		/// <param name="arena"></param>
		/// <param name="scenario"></param>
		/// <param name="alsoHeld"></param>
		/// <param name="slotsOverride"></param>
		private static WindowReading RunWindow(JobEntry job, BuffSubject subject, int buffLevel, int characterLevel, float slotScale,
			bool applyBuff, Map arena, BuffScenario scenario, BuffSubject[] alsoHeld = null,
			IReadOnlyDictionary<int, float> slotsOverride = null)
		{
			alsoHeld ??= [];
			var character = (Character)null;
			var allies = new List<Character>();
			var mobs = new List<Mob>();

			// The mobs fight back on the same clock the character swings on, so
			// a control window and its treatment window hold the same number of
			// exchanges every run.
			GameClock.Use(new VirtualClock());

			try
			{
				var stat = JobCatalog.GetPrimaryStat(job);

				var spread = BuffScenarios.Spread(scenario, stat, characterLevel);

				character = SyntheticActors.CreateCharacter(job.JobId, characterLevel, spread, arena: arena);
				ReferenceGear.Equip(character, job);

				var buffSkill = SyntheticActors.GiveSkill(character, subject.SkillId, buffLevel);
				var basicSkill = SyntheticActors.GiveSkill(character, BasicAttackOf(character, job), 1);
				var attackSkill = scenario.UseSkill ? DamageSkillOf(character, job, buffLevel) ?? basicSkill : basicSkill;

				// The rest of the party stands beside the caster and swings its
				// own basic attack. A party buff is worth what it adds to every
				// character holding it, so the offensive reading is the party's
				// total - that, and nothing else, is what makes Priest_Blessing
				// cost more than Swordman_GungHo for the same percentage.
				var allySkills = new List<Skill>();

				for (var i = 1; i < scenario.PartySize; ++i)
				{
					var ally = SyntheticActors.CreateCharacter(job.JobId, characterLevel, spread,
						new Position(i * 15f, 0, -15f), arena);

					ReferenceGear.Equip(ally, job);
					SyntheticActors.GiveSkill(ally, subject.SkillId, buffLevel);
					allySkills.Add(SyntheticActors.GiveSkill(ally, BasicAttackOf(ally, job), 1));

					allies.Add(ally);
				}

				var mobData = SfrDefenseProbe.FindHostileReferenceMob(characterLevel);

				// A ring of targets rather than one, so a buff that widens what
				// a swing reaches has something to reach. Positions are relative
				// to the arena center, matching CreateMob's contract - the
				// character sits at center, so this is melee range all round.
				//
				// Deliberately not hostile: nothing on the map acts except the
				// character. Monster AI is what made the buff-free noise floor
				// read a 30% defensive swing, because how many swings a mob gets
				// in over a window is decided by RNG the harness does not yet
				// seed on pool threads. Incoming damage is sampled instead.
				for (var i = 0; i < BuffDials.MobCount; ++i)
				{
					var angle = i * 2 * Math.PI / BuffDials.MobCount;
					var offset = new Position(
						(float)(Math.Cos(angle) * ScenarioMatrix.MeleeDistance), 0,
						(float)(Math.Sin(angle) * ScenarioMatrix.MeleeDistance));

					mobs.Add(SyntheticActors.CreateMob(mobData.Id, offset, arena));
				}

				SfrDefenseProbe.Fortify(character);
				SfrDefenseProbe.Refill(character);

				foreach (var m in mobs)
					SfrDefenseProbe.Fortify(m);

				foreach (var ally in allies)
				{
					SfrDefenseProbe.Fortify(ally);
					SfrDefenseProbe.Refill(ally);
				}

				// After both sides are fortified and geared, and before any buff
				// is applied, so a buff's own modifier composes on top of what
				// the scenario set rather than being overwritten by it.
				BuffScenarios.Load(scenario, character, mobs);

				foreach (var ally in allies)
					BuffScenarios.LoadCharacter(scenario, ally, mobs[0]);

				var focus = mobs[0];
				character.Direction = character.Position.GetDirection(focus.Position);

				var tick = TimeSpan.FromMilliseconds(TickMs);
				var clock = GameClock.Current;
				var landed = Array.Empty<BuffId>();

				var application = default(Application);

				var extraSkills = alsoHeld.Select(s => SyntheticActors.GiveSkill(character, s.SkillId, buffLevel)).ToArray();

				using (var scope = new BuffCaptionScope(buffSkill, subject, slotScale, slotsOverride))
				using (var recorder = new SfrPressRecorder(character, pinRolls: false, allies: allies.Cast<ICombatEntity>().ToArray()))
				{
					for (var elapsed = 0; elapsed < BuffDials.SettleMs; elapsed += TickMs)
					{
						SkillPressProbe.Step(clock, tick);
						arena.Update(tick);
						SfrDefenseProbe.Refill(character);
					}

					if (applyBuff)
					{
						application = Apply(subject, buffSkill, character, mobs, buffLevel);
						landed = application.Landed;

						// Only a press that really reaches the party puts its buff
						// on them. A self buff leaves the other three unbuffed,
						// which is exactly why it is worth a quarter of what the
						// same percentage is worth party-wide.
						if (subject.IsPartyWide)
						{
							foreach (var ally in allies)
								ApplyTo(subject, ally, character, landed, buffLevel);
						}

						// The rest of the stack goes up beside it, each through
						// its own press, so what is measured is the buffs a
						// character really holds at once.
						for (var i = 0; i < alsoHeld.Length; ++i)
							Apply(alsoHeld[i], extraSkills[i], character, mobs, buffLevel);
					}

					// What one of the monster's swings gets through for, run
					// through the whole pipeline with dodge, block and crit
					// live. Both halves of a pair sample from the same seed and
					// the same count, so the only thing that can move this is
					// the buff itself.
					var incoming = SampleIncoming(mobs, character);

					recorder.Clear();

					var sinceSwing = 0f;

					for (var elapsed = 0; elapsed < BuffDials.WindowMs; elapsed += TickMs)
					{
						// Held up for the whole window rather than pressed once.
						// Uptime is priced separately from what the buff is worth
						// while it is on, so a window outliving the duration
						// would fold a second term into the reading - a 600 s
						// window against a 300 s buff measures half the buff.
						if (applyBuff && landed.Length > 0 && !landed.Any(application.OnEnemy ? focus.IsBuffActive : character.IsBuffActive))
							Apply(subject, buffSkill, character, mobs, buffLevel);

						// Re-asserted every tick rather than set once, because a
						// stacking buff's handler decays it back off inside the
						// window.
						if (applyBuff && subject.Stacks > 0)
							HoldStacks(subject, application.OnEnemy ? mobs.Cast<ICombatEntity>() : [character], landed);

						// Re-read after every swing rather than once up front:
						// ShootTime divides by SklSpdRate, which is where an
						// attack-speed buff lands, so a fixed interval would
						// price one at nothing.
						var interval = Math.Max(TickMs, attackSkill.Properties.GetFloat(PropertyName.ShootTime));

						if (sinceSwing >= interval)
						{
							SfrDefenseProbe.Dispatch(attackSkill, character, focus);

							for (var i = 0; i < allies.Count; ++i)
								SfrDefenseProbe.Dispatch(allySkills[i], allies[i], focus);

							sinceSwing = 0f;
						}

						SkillPressProbe.Step(clock, tick);
						arena.Update(tick);

						// SP only. Topping HP up is what keeps the window from
						// ending in a corpse, and Fortify already did that.
						character.Properties.SetFloat(PropertyName.SP, character.Properties.GetFloat(PropertyName.MSP));

						foreach (var ally in allies)
						{
							ally.Properties.SetFloat(PropertyName.SP, ally.Properties.GetFloat(PropertyName.MSP));

							if (applyBuff && subject.IsPartyWide && landed.Length > 0 && !landed.Any(ally.IsBuffActive))
								ApplyTo(subject, ally, character, landed, buffLevel);
						}

						sinceSwing += TickMs;
					}

					return new WindowReading(landed, application.OnEnemy, application.Observed,
						recorder.TotalDamage(), incoming,
						mobs.Sum(recorder.HitsOn), BuffDials.IncomingSamples);
				}
			}
			finally
			{
				GameClock.Use(null);
				SyntheticActors.Cleanup(character, mobs.ToArray());

				foreach (var ally in allies)
					SyntheticActors.Cleanup(ally);
			}
		}

		/// <summary>
		/// Returns what one incoming swing gets through for, averaged over the
		/// attack types the ring swings.
		/// </summary>
		/// <remarks>
		/// One mob per attack type, each sampled for an equal share of
		/// BuffDials.IncomingSamples, so the total count is unchanged and every
		/// window still fixes it by construction. The mean is flat because the
		/// split is: what is being read is what the character takes from a
		/// fight that contains both kinds of swing, and nothing measures which
		/// kind it meets more often.
		///
		/// Without the magic half a buff that only raises MDEF mitigates
		/// nothing the probe can see, reads 1.000 at every scale and is held
		/// as unpriceable for a reason that belongs to the probe.
		/// </remarks>
		/// <param name="mobs"></param>
		/// <param name="character"></param>
		private static float SampleIncoming(List<Mob> mobs, Character character)
		{
			var attacks = BuffDials.IncomingAttacks;
			var share = BuffDials.IncomingSamples / attacks.Length;
			var total = 0f;

			for (var i = 0; i < attacks.Length; ++i)
			{
				var mob = mobs[i % mobs.Count];
				var sample = HitSampler.Sample(mob, character, new Skill(mob, attacks[i], 1), share);

				total += sample.EffectivePerCast;
			}

			return total / attacks.Length;
		}

		/// <summary>
		/// Holds a stacking buff's overbuff counter at the count the dial
		/// declares, on whichever side the buff landed.
		/// </summary>
		/// <remarks>
		/// Set rather than incremented, so the handler's own cap and decay both
		/// lose to it: what is being measured is what the buff is worth at that
		/// many stacks, and letting the count drift would fold the decay rate
		/// into a reading that is supposed to be magnitude alone.
		/// </remarks>
		/// <param name="subject"></param>
		/// <param name="holders"></param>
		/// <param name="landed"></param>
		private static void HoldStacks(BuffSubject subject, IEnumerable<ICombatEntity> holders, BuffId[] landed)
		{
			foreach (var holder in holders)
			{
				var changed = false;

				foreach (var buffId in landed)
				{
					if (!holder.TryGetBuff(buffId, out var buff) || buff.OverbuffCounter == subject.Stacks)
						continue;

					// Set again after activating, because a handler's own stack
					// cap runs on activation and would clamp the held count.
					buff.OverbuffCounter = subject.Stacks;
					buff.Activate(ActivationType.Overbuff);
					buff.OverbuffCounter = subject.Stacks;
					changed = true;
				}

				if (changed)
					holder.Properties.InvalidateAll();
			}
		}

		/// <summary>
		/// Puts the buffs the press produced onto a party member.
		/// </summary>
		/// <remarks>
		/// Applied rather than cast at: the harness has no way to make a press
		/// target an ally - Map.GetCharacters excludes a DummyConnection - so a
		/// party buff would otherwise measure as a self buff with extra
		/// bystanders. What the press decided is already known from the caster's
		/// own application, and this copies it.
		/// </remarks>
		/// <param name="subject"></param>
		/// <param name="ally"></param>
		/// <param name="caster"></param>
		/// <param name="landed"></param>
		/// <param name="buffLevel"></param>
		private static void ApplyTo(BuffSubject subject, Character ally, Character caster, BuffId[] landed, int buffLevel)
		{
			foreach (var buffId in landed.Where(b => !ally.IsBuffActive(b)))
				ally.StartBuff(buffId, buffLevel, caster.Properties.GetFloat(PropertyName.INT), TimeSpan.FromHours(1), caster, subject.SkillId);

			ally.Properties.InvalidateAll();
		}

		/// <summary>
		/// Puts the press's effect up, on whichever side the press itself puts
		/// it, and falls back to applying it to the caster when the press does
		/// nothing.
		/// </summary>
		/// <remarks>
		/// The press is preferred because it is authoritative: it decides the
		/// buff's level, its second argument, its duration and - the reason this
		/// returns a side at all - its target. A debuff put on the caster
		/// measures its own effect with the sign reversed, since cutting block
		/// helps whoever is hitting the holder. Peltasta_SwashBuckling read
		/// 0.929 defensive that way, when what it does is make an enemy easier
		/// to fight.
		///
		/// Read off the actors rather than resolved from the handler, so a
		/// skill that buffs the caster and debuffs the enemy in one press is
		/// classified by what it did.
		///
		/// The fallback exists for the IDynamicCasted holds the probe cannot
		/// drive - Priest_Blessing and Priest_Aspersion do all their work in a
		/// private continuation, so their Handle applies nothing. It assumes the
		/// caster's side and says so through Observed.
		/// </remarks>
		/// <param name="subject"></param>
		/// <param name="skill"></param>
		/// <param name="character"></param>
		/// <param name="mobs"></param>
		/// <param name="buffLevel"></param>
		private static Application Apply(BuffSubject subject, Skill skill, Character character, List<Mob> mobs, int buffLevel)
		{
			try
			{
				SfrDefenseProbe.Dispatch(skill, character, mobs[0]);
			}
			catch (Exception)
			{
				// A press that throws is the same case as one that applies
				// nothing, and the fallback below covers both.
			}

			var onEnemy = subject.Buffs.Where(b => mobs.Any(m => m.IsBuffActive(b))).ToArray();

			if (onEnemy.Length > 0)
			{
				// The press reaches whatever its own area reaches, and the
				// character attacks one of the ring. Spreading it keeps what is
				// measured the same whichever mob that is.
				foreach (var mob in mobs)
				{
					foreach (var buffId in onEnemy.Where(b => !mob.IsBuffActive(b)))
						mob.StartBuff(buffId, buffLevel, 0, TimeSpan.FromHours(1), character, subject.SkillId);

					mob.Properties.InvalidateAll();
				}

				return new Application(onEnemy, true, true);
			}

			var onCaster = subject.Buffs.Where(character.IsBuffActive).ToArray();

			if (onCaster.Length > 0)
				return new Application(onCaster, false, true);

			foreach (var buffId in subject.Buffs)
			{
				// NumArg2 carries the caster stat for the buffs that read one;
				// INT is the only one any converted handler uses so far.
				character.StartBuff(buffId, buffLevel, character.Properties.GetFloat(PropertyName.INT),
					TimeSpan.FromHours(1), character, subject.SkillId);
			}

			character.Properties.InvalidateAll();

			return new Application(subject.Buffs.Where(character.IsBuffActive).ToArray(), false, false);
		}

		/// <summary>
		/// Returns the basic attack the character's reference weapon fires.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="job"></param>
		private static SkillId BasicAttackOf(Character character, JobEntry job)
		{
			var weapon = character.Inventory.GetItem(EquipSlot.RightHand);

			return weapon == null ? BasicAttacks.Default(job) : BasicAttacks.For(job, weapon.Data.EquipType1);
		}
	}

	/// <summary>
	/// Holds a skill's caption ratios at a chosen multiple of what the data
	/// carries, for the length of one window.
	/// </summary>
	/// <remarks>
	/// SyntheticActors.GiveSkill hands every measured skill its own SkillData,
	/// which is what a utility press reads through skill.Properties. A buff
	/// handler reads neither: BuffHandler.GetCaptionRatio resolves off SkillDb
	/// and buff.NumArg1, because a handler holds a Buff and no Skill. So the
	/// shared row has to move as well, and that is process-wide state - hence
	/// the lock, which holds for the whole window rather than for the write.
	///
	/// The lock is per skill row, not one lock for the pass. What a window
	/// mutates is exactly one SkillData, and two buffs are two different rows,
	/// so the only real conflict is two windows on the same skill - which the
	/// pricer never asks for, since it solves one scale at a time per buff. A
	/// single process-wide lock protected the same invariant and also serialised
	/// the whole roster: every window is CPU under the virtual clock rather than
	/// the Thread.Sleep an SFR press mostly is, so holding one lock across it
	/// pinned the pass to one core.
	///
	/// Every declared slot is scaled by the same number. The pass never learns
	/// what a slot means - a flat value, a percentage and a per-point
	/// coefficient are all just magnitudes, and the caption text that declares
	/// the unit is untouched.
	/// </remarks>
	public sealed class BuffCaptionScope : IDisposable
	{
		private static readonly ConcurrentDictionary<SkillId, object> _rowLocks = new();

		private readonly object _rowLock;
		private readonly Skill _skill;
		private readonly SkillData _shared;
		private readonly (float Base, float ByLevel)[] _saved = new (float, float)[3];
		private readonly (float Base, float ByLevel)[] _savedShared = new (float, float)[3];

		/// <summary>
		/// Scales the skill's caption ratios until disposed.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="subject"></param>
		/// <param name="scale"></param>
		/// <param name="slotsOverride">
		/// Slot seeds to install instead of subject.Slots. Any of the three
		/// slots it omits is hard-zeroed rather than left at whatever the row
		/// already carries, so an isolated slot measures alone.
		/// </param>
		public BuffCaptionScope(Skill skill, BuffSubject subject, float scale, IReadOnlyDictionary<int, float> slotsOverride = null)
		{
			_skill = skill;
			_rowLock = _rowLocks.GetOrAdd(subject.SkillId, _ => new object());

			Monitor.Enter(_rowLock);

			try
			{
				var data = skill.Data;
				var slots = slotsOverride ?? subject.Slots;

				_saved[0] = (data.CaptionRatio1, data.CaptionRatio1ByLevel);
				_saved[1] = (data.CaptionRatio2, data.CaptionRatio2ByLevel);
				_saved[2] = (data.CaptionRatio3, data.CaptionRatio3ByLevel);

				Install(data, slots, scale, subject.PinnedMagnitudes);

				if (ZoneServer.Instance.Data.SkillDb.TryFind(subject.SkillId, out var shared) && !ReferenceEquals(shared, data))
				{
					_shared = shared;

					_savedShared[0] = (shared.CaptionRatio1, shared.CaptionRatio1ByLevel);
					_savedShared[1] = (shared.CaptionRatio2, shared.CaptionRatio2ByLevel);
					_savedShared[2] = (shared.CaptionRatio3, shared.CaptionRatio3ByLevel);

					Install(shared, slots, scale, subject.PinnedMagnitudes);
				}

				skill.Properties.InvalidateAll();
			}
			catch
			{
				Monitor.Exit(_rowLock);
				throw;
			}
		}

		/// <summary>
		/// Puts every slot at its seed magnitude times the given scale, and
		/// hard-zeroes whichever of the three it does not name, whatever shape
		/// the row it replaces was in.
		/// </summary>
		/// <remarks>
		/// Written flat, so the buff resolves to exactly that magnitude at every
		/// level and the reading does not depend on which buff level the window
		/// runs at.
		///
		/// A slot the map omits is zeroed rather than left at the row's own
		/// value, which is what makes isolating one axis of a multi-slot buff
		/// possible: leaving an unnamed slot alone would let its original
		/// magnitude leak into a reading that is supposed to be that slot alone.
		///
		/// Multiplying the saved row instead made the measurement a function of
		/// the row's shape as well as its size: a written row carries its whole
		/// magnitude in the per-level term, so re-reading it at a fixed buff
		/// level returned a fraction of what had been priced.
		/// </remarks>
		/// <param name="data"></param>
		/// <param name="slots"></param>
		/// <param name="scale"></param>
		/// <param name="pinned"></param>
		private static void Install(SkillData data, IReadOnlyDictionary<int, float> slots, float scale,
			IReadOnlyDictionary<int, float> pinned)
		{
			(data.CaptionRatio1, data.CaptionRatio1ByLevel) = (Magnitude(slots, pinned, 1, scale), 0f);
			(data.CaptionRatio2, data.CaptionRatio2ByLevel) = (Magnitude(slots, pinned, 2, scale), 0f);
			(data.CaptionRatio3, data.CaptionRatio3ByLevel) = (Magnitude(slots, pinned, 3, scale), 0f);
		}

		/// <summary>
		/// Returns the magnitude one slot is installed at.
		/// </summary>
		/// <remarks>
		/// A pinned slot takes its own magnitude whatever the scale and whatever
		/// the seed map says, including when the map omits it to isolate another
		/// slot: it is held at that number by design, so a window that measured
		/// it anywhere else would be measuring a buff the game never grants.
		/// </remarks>
		/// <param name="slots"></param>
		/// <param name="pinned"></param>
		/// <param name="slot"></param>
		/// <param name="scale"></param>
		private static float Magnitude(IReadOnlyDictionary<int, float> slots, IReadOnlyDictionary<int, float> pinned, int slot, float scale)
		{
			if (pinned != null && pinned.TryGetValue(slot, out var held))
				return held;

			return slots.TryGetValue(slot, out var seed) ? seed * scale : 0f;
		}

		/// <summary>
		/// Restores the skill's own caption ratios.
		/// </summary>
		public void Dispose()
		{
			var data = _skill.Data;

			(data.CaptionRatio1, data.CaptionRatio1ByLevel) = _saved[0];
			(data.CaptionRatio2, data.CaptionRatio2ByLevel) = _saved[1];
			(data.CaptionRatio3, data.CaptionRatio3ByLevel) = _saved[2];

			if (_shared != null)
			{
				(_shared.CaptionRatio1, _shared.CaptionRatio1ByLevel) = _savedShared[0];
				(_shared.CaptionRatio2, _shared.CaptionRatio2ByLevel) = _savedShared[1];
				(_shared.CaptionRatio3, _shared.CaptionRatio3ByLevel) = _savedShared[2];
			}

			_skill.Properties.InvalidateAll();

			Monitor.Exit(_rowLock);
		}
	}
}
