﻿// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Siegfried Pammer" email="siegfriedpammer@gmail.com"/>
//     <version>$Revision: 5529 $</version>
// </file>

using System;

namespace SyntaxEdit.Snippets
{
	/// <summary>
	/// Provides information about the event that occured during use of snippets.
	/// </summary>
	public class SnippetEventArgs : EventArgs
	{
		/// <summary>
		/// Gets the reason for deactivation.
		/// </summary>
		public DeactivateReason Reason { get; private set; }
		
		/// <summary>
		/// Creates a new SnippetEventArgs object, with a DeactivateReason.
		/// </summary>
		public SnippetEventArgs(DeactivateReason reason)
		{
			this.Reason = reason;
		}
	}
	
	/// <summary>
	/// Describes the reason for deactivation of a <see cref="SnippetElement" />.
	/// </summary>
	public enum DeactivateReason
	{
		/// <summary>
		/// Unknown reason.
		/// </summary>
		Unknown,
		/// <summary>
		/// Snippet was deleted.
		/// </summary>
		Deleted,
		/// <summary>
		/// There are no active elements in the snippet.
		/// </summary>
		NoActiveElements,
		/// <summary>
		/// The SnippetInputHandler was detached.
		/// </summary>
		InputHandlerDetached,
		/// <summary>
		/// Return was pressed by the user.
		/// </summary>
		ReturnPressed,
		/// <summary>
		/// Escape was pressed by the user.
		/// </summary>
		EscapePressed
	}
}
