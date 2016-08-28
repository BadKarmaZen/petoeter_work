using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO;
using DayCare.Core;
using DayCare.Model.Lite;

namespace DayCare.ViewModels.Images
{
	class PasportImageViewModel : Screen
	{
		const double DefaultWidthLandscape = 800;
		const double DefaultWidthPortrait = 450;
		const double DefaultHeight = 600;
		const double DefaultBorderWidth = 210;
		const double DefaultBorderHeight = 270;

		#region Members
		private BitmapImage _croppedImage;
		private BitmapImage _resultImage;
		public Bitmap Original { get; set; }
		public Bitmap CroppedBitmap { get; set; }

		private int _leftPosition;
		private int _leftBorderPosition;
		private int _topPosition;
		private int _topBorderPosition;

		private double _zoom;
		private double _borderZoom;

		private double _borderWidth;
		private double _borderHeight;

		private double _imageWidth;

		public double ImageWidth
		{
			get { return _imageWidth; }
			set { _imageWidth = value; NotifyOfPropertyChange(()=>ImageWidth); }
		}
		private double _imageHeight;

		public double ImageHeight
		{
			get { return _imageHeight; }
			set { _imageHeight = value; NotifyOfPropertyChange(() => ImageHeight); }
		}
		#endregion

		#region Properties

		public DayCare.Core.Events.ShowSnapshot Info { get; set; }

		public BitmapImage CroppedImage
		{
			get { return _croppedImage; }
			set { _croppedImage = value; NotifyOfPropertyChange(() => CroppedImage); }
		}

		public BitmapImage ResultImage
		{
			get { return _resultImage; }
			set { _resultImage = value; NotifyOfPropertyChange(() => ResultImage); }
		}
		public int LeftPosition
		{
			get { return _leftPosition; }
			set
			{
				_leftPosition = value;
				NotifyOfPropertyChange(() => LeftPosition);
				LeftBorderPosition = (int)(LeftPosition * (ImageWidth - BorderWidth) / 100.0);
				UpdateResult();
			}
		}


		public int LeftBorderPosition
		{
			get { return _leftBorderPosition; }
			set { _leftBorderPosition = value; NotifyOfPropertyChange(() => LeftBorderPosition); }
		}

		public int TopPosition
		{
			get { return _topPosition; }
			set
			{
				_topPosition = value;
				TopBorderPosition = -(int)(value * (ImageHeight - BorderHeight) / 100.0);
				NotifyOfPropertyChange(() => TopPosition);
				UpdateResult();
			}
		}
		public int TopBorderPosition
		{
			get { return _topBorderPosition; }
			set { _topBorderPosition = value; NotifyOfPropertyChange(() => TopBorderPosition); }
		}

		public double BorderZoom
		{
			get { return _borderZoom; }
			set
			{
				_borderZoom = value;

				BorderWidth = DefaultBorderWidth * _borderZoom / 100.0;
				BorderHeight = DefaultBorderHeight * _borderZoom / 100.0;

				NotifyOfPropertyChange(() => BorderZoom); 
				
				UpdateResult();
			}
		}
		public double BorderWidth
		{
			get { return _borderWidth; }
			set { _borderWidth = value; NotifyOfPropertyChange(() => BorderWidth); }
		}

		public double BorderHeight
		{
			get { return _borderHeight; }
			set { _borderHeight = value; NotifyOfPropertyChange(() => BorderHeight); }
		}

		#endregion


		private double _horizontalPosition;

		public double HorizontalPosition
		{
			get { return _horizontalPosition; }
			set { _horizontalPosition = value; NotifyOfPropertyChange(() => HorizontalPosition); }
		}
		private double _vertitalPosition;

		public double VertitalPosition
		{
			get { return _vertitalPosition; }
			set { _vertitalPosition = value; NotifyOfPropertyChange(() => VertitalPosition); }
		}

		public double Zoom
		{
			get { return _zoom; }
			set
			{
				_zoom = value;
				NotifyOfPropertyChange(() => Zoom);
				UpdateResult();
			}
		}
		
		public PasportImageViewModel(DayCare.Core.Events.ShowSnapshot info)
		{
			Info = info;

			LoadOriginal();

			BorderZoom = 100.0;
		}

		public void LoadOriginal()
		{
			if (File.Exists(Info.FileName) == false)
				return;

			Original = Image.FromFile(Info.FileName) as Bitmap;

			CalculateZoom();
		}

		private void CalculateZoom()
		{
			var zoom = 1.0;

			if (Original.Width > Original.Height)
			{
				//	landscape
				ImageWidth = DefaultWidthLandscape;
				ImageHeight = DefaultHeight;				
			}
			else
			{
				//	portrait
				ImageWidth = DefaultWidthPortrait;
				ImageHeight = DefaultHeight;
			}

			while ((Original.Width * zoom) >= ImageWidth || (Original.Height * zoom) >= DefaultHeight)
			{
				zoom *= 0.99;
			}

			Zoom = 100.0 * zoom;
		}

		public void RotateLeftAction()
		{
			Original.RotateFlip(RotateFlipType.Rotate270FlipNone);

			CalculateZoom();

			UpdateResult();
		}

		public void RotateRightAction()
		{
			Original.RotateFlip(RotateFlipType.Rotate90FlipNone);

			CalculateZoom();

			UpdateResult();
		}

		public void UpdateResult()
		{
			if (Original != null)
			{
				Size newSize = new Size((int)(Original.Width * _zoom / 100), (int)(Original.Height * _zoom / 100));

				Bitmap bmp = new Bitmap(Original, newSize);

				ResultImage = BitmapToImageSource(bmp);

				var cropLeft = (LeftBorderPosition);//(int)(295 + HorizontalPosition);
				var cropTop = (TopBorderPosition);

				Rectangle crop = new Rectangle((int)cropLeft, cropTop, (int)Math.Max(1, BorderWidth), (int)Math.Max(1, BorderHeight));
				CroppedBitmap = CropBitmap(bmp, crop);

				CroppedImage = BitmapToImageSource(CroppedBitmap);
			}
		}

		public static Bitmap CropBitmap(Bitmap b, Rectangle r)
		{
			Bitmap nb = new Bitmap(r.Width, r.Height);
			Graphics g = Graphics.FromImage(nb);
			g.DrawImage(b, -r.X, -r.Y);
			return nb;
		}

		BitmapImage BitmapToImageSource(Bitmap bitmap)
		{
			using (MemoryStream memory = new MemoryStream())
			{
				bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
				memory.Position = 0;
				BitmapImage bitmapimage = new BitmapImage();
				bitmapimage.BeginInit();
				bitmapimage.StreamSource = memory;
				bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapimage.EndInit();

				return bitmapimage;
			}
		}

		public void UseSnapshotAction() 
		{ 
			UpdateResult();

			var path = Path.GetTempFileName();
			var thumbnail = CroppedBitmap.GetThumbnailImage(105, 135, () => { return true; }, IntPtr.Zero);
			thumbnail.Save(path);
			PetoeterImageManager.SaveFile(Info.FileId, path);

			File.Delete(path);

			//	save image
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.SaveSnapshot { FileId = Info.FileId });

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
					new Core.Events.ShowSnapshot());
		}

		public void CancelAction()
		{ 
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.ShowSnapshot());
		}
	}
}
