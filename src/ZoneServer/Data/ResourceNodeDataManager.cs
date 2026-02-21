using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Melia.Zone.Data.Spawning;
using Newtonsoft.Json;
using Yggdrasil.Logging;

namespace Melia.Zone.Data
{
	/// <summary>
	/// Loads and manages definitions for different types of resource nodes.
	/// Provides access to node data based on type name.
	/// </summary>
	public static class ResourceNodeDataManager
	{
		private static readonly string DefinitionsFilePath = Path.Combine("system", "db", "procedural", "resource_nodes.json");

		// Dictionary to store loaded definitions, keyed by NodeTypeName (case-insensitive)
		private static Dictionary<string, ResourceNodeDefinition> _nodeDefinitions = new(StringComparer.OrdinalIgnoreCase);

		/// <summary>
		/// Loads the resource node definitions from the JSON file.
		/// Should be called once during server startup.
		/// </summary>
		/// <returns>True if loading was successful, false otherwise.</returns>
		public static bool LoadNodeDefinitions()
		{
			Log.Info("Loading resource node definitions...");
			if (!File.Exists(DefinitionsFilePath))
			{
				Log.Error($"Resource node definitions file not found at: {Path.GetFullPath(DefinitionsFilePath)}");
				_nodeDefinitions = new Dictionary<string, ResourceNodeDefinition>(StringComparer.OrdinalIgnoreCase); // Ensure dictionary exists even if empty
				return false;
			}

			try
			{
				var jsonContent = File.ReadAllText(DefinitionsFilePath);
				var definitionsList = JsonConvert.DeserializeObject<List<ResourceNodeDefinition>>(jsonContent);

				if (definitionsList == null)
				{
					Log.Error($"Failed to deserialize node definitions from {DefinitionsFilePath}. Result was null.");
					_nodeDefinitions = new Dictionary<string, ResourceNodeDefinition>(StringComparer.OrdinalIgnoreCase);
					return false;
				}

				// Populate the dictionary, ensuring type names are valid keys
				_nodeDefinitions = definitionsList
									.Where(def => !string.IsNullOrWhiteSpace(def.NodeTypeName))
									.ToDictionary(def => def.NodeTypeName, def => def, StringComparer.OrdinalIgnoreCase);

				Log.Info($"Loaded {_nodeDefinitions.Count} resource node definitions.");
				return true;
			}
			catch (JsonException jsonEx)
			{
				Log.Error($"JSON Error loading node definitions from {DefinitionsFilePath}: {jsonEx.Message}");
				Log.Error(jsonEx);
				_nodeDefinitions = new Dictionary<string, ResourceNodeDefinition>(StringComparer.OrdinalIgnoreCase); // Clear potentially bad data
				return false;
			}
			catch (Exception ex)
			{
				Log.Error($"Error loading node definitions from {DefinitionsFilePath}");
				Log.Error(ex);
				_nodeDefinitions = new Dictionary<string, ResourceNodeDefinition>(StringComparer.OrdinalIgnoreCase); // Clear potentially bad data
				return false;
			}
		}

		/// <summary>
		/// Gets the resource node definition data for a specific node type name.
		/// </summary>
		/// <param name="nodeTypeName">The case-insensitive type name (e.g., "Sturdy Sapling").</param>
		/// <param name="nodeDefinition">The definition data if found, otherwise null.</param>
		/// <returns>True if a definition was found for the given type name, false otherwise.</returns>
		public static bool TryGetData(string nodeTypeName, out ResourceNodeDefinition nodeDefinition)
		{
			nodeDefinition = null;
			if (string.IsNullOrEmpty(nodeTypeName))
			{
				return false;
			}
			// Dictionary lookup is efficient and handles case-insensitivity based on its comparer
			return _nodeDefinitions.TryGetValue(nodeTypeName, out nodeDefinition);
		}

		/// <summary>
		/// Gets the resource node definition data for a specific node type name.
		/// Returns null if the type name is not found.
		/// </summary>
		/// <param name="nodeTypeName">The case-insensitive type name.</param>
		/// <returns>The ResourceNodeDefinition or null.</returns>
		public static ResourceNodeDefinition GetData(string nodeTypeName)
		{
			TryGetData(nodeTypeName, out var definition);
			return definition; // Returns null if TryGetData fails
		}

		/// <summary>
		/// Gets a list of all known node type names.
		/// </summary>
		/// <returns>A list of node type name strings.</returns>
		public static List<string> GetAllNodeTypeNames()
		{
			// Return keys from the dictionary
			return _nodeDefinitions.Keys.ToList();
		}

		/// <summary>
		/// Gets all loaded resource node definitions.
		/// </summary>
		/// <returns>An enumerable collection of all definitions.</returns>
		public static IEnumerable<ResourceNodeDefinition> GetAllDefinitions()
		{
			return _nodeDefinitions.Values;
		}

	}
}
