using System;
using Xamarin.Forms.Maps;

namespace CoffeeDiary
{
	public class ExtendedMap : Map
	{
		public event EventHandler<TapEventArgs> Tap;

		public class TapEventArgs : EventArgs
		{
			public Position Position { get; set; }
		}

		public ExtendedMap()
		{

		}

		public ExtendedMap(MapSpan region) : base(region)
		{

		}

		public void OnTap(Position coordinate) //new position triggered on tap 
		{
			OnTap(new TapEventArgs { Position = coordinate });
		}

		protected virtual void OnTap(TapEventArgs e) //handler for ontap
		{
			var handler = Tap;
			if (handler != null) handler(this, e);
		}
	}
}