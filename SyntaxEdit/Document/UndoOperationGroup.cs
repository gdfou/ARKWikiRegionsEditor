// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <author name="Daniel Grunwald"/>
//     <version>$Revision: 5454 $</version>
// </file>

using System;
using System.Diagnostics;
using SyntaxEdit.Utils;

namespace SyntaxEdit.Document
{
	/// <summary>
	/// This class stacks the last x operations from the undostack and makes
	/// one undo/redo operation from it.
	/// </summary>
	sealed class UndoOperationGroup : IUndoableOperationWithContext
	{
		IUndoableOperation[] undolist;
		
		public UndoOperationGroup(Deque<IUndoableOperation> stack, int numops)
		{
			if (stack == null)  {
				throw new ArgumentNullException("stack");
			}
			
			Debug.Assert(numops > 0 , "UndoOperationGroup : numops should be > 0");
			if (numops > stack.Count) {
				numops = stack.Count;
			}
			undolist = new IUndoableOperation[numops];
			for (int i = 0; i < numops; ++i) {
				undolist[i] = stack.PopBack();
			}
		}
		
		public void Undo()
		{
			for (int i = 0; i < undolist.Length; ++i) {
				undolist[i].Undo();
			}
		}
		
		public void Undo(UndoStack stack)
		{
			for (int i = 0; i < undolist.Length; ++i) {
				stack.RunUndo(undolist[i]);
			}
		}
		
		public void Redo()
		{
			for (int i = undolist.Length - 1; i >= 0; --i) {
				undolist[i].Redo();
			}
		}
		
		public void Redo(UndoStack stack)
		{
			for (int i = undolist.Length - 1; i >= 0; --i) {
				stack.RunRedo(undolist[i]);
			}
		}
	}
}
