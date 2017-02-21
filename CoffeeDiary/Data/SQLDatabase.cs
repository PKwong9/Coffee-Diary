using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace CoffeeDiary
{
	public class SQLDatabase
	{
		static object locker = new object();

		SQLiteConnection database;

		/// <summary>
		/// Initializes a new instance of the <see cref="Tasky.DL.TaskDatabase"/> TaskDatabase. 
		/// if the database doesn't exist, it will create the database and all the tables.
		/// </summary>
		/// <param name='path'>
		/// Path.
		/// </param>
		public SQLDatabase()
		{
			database = DependencyService.Get<ISQLite>().GetConnection();
			// create the tables
			database.CreateTable<ImageItem>();
		}

		public IEnumerable<ImageItem> GetItems()
		{
			lock (locker)
			{
				return (from i in database.Table<ImageItem>() select i).ToList();
			}
		}

		public IEnumerable<ImageItem> GetItemsNotDone()
		{
			lock (locker)
			{
				return database.Query<ImageItem>("SELECT * FROM [Image] WHERE [Done] = 0");
			}
		}

		public ImageItem GetItem(int id)
		{
			lock (locker)
			{
				return database.Table<ImageItem>().FirstOrDefault(x => x.ID == id);
			}
		}

		public int SaveItem(ImageItem item)
		{
			lock (locker)
			{
				if (item.ID != 0)
				{
					database.Update(item);
					return item.ID;
				}
				else {
					return database.Insert(item);
				}
			}
		}

		public int DeleteItem(int id)
		{
			lock (locker)
			{
				return database.Delete<ImageItem>(id);
			}
		}
	}
}


