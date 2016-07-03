﻿using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model.Lite
{
	public class PetoeterDb : LiteDatabase
	{
		public SystemSettings Settings { get; set; }

		public PetoeterDb(string file)
			: base(file)
		{
		}

		public const string FileName = @"E:\petoeter_lite.ldb";

		protected override void OnModelCreating(BsonMapper mapper) 
		{
			base.OnModelCreating(mapper);

			mapper.Entity<Account>()
				.DbRef(a => a.Members, "member")
				.DbRef(a => a.Children, "children");

			mapper.Entity<Presence>()
				.DbRef(p => p.Child, "children")
				.DbRef(p => p.BroughtBy, "member")
				.DbRef(p => p.TakenBy, "member");
		}

		public LiteCollection<Account> Accounts
		{
			get
			{
				return GetCollection<Account>("account")
					.Include(a => a.Members)
					.Include(a => a.Children);
			}
		}

		public LiteCollection<Member> Members
		{
			get
			{
				return GetCollection<Member>("member");
			}
		}

		public LiteCollection<Child> Children
		{
			get
			{
				return GetCollection<Child>("children");
			}
		}

		public LiteCollection<Date> Holidays
		{
			get
			{
				return GetCollection<Date>("holidays");
			}
		}

		public LiteCollection<Presence> Presences 
		{
			get
			{
				return GetCollection<Presence>("presence")
					.Include(p => p.BroughtBy)
					.Include(p => p.TakenBy)
					.Include(a => a.Child);
			}
		}
		public SystemSettings GetSettings()
		{
			Settings = GetCollection<SystemSettings>("SystemSettings").FindAll().FirstOrDefault();

			if (Settings == null)
			{
				Settings = new SystemSettings { };
				GetCollection<SystemSettings>("SystemSettings").Insert(Settings);
			}

			return Settings;
		}

		public void UpdateSystemSettings()
		{
			GetCollection<SystemSettings>("SystemSettings").Update(Settings);
		}


	}

	public class PetoeterImageManager
	{
		public const string Olaf = "img/olaf";

		private static System.Windows.Media.Imaging.BitmapImage _olaf;

		public static System.Windows.Media.Imaging.BitmapImage GetImage(string fileId)
		{
			if (string.IsNullOrWhiteSpace(fileId))
			{
				if (_olaf == null)
				{
					_olaf = GetImage(Olaf);					
				}

				return _olaf;
			}

			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				var file = db.FileStorage.FindById(fileId);

				if (file == null)
				{
					if (fileId == Olaf)
					{
						SaveFile(Olaf, @"E:\[Development]\[Home]\petoeter_work\DayCare\DayCare\Resources\olaf.png");
					}

					return null;		 
				}
				
				using(var reader = file.OpenRead())
				{
					System.IO.MemoryStream mem = new System.IO.MemoryStream();
					mem.SetLength(file.Length);

					reader.Read(mem.GetBuffer(), 0, (int)file.Length);
					mem.Flush();

					System.Windows.Media.Imaging.BitmapImage image = new System.Windows.Media.Imaging.BitmapImage();

					image.BeginInit();
					image.StreamSource = mem;
					image.EndInit();

					image.Freeze();

					return image;
				}
			}
		}

		public static void SaveFile(string fileId, string fileName)
		{
			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				db.FileStorage.Upload(fileId, fileName);
			}
		}

		internal static void RemoveFile(string fileId)
		{
			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				db.FileStorage.Delete(fileId);
			}
		}
	}
}
