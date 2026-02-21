using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodingSeb.ExpressionEvaluator;

namespace Melia.Shared.Versioning.MEnums
{
	/// <summary>
	/// Preprocessor for text-files inspired by C.
	/// </summary>
	/// <remarks>
	/// Supports basic versions of
	/// - #define
	/// - #undef
	/// - #if
	/// - #else
	/// - #elif
	/// - #endif
	/// - #include
	/// </remarks>
	public partial class Preprocessor
	{
		private readonly ExpressionEvaluator _evaluator = new ExpressionEvaluator();

		/// <summary>
		/// Defines variable.
		/// </summary>
		/// <param name="identifier"></param>
		/// <param name="value"></param>
		public void Define(string identifier, object value)
		{
			_evaluator.Variables[identifier] = value;
		}

		/// <summary>
		/// Undefines variable.
		/// </summary>
		/// <param name="identifier"></param>
		public void Undefine(string identifier)
		{
			_evaluator.Variables.Remove(identifier);
		}

		/// <summary>
		/// Returns variable by name via out if it exists. Returns whether
		/// the variable was found or not.
		/// </summary>
		/// <param name="identifier"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool TryGetDefined(string identifier, out object value)
		{
			return _evaluator.Variables.TryGetValue(identifier, out value);
		}

		/// <summary>
		/// Returns true if a variable with the given name was defined.
		/// </summary>
		/// <param name="identifier"></param>
		/// <returns></returns>
		public bool VariableExists(string identifier)
		{
			return _evaluator.Variables.ContainsKey(identifier);
		}

		/// <summary>
		/// Processes given file and returns the processed content.
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public string ProcessFile(string filePath)
		{
			var processedLines = new List<string>();
			this.ProcessFile(filePath, ref processedLines);

			return string.Join(Environment.NewLine, processedLines);
		}

		/// <summary>
		/// Processes the file and adds all processed lines to the given list.
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="linesOut"></param>
		private void ProcessFile(string filePath, ref List<string> linesOut)
		{
			var lines = File.ReadAllLines(filePath);
			this.ProcessLines(filePath, lines, ref linesOut);
		}

