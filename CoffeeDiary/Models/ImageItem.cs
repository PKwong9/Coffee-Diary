using System;
using SQLite;

namespace CoffeeDiary
{
	public class ImageItem
	{

		public ImageItem()
		{
		}

		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public string imageFilePath { get; set; }
	}
}

