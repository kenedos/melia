using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Effects;
using Yggdrasil.Util;

namespace Melia.Zone.Buffs.Handlers.Scouts.Thaumaturge
{
	[Package("laima")]
	[BuffHandler(BuffId.Transmute_Debuff)]
	public class Transmute_DebuffOverride : BuffHandler
	{
		private static readonly string[] TransmuteForms = new[]
		{
			"Onion",
			"Hanaming",
			"Chupacabra_Blue",
			"Popolion_Blue",
			"Bokchoy",
			"Jukopus",
			"bushspider",
			"jellyfish",
			"puragi",
			"honey_bee",
			"zigri",
			"Fisherman",
			"Ridimed",
			"Sakmoli",
			"thornball",
			"truffle",
			"Beetle",
		};

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			var rng = RandomProvider.Get();
			var className = TransmuteForms[rng.Next(TransmuteForms.Length)];

			if (!ZoneServer.Instance.Data.MonsterDb.TryFind(className, out var monster))
				return;

			target.AddEffect("Transmute", new TransmuteEffect(monster.Id));
			target.StartBuff(BuffId.Common_Silence, buff.Duration);
			Send.ZC_NORMAL.Transmutation(target, monster.Id);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			target.RemoveBuff(BuffId.Common_Silence);
			target.RemoveEffect("Transmute");
			Send.ZC_NORMAL.Transmutation(target, 0);
		}
	}
}
