using System;
using System.Collections.Generic;
using System.Reflection;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using Melia.Shared.Data.Database;
using Melia.Zone.World.Items;
// using Melia.Zone.World.MiniGames; // Removed: MiniGames namespace deleted

namespace Melia.Zone.Scripting
{
	/// <summary>
	/// Manager for scriptable functions.
	/// </summary>
	/// <remarks>
	/// Primarily used for the calculation of properties and results
	/// related to combat and skill usage.
	/// </remarks>
	public static class ScriptableFunctions
	{
		// A list with all delegate collections for easy iteration
		// during setup and dedicated lists for easy access afterwards.
		// New collections can be added by creating a new delegate type
		// and adding a public collection for it below. The public
		// collections are added to the collections list automatically.

		private static readonly List<IDelegateCollection> Collections = new();

		public static readonly DelegateCollection<CharacterCalcFunc> Character = new();
		public static readonly DelegateCollection<MonsterCalcFunc> Monster = new();
		public static readonly DelegateCollection<CompanionCalcFunc> Companion = new();
		public static readonly DelegateCollection<MonsterCharacterCalcFunc> MonsterCharacter = new();
		public static readonly DelegateCollection<SkillCalcFunc> Skill = new();
		public static readonly DelegateCollection<CardScriptFunc> Card = new();
		public static readonly DelegateCollection<CombatCalcFunction> Combat = new();
		public static readonly DelegateCollection<StatusCalcFunction> Status = new();
		public static readonly DelegateCollection<SkillHitFunction> SkillHit = new();
		public static readonly DelegateCollection<ItemCalcFunc> ItemCalc = new();
		public static readonly DelegateCollection<ItemScriptFunc> Item = new();
		public static readonly DelegateCollection<ItemUseOnItemScriptFunc> ItemUseOnItem = new();
		public static readonly DelegateCollection<ItemEquipScriptFunc> Equip = new();
		public static readonly DelegateCollection<ItemUnequipScriptFunc> Unequip = new();
		public static readonly DelegateCollection<NormalTxScriptFunc> NormalTx = new();
		public static readonly DelegateCollection<NormalTxNumScriptFunc> NormalTxNum = new();
		public static readonly DelegateCollection<ItemTxNumScriptFunc> ItemTx = new();
		public static readonly DelegateCollection<DialogTxScriptFunc> DialogTx = new();
		public static readonly DelegateCollection<CustomCommandScriptFunc> CustomCommand = new();
		public static readonly DelegateCollection<AbilityUnlockFunc> AbilityUnlock = new();
		public static readonly DelegateCollection<AbilityPriceFunc> AbilityPrice = new();
		public static readonly DelegateCollection<MapScriptFunc> Map = new();
		public static readonly DelegateCollection<PadFunc> Pad = new();
		// Removed: MiniGameStageFunc references deleted Stage type from MiniGames
		// public static readonly DelegateCollection<MiniGameStageFunc> MiniGameStages = new();
		public static readonly DelegateCollection<CompanionPriceFunc> CompanionPrice = new();
		public static readonly DelegateCollection<ItemSetScriptFunc> ItemSet = new();
		public static readonly DelegateCollection<SkillBuffHookFunc> SkillBuffHook = new();

		/// <summary>
		/// Sets up delegate collections.
		/// </summary>
		static ScriptableFunctions()
		{
			foreach (var fieldInfo in typeof(ScriptableFunctions).GetFields(BindingFlags.Static | BindingFlags.Public))
			{
				if (fieldInfo.FieldType.GetInterface(nameof(IDelegateCollection)) != null)
					Collections.Add((IDelegateCollection)fieldInfo.GetValue(null));
			}
		}

		/// <summary>
		/// Loads and registers scriptable functions on the given object.
		/// </summary>
		/// <param name="obj"></param>
		public static void Load(object obj)
		{
			foreach (var method in obj.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
			{
				foreach (var attribute in method.GetCustomAttributes<ScriptableFunctionAttribute>(false))
				{
					var funcName = attribute.ScriptFuncName ?? method.Name;
					var registered = false;

					foreach (var col in Collections)
					{
						if (col.TryRegister(funcName, method))
						{
							registered = true;
							break;
						}
					}

					if (!registered)
						throw new Exception($"Unknown method signature for scriptable function '{method.Name}' on '{obj.GetType().Name}'.");
				}
			}
		}
	}

	/// <summary>
	/// Used to mark a method as a custom command script handler.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class ScriptableFunctionAttribute : Attribute
	{
		/// <summary>
		/// Returns the name of the script that is handled by the function.
		/// </summary>
		public string ScriptFuncName { get; }

		/// <summary>
		/// Creates new attribute that uses the name of the method it's
		/// on as the script function name.
		/// </summary>
		public ScriptableFunctionAttribute()
		{
			// Getting the method name actually happens in the scriptable
			// function loading code, see ScriptableFunctions.
		}

		/// <summary>
		/// Creates new attribute.
		/// </summary>
		/// <param name="scriptFuncName"></param>
		public ScriptableFunctionAttribute(string scriptFuncName)
		{
			this.ScriptFuncName = scriptFuncName;
		}
	}

