﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Network;
using Yggdrasil.Scheduling;

namespace Melia.Zone.World
{
	/// <summary>
	/// Simulates a day/night cycle by tinting the players' game graphics.
	/// </summary>
	public class DayNightCycle : IUpdateable
	{
		private readonly static int TransitionFps = 2;

		public readonly static DaylightParameters DawnParameters = new(289, 238, 238);
		public readonly static DaylightParameters DayParameters = new(255, 255, 255);
		public readonly static DaylightParameters DuskParameters = new(306, 221, 221);
		public readonly static DaylightParameters NightParameters = new(127, 127, 180, 1, 1.2f);

		private readonly TimeSpan _transitionTime;
		private long _transitionId;
		private TimeOfDay _prevTimeOfDay;

		/// <summary>
		/// Returns true if the time of day is currently fixed.
		/// </summary>
		public bool IsFixed { get; private set; }

		/// <summary>
		/// Returns the current daylight parameters.
		/// </summary>
		public DaylightParameters CurrentParameters { get; private set; }

		/// <summary>
		/// Initializes day/night cycle.
		/// </summary>
		public DayNightCycle()
		{
			_transitionTime = GameTime.OneHour;
			_prevTimeOfDay = GameTime.Now.TimeOfDay;

			switch (_prevTimeOfDay)
			{
				case TimeOfDay.Dawn: this.CurrentParameters = DawnParameters; break;
				case TimeOfDay.Day: this.CurrentParameters = DayParameters; break;
				case TimeOfDay.Dusk: this.CurrentParameters = DuskParameters; break;
				case TimeOfDay.Night: this.CurrentParameters = NightParameters; break;
			}

			ZoneServer.Instance.ServerEvents.PlayerReady.Subscribe(this.OnPlayerReady);
		}

		/// <summary>
		/// Sends initial daylight parameters to players on login.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnPlayerReady(object sender, PlayerEventArgs e)
		{
			if (!ZoneServer.Instance.Conf.World.EnableDayNightCycle)
				return;

			Send.ZC_DAYLIGHT_FIXED(e.Character, true, this.CurrentParameters);
		}

		/// <summary>
		/// Updates the day/night cycle, potentially transitioning between
		/// the two.
		/// </summary>
		/// <param name="elapsed"></param>
		public void Update(TimeSpan elapsed)
		{
			if (!ZoneServer.Instance.Conf.World.EnableDayNightCycle)
				return;

			if (this.IsFixed)
				return;

			var now = GameTime.Now;
			var timeOfDay = now.TimeOfDay;

			if (timeOfDay != _prevTimeOfDay)
			{
				switch (timeOfDay)
				{
					case TimeOfDay.Dawn: this.TransitionToDawn(); break;
					case TimeOfDay.Day: this.TransitionToDay(); break;
					case TimeOfDay.Dusk: this.TransitionToDusk(); break;
					case TimeOfDay.Night: this.TransitionToNight(); break;
				}
			}

			_prevTimeOfDay = timeOfDay;
		}

		/// <summary>
		/// Returns the daylight parameters for the given time of day.
		/// </summary>
		/// <param name="timeOfDay"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		private DaylightParameters GetParameters(TimeOfDay timeOfDay)
		{
			switch (timeOfDay)
			{
				case TimeOfDay.Dawn: return DawnParameters;
				case TimeOfDay.Day: return DayParameters;
				case TimeOfDay.Dusk: return DuskParameters;
				case TimeOfDay.Night: return NightParameters;

				default:
					throw new ArgumentException($"Unknown time of day '{timeOfDay}'.");
			}
		}

		/// <summary>
		/// Fixes the time of day to the given value.
		/// </summary>
		/// <param name="timeOfDay"></param>
		public void FixTimeOfDay(TimeOfDay timeOfDay)
		{
			this.IsFixed = true;
			this.CurrentParameters = this.GetParameters(timeOfDay);

			Send.ZC_DAYLIGHT_FIXED(true, this.CurrentParameters);
		}

		/// <summary>
		/// Unfixes the time of day and updates the day night cycle.
		/// </summary>
		public void UnfixTimeOfDay()
		{
			this.IsFixed = false;
			this.CurrentParameters = this.GetParameters(GameTime.Now.TimeOfDay);

			Send.ZC_DAYLIGHT_FIXED(true, this.CurrentParameters);
		}

