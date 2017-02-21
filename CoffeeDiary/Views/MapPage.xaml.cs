using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms.Maps;
using Xamarin.Forms;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CoffeeDiary
{
	public partial class MapPage : ContentPage
	{
		public static ExtendedMap map;
		public static Position position;
		public static Geocoder geoCoder;
		public static String address = "";

		public static double GetLatitude() //gets latitude of current position
		{
			return position.Latitude;
		}

		public static double GetLongtitude() //gets longtitude of current position
		{
			return position.Longitude;
		}

		public MapPage()
		{
			InitializeComponent();
			geoCoder = new Geocoder();
			position = new Position(0, 0);

			map = new ExtendedMap( //creates and initiates new map 
				MapSpan.FromCenterAndRadius(
			new Position(-33.8675, 151.207), Distance.FromMiles(0.1)))
			{
				IsShowingUser = true,
				VerticalOptions = LayoutOptions.FillAndExpand
			};
			var stack = new StackLayout { Spacing = 0 };
			stack.Children.Add(map);
			stack.Children.Add(map_SaveBtn);

			Content = stack;

		}

		public static async Task GetAddress(Position position) //gets closest addresses for position and converts to string address 
		{
			var addresses = await geoCoder.GetAddressesForPositionAsync(position);
			address = ""; 

			foreach (var addrs in addresses)
			{
				address += addrs;
			}
			address = address.Replace("\n", ", ");

	
			Debug.WriteLine(address);
		}

		public static async void TriggerTapEventOnMap(Position position) //triggered when user taps on map 
		{
			await GetAddress(position);
			var pin = new CustomPin //creates new custom pin 
			{
				Pin = new Pin
				{
					Type = PinType.Place,
					Position = position,
					Label = "Location:",
					Address = address
				}
			};

			MapPage.position = position;
			map.Pins.Clear(); //clears existing pin
			map.Pins.Add(pin.Pin); //adds new pin 
		}

		private async void SaveLocation(object sender, EventArgs e) //returns a string address to AddEntry page and returns to AddEntry
		{
			await Navigation.PopModalAsync();
		}

	}
}
