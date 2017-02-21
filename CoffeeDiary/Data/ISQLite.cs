using System;
using SQLite;

namespace CoffeeDiary
{
	public interface ISQLite
	{
		SQLiteConnection GetConnection();
	}
}
