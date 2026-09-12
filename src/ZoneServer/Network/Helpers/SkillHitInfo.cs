using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Network;
using Melia.Shared.Network.Helpers;
using Melia.Shared.Versioning;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Yggdrasil.Util;

namespace Melia.Zone.Network.Helpers
{
	public static class HitInfoHelpers
	{
		/// <summary>
		/// Adds skill hit info data to the packet.
		/// </summary>
		/// <param name="packet"></param>
		/// <param name="skillHitInfo"></param>
		public static void AddSkillHitInfo(this Packet packet, SkillHitInfo skillHitInfo)
		{
			packet.PutByte(0);
			packet.PutByte((byte)skillHitInfo.AttackType); // attack type?
			packet.PutByte(0);
			packet.PutByte(0);

			packet.PutInt(skillHitInfo.Target.Handle);
			packet.AddHitInfo(skillHitInfo.HitInfo);

			if (Versions.Protocol > 500)
			{
				packet.PutInt(skillHitInfo.IsKnockBack ? 1 : 0);
				packet.PutInt((int)skillHitInfo.AniTime.TotalMilliseconds);
				packet.PutShort((short)skillHitInfo.HitDelay.TotalMilliseconds);
				packet.PutByte((byte)skillHitInfo.HitEffect);
				packet.PutByte(0);
				packet.PutByte(skillHitInfo.TargetIndex);
				packet.PutByte(skillHitInfo.HitFrameIndex);
				packet.PutShort(0);
			}
			else
			{
				var cooldownTime = (int)skillHitInfo.Skill.Data.CooldownTime.TotalMilliseconds;
				var hitDelay = (short)skillHitInfo.Skill.Data.DefaultHitDelay.TotalMilliseconds;
				packet.PutInt(cooldownTime);
				packet.PutShort(hitDelay);
				packet.PutByte((byte)skillHitInfo.HitEffect);
				packet.PutInt(0);
				packet.PutByte(skillHitInfo.IsKnockBack);
			}
			packet.PutInt(skillHitInfo.ForceId); // This being set to anything causes a delay in the dagger damage animation
			packet.PutShort(0);
			packet.PutShort(0);

			var additionalPacket = skillHitInfo.AdditionalPacket;
			var additionalPacketSize = additionalPacket?.Length ?? 0;

			packet.PutShort((short)additionalPacketSize); // count1
			packet.PutByte(skillHitInfo.VarInfoCount); // count2
			packet.PutByte(0);

			if (skillHitInfo.IsKnockBack)
				packet.AddKnockbackInfo(skillHitInfo.KnockBackInfo);

			if (additionalPacketSize > 0)
				packet.PutBin(additionalPacket);

			// for count2
			{
				// Type 0 is for multi hits. The damage is divided by
				// the hit count and the displayed damage splits up
				// into multiple hits.
				if (skillHitInfo.VarInfoCount >= 1)
				{
					packet.PutByte(0);
					packet.PutInt(skillHitInfo.HitCount);
					if (Versions.Protocol > 500)
					{
						packet.PutByte(0);
						packet.PutInt(0);
					}
				}

				// The purpose of type 3 is unknown, usually comes with a
				// negative float.
				if (skillHitInfo.VarInfoCount >= 2)
				{
					packet.PutByte(3);
					packet.PutFloat(-1845);
				}
			}
		}

		/// <summary>
		/// Adds the body of a hit info packet to the packet.
		/// </summary>
		/// <param name="packet"></param>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="hitInfo"></param>
		public static void AddHitInfoPacket(this Packet packet, ICombatEntity attacker, ICombatEntity target, HitInfo hitInfo)
		{
			packet.PutInt(target.Handle);
			packet.PutInt(attacker.Handle);
			packet.PutInt((int)hitInfo.SkillId);

			packet.AddHitInfo(hitInfo);

			packet.PutByte(0);
			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutInt(hitInfo.ForceId);
			if (Versions.Client > KnownVersions.ClosedBeta1)
			{
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutFloat(hitInfo.UnkFloat1);
				packet.PutFloat(hitInfo.DamageRatio);
				packet.PutInt(hitInfo.HitCount);
				packet.PutByte(1);
				packet.PutInt(0);
				packet.PutInt((int)hitInfo.AniTime.TotalMilliseconds);
			}
			else
			{
				packet.PutByte(1);
				packet.PutInt((int)hitInfo.AniTime.TotalMilliseconds);
			}
		}

		/// <summary>
		/// Returns a framed hit info packet, for embedding in the additional
		/// packet of a skill hit.
		/// </summary>
		/// <remarks>
		/// An embedded packet carries its own header, with the id and checksum
		/// fields left at zero the way official's embedded packets do, and is
		/// padded out to the size its op declares.
		/// </remarks>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="hitInfo"></param>
		public static byte[] BuildHitInfoPacket(ICombatEntity attacker, ICombatEntity target, HitInfo hitInfo)
		{
			using var packet = Packet.Rent(Op.ZC_HIT_INFO);

			packet.AddHitInfoPacket(attacker, target, hitInfo);

			var op = OpTable.GetOp(Op.ZC_HIT_INFO);
			var headerSize = Versions.Client >= 174236 ? sizeof(short) + sizeof(int) + sizeof(int) : sizeof(short) + sizeof(int);
			var buffer = new byte[Math.Max(OpTable.GetSize(op), headerSize + packet.Length)];

			buffer[0] = (byte)(op & 0xFF);
			buffer[1] = (byte)((op >> 8) & 0xFF);
			packet.Build(ref buffer, headerSize);

			return buffer;
		}

		/// <summary>
		/// Adds hit info data to the packet.
		/// </summary>
		/// <param name="packet"></param>
		/// <param name="hitInfo"></param>
		public static void AddHitInfo(this Packet packet, HitInfo hitInfo)
		{
			packet.PutInt((int)hitInfo.Damage);
			packet.PutInt((int)hitInfo.Hp);
			packet.PutInt(hitInfo.HpPriority);
			packet.PutShort(hitInfo.KnockBackType != KnockBackType.None ? (short)hitInfo.KnockBackType : (short)hitInfo.Type);

			packet.PutByte(0);
			packet.PutByte(0);
			packet.PutByte(0);
			packet.PutByte((byte)hitInfo.AttackType);

			packet.PutShort((short)hitInfo.ResultType);

			packet.PutByte(hitInfo.IsHit);
			packet.PutByte((byte)Math2.Clamp(0, byte.MaxValue, (int)hitInfo.HitDelay.TotalMilliseconds));
			packet.PutByte(0);
			packet.PutByte(0);
		}
	}
}
