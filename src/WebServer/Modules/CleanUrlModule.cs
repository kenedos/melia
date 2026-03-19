using System;
using System.IO;
using System.Threading.Tasks;
using EmbedIO;

namespace Melia.Web.Modules
{
	/// <summary>
	/// Module that handles clean URL routing for static pages.
	/// Maps URLs like /reset-password to their corresponding .html files.
	/// </summary>
	public class CleanUrlModule : WebModuleBase
	{
		private readonly string _webRoot;

		/// <summary>
		/// Creates a new CleanUrlModule.
		/// </summary>
		/// <param name="baseRoute">The base route (usually "/").</param>
		/// <param name="webRoot">The root directory for web files.</param>
		public CleanUrlModule(string baseRoute, string webRoot) : base(baseRoute)
		{
			_webRoot = webRoot;
		}

		public override bool IsFinalHandler => false;

		protected override async Task OnRequestAsync(IHttpContext context)
		{
			var path = context.RequestedPath;

			// Skip if the path already has an extension
			if (Path.HasExtension(path))
				return;

			// Skip API routes
			if (path.StartsWith("/api/", StringComparison.OrdinalIgnoreCase))
				return;

			// Skip other known routes
			if (path.StartsWith("/toslive/", StringComparison.OrdinalIgnoreCase))
				return;

			// Check if an .html file exists for this path
			var htmlPath = path.TrimStart('/') + ".html";
			var fullPath = Path.Combine(_webRoot, htmlPath);

			if (File.Exists(fullPath))
			{
				// Serve the HTML file
				context.Response.ContentType = "text/html; charset=utf-8";

				var content = await File.ReadAllTextAsync(fullPath);
				await context.SendStringAsync(content, "text/html", System.Text.Encoding.UTF8);
			}
			// If no matching HTML file, let other modules handle it
		}
	}
}
