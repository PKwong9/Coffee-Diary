using Xamarin.Forms;

namespace CoffeeDiary
{
	public partial class App : Application
	{
		public static double ScreenHeight;
		public static double ScreenWidth;
		public static SQLDatabase database;

		public App()
		{
			MainPage =new HomePage(); 
		}

		public static SQLDatabase Database
		{
			get
			{
				if (database == null)
				{
					database = new SQLDatabase();
				}
				return database;
			}
		}
	
		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}

