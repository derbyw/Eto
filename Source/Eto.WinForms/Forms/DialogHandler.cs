using sd = System.Drawing;
using swf = System.Windows.Forms;
using Eto.Forms;
using System;
using System.Threading.Tasks;

namespace Eto.WinForms.Forms
{
	public class DialogHandler : WindowHandler<swf.Form, Dialog, Dialog.ICallback>, Dialog.IHandler
	{
		Button button;

		
		public DialogHandler()
		{
			Control = new swf.Form
			{
				StartPosition = swf.FormStartPosition.CenterParent,
				AutoSize = true,
				Size = sd.Size.Empty,
				MinimumSize = sd.Size.Empty,
				ShowInTaskbar = false,
				ShowIcon = false,
				MaximizeBox = false,
				MinimizeBox = false
			};
		}

		protected override swf.FormBorderStyle DefaultWindowStyle
		{
			get { return swf.FormBorderStyle.FixedDialog; }
		}

		public Button AbortButton { get; set; }

		public Button DefaultButton
		{
			get
			{
				return button;
			}
			set
			{
				button = value;
				if (button != null)
				{
					var b = button.ControlObject as swf.IButtonControl;
					Control.AcceptButton = b;
				}
				else
					Control.AcceptButton = null;
			}
		}

		public DialogDisplayMode DisplayMode { get; set; }

		public void ShowModal()
		{
			Control.ShowDialog();
			Control.Owner = null; // without this, the dialog is still active as part of the owner form
        }

		protected override void Initialize()
		{
			base.Initialize();
			Widget.KeyDown += Widget_KeyDown;
		}

		void Widget_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Escape && AbortButton != null)
			{
				AbortButton.PerformClick();
				e.Handled = true;
			}
		}

		public Task ShowModalAsync()
		{
			var tcs = new TaskCompletionSource<bool>();
			Application.Instance.AsyncInvoke(() =>
			{
				ShowModal();

				tcs.SetResult(true);
			});
			return tcs.Task;
		}

        public void InsertDialogButton(bool positive, int index, Button item)
        {

        }

        public void RemoveDialogButton(bool positive, int index)
        {

        }
    }
}
