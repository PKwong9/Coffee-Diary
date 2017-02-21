using Android.Gms.Maps;
using CoffeeDiary.Android;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CoffeeDiary.ExtendedMap), typeof(ExtendedMapRenderer))]

namespace CoffeeDiary.Android
{
	public class ExtendedMapRenderer : MapRenderer, IOnMapReadyCallback
	{
		private GoogleMap _map;

		public void OnMapReady(GoogleMap googleMap)
		{
			_map = googleMap;
			if (_map != null)
				_map.MapClick += googleMap_MapClick;
		}

		protected override void OnElementChanged(ElementChangedEventArgs<View> e)
		{
			if (_map != null)
				_map.MapClick -= googleMap_MapClick;
			base.OnElementChanged(e);
			if (Control != null)
				((MapView)Control).GetMapAsync(this);
		}

		private void googleMap_MapClick(object sender, GoogleMap.MapClickEventArgs e)
		{
			((ExtendedMap)Element).OnTap(new Position(e.Point.Latitude, e.Point.Longitude));
		}
	}
}