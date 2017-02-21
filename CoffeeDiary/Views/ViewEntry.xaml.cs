using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;

using Xamarin.Forms;

namespace CoffeeDiary
{
	public partial class ViewEntry : ContentPage
	{
		public TodoItem item;

		public ViewEntry(TodoItem item)
		{
			InitializeComponent();
			this.item = item;
			BindingContext = item;

			if (!item.Path.Equals(""))
			{
				image.Source = ImageSource.FromFile(item.Path);
			}
			else
			{
				Debug.WriteLine("No image available!");
			}
		}

		private async void HomePage_button(object sender, EventArgs e)
		{
			await Navigation.PopModalAsync();
		}
	}
}
