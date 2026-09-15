using System;
using System.Globalization;
using Melia.Shared.World;

namespace Melia.Zone.World.Quests
{
	/// <summary>
	/// Represents one of a quest's three phases, each of which has its own
	/// objective label, narration, map, NPC and minimap marker.
	/// </summary>
	public class QuestPhase
	{
		/// <summary>
		/// Returns the status the quest is in while this phase is active.
		/// </summary>
		public QuestStatus Status { get; }

		/// <summary>
		/// Gets or sets the short objective label shown in the tracker.
		/// </summary>
		public string ObjectiveLabel { get; set; }

		/// <summary>
		/// Gets or sets the longer narration line shown in the quest log.
		/// </summary>
		public string Story { get; set; }

		/// <summary>
		/// Gets or sets the class name of the map this phase takes place on.
		/// </summary>
		public string MapClassName { get; set; }

		/// <summary>
		/// Gets or sets the unique name of the NPC this phase revolves around.
		/// </summary>
		public string NpcUniqueName { get; set; }

		/// <summary>
		/// Gets or sets the name of the client anchor the marker sits on,
		/// if the marker is anchor-based rather than coordinate-based.
		/// </summary>
		public string AnchorName { get; set; }

		/// <summary>
		/// Gets or sets the position of the minimap marker.
		/// </summary>
		public Position Position { get; set; }

		/// <summary>
		/// Gets or sets the radius of the minimap marker.
		/// </summary>
		public float Radius { get; set; }

		/// <summary>
		/// Creates a new phase for the given status.
		/// </summary>
		/// <param name="status"></param>
		public QuestPhase(QuestStatus status)
		{
			this.Status = status;
		}

		/// <summary>
		/// Sets the phase's marker from a location string in the client's
		/// format, which is either "map x y z radius" or "map ANCHOR radius".
		/// </summary>
		/// <param name="location"></param>
		public void SetLocation(string location)
		{
			if (string.IsNullOrWhiteSpace(location))
				return;

			var parts = location.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length < 2)
				return;

			this.MapClassName = parts[0];

			if (parts.Length >= 5 && TryParse(parts[1], out var x) && TryParse(parts[2], out var y) && TryParse(parts[3], out var z))
			{
				this.Position = new Position(x, y, z);
				this.Radius = TryParse(parts[4], out var radius) ? radius : 0;
				return;
			}

			this.AnchorName = parts[1];
			this.Radius = parts.Length >= 3 && TryParse(parts[2], out var anchorRadius) ? anchorRadius : 0;
		}

		/// <summary>
		/// Parses a value from the client's location string, returns false
		/// if it wasn't a number.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		private static bool TryParse(string value, out float result)
			=> float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
	}
}
