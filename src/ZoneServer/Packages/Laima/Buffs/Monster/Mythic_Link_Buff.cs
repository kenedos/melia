using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Buffs.Handlers.Monster;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Buffs.Handlers.Laima.Monster
{
	/// <summary>
	/// Handler for Mythic_Link_Buff.
	/// Spawns minions and creates damage-sharing links between the leader,
	/// its minions, and any same-class monsters found nearby.
	/// Works like Physical Link lv10.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Mythic_Link_Buff)]
	public class Mythic_Link_BuffOverride : BuffHandler
	{
		private const string LinkMembersVar = "Melia.Mythic.LinkMembers";
		private const string LinkIdVar = "Melia.Mythic.LinkId";
		private const string MinionHandlesVar = "Melia.Mythic.LinkMinions";
		private const string LinkTexture = "Linker3";
		private const int TargetMinionCount = 5;
		private const float ScanRange = 150f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is not Mob monster)
				return;

			MythicBuffHelper.ApplyMythicStats(monster);
			buff.SetUpdateTime(2000);

			var linkId = ZoneServer.Instance.World.CreateLinkHandle();
			buff.Vars.Set(LinkIdVar, linkId);
			buff.Vars.Set(LinkMembersVar, new List<int> { monster.Handle });
			buff.Vars.Set(MinionHandlesVar, new List<int>());
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target is not Mob monster || monster.Map == null || monster.IsDead)
				return;

			if (!buff.Vars.TryGet<List<int>>(LinkMembersVar, out var memberHandles))
				return;
			if (!buff.Vars.TryGet<List<int>>(MinionHandlesVar, out var minionHandles))
			{
				minionHandles = new List<int>();
				buff.Vars.Set(MinionHandlesVar, minionHandles);
			}

			var linkId = buff.Vars.GetInt(LinkIdVar);
			var changed = false;

			MythicBuffHelper.MaintainMinions(buff, monster, minionHandles, TargetMinionCount);

			// Remove dead/gone members from link
			for (var i = memberHandles.Count - 1; i >= 1; i--)
			{
				if (!monster.Map.TryGetCombatEntity(memberHandles[i], out var member) || member.IsDead)
				{
					memberHandles.RemoveAt(i);
					changed = true;
				}
			}

			// Add minions that aren't linked yet
			foreach (var handle in minionHandles)
			{
				if (!memberHandles.Contains(handle))
				{
					memberHandles.Add(handle);
					changed = true;
				}
			}

			// Scan for same-class monsters to link
			var scanRange = ScanRange;
			var nearbyMobs = monster.Map.GetActorsInRange<Mob>(monster.Position, scanRange,
				m => !m.IsDead
					&& m.Handle != monster.Handle
					&& m.Data?.ClassName == monster.Data?.ClassName
					&& !m.IsBuffActive(BuffId.Mythic_Link_mon_Buff));

			foreach (var mob in nearbyMobs)
			{
				if (!memberHandles.Contains(mob.Handle))
				{
					memberHandles.Add(mob.Handle);
					changed = true;
				}
			}

			if (changed)
			{
				this.ApplyLinkToMembers(monster, memberHandles, linkId);
				this.RefreshLinkVisuals(monster, memberHandles, linkId);
			}
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is not Mob monster)
				return;

			if (!buff.Vars.TryGet<List<int>>(LinkMembersVar, out var memberHandles))
				return;

			var linkId = buff.Vars.GetInt(LinkIdVar);

			foreach (var handle in memberHandles)
			{
				if (monster.Map != null && monster.Map.TryGetCombatEntity(handle, out var member))
					member.StopBuff(BuffId.Mythic_Link_mon_Buff);
			}

			for (var i = 1; i < memberHandles.Count + 5; i++)
				monster.RemoveEffect($"MythicLink_{linkId}_{i}");
		}

		private void ApplyLinkToMembers(Mob leader, List<int> memberHandles, int linkId)
		{
			foreach (var handle in memberHandles)
			{
				if (leader.Map == null || !leader.Map.TryGetCombatEntity(handle, out var member))
					continue;

				if (member.IsBuffActive(BuffId.Mythic_Link_mon_Buff))
					member.StopBuff(BuffId.Mythic_Link_mon_Buff);

				var linkBuff = member.StartBuff(BuffId.Mythic_Link_mon_Buff, 10, 0, TimeSpan.Zero, leader);
				if (linkBuff != null)
				{
					linkBuff.Vars.Set("Melia.Link.Members", memberHandles);
					linkBuff.Vars.Set("Melia.Link.Id", linkId);
					linkBuff.Vars.Set("Melia.Link.Leader", leader.Handle);
				}
			}
		}

		private void CreateLinkVisuals(Mob leader, List<int> memberHandles, int linkId)
		{
			for (var i = 1; i < memberHandles.Count; i++)
			{
				var pairHandles = new List<int> { leader.Handle, memberHandles[i] };
				var pairLinkId = ZoneServer.Instance.World.CreateLinkHandle();

				var effect = new LinkerVisualEffect(
					pairLinkId,
					LinkTexture,
					true,
					pairHandles,
					0.25f,
					"None",
					0.5f,
					"swd_blow_cloth2"
				);

				leader.AddEffect($"MythicLink_{linkId}_{i}", effect);
			}
		}

		private void RefreshLinkVisuals(Mob leader, List<int> memberHandles, int linkId)
		{
			for (var i = 1; i < memberHandles.Count + 10; i++)
				leader.RemoveEffect($"MythicLink_{linkId}_{i}");

			this.CreateLinkVisuals(leader, memberHandles, linkId);
		}
	}
}
