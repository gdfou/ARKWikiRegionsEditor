﻿// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <author name="Daniel Grunwald"/>
//     <version>$Revision: 5529 $</version>
// </file>

using System;
using System.ComponentModel;
using System.Globalization;

namespace SyntaxEdit.Highlighting
{
	/// <summary>
	/// Converts between strings and <see cref="IHighlightingDefinition"/> by treating the string as the definition name
	/// and calling <c>HighlightingManager.Instance.<see cref="HighlightingManager.GetDefinition">GetDefinition</see>(name)</c>.
	/// </summary>
	public sealed class HighlightingDefinitionTypeConverter : TypeConverter
	{
		/// <inheritdoc/>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
				return true;
			else
				return base.CanConvertFrom(context, sourceType);
		}
		
		/// <inheritdoc/>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string definitionName = value as string;
			if (definitionName != null)
				return HighlightingManager.Instance.GetDefinition(definitionName);
			else
				return base.ConvertFrom(context, culture, value);
		}
		
		/// <inheritdoc/>
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(string))
				return true;
			else
				return base.CanConvertTo(context, destinationType);
		}
		
		/// <inheritdoc/>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			IHighlightingDefinition definition = value as IHighlightingDefinition;
			if (definition != null && destinationType == typeof(string))
				return definition.Name;
			else
				return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
