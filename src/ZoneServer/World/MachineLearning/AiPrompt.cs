using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Zone.Events;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.World.MachineLearning
{
	public class AiPrompt
	{
		public string prompt { get; private set; }

		public List<string> stopping_strings { get; private set; }

		public AiPrompt(string sourceName, string prompt)
		{
			this.prompt = prompt;
			this.stopping_strings = new List<string>
			{
				sourceName + ":",
				"<|endoftext|>",
				"\\end",
				"\n\n"
			};

		}
	}

	public static class AiPromptConverter
	{
		public static string ToMemoryText(Npc npc, Character player, string playerMessage, string npcMessage)
		{
			return player.Name + ":" + playerMessage + "\n" + npc.Name + ":" + npcMessage + "\n";
		}
	}

	public class AiPromptResult
	{
		public string text { get; set; }
	}

	public class AiPromptResponse
	{
		public List<AiPromptResult> results { get; set; }
	}

}
