// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <author name="Daniel Grunwald"/>
//     <version>$Revision: 4142 $</version>
// </file>

using System;
using System.Windows;
using System.Windows.Input;

using SyntaxEdit.Editing;

namespace SyntaxEdit.CodeCompletion
{
	/// <summary>
	/// Insight window that shows an OverloadViewer.
	/// </summary>
	public class OverloadInsightWindow : InsightWindow
	{
		OverloadViewer overloadViewer = new OverloadViewer();
		
		/// <summary>
		/// Creates a new OverloadInsightWindow.
		/// </summary>
		public OverloadInsightWindow(TextArea textArea) : base(textArea)
		{
			overloadViewer.Margin = new Thickness(2,0,0,0);
			this.Content = overloadViewer;
		}
		
		/// <summary>
		/// Gets/Sets the item provider.
		/// </summary>
		public IOverloadProvider Provider {
			get { return overloadViewer.Provider; }
			set { overloadViewer.Provider = value; }
		}
		
		/// <inheritdoc/>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (!e.Handled && this.Provider.Count > 1) {
				switch (e.Key) {
					case Key.Up:
						e.Handled = true;
						overloadViewer.ChangeIndex(-1);
						break;
					case Key.Down:
						e.Handled = true;
						overloadViewer.ChangeIndex(+1);
						break;
				}
			}
		}
	}
}
