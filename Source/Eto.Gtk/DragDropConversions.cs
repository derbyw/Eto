using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eto = Eto.Forms;

namespace Eto.GtkSharp
{
	public static class DragDropConversions
	{
		public static Gdk.DragAction ToPlatformDropAction(this eto.DragEffects dragAction)
		{
			Gdk.DragAction action = (Gdk.DragAction)0;

			if ((dragAction & eto.DragEffects.Copy) != 0)
			{
				action |= Gdk.DragAction.Copy;
			}

			if ((dragAction & eto.DragEffects.Move) != 0)
			{
				action |= Gdk.DragAction.Move;
			}

			if ((dragAction & eto.DragEffects.Link) != 0)
			{
				action |= Gdk.DragAction.Link;
			}

			return action;
		}

		public static eto.DragEffects ToEtoDropAction(this Gdk.DragAction dragAction)
		{
			eto.DragEffects action = (eto.DragEffects)0;

			if ((dragAction & Gdk.DragAction.Copy) != 0)
			{
				action |= eto.DragEffects.Copy;
			}

			if ((dragAction & Gdk.DragAction.Move) != 0)
			{
				action |= eto.DragEffects.Move;
			}

			if ((dragAction & Gdk.DragAction.Link) != 0)
			{
				action |= eto.DragEffects.Link;
			}

			return action;
		}
	}
}
