using System;
using Eto.Forms;
using System.Text;
using Eto.Drawing;
using System.Collections.Generic;

namespace Eto.Test.Sections.Behaviors
{
	[Section("Behaviors", "Drag and Drop")]
	public class DragDropSection : Panel
	{
		public DragDropSection()
		{
			var buttonSource = new Button { Text = "Source", AllowDrag = true };
			var panelSource = new Panel { BackgroundColor = Colors.Red, AllowDrag = true, Width = 50, Height = 50 };
			var buttonDestination = new Button { Text = "Drop here!", AllowDrop = true };
			var showDragOverEvents = new CheckBox { Text = "Show DragOver Events" };

			buttonSource.MouseDown += (sender, e) =>
			{
				buttonSource.DoDragDrop(new DataObject
				{
					Text = "test button",
					Uris = new Uri[]
					{
						new Uri(@"c:\button.txt"),
						new Uri(@"c:\test2.txt"),
					}
				}, DragEffects.Link);
			};

			panelSource.MouseDown += (sender, e) =>
			{
				panelSource.DoDragDrop(new DataObject(), DragEffects.Link);
			};

			buttonDestination.DragDrop += (sender, e) => Log.Write(sender, $"DragDrop: {WriteDragInfo(e)}");

			buttonDestination.DragEnter += (sender, e) => Log.Write(sender, $"DragEnter: {WriteDragInfo(e)}");
			buttonDestination.DragLeave += (sender, e) => Log.Write(sender, $"DragLeave: {WriteDragInfo(e)}");

			buttonDestination.DragOver += (sender, e) =>
			{
				if (showDragOverEvents.Checked == true)
					Log.Write(sender, $"DragOver: {WriteDragInfo(e)}");
				e.Handled = true;

				if (e.Source != null)
				{
					// As example allow drop of button objects but not panels
					if (e.Source is Button)
						e.Effects = DragEffects.Link;
					else
						e.Effects = DragEffects.None;
				}
				else
				{
					e.Effects = DragEffects.Copy;
				}
			};

			var layout = new DynamicLayout { Padding = 10, DefaultSpacing = new Size(4, 4) };
			layout.BeginCentered();
			layout.Add(showDragOverEvents);
			layout.EndCentered();
			layout.BeginHorizontal();
			layout.AddColumn("Drag sources:", buttonSource, panelSource, null);
			layout.Add(null);
			layout.AddColumn("Drag destinations:", buttonDestination, null);
			layout.EndHorizontal();

			Content = layout;
		}

		string WriteDragInfo(DragEventArgs e)
		{
			e.Handled = true;

			var sb = new StringBuilder();
			var obj = e.Source?.GetType().ToString() ?? "Uknown";
			sb.AppendLine($"Source: {obj}");

			var data = e.Data;
			if (data.Text != null)
				sb.AppendLine("Dropped text data: " + data.Text);
			if (data.Uris != null)
			{
				sb.AppendLine("Dropped URIs:");
				foreach (var uri in data.Uris)
				{
					sb.AppendLine("\t" + uri.LocalPath);
				}
			}
			return sb.ToString();
		}
	}
}
