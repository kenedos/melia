using System;

namespace Melia.Shared.ObjectProperties
{
	public class StringProperty : Properties.StringVariable, IProperty
	{
		/// <summary>
		/// Creates new property.
		/// </summary>
		/// <param name="ident"></param>
		/// <param name="value"></param>
		public StringProperty(string ident, string value = null)
			: base(ident, value)
		{
		}

		/// <summary>
		/// Creates new property.
		/// </summary>
		/// <param name="ident"></param>
		/// <param name="value"></param>
		public StringProperty(string ident, Enum value)
			: base(ident, value.ToString())
		{
		}
	}
	/// <summary>
	/// A string-type property that references the result of a function.
	/// </summary>
	public class RStringProperty : StringProperty, IUnsettableProperty, IProperty
	{
		private readonly Func<string> _getter;

		/// <summary>
		/// Returns the referenced value of the property.
		/// </summary>
		public override string Value
		{
			get => _getter();
			set { }
		}

		/// <summary>
		/// Creates new property.
		/// </summary>
		/// <param name="ident"></param>
		/// <param name="getter"></param>
		public RStringProperty(string ident, Func<string> getter) : base(ident, string.Empty)
		{
			_getter = getter;
		}
	}
}
