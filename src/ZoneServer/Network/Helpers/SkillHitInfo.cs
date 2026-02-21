using System;
using Melia.Shared.Network;
using Melia.Shared.Network.Helpers;
using Melia.Shared.Versioning;
using Melia.Zone.Skills.Combat;

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
				packet.PutShort((short)skillHitInfo.DamageDelay.TotalMilliseconds);
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutShort((short)skillHitInfo.SkillHitDelay.TotalMilliseconds); // Skill Hit Delay? Adds pause in attack animation?

				packet.PutByte((byte)skillHitInfo.HitEffect);
				packet.PutByte(1); // Was 1, but after melia update, was set to 0.

			}
			else
			{
				var cooldownTime = (int)skillHitInfo.Skill.Data.CooldownTime.TotalMilliseconds;
				var hitDelay = (short)skillHitInfo.Skill.Data.DefaultHitDelay.TotalMilliseconds;
				packet.PutInt(cooldownTime);
				packet.PutShort(hitDelay);

				packet.PutByte((byte)skillHitInfo.HitEffect);
				packet.PutInt(0);
				packet.PutByte(skillHitInfo.IsKnockBack); // Was 1, but after melia update, was set to 0.
			}
			if (Versions.Protocol > 500)
				packet.PutInt(0);
			packet.PutInt(skillHitInfo.ForceId); // This being set to anything causes a delay in the dagger damage animation
			packet.PutShort(0);
			packet.PutShort(0);

			packet.PutShort(0); // count1
			packet.PutByte(skillHitInfo.VarInfoCount); // count2
			packet.PutByte(0);

			if (skillHitInfo.IsKnockBack)
				packet.AddKnockbackInfo(skillHitInfo.KnockBackInfo);

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
		/// Adds hit info data to the packet.
		/// </summary>
		/// <param name="packet"></param>
		/// <param name="hitInfo"></param>
		public static void AddHitInfo(this Packet packet, HitInfo hitInfo)
		{
			packet.PutInt((int)hitInfo.Damage);
			packet.PutInt((int)hitInfo.Hp);
			packet.PutInt(hitInfo.HpPriority);
			packet.PutShort((short)hitInfo.Type);

			packet.PutByte(0);
			packet.PutByte(0);
			packet.PutByte(0);
			packet.PutByte((byte)hitInfo.AttackType);

			packet.PutShort((short)hitInfo.ResultType);

			packet.PutByte(0);
			packet.PutByte(0);
			packet.PutByte(0);
			packet.PutByte(0);
		}
	}
}