		/// <summary>
		/// Transitions to dawn.
		/// </summary>
		private async void TransitionToDawn()
		{
			await this.Transition(this.CurrentParameters, DawnParameters, _transitionTime);
		}
		/// <summary>
		/// Transitions to day.
		/// </summary>
		private async void TransitionToDay()
		{
			this.PlayBellSound();
			await this.Transition(this.CurrentParameters, DayParameters, _transitionTime);
		}

		/// <summary>
		/// Transitions to dawn.
		/// </summary>
		private async void TransitionToDusk()
		{
			await this.Transition(this.CurrentParameters, DuskParameters, _transitionTime);
		}

		/// <summary>
		/// Transitions to night.
		/// </summary>
		private async void TransitionToNight()
		{
			this.PlayBellSound();
			await this.Transition(this.CurrentParameters, NightParameters, _transitionTime);
		}

		/// <summary>
		/// Plays bell sound for all characters on all maps.
		/// </summary>
		private void PlayBellSound()
		{
			foreach (var character in ZoneServer.Instance.World.GetCharacters())
				Send.ZC_PLAY_SOUND(character, "chapel_bell_sound_01");
		}

		/// <summary>
		/// Transitions from one daylight color to another over the given
		/// time span.
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="transitionTime"></param>
		/// <returns></returns>
		private async Task Transition(DaylightParameters from, DaylightParameters to, TimeSpan transitionTime)
		{
			var transitionId = Interlocked.Increment(ref _transitionId);

			var timeMs = (int)transitionTime.TotalMilliseconds;
			var fps = TransitionFps;
			var steps = timeMs / (1000 / fps);
			var delayPerStep = timeMs / steps;

			var fixedOnStart = this.IsFixed;

			for (var i = 0; i <= steps; ++i)
			{
				if (transitionId != _transitionId || fixedOnStart != this.IsFixed)
					return;

				var progress = (float)i / steps;
				var stepColor = from.Lerp(to, progress);

				this.CurrentParameters = stepColor;

				Send.ZC_DAYLIGHT_FIXED(true, stepColor);

				await Task.Delay(delayPerStep);
			}
		}
	}

	/// <summary>
	/// Parameters for a fixed daylight change.
	/// </summary>
	public struct DaylightParameters
	{
		public int R;
		public int G;
		public int B;

		public float MapLightStrength;
		public float ModelLightStrength;

		public readonly float FR => this.R / 255f;
		public readonly float FG => this.G / 255f;
		public readonly float FB => this.B / 255f;

		/// <summary>
		/// Creates new parameters from the given values.
		/// </summary>
		/// <param name="r"></param>
		/// <param name="g"></param>
		/// <param name="b"></param>
		public DaylightParameters(int r, int g, int b)
			: this(r, g, b, 1, 1)
		{
		}

		/// <summary>
		/// Creates new parameters from the given values.
		/// </summary>
		/// <param name="r"></param>
		/// <param name="g"></param>
		/// <param name="b"></param>
		/// <param name="mapLightStrength"></param>
		/// <param name="modelLightStrength"></param>
		public DaylightParameters(int r, int g, int b, float mapLightStrength, float modelLightStrength)
		{
			this.R = r;
			this.G = g;
			this.B = b;

			this.MapLightStrength = mapLightStrength;
			this.ModelLightStrength = modelLightStrength;
		}

		/// <summary>
		/// Returns a color that is a linear interpolation between the two
		/// given colors.
		/// </summary>
		/// <param name="otherColor"></param>
		/// <param name="progress"></param>
		/// <returns></returns>
		public DaylightParameters Lerp(DaylightParameters otherColor, float progress)
		{
			var color = this;

			return new DaylightParameters
			{
				R = (int)(color.R + (otherColor.R - color.R) * progress),
				G = (int)(color.G + (otherColor.G - color.G) * progress),
				B = (int)(color.B + (otherColor.B - color.B) * progress),
				MapLightStrength = color.MapLightStrength + (otherColor.MapLightStrength - color.MapLightStrength) * progress,
				ModelLightStrength = color.ModelLightStrength + (otherColor.ModelLightStrength - color.ModelLightStrength) * progress,
			};
		}
	}
}
