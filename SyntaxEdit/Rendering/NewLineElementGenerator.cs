// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <author name="Daniel Grunwald"/>
//     <version>$Revision: 5263 $</version>
// </file>

using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

using SyntaxEdit.Document;
using SyntaxEdit.Utils;

namespace SyntaxEdit.Rendering
{
	// This class is internal because it does not need to be accessed by the user - it can be configured using TextEditorOptions.
	
	/// <summary>
	/// Elements generator that displays "�" at the end of lines.
	/// </summary>
	/// <remarks>
	/// This element generator can be easily enabled and configured using the
	/// <see cref="TextEditorOptions"/>.
	/// </remarks>
	sealed class NewLineElementGenerator : VisualLineElementGenerator, IBuiltinElementGenerator
	{
		void IBuiltinElementGenerator.FetchOptions(TextEditorOptions options)
		{
		}
		
		/// <inheritdoc/>
		public override int GetFirstInterestedOffset(int startOffset)
		{
			DocumentLine lastDocumentLine = CurrentContext.VisualLine.LastDocumentLine;
			if (lastDocumentLine.DelimiterLength > 0)
				return lastDocumentLine.Offset + lastDocumentLine.Length;
			else
				return -1;
		}
		
		/// <inheritdoc/>
		public override VisualLineElement ConstructElement(int offset)
		{
			string newlineText;
			DocumentLine lastDocumentLine = CurrentContext.VisualLine.LastDocumentLine;
			if (lastDocumentLine.DelimiterLength == 2) {
				newlineText = "\u00B6";
			} else if (lastDocumentLine.DelimiterLength == 1) {
				char newlineChar = CurrentContext.Document.GetCharAt(lastDocumentLine.Offset + lastDocumentLine.Length);
				if (newlineChar == '\r')
					newlineText = "\\r";
				else if (newlineChar == '\n')
					newlineText = "\\n";
				else
					newlineText = "?";
			} else {
				return null;
			}
			FormattedText text = TextFormatterFactory.CreateFormattedText(
				CurrentContext.TextView,
				newlineText,
				CurrentContext.GlobalTextRunProperties.Typeface,
				CurrentContext.GlobalTextRunProperties.FontRenderingEmSize,
				Brushes.LightGray
			);
			return new NewLineTextElement(text);
		}
		
		sealed class NewLineTextElement : FormattedTextElement
		{
			public NewLineTextElement(FormattedText text) : base(text, 0)
			{
				BreakBefore = LineBreakCondition.BreakPossible;
				BreakAfter = LineBreakCondition.BreakRestrained;
			}
			
			public override int GetNextCaretPosition(int visualColumn, LogicalDirection direction, CaretPositioningMode mode)
			{
				// only place a caret stop before the newline, no caret stop after it
				if (visualColumn > this.VisualColumn && direction == LogicalDirection.Backward ||
				    visualColumn < this.VisualColumn && direction == LogicalDirection.Forward)
				{
					return this.VisualColumn;
				} else {
					return -1;
				}
			}
			
			public override bool HandlesLineBorders {
				get { return true; }
			}
		}
	}
}
