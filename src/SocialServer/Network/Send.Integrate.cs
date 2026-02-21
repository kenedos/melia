using Melia.Shared.Network;
using Melia.Social.World;

namespace Melia.Social.Network
{
	public static partial class Send
	{
		public static class SC_FROM_INTEGRATE
		{
			/// <summary>
			/// Purpose currently unknown.
			/// </summary>
			/// <param name="user"></param>
			public static void Unknown_01(SocialUser user)
			{
				var packet = new Packet(Op.SC_FROM_INTEGRATE);
				packet.PutInt(NormalOp.Integrate.Unknown_01);

				packet.PutLong(user.Id);
				packet.PutInt(0);

				user.Connection?.Send(packet);
			}

			/// <summary>
			/// Purpose currently unknown.
			/// </summary>
			/// <param name="user"></param>
			public static void AutoMatchComplete(SocialUser user)
			{
				var packet = new Packet(Op.SC_FROM_INTEGRATE);

				packet.PutInt(NormalOp.Integrate.AutoMatchComplete);
				packet.PutLong(user.Id);

				user.Connection?.Send(packet);
			}

			/// <summary>
			/// Purpose currently unknown.
			/// </summary>
			/// <param name="user"></param>
			public static void Unknown_14(SocialUser user)
			{
				var packet = new Packet(Op.SC_FROM_INTEGRATE);
				packet.PutInt(NormalOp.Integrate.Unknown_14);

				packet.PutLong(user.Id);
				packet.PutInt(0); // 728

				user.Connection?.Send(packet);
			}

			/// <summary>
			/// Shows the player the current auto match queue with the player and players ready.
			/// </summary>
			/// <param name="user"></param>
			/// <param name="usersInQueue"></param>
			/// <param name="usersReady"></param>
			public static void AutoMatchQueueUpdate(SocialUser user, int usersInQueue, int usersReady = 0)
			{
				var packet = new Packet(Op.SC_FROM_INTEGRATE);
				packet.PutInt(NormalOp.Integrate.AutoMatchQueueUpdate);

				packet.PutLong(user.Id);
				packet.PutInt(usersInQueue);
				packet.PutInt(usersReady);

				user.Connection?.Send(packet);
			}

			/// <summary>
			/// Shows the player a total number of users in the auto match queue.
			/// </summary>
			/// <param name="user"></param>
			/// <param name="queueTotalUserCount"></param>
			public static void AutoMatchQueueTotalUsers(SocialUser user, int queueTotalUserCount)
			{
				var packet = new Packet(Op.SC_FROM_INTEGRATE);
				packet.PutInt(NormalOp.Integrate.AutoMatchQueueTotal);

				packet.PutLong(user.Id);
				packet.PutInt(queueTotalUserCount);

				user.Connection?.Send(packet);
			}

			/// <summary>
			/// Purpose currently unknown.
			/// </summary>
			/// <param name="user"></param>
			public static void Unknown_19(SocialUser user)
			{
				var packet = new Packet(Op.SC_FROM_INTEGRATE);
				packet.PutInt(NormalOp.Integrate.Unknown_19);

				packet.PutLong(user.Id);
				packet.PutLong(user.Id);
				packet.PutEmptyBin(16);
				packet.PutLpString("WEEK");
				packet.PutLong(1);

				user.Connection?.Send(packet);
			}
		}
	}
}
