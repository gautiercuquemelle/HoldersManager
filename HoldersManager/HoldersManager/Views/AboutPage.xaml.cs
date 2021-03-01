using System;
using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace HoldersManager.Views
{
    public partial class AboutPage : ContentPage
    {
        public Xamarin.Forms.Maps.Map Map { get; set; }
        public AboutPage()
        {
            InitializeComponent();

            Map = new Xamarin.Forms.Maps.Map();
            Map.MapType = MapType.Hybrid;
            
            try
            {
                var getlocation = Geolocation.GetLastKnownLocationAsync();
                getlocation.Wait();
                var location = getlocation.Result;

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    Position position = new Position(location.Latitude, location.Longitude);
                    MapSpan mapSpan = new MapSpan(position, 0.08, 0.08);
                    //map = new Xamarin.Forms.Maps.Map(mapSpan);

                    var pinSource = new Pin();
                    pinSource.Type = PinType.Place;
                    pinSource.Label = "Exposure place";
                    pinSource.Position = new Position(location.Latitude, location.Longitude);
                    Map.Pins.Add(pinSource);
                    Map.MoveToRegion(mapSpan);

                }
            }
            catch (Exception ex)
            {
                Debug.Write("Cant get GPS coordinates : " + ex.Message, "Error");
            }

            stackLayout.Children.Add(Map);
        }

      
        private void Map_Clicked(object sender, EventArgs e)
        {
            Map.MapType = MapType.Street;
        }

        private void Hybrid_Clicked(object sender, EventArgs e)
        {
            Map.MapType = MapType.Hybrid;
        }

        private void Satelite_Clicked(object sender, EventArgs e)
        {
            Map.MapType = MapType.Satellite;
        }
    }
}