using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Plugin.Media;
using Xamarin.Forms;
using System.Diagnostics;
namespace CoffeeDiary
{
	public partial class AddEntry : ContentPage
	{
		TodoItemManager manager;
		public string filepath;
		public int ID;
		public string imageFilePath;

		public AddEntry()
		{
			InitializeComponent();
			manager = TodoItemManager.DefaultManager;
			myEditor.Text = "What did you think about your experience?"; //initialize the Editor.Text and TextColor on the XAML file or on the constructor on the code behind with the PlaceHolder or whatever you want.
			filepath = "";
			saveBtn.Clicked += Save;
			backBtn.Clicked += Back;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			locationBtn.Text = (MapPage.address.Equals("") ? "Add Location Here" : MapPage.address);

		}

		private async void Save(object sender, EventArgs e) { //triggered when Save button is clicked
			string name = entry.Text;
			string coffee = entry2.Text;
			string review = myEditor.Text;
			string address = locationBtn.Text;

			await AddItem(new TodoItem(name,coffee,review,address,filepath)); //adds new TodoItem
		}

		private async Task AddItem(TodoItem item) //save Entry Todoitem and back to HomePage
		{
			await manager.SaveTaskAsync(item);
			MapPage.address = "";
			await Navigation.PopModalAsync();
		}

		private void SaveImageItem()
		{
			App.Database.SaveItem(new ImageItem());
			Debug.WriteLine(imageFilePath);
		}

		private async void Back(object sender, EventArgs e) //back to HomePage and triggered when Cancel button clicked
		{
			await Navigation.PopModalAsync();
		}

		private void myEditor_Focused(object sender, FocusEventArgs e) //triggered when the user taps on the Editor to interact with it
		{
			if (myEditor.Text.Equals("What did you think about your experience?")) //if you have the placeholder showing, erase it and set the text color to black
			{
				myEditor.Text = "";
			}
		}

		private void myEditor_Unfocused(object sender, FocusEventArgs e) //triggered when the user taps "Done" or outside of the Editor to finish the editing
		{
			if (myEditor.Text.Equals("")) //if there is text there, leave it, if the user erased everything, put the placeholder Text back and set the TextColor to gray
			{
				myEditor.Text = "What did you think about your experience?";
			}
		}

		private async void LocationButton_onclick(object sender, EventArgs e) //navigates to Map for address input
		{
			await Navigation.PushModalAsync(new MapPage());

		}

		private async void ActionSheet_onclick(object sender, EventArgs e) //triggered when take photo button is clicked
		{
			var action = await DisplayActionSheet("Choose a photo!", "Cancel", null, "Take Photo", "From Album");
			switch (action)
			{
				case "Take Photo": //take photo from camera 
					takePhoto();
					break;
				case "From Album": //choose photo from album
					choosePhoto();
					break;               
            }
		}

		private async void takePhoto() //take photo with device camera
		{

		  if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
		  {
			  await DisplayAlert("No Camera", "No camera avaialble.", "OK");
			  return;
		  }

		  var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
		  {

			  Directory = "Temp",
				Name = "image"
		  });

		  if (file == null)
			  return;

			await DisplayAlert("File Location", file.Path, "OK"); //shows file location of saved photo


			image.Source = ImageSource.FromStream(() =>
		  {
			  var stream = file.GetStream();
			  file.Dispose();
			  return stream;
		  });
			filepath = file.Path;
			imageFilePath = file.Path;
			SaveImageItem();
		}

		private async void choosePhoto() //choose existing photo from device album
		{
			if (!CrossMedia.Current.IsPickPhotoSupported)
			{
				await DisplayAlert("Photos Not Supported", ":(Permission not granted to photos.)", "OK");
				return;
			}
			var file = await CrossMedia.Current.PickPhotoAsync();

			    if (file == null)
				return;

			image.Source = ImageSource.FromStream(() =>
			{
				 var stream = file.GetStream();
				 file.Dispose();
				 return stream;

			});

			filepath = file.Path;
			imageFilePath = file.Path;
			SaveImageItem();
		}
	}
}