using System.Threading;

namespace Melia.Shared.L10N
{
	/// <summary>
	/// Provides quick access to a global localizer, loaded with the
	/// server's default language. Supports per-async-context overrides
	/// for per-player localization.
	/// </summary>
	public static class Localization
	{
		private static Localizer _defaultLocalizer = new Localizer();
		private static readonly AsyncLocal<Localizer> _contextLocalizer = new();

		/// <summary>
		/// Returns the active localizer, preferring the per-context
		/// override if set, otherwise the global default.
		/// </summary>
		private static Localizer ActiveLocalizer
			=> _contextLocalizer.Value ?? _defaultLocalizer;

		/// <summary>
		/// Sets the localizer that is to be used to translate strings.
		/// </summary>
		/// <param name="localizer"></param>
		internal static void SetLocalizer(Localizer localizer)
			=> _defaultLocalizer = localizer;

		/// <summary>
		/// Sets a per-async-context localizer override. While set,
		/// all L() calls on this async flow will use this localizer.
		/// Pass null to clear the override.
		/// </summary>
		/// <param name="localizer"></param>
		public static void SetContextLocalizer(Localizer localizer)
			=> _contextLocalizer.Value = localizer;

		/// <summary>
		/// Returns translated string, or id if no translated version
		/// of id exists.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static string Get(string id)
			=> ActiveLocalizer.Get(id);

		/// <summary>
		/// Returns translated string in context, or id if no translated
		/// version of id exists.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		public static string GetParticular(string context, string id)
			=> ActiveLocalizer.GetParticular(context, id);

		/// <summary>
		/// Returns translated string as singular or plural, based on n,
		/// or id/idPlural if no translated version of id exists.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="idPlural"></param>
		/// <param name="n"></param>
		/// <returns></returns>
		public static string GetPlural(string id, string idPlural, int n)
			=> ActiveLocalizer.GetPlural(id, idPlural, n);

		/// <summary>
		/// Returns translated string in context as singular or plural,
		/// based on n, or id/idPlural if no translated version of id exists.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		public static string GetParticularPlural(string context, string id, string idPlural, int n)
			=> ActiveLocalizer.GetParticularPlural(context, id, idPlural, n);
	}
}
