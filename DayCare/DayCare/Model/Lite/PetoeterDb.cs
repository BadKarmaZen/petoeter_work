using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model.Lite
{
	public class PetoeterDb : LiteDatabase
	{

		public const string TableAccount = "account";
		public const string TableMember = "member";
		public const string TableChildren = "children";
		public const string TableHolidays = "holidays";
		public const string TablePresence = "presence";
		public const string TableSystem = "SystemSettings";
		public const string TablePrices = "prices";

		public SystemSettings Settings { get; set; }

		public PetoeterDb(string file)
			: base(file)
		{
		}

		public const string FileName = @"petoeter_lite.ldb";

		protected override void OnModelCreating(BsonMapper mapper) 
		{
			base.OnModelCreating(mapper);

			mapper.Entity<Account>()
				.DbRef(a => a.Members, PetoeterDb.TableMember)
				.DbRef(a => a.Children, PetoeterDb.TableChildren);

			mapper.Entity<Presence>()
				.DbRef(p => p.Child, PetoeterDb.TableChildren)
				.DbRef(p => p.BroughtBy, PetoeterDb.TableMember)
				.DbRef(p => p.TakenBy, PetoeterDb.TableMember);
		}

		public LiteCollection<Account> Accounts
		{
			get
			{
				return GetCollection<Account>(PetoeterDb.TableAccount)
					.Include(a => a.Members)
					.Include(a => a.Children);
			}
		}

		public LiteCollection<Member> Members
		{
			get
			{
				return GetCollection<Member>(PetoeterDb.TableMember);
			}
		}

		public LiteCollection<Child> Children
		{
			get
			{
				return GetCollection<Child>(PetoeterDb.TableChildren);
			}
		}

		public LiteCollection<Date> Holidays
		{
			get
			{
				return GetCollection<Date>(PetoeterDb.TableHolidays);
			}
		}

		public LiteCollection<Presence> Presences 
		{
			get
			{
				return GetCollection<Presence>(PetoeterDb.TablePresence)
					.Include(p => p.BroughtBy)
					.Include(p => p.TakenBy)
					.Include(a => a.Child);
			}
		}
		public SystemSettings GetSettings()
		{
			Settings = GetCollection<SystemSettings>(PetoeterDb.TableSystem).FindAll().FirstOrDefault();

			if (Settings == null)
			{
				Settings = new SystemSettings { };
				GetCollection<SystemSettings>(PetoeterDb.TableSystem).Insert(Settings);
			}

			return Settings;
		}

		public void UpdateSystemSettings()
		{
			GetCollection<SystemSettings>(PetoeterDb.TableSystem).Update(Settings);
		}
		
		internal void DropAll()
		{
			DropCollection(PetoeterDb.TableAccount);
			DropCollection(PetoeterDb.TableChildren);
			DropCollection(PetoeterDb.TableMember);
			DropCollection(PetoeterDb.TablePresence);
			DropCollection(PetoeterDb.TableHolidays);
			DropCollection(PetoeterDb.TableSystem);

			var fileIds = (from fs in FileStorage.FindAll()
										 select fs.Id).ToList();
			fileIds.ForEach(id => FileStorage.Delete(id));
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
						SaveFile(Olaf, @"olaf.png");
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