		/// <summary>
		/// Processes the file and adds all processed lines to the given list,
		/// using the file path as reference for relative includes.
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="lines"></param>
		/// <param name="linesOut"></param>
		private void ProcessLines(string filePath, string[] lines, ref List<string> linesOut)
		{
			var level = 0;
			var skipLevel = 0;
			var ifs = new Dictionary<int, bool>();

			for (var i = 0; i < lines.Length; ++i)
			{
				// Skip comment lines
				var line = lines[i].Trim();
				if (line.StartsWith("//"))
					continue;

				// Replace trailing comments
				line = TrailingCommentsRegex().Replace(line, "").Trim();

				// Handle #if, checking the expression and setting a flag
				// if the #if's body is supposed to get skipped.
				if (line.StartsWith("#if"))
				{
					// Increment the level we're on as we enter the #if,
					// to keep track of the level we're on for skipping
					// and ending the #if.
					level++;

					// Check that this #if doesn't follow an unclosed #if
					if (ifs.ContainsKey(level))
						throw new Exception($"A previous if hasn't been closed yet.");

					// Remember the existence of this if
					ifs[level] = false;

					// Ignore this #if if the processor is currently skipping
					if (skipLevel != 0)
						continue;

					// Evaluate the expression
					var expression = line[3..].Trim();
					var stepInside = this.EvaluateIfCondition(expression);

					// If the expression evaluated to true, mark this if as
					// handled and continue. Otherwise, mark it as not
					// handled and start skipping.
					if (stepInside)
					{
						ifs[level] = true;
					}
					else
					{
						skipLevel = level;
						ifs[level] = false;
					}

					continue;
				}
				// Handle #elif, checking the expression and start skipping
				// it didn't evaluate to true. Only applies if hasn't been
				// handled yet.
				else if (line.StartsWith("#elif"))
				{
					// Check that an #if preceded this #elif
					if (!ifs.TryGetValue(level, out var value))
						throw new Exception($"Unexpected 'elif' without matching 'if'.");

					// Ignore this #elif if another #if or #elif was already
					// executed
					if (value)
					{
						skipLevel = level;
						continue;
					}

					// Ignore this #elif if the processor is currently
					// skipping from a lower level
					if (skipLevel != 0 && level != skipLevel)
						continue;

					// Evaluate the expression
					var expression = line.Substring(5).Trim();
					var stepInside = this.EvaluateIfCondition(expression);

					// If the expression evaluated to true, stop skipping
					// and mark the if as handled. If not, start or continue
					// skipping and mark the if as not handled.
					if (stepInside)
					{
						skipLevel = 0;
						ifs[level] = true;
					}
					else
					{
						skipLevel = level;
						ifs[level] = false;
					}

					continue;
				}
				// Handle #else, which is entered if the #if hasn't been
				// handled yet..
				else if (line.StartsWith("#else"))
				{
					// Check that an #if preceded this #else
					if (!ifs.TryGetValue(level, out var value))
						throw new Exception($"Unexpected 'else' without matching 'if'.");

					// Ignore this #else if an #if or #elif was already
					// executed
					if (value)
					{
						skipLevel = level;
						continue;
					}

					// Ignore this #else if the processor is currently
					// skipping from a lower level
					if (skipLevel != 0 && level != skipLevel)
						continue;

					// Stop skipping since the #if wasn't handled yet and
					// mark the #if as handled.
					skipLevel = 0;
					ifs[level] = true;

					continue;
				}
				// Handle #endif, stopping the skipping enabled by a
				// previous #if or #elif
				else if (line.StartsWith("#endif"))
				{
					// Check that an #if preceded this #endif
					if (!ifs.ContainsKey(level))
						throw new Exception("Unexpected 'endif' without matching 'if'.");

					// Reset skipping if this #endif belongs to the #if that
					// initiated the skipping.
					if (level == skipLevel)
						skipLevel = 0;

					// Remove the if from tracking and lower the level as
					// we're leaving the #if.
					ifs.Remove(level);
					level--;

					continue;
				}

				// Skip line if an #if enabled skipping because its body
				// is supposed to be ignored
				if (skipLevel > 0)
					continue;

				// Handle #define, defining a variable
				if (line.StartsWith("#define"))
				{
					var match = Regex.Match(line, @"#define\s+(?<identifier>[a-z][a-z0-9_]*)\s+(?<value>[a-z0-9]+)", RegexOptions.IgnoreCase);
					if (!match.Success)
						throw new Exception("Invalid define.");

					var identifier = match.Groups["identifier"].Value;
					var value = match.Groups["value"].Value;

					if (value == "true")
						this.Define(identifier, true);
					else if (value == "false")
						this.Define(identifier, false);
					else if (Regex.IsMatch(value, @"^[-+]?[0-9]+$"))
						this.Define(identifier, Convert.ToInt32(value));
					else
						this.Define(identifier, value);

					continue;
				}
				// Handle #undef, undefining a variable
				else if (line.StartsWith("#undef"))
				{
					var match = Regex.Match(line, @"#undef\s+(?<identifier>[a-z][a-z0-9]*)", RegexOptions.IgnoreCase);
					if (!match.Success)
						throw new Exception("Invalid undef.");

					var identifier = match.Groups["identifier"].Value;
					this.Undefine(identifier);

					continue;
				}
				// Handle #include, which branches the processor to process
				// another file before going to the next line of current one
				else if (line.StartsWith("#include"))
				{
					var match = Regex.Match(line, @"#include\s+""(?<path>[^""]+)""", RegexOptions.IgnoreCase);
					if (!match.Success)
						throw new Exception("Invalid include.");

					var includedFilePath = match.Groups["path"].Value.Replace("\\", "/");
					var rootFolderPath = Path.GetDirectoryName(filePath);
					if (includedFilePath.StartsWith("/"))
					{
						includedFilePath = includedFilePath.Substring(1);
						rootFolderPath = Directory.GetCurrentDirectory();
					}

					var fullPath = Path.Combine(rootFolderPath, includedFilePath);
					if (File.Exists(fullPath))
						this.ProcessFile(fullPath, ref linesOut);

					continue;
				}

				linesOut.Add(line);
			}
		}

		/// <summary>
		/// Evaluates expression and returns whether it evalutes to true
		/// or false.
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		private bool EvaluateIfCondition(string expression)
		{
			var stepInside = false;

			if (Regex.IsMatch(expression, @"^[a-z][a-z0-9]*$", RegexOptions.IgnoreCase))
			{
				if (_evaluator.Variables.TryGetValue(expression, out var value) && value != null)
				{
					switch (value)
					{
						case bool boolValue: if (boolValue) stepInside = true; break;
						case int intValue: if (intValue != 0) stepInside = true; break;
						default: stepInside = true; break;
					}
				}
			}
			else
			{
				stepInside = _evaluator.Evaluate<bool>(expression);
			}

			return stepInside;
		}

		[GeneratedRegex("//.*")]
		private static partial Regex TrailingCommentsRegex();
	}
}
