using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Util;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Scripting.Shortcuts;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the confidence a character gains from defeating a Pawndel or
	/// a Pawnd, which is what lets the Demon Transform Scroll be used.
	/// </summary>
	/// <remarks>
	/// The confidence only holds while the character is on the church's first
	/// floor with the quest running, so it gives out as soon as either stops
	/// being true.
	/// </remarks>
	[BuffHandler(BuffId.CHAPLE576_MQ_06)]
	public class Chaple576Mq06Confidence : BuffHandler
	{
		/// <summary>
		/// The quest the confidence belongs to.
		/// </summary>
		public const int LegendaryTrickQuestId = 8515;

		/// <summary>
		/// The map the confidence is earned and spent on.
		/// </summary>
		public const string ChurchFirstFloor = "d_chapel_57_6";

		public override void WhileActive(Buff buff)
		{
			if (buff.Target is not Character character)
				return;

			if (character.Map == null || character.Map.ClassName != ChurchFirstFloor || character.Layer != 0)
			{
				character.StopBuff(BuffId.CHAPLE576_MQ_06);
				return;
			}

			if (!character.Quests.IsActive(LegendaryTrickQuestId) || character.Quests.IsCompletable(LegendaryTrickQuestId))
				character.StopBuff(BuffId.CHAPLE576_MQ_06);
		}
	}

	/// <summary>
	/// Handle for the demon skin the Demon Transform Scroll puts on the
	/// character, which is what lets them talk to the church's demons and
	/// lead them to the altar.
	/// </summary>
	[BuffHandler(BuffId.CHAPLE576_MQ_06_1)]
	public class Chaple576Mq06Disguise : BuffHandler, ITransformationBuff
	{
		/// <summary>
		/// The quest the disguise belongs to.
		/// </summary>
		public const int LegendaryTrickQuestId = 8515;

		/// <summary>
		/// The map the disguise is meant for.
		/// </summary>
		public const string ChurchFirstFloor = "d_chapel_57_6";

		/// <summary>
		/// The two demons the scroll lets the character pass for.
		/// </summary>
		public static readonly string[] DemonClassNames = { "Pawndel", "pawnd" };

		/// <summary>
		/// The dialog the demons answer to while a character is disguised.
		/// </summary>
		public const string DemonDialogName = "CHAPLE576_MQ_06_MON";

		/// <summary>
		/// The name the disguise is stored under, so it is replayed to anyone
		/// who comes into view of the character while it lasts.
		/// </summary>
		private const string DisguiseEffectName = "Chaple576Mq06Disguise";

		/// <summary>
		/// Holds the faction the character had before the disguise, so it can
		/// be handed back when the disguise ends.
		/// </summary>
		private const string FactionVar = "Melia.Chaple576Mq06.Faction";

		/// <summary>
		/// Holds the class id of the demon the character was turned into.
		/// </summary>
		private const string DemonIdVar = "Melia.Chaple576Mq06.DemonId";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is not Character character)
				return;

			var demonId = PickDemonId();
			if (demonId == 0)
				return;

			buff.Vars.SetInt(FactionVar, (int)character.Faction);
			buff.Vars.SetInt(DemonIdVar, demonId);

			// The disguise is what stops the demons from taking the character
			// for prey, so the character's own side has to be put aside while
			// it lasts.
			character.Faction = FactionType.Peaceful;

			// The effect is what the appearance is carried on, so the client is
			// told now and again to everyone who comes into view later.
			character.AddEffect(DisguiseEffectName, new TransmuteEffect(demonId));

			SetDemonsTalkable(character);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target is not Character character)
				return;

			// Leading demons across the church is a long walk, and the game
			// does not let it run the character out of stamina.
			character.Properties.Stamina = character.MaxStamina;

			if (character.Map == null || character.Map.ClassName != ChurchFirstFloor || character.Layer != 0)
			{
				character.StopBuff(BuffId.CHAPLE576_MQ_06_1);
				return;
			}

			if (!character.Quests.IsActive(LegendaryTrickQuestId) || character.Quests.IsCompletable(LegendaryTrickQuestId))
			{
				character.StopBuff(BuffId.CHAPLE576_MQ_06_1);
				return;
			}

			// Covers demons that wandered in or respawned since the last tick,
			// which the client only offers as conversation targets once they
			// are told they have a dialog.
			SetDemonsTalkable(character);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is not Character character)
				return;

			character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("The transformation wore off.{nl}Defeat the Pawndels and Pawnds and use the Transformation Scroll!"), 10);
			character.PlayAnimation("ASTD");
			Send.ZC_NORMAL.StopAnimation(character);

			if (buff.Vars.TryGetInt(FactionVar, out var faction))
				character.Faction = (FactionType)faction;
			else
				character.Faction = FactionType.Law;

			// Dropping the effect is what puts the character's own face back.
			character.RemoveEffect(DisguiseEffectName);
			Send.ZC_NORMAL.Transmutation(character, 0);

			SetDemonsTalkable(character);

			// Any demon still following the character goes back to being a
			// demon, since the disguise that softened it up is gone.
			QuestAlly.ReleaseAllies(character);
		}

		/// <summary>
		/// Picks which of the church's demons the character is turned into.
		/// </summary>
		/// <returns>The monster's class id, or zero if none could be found.</returns>
		private static int PickDemonId()
		{
			var className = DemonClassNames[GameRandom.Get().Next(DemonClassNames.Length)];

			if (!ZoneServer.Instance.Data.MonsterDb.TryFind(className, out var data))
				return 0;

			return data.Id;
		}

		/// <summary>
		/// Marks the church's demons as something the character can talk to
		/// while disguised, and unmarks them once the disguise is gone.
		/// </summary>
		/// <remarks>
		/// A monster is normally something to fight rather than speak to, and
		/// the client decides which it is from the dialog name it was sent
		/// with. Only a disguised character may talk to these demons, so the
		/// name is put on and taken off along with the disguise.
		/// </remarks>
		/// <param name="character"></param>
		private static void SetDemonsTalkable(Character character)
		{
			if (character.Map == null)
				return;

			var disguised = character.IsBuffActive(BuffId.CHAPLE576_MQ_06_1);

			foreach (var demon in character.Map.GetMonsters(IsDemon).OfType<Mob>())
			{
				var wanted = disguised ? DemonDialogName : "";

				if (demon.DialogName == wanted)
					continue;

				demon.DialogName = wanted;

				// The client already knows this demon by the other name, so it
				// has to be told about the change.
				if (demon.Map != null)
					Send.ZC_UPDATED_MONSTERAPPEARANCE(demon);
			}
		}

		/// <summary>
		/// Returns whether the given monster is one of the church's demons.
		/// </summary>
		/// <param name="monster"></param>
		/// <returns></returns>
		private static bool IsDemon(IMonster monster)
		{
			if (!ZoneServer.Instance.Data.MonsterDb.TryFind(monster.Id, out var data))
				return false;

			return Array.IndexOf(DemonClassNames, data.ClassName) != -1;
		}
	}
}