	/// <summary>
	/// A function that calculates a value for a character.
	/// </summary>
	/// <param name="character"></param>
	/// <returns></returns>
	public delegate float CharacterCalcFunc(Character character);

	/// <summary>
	/// A function that calculates a value for a companion.
	/// </summary>
	/// <param name="companion"></param>
	/// <returns></returns>
	public delegate float CompanionCalcFunc(Companion companion);

	/// <summary>
	/// A function that calculates a value for a monster.
	/// </summary>
	/// <param name="monster"></param>
	/// <returns></returns>
	public delegate float MonsterCalcFunc(Mob monster);

	/// <summary>
	/// A function that calculates a value for a monster based off a character.
	/// </summary>
	/// <param name="monster"></param>
	/// <param name="character"></param>
	/// <returns></returns>
	public delegate float MonsterCharacterCalcFunc(Mob monster, Character character);

	/// <summary>
	/// A function that calculates a value for an item.
	/// </summary>
	/// <param name="item"></param>
	/// <returns></returns>
	public delegate float ItemCalcFunc(Item item);

	/// <summary>
	/// A function that handles pad behavior
	/// </summary>
	/// <param name="caster"></param>
	/// <param name="skill"></param>
	/// <param name="pad"></param>
	/// <returns></returns>
	public delegate void PadFunc(ICombatEntity caster, Skill skill, Pad pad);

	/// <summary>
	/// A function that calculates a value for a skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	public delegate float SkillCalcFunc(Skill skill);

	/// <summary>
	/// A function that calculates values related to skill usage and combat.
	/// </summary>
	/// <param name="attacker"></param>
	/// <param name="target"></param>
	/// <param name="skill"></param>
	/// <param name="skillHitResult"></param>
	/// <returns></returns>
	public delegate float CombatCalcFunction(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult);

	/// <summary>
	/// A function that calculates values related to buff/status application.
	/// </summary>
	/// <param name="caster">The entity applying the buff.</param>
	/// <param name="target">The entity receiving the buff.</param>
	/// <param name="skill">The skill causing the buff.</param>
	/// <param name="buffId">The ID of the buff being applied.</param>
	/// <param name="initialValue">The initial chance or duration.</param>
	/// <returns>The modified chance or duration.</returns>
	public delegate float StatusCalcFunction(ICombatEntity caster, ICombatEntity target, Skill skill, BuffId buffId, float initialValue);

	/// <summary>
	/// A function that determines the result of a skill hitting a target.
	/// </summary>
	/// <param name="attacker"></param>
	/// <param name="target"></param>
	/// <param name="skill"></param>
	/// <returns></returns>
	public delegate SkillHitResult SkillHitFunction(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier);

	/// <summary>
	/// A function that determines whether an ability can be learned.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="argStr"></param>
	/// <param name="argInt"></param>
	/// <param name="data"></param>
	/// <returns></returns>
	public delegate bool AbilityUnlockFunc(Character character, string argStr, int argInt, AbilityData data);

	/// <summary>
	/// A function that returns the price and time it takes to learn an
	/// ability.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="abilityData"></param>
	/// <param name="abilityLevel"></param>
	/// <param name="maxLevel"></param>
	/// <param name="price"></param>
	/// <param name="time"></param>
	/// <returns></returns>
	public delegate void AbilityPriceFunc(Character character, AbilityData abilityData, int abilityLevel, int maxLevel, out int price, out int time);


	/// <summary>
	/// A function that handles map specific behaviors.
	/// </summary>
	/// <param name="character"></param>
	public delegate void MapScriptFunc(Character character);

	// Removed: Stage type was in deleted MiniGames namespace
	// public delegate void MiniGameStageFunc(Character character, Stage stage);

	/// <summary>
	/// A function that calculates the price for a companion stat training.
	/// </summary>
	/// <param name="companion"></param>
	/// <param name="statName"></param>
	/// <returns></returns>
	public delegate int CompanionPriceFunc(Companion companion, string statName);

	/// <summary>
	/// A function that handles item set bonus changes.
	/// Called when the equipped piece count changes, allowing the script
	/// to handle all threshold logic internally.
	/// </summary>
	/// <param name="character">The character whose equipment changed.</param>
	/// <param name="setData">The item set data.</param>
	/// <param name="oldPieceCount">The previous number of equipped pieces.</param>
	/// <param name="newPieceCount">The new number of equipped pieces.</param>
	public delegate void ItemSetScriptFunc(Character character, ItemSetData setData, int oldPieceCount, int newPieceCount);

	/// <summary>
	/// A function that handles skill-based buff hooks.
	/// Called when a buff starts or ends on an entity that has the skill.
	/// </summary>
	/// <param name="skill">The skill associated with this handler.</param>
	/// <param name="target">The entity receiving the buff.</param>
	/// <param name="buff">The buff that started or ended.</param>
	public delegate void SkillBuffHookFunc(Skill skill, ICombatEntity target, Buff buff);
}
