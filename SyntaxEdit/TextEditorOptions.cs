// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <author name="Daniel Grunwald"/>
//     <version>$Revision: 5529 $</version>
// </file>

using System;
using System.ComponentModel;
using System.Text;

namespace SyntaxEdit
{
	/// <summary>
	/// A container for the text editor options.
	/// </summary>
	[Serializable]
	public class TextEditorOptions : INotifyPropertyChanged
	{
		#region PropertyChanged handling
		/// <inheritdoc/>
		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;
		
		/// <summary>
		/// Raises the PropertyChanged event.
		/// </summary>
		/// <param name="propertyName">The name of the changed property.</param>
		protected void OnPropertyChanged(string propertyName)
		{
			OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}
		
		/// <summary>
		/// Raises the PropertyChanged event.
		/// </summary>
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if (PropertyChanged != null) {
				PropertyChanged(this, e);
			}
		}
		#endregion
		
		#region ShowSpaces / ShowTabs / ShowEndOfLine / ShowBoxForControlCharacters
		bool showSpaces;
		
		/// <summary>
		/// Gets/Sets whether to show � for spaces.
		/// </summary>
		/// <remarks>The default value is <c>false</c>.</remarks>
		[DefaultValue(false)]
		public virtual bool ShowSpaces {
			get { return showSpaces; }
			set {
				if (showSpaces != value) {
					showSpaces = value;
					OnPropertyChanged("ShowSpaces");
				}
			}
		}
		
		bool showTabs;
		
		/// <summary>
		/// Gets/Sets whether to show � for tabs.
		/// </summary>
		/// <remarks>The default value is <c>false</c>.</remarks>
		[DefaultValue(false)]
		public virtual bool ShowTabs {
			get { return showTabs; }
			set {
				if (showTabs != value) {
					showTabs = value;
					OnPropertyChanged("ShowTabs");
				}
			}
		}
		
		bool showEndOfLine;
		
		/// <summary>
		/// Gets/Sets whether to show � at the end of lines.
		/// </summary>
		/// <remarks>The default value is <c>false</c>.</remarks>
		[DefaultValue(false)]
		public virtual bool ShowEndOfLine {
			get { return showEndOfLine; }
			set {
				if (showEndOfLine != value) {
					showEndOfLine = value;
					OnPropertyChanged("ShowEndOfLine");
				}
			}
		}
		
		bool showBoxForControlCharacters = true;
		
		/// <summary>
		/// Gets/Sets whether to show a box with the hex code for control characters.
		/// </summary>
		/// <remarks>The default value is <c>true</c>.</remarks>
		[DefaultValue(true)]
		public virtual bool ShowBoxForControlCharacters {
			get { return showBoxForControlCharacters; }
			set {
				if (showBoxForControlCharacters != value) {
					showBoxForControlCharacters = value;
					OnPropertyChanged("ShowBoxForControlCharacters");
				}
			}
		}
		#endregion
		
		#region EnableHyperlinks
		bool enableHyperlinks = true;
		
		/// <summary>
		/// Gets/Sets whether to enable clickable hyperlinks in the editor.
		/// </summary>
		/// <remarks>The default value is <c>true</c>.</remarks>
		[DefaultValue(true)]
		public virtual bool EnableHyperlinks {
			get { return enableHyperlinks; }
			set {
				if (enableHyperlinks != value) {
					enableHyperlinks = value;
					OnPropertyChanged("EnableHyperlinks");
				}
			}
		}
		
		bool enableEmailHyperlinks = true;
		
		/// <summary>
		/// Gets/Sets whether to enable clickable hyperlinks for e-mail addresses in the editor.
		/// </summary>
		/// <remarks>The default value is <c>true</c>.</remarks>
		[DefaultValue(true)]
		public virtual bool EnableEmailHyperlinks {
			get { return enableEmailHyperlinks; }
			set {
				if (enableEmailHyperlinks != value) {
					enableEmailHyperlinks = value;
					OnPropertyChanged("EnableEMailHyperlinks");
				}
			}
		}
		
