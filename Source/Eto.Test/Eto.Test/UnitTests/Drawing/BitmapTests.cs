﻿using Eto.Drawing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eto.Test.UnitTests.Drawing
{
	[TestFixture]
	public class BitmapTests
	{
		public BitmapTests()
		{
			// initialize test generator if running through IDE or nunit-gui
			TestBase.Initialize();
		}

		[Test]
		public void TestClone32Bit()
		{
			var image = TestIcons.Textures;
			var clone = image.Clone();
			ValidateImages(image, clone);
		}

		[Test]
		public void TestClone8BitIndexed()
		{
			var image = TestIcons.TexturesIndexed;
			var clone = image.Clone();
			ValidateImages(image, clone);
		}

		[TestCase(0, 0, 128, 128)]
		[TestCase(32, 32, 64, 64)]
		[TestCase(32, 32, 96, 64)]
		[TestCase(32, 32, 64, 96)]
		[TestCase(0, 0, 96, 64)]
		[TestCase(0, 0, 64, 96)]
		public void TestClone32BitRectangle(int x, int y, int width, int height)
		{
			var image = TestIcons.Textures;
			var rect = new Rectangle(x, y, width, height);
			var clone = image.Clone(rect);
			ValidateImages(image, clone, rect);
		}

		[TestCase(0, 0, 128, 128)]
		[TestCase(32, 32, 64, 64)]
		[TestCase(32, 32, 96, 64)]
		[TestCase(32, 32, 64, 96)]
		[TestCase(0, 0, 96, 64)]
		[TestCase(0, 0, 64, 96)]
		public void TestClone8BitIndexedRectangle(int x, int y, int width, int height)
		{
			var image = TestIcons.TexturesIndexed;
			var rect = new Rectangle(x, y, width, height);
			var clone = image.Clone(rect);
			ValidateImages(image, clone, rect);
		}

		[TestCase(1.0f, 0.0f, 0.0f)]
		[TestCase(0.0f, 1.0f, 0.0f)]
		[TestCase(0.0f, 0.0f, 1.0f)]
		[TestCase(0.0f, 1.0f, 1.0f)]
		[TestCase(1.0f, 0.0f, 1.0f)]
		[TestCase(1.0f, 1.0f, 0.0f)]
		public void TestGetPixelWithLock24bit(float red, float green, float blue)
		{
			var colorSet = new Color(red, green, blue);

			var image = new Bitmap(new Size(1, 1), PixelFormat.Format24bppRgb);
			image.SetPixel(0, 0, colorSet);

			using (var data = image.Lock())
			{
				var colorGet = data.GetPixel(0, 0);

				if (colorSet != colorGet)
				{
					Assert.Fail("Pixels are not the same (SetPixel: {0}, GetPixel: {1})", colorSet, colorGet);
				}
			}
		}

		[TestCase(1.0f, 0.0f, 0.0f)]
		[TestCase(0.0f, 1.0f, 0.0f)]
		[TestCase(0.0f, 0.0f, 1.0f)]
		[TestCase(0.0f, 1.0f, 1.0f)]
		[TestCase(1.0f, 0.0f, 1.0f)]
		[TestCase(1.0f, 1.0f, 0.0f)]
		public void TestGetPixelWithoutLock24bit(float red, float green, float blue)
		{
			var colorSet = new Color(red, green, blue);

			var image = new Bitmap(new Size(1, 1), PixelFormat.Format24bppRgb);
			image.SetPixel(0, 0, colorSet);

			var colorGet = image.GetPixel(0, 0);

			if (colorSet != colorGet)
			{
				Assert.Fail("Pixels are not the same (SetPixel: {0}, GetPixel: {1})", colorSet, colorGet);
			}
		}

		[TestCase(1.0f, 0.0f, 0.0f, 1.0f)]
		[TestCase(0.0f, 1.0f, 0.0f, 0.5f)]
		[TestCase(0.0f, 0.0f, 1.0f, 0.0f)]
		[TestCase(0.0f, 1.0f, 1.0f, 1.0f)]
		[TestCase(1.0f, 0.0f, 1.0f, 0.5f)]
		[TestCase(1.0f, 1.0f, 0.0f, 0.0f)]
		public void TestGetPixelWithLock32bit(float red, float green, float blue, float alpha)
		{
			var colorSet = new Color(red, green, blue, alpha);

			var image = new Bitmap(new Size(1, 1), PixelFormat.Format32bppRgba);
			image.SetPixel(0, 0, colorSet);

			using (var data = image.Lock())
			{
				var colorGet = data.GetPixel(0, 0);

				if (colorSet != colorGet)
				{
					Assert.Fail("Pixels are not the same (SetPixel: {0}, GetPixel: {1})", colorSet, colorGet);
				}
			}
		}

		[TestCase(1.0f, 0.0f, 0.0f, 1.0f)]
		[TestCase(0.0f, 1.0f, 0.0f, 0.5f)]
		[TestCase(0.0f, 0.0f, 1.0f, 0.0f)]
		[TestCase(0.0f, 1.0f, 1.0f, 1.0f)]
		[TestCase(1.0f, 0.0f, 1.0f, 0.5f)]
		[TestCase(1.0f, 1.0f, 0.0f, 0.0f)]
		public void TestGetPixelWithoutLock32bit(float red, float green, float blue, float alpha)
		{
			var colorSet = new Color(red, green, blue, alpha);

			var image = new Bitmap(new Size(1, 1), PixelFormat.Format32bppRgba);
			image.SetPixel(0, 0, colorSet);

			var colorGet = image.GetPixel(0, 0);

			if (colorSet != colorGet)
			{
				Assert.Fail("Pixels are not the same (SetPixel: {0}, GetPixel: {1})", colorSet, colorGet);
			}
		}

		static void ValidateImages(Bitmap image, Bitmap clone, Rectangle? rect = null)
		{
			var testRect = rect ?? new Rectangle(image.Size);
			using (var imageData = image.Lock())
			using (var cloneData = clone.Lock())
			{
				if (imageData.BytesPerPixel == 1)
				{
					unsafe
					{
						// test pixels directly
						byte* imageptr = (byte*)imageData.Data;
						imageptr += testRect.Top * imageData.ScanWidth + testRect.Left;
						byte* cloneptr = (byte*)cloneData.Data;
						for (int y = 0; y < testRect.Height; y++)
						{
							byte* imagerow = imageptr;
							byte* clonerow = cloneptr;
							for (int x = 0; x < testRect.Width; x++)
							{
								var imagePixel = *(imagerow++);
								var clonePixel = *(clonerow++);
								if (imagePixel != clonePixel)
								{
									Assert.Fail("Image pixels are not the same at position {0},{1} (source: {2}, clone: {3})", x, y, imagePixel, clonePixel);
								}
							}
							imageptr += imageData.ScanWidth;
							cloneptr += cloneData.ScanWidth;
						}
					}
				}
				else
					for (int x = 0; x < testRect.Width; x++)
						for (int y = 0; y < testRect.Height; y++)
						{
							var imagePixel = imageData.GetPixel(x + testRect.Left, y + testRect.Top);
							var clonePixel = cloneData.GetPixel(x, y);
							if (imagePixel != clonePixel)
								Assert.Fail("Image pixels are not the same at position {0},{1} (source: {2}, clone: {3})", x, y, imagePixel, clonePixel);
						}
			}
		}
	}
}
