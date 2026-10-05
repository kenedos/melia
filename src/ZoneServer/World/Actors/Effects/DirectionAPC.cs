using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Logging;
using Yggdrasil.Scheduling;

namespace Melia.Zone.World.Actors.Effects
{
	public class DirectionAPC : Effect, IUpdateable
	{
		private readonly static TimeSpan DefaultUpdateInterval = TimeSpan.FromSeconds(7);
		private DateTime _lastUpdate;
		public TimeSpan UpdateInterval { get; set; } = DefaultUpdateInterval;

		public string EffectString { get; }
		public int EffectId { get; }
		public int I1 { get; }
		public int I2 { get; }

		public DirectionAPC(string packetString, int i1, int i2)
		{
			if (!ZoneServer.Instance.Data.PacketStringDb.TryFind(packetString, out var data))
			{
				throw new ArgumentException($"Packet string '{packetString}' not found.");
			}

			this.EffectString = packetString;
			this.EffectId = data.Id;
			this.I1 = i1;
			this.I2 = i2;
		}

		public IActor Actor { get; private set; }
		public int TrackIndex { get; private set; }

		public override void ShowEffect(IZoneConnection conn, IActor actor)
		{
			this.Actor = actor;
			this.TrackIndex = this.I1 - 1;
			Send.ZC_DIRECTION_APC(conn, actor, this.EffectId, this.I1, this.I2, (float)ZoneServer.Instance.World.WorldTime.Elapsed.TotalSeconds);
		}

		public void Update(TimeSpan elapsed)
		{
			var now = DateTime.Now;
			var sinceLastUpdate = now - _lastUpdate;

			if (sinceLastUpdate >= this.UpdateInterval && this.Actor != null)
			{
				if (this.TrackIndex >= this.I2)
					this.TrackIndex = 1;
				else
					this.TrackIndex += 1;
				Send.ZC_NORMAL.NPC_PlayTrack(this.Actor, this.EffectString, this.I1, this.TrackIndex, (float)ZoneServer.Instance.World.WorldTime.Elapsed.TotalSeconds);
				_lastUpdate = now;
			}
		}
	}
}