		bool requireControlModifierForHyperlinkClick = true;
		
		/// <summary>
		/// Gets/Sets whether the user needs to press Control to click hyperlinks.
		/// The default value is true.
		/// </summary>
		/// <remarks>The default value is <c>true</c>.</remarks>
		[DefaultValue(true)]
		public virtual bool RequireControlModifierForHyperlinkClick {
			get { return requireControlModifierForHyperlinkClick; }
			set {
				if (requireControlModifierForHyperlinkClick != value) {
					requireControlModifierForHyperlinkClick = value;
					OnPropertyChanged("RequireControlModifierForHyperlinkClick");
				}
			}
		}
		#endregion
		
		#region TabSize / IndentationSize / ConvertTabsToSpaces / GetIndentationString
		// I'm using '_' prefixes for the fields here to avoid confusion with the local variables
		// in the methods below.
		// The fields should be accessed only by their property - the fields might not be used
		// if someone overrides the property.
		
		int _indentationSize = 4;
		
		/// <summary>
		/// Gets/Sets the width of one indentation unit.
		/// </summary>
		/// <remarks>The default value is 4.</remarks>
		[DefaultValue(4)]
		public virtual int IndentationSize {
			get { return _indentationSize; }
			set {
				if (value < 1)
					throw new ArgumentOutOfRangeException("value", value, "value must be positive");
				// sanity check; a too large value might cause WPF to crash internally much later
				// (it only crashed in the hundred thousands for me; but might crash earlier with larger fonts)
				if (value > 1000)
					throw new ArgumentOutOfRangeException("value", value, "indentation size is too large");
				if (_indentationSize != value) {
					_indentationSize = value;
					OnPropertyChanged("IndentationSize");
					OnPropertyChanged("IndentationString");
				}
			}
		}
		
		bool _convertTabsToSpaces;
		
		/// <summary>
		/// Gets/Sets whether to use spaces for indentation instead of tabs.
		/// </summary>
		/// <remarks>The default value is <c>false</c>.</remarks>
		[DefaultValue(false)]
		public virtual bool ConvertTabsToSpaces {
			get { return _convertTabsToSpaces; }
			set {
				if (_convertTabsToSpaces != value) {
					_convertTabsToSpaces = value;
					OnPropertyChanged("ConvertTabsToSpaces");
					OnPropertyChanged("IndentationString");
				}
			}
		}
		
		/// <summary>
		/// Gets the text used for indentation.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
		[Browsable(false)]
		public string IndentationString {
			get { return GetIndentationString(1); }
		}
		
		/// <summary>
		/// Gets text required to indent from the specified <paramref name="column"/> to the next indentation level.
		/// </summary>
		public virtual string GetIndentationString(int column)
		{
			if (column < 1)
				throw new ArgumentOutOfRangeException("column", column, "Value must be at least 1.");
			int indentationSize = this.IndentationSize;
			if (ConvertTabsToSpaces) {
				return new string(' ', indentationSize - ((column - 1) % indentationSize));
			} else {
				return "\t";
			}
		}
		#endregion
		
		bool cutCopyWholeLine = true;
		
		/// <summary>
		/// Gets/Sets whether copying without a selection copies the whole current line.
		/// </summary>
		[DefaultValue(true)]
		public virtual bool CutCopyWholeLine {
			get { return cutCopyWholeLine; }
			set {
				if (cutCopyWholeLine != value) {
					cutCopyWholeLine = value;
					OnPropertyChanged("CutCopyWholeLine");
				}
			}
		}
		
		bool allowScrollBelowDocument;
		
		/// <summary>
		/// Gets/Sets whether the user can scroll below the bottom of the document.
		/// The default value is false; but it a good idea to set this property to true when using folding.
		/// </summary>
		[DefaultValue(false)]
		public virtual bool AllowScrollBelowDocument {
			get { return allowScrollBelowDocument; }
			set {
				if (allowScrollBelowDocument != value) {
					allowScrollBelowDocument = value;
					OnPropertyChanged("AllowScrollBelowDocument");
				}
			}
		}
	}
}
