using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Melia.Zone.World.Actors;
using Melia.Zone.Network;
using Newtonsoft.Json;
using System.IO;
using Yggdrasil.Logging;
using Melia.Shared.Data.Database;
using static System.Net.WebRequestMethods;
using Yggdrasil.Scripting;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Shared.World;
using System.Net.NetworkInformation;

namespace Melia.Zone.World.MachineLearning
{
	/// <summary>
	/// Represents the brain of a certain entity.
	/// </summary>
	public class AiEngine
	{
		private static readonly HttpClient _httpClient = new HttpClient();
		private static readonly string _apiAddress = "http://localhost:5000/api/v1/generate";

		/// <summary>
		/// The entity this AiEngine controls.
		/// </summary>
		private ICombatEntity _entity;

		/// <summary>
		/// Name of entity this AiEngine controls
		/// </summary>
		private string _entityName;

		/// <summary>
		/// This Entity's AI Persona.
		/// The persona is a textual description of this entity. Its likes, dislikes,
		/// behaviour, knowledge, physical appearance, etc.
		/// </summary>
		public string Persona { get; private set; }

		/// <summary>
		/// This Entity's AI chat memory. Contains dialogs between
		/// multiple players.
		/// chatting.
		/// </summary>
		public string DialogMemory { get; private set; }

		/// <summary>
		/// This Entity's AI opinions or thoughts towards a certain source.
		/// Key: Source
		/// Value: What they think about the source
		/// </summary>
		public Dictionary<string, string> Opinions { get; private set; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="persona"></param>
		public AiEngine(ICombatEntity entity, string persona)
		{
			_entity = entity;
			_entityName = entity.Name;
			this.Persona = this.LoadPersona(persona);
			this.DialogMemory = "";
		}

		private void MoveTo(Position pos)
		{
			var movement = this._entity.Components.Get<MovementComponent>().MoveTo(pos);
		}

		private async Task PerformReflection()
		{

		}



		/// <summary>
		/// Attempts to create an opinion for entity in towards the given
		/// source. Receives the dialog between this entity and the source
		/// to analyze what the entity thinks about the source.
		/// </summary>
		/// <param name="sourceName"></param>
		/// <param name="dialog"></param>
		/// <returns></returns>
		public async void CreateOpinion(string sourceName, string dialog)
		{
			var replacements = new Dictionary<string, string>();
			replacements.Add("ACTOR1", _entityName);
			replacements.Add("ACTOR2", sourceName);
			replacements.Add("DIALOG", dialog);

			var prompt = this.LoadPromptTemplate("Opinion", replacements);
			var reflection = await this.SendPrompt(_entityName, prompt);
			if (reflection != null)
			{
				if (this.Opinions.ContainsKey("sourceName"))
				{
					this.Opinions[sourceName] += reflection;
				}
				else
				{
					this.Opinions.Add(sourceName, reflection);
				}
			}
		}

		/// <summary>
		/// Receives a dialogue and responds to it
		/// </summary>
		/// <param name="sourceName">Name of source that originated this message</param>
		/// <param name="dialog">The message text itself</param>
		public async Task ReceiveDialog(string sourceName, string dialog)
		{
			// Creates dialog entity will say
			var replacements = new Dictionary<string, string>();
			replacements.Add("PERSONA", this.Persona);
			replacements.Add("MEMORY", this.DialogMemory);

			var prompt = this.LoadPromptTemplate("Dialog", replacements);
			prompt += sourceName + ":" + dialog + "\n" + _entityName + ":";

			var reply = await this.SendPrompt(sourceName, prompt);

			if (reply != null)
			{
				Send.ZC_CHAT(_entity, reply);
				this.DialogMemory += sourceName + ":" + dialog + "\n" + _entityName + ":" + reply + "\n";
				
				// What this entity thought of this interaction
				this.CreateOpinion(sourceName, dialog);
			}
		}

		/// <summary>
		/// Sends prompt to an AI API
		/// </summary>
		/// <param name="prompt"></param>
		/// <returns></returns>
		private async Task<string> SendPrompt(string sourceName, string text)
		{
			try
			{
				var prompt = new AiPrompt(sourceName, text);
				string body = JsonConvert.SerializeObject(prompt);

				var response = await _httpClient.PostAsync(_apiAddress,
					new StringContent(
						body,
						Encoding.UTF8,
						"application/json"));

				var content = await response.Content.ReadAsStringAsync();

				if (response.IsSuccessStatusCode)
				{
					var promptResponse = JsonConvert.DeserializeObject<AiPromptResponse>(content);

					if (promptResponse != null && promptResponse.results != null && promptResponse.results.Count > 0)
					{
						return promptResponse.results[0].text;
					}
				}

				return null;
			}
			catch (Exception ex)
			{
				Log.Error($"SendPrompt: Sending prompt '{text}' failed for address '{_apiAddress}'. Error code: {ex.Message}");
				return null;
			}
		}

		/// <summary>
		/// Loads a template file and replace certain keys under tags
		/// with given values of dictionary.
		/// </summary>
		/// <param name="templateName"></param>
		/// <param name="replacements"></param>
		/// <returns></returns>
		/// <exception cref="FileNotFoundException"></exception>
		private string LoadPromptTemplate(string templateName, Dictionary<string, string> replacements)
		{
			var filePath = "system\\scripts\\ml\\templates\\" + templateName + ".txt";
			if (!System.IO.File.Exists(filePath))
			{
				throw new FileNotFoundException($"Template file not found at '{templateName}'");
			}

			string templateContent = System.IO.File.ReadAllText(filePath).Replace("\r", "");
			string[] lines = templateContent.Split('\n');
			StringBuilder ret = new StringBuilder();

			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i].Trim();
				if (!line.StartsWith("#") && !line.StartsWith("//"))
				{
					foreach (var replacement in replacements)
					{
						line = line.Replace($"<{replacement.Key}>", replacement.Value);
					}
					ret.AppendLine(line);
				}
			}

			return ret.ToString();
		}

		/// <summary>
		/// Loads a persona
		/// </summary>
		/// <param name="personaName"></param>
		private string LoadPersona(string personaName)
		{
			string currentDirectory = Environment.CurrentDirectory;
			Console.WriteLine("Current Directory: " + currentDirectory);

			var filePath = "system\\scripts\\ml\\personas\\" + personaName + ".txt";
			if (!System.IO.File.Exists(filePath))
			{
				throw new FileNotFoundException($"Template file not found at '{filePath}'");
			}

			return System.IO.File.ReadAllText(filePath).Replace("\r", "");
		}
	}
}
