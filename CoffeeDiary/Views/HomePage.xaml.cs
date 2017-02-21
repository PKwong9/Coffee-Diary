using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;



namespace CoffeeDiary
{
	public partial class HomePage : ContentPage
	{

		TodoItemController toDoController;
		ObservableCollection<TodoItem> display_list;

		public HomePage()
		{
			toDoController = new TodoItemController();

			InitializeComponent();

			todoList.ItemsSource = display_list; //sets todoList source to display_list for entry display of Name and date of creation
		}

		protected override async void OnAppearing() //on first appearance of HomePage for todoList
		{
			base.OnAppearing();

			await RefreshItems(true, syncItems: false);
		}

		public async void OnSelected(object sender, SelectedItemChangedEventArgs e) //called when todoList item is selected
		{
			if (e.SelectedItem == null)
			{
				return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
			}

			await Navigation.PushModalAsync(new ViewEntry((TodoItem)e.SelectedItem));
			ListView lst = (ListView)sender;
			lst.SelectedItem = null;
		}

		public async void OnRefresh(object sender, EventArgs e) //refreshes the current todoList when user pulls down list
		{
			try
			{
				await RefreshItems(true, true);
			}
			catch
			{
				await DisplayAlert("No Network", "There is no network!", "Cancel");
			}

			todoList.IsRefreshing = false;

		}

		private async Task RefreshItems(bool showActivityIndicator, bool syncItems) //refresh activity for todoList
		{

			Debug.WriteLine("Enter RefreshItem()");
			using (var scope = new ActivityIndicatorScope(syncIndicator, showActivityIndicator))
			{
					toDoController.Refresh(syncItems);
					if (toDoController.error) {
						throw new Exception();
					} 
					display_list = toDoController.GetSource();
					todoList.ItemsSource = display_list;
			}

		}
	

		private class ActivityIndicatorScope : IDisposable //visual control to indicate activity is going on 
		{
			private bool showIndicator;
			private ActivityIndicator indicator;
			private Task indicatorDelay;

			public ActivityIndicatorScope(ActivityIndicator indicator, bool showIndicator)
			{
				this.indicator = indicator;
				this.showIndicator = showIndicator;

				if (showIndicator)
				{
					indicatorDelay = Task.Delay(2000);
					SetIndicatorActivity(true);
				}
				else
				{
					indicatorDelay = Task.FromResult(0);
				}
			}

			private void SetIndicatorActivity(bool isActive) //sets ActivityIndicator status
			{
				this.indicator.IsVisible = isActive;
				this.indicator.IsRunning = isActive;
			}

			public void Dispose()
			{
				if (showIndicator)
				{
					indicatorDelay.ContinueWith(t => SetIndicatorActivity(false), TaskScheduler.FromCurrentSynchronizationContext());
				}
			}
		}


		private async void AddEntry_onclick(object sender, EventArgs e) //triggered when AddEntryBtn is clicked
		{
			await Navigation.PushModalAsync(new AddEntry());

		}


		public async void OnDelete(object sender, EventArgs e) //deletes item from todoList and updates Azure 
		{
			var m = (MenuItem)sender;
			TodoItem item = (TodoItem) m.CommandParameter;

			toDoController.display_list.Remove(item);
			await toDoController.Delete(item);
		}

	}
}

