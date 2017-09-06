using Eto.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eto.Forms
{
	/// <summary>
	/// Drag/Drop action data.
	/// </summary>
	[Handler(typeof(IHandler))]
	public class DataObject : Widget
	{
		new IHandler Handler => (IHandler)base.Handler;

		public DataObject()
		{
		}

		public DataObject(IHandler handler) : base(handler)
		{
		}

		public string[] Types => Handler.Types;

		public void SetString(string value, string type) => Handler.SetString(value, type);

		public void SetData(byte[] value, string type) => Handler.SetData(value, type);

		public string GetString(string type) => Handler.GetString(type);

		public byte[] GetData(string type) => Handler.GetData(type);

		public string Text
		{
			get { return Handler.Text; }
			set { Handler.Text = value; }
		}

		public string Html
		{
			get { return Handler.Html; }
			set { Handler.Html = value; }
		}

		public Image Image
		{
			get { return Handler.Image; }
			set { Handler.Image = value; }
		}

		public void Clear() => Handler.Clear();

		public new interface IHandler : Widget.IHandler
		{
			/// <summary>
			/// Gets the type id's for each type of data in the data object.
			/// </summary>
			/// <value>The content types in the data object.</value>
			string[] Types { get; }

			/// <summary>
			/// Sets a string into the data object with the specified type identifier.
			/// </summary>
			/// <remarks>
			/// This is useful when setting alternate string values into the data object that are not plain text.
			/// If you are storing plain text, use the <see cref="Text"/> property instead.
			/// </remarks>
			/// <seealso cref="GetString"/>
			/// <param name="value">Value to set in the data object.</param>
			/// <param name="type">Type identifier that was used to store the data.</param>
			void SetString(string value, string type);

			/// <summary>
			/// sets a data array into the data object with the specified type identifier.
			/// </summary>
			/// <param name="value">Data to store in the data object.</param>
			/// <param name="type">Type identifier to store the data.</param>
			void SetData(byte[] value, string type);

			/// <summary>
			/// Gets a string from the data object with the specified type identifier.
			/// </summary>
			/// <returns>The string.</returns>
			/// <seealso cref="SetString"/>
			/// <param name="type">Type identifier that was used to store the data.</param>
			string GetString(string type);

			/// <summary>
			/// Gets a data array from the data object with the specified type identifier.
			/// </summary>
			/// <returns>The data array, or null if not found in the data object.</returns>
			/// <seealso cref="SetData"/>
			/// <param name="type">Type identifier that was used to store the data.</param>
			byte[] GetData(string type);

			/// <summary>
			/// Gets or sets the plain text in the data object.
			/// </summary>
			/// <value>The plain text in the data object, or null if no plain text string in the data object.</value>
			string Text { get; set; }

			/// <summary>
			/// Gets or sets html text in the data object.
			/// </summary>
			/// <value>The html value in the data object, or null if no html in the data object.</value>
			string Html { get; set; }

			/// <summary>
			/// Gets or sets an image in the data object.
			/// </summary>
			/// <value>The image in the data object, or null if no image is in the data object.</value>
			Image Image { get; set; }

			/// <summary>
			/// Clears the data object completely of all values
			/// </summary>
			void Clear();

		}
	}
}
