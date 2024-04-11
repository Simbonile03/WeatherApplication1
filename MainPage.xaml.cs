using Android.Health.Connect.DataTypes.Units;
using Newtonsoft.Json;
using Org.Apache.Http.Client;
using System;
using System.Net.Http;
using WeatherApplication1.Models;
using static Android.Provider.DocumentsContract;
using static WeatherApplication1.Models.WeatherInformation;

namespace WeatherApplication1
{
    public partial class MainPage : ContentPage
    {
        private float _temperature;

        public float Temperature
        {
            get { return _temperature; }
            set 
            { 
                _temperature = value; 
                OnPropertyChanged(); 
            }

        }

        private int _humidity;

        public int Humidity
        {
            get { return _humidity; }
            set
            {

                _humidity = value;
                OnPropertyChanged();
            }

        }

        private float _wind;

        public float Wind

        {
            get { return _wind; }
            set
            {

                _wind = value;
                OnPropertyChanged();
            }

        }
        private int _pressure;


        public int Pressure

        {
            get { return _pressure; }
            set
            {

                _pressure = value;
                OnPropertyChanged();
            }

        }
        private int _clouds;

        private string _city;

        public string City

        {
            get { return _city; }
            set
            {

                _city = value;
                OnPropertyChanged();
            }

        }       


        public int Clouds

        {
            get { return _clouds; }
            set
            {

                _clouds = value;
                OnPropertyChanged();
            }

        }

        private HttpClient _client;

        public MainPage()
        {
            InitializeComponent();

            BindingContext = this;

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            GetWeatherInformation(_client);
            GetCurrentLocation();
            
        }
        private async void GetCurrentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    // Store latitude and longitude
                    double latitude = location.Latitude;
                    double longitude = location.Longitude;

                    // Call GetCityAndWeather method with current location
                    GetCityAndWeather(latitude, longitude);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to get location: {ex}");
            }
        }


        public async void GetWeatherInformation(object parameters)
            {

                var placemarks = await Geocoding.GetPlacemarksAsync(44.34, 10.99);
                var placemark = placemarks?.FirstOrDefault();
               if (placemark != null)
               {
                    City = placemark.Locality;
               }
               string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q={City}&appid=ccb5c9cc4bd067f24a369d15513b3e22&units=metric";
            
                string response = await _client.GetStringAsync(new Uri(apiUrl));

                Rootobject responseWeather = JsonConvert.DeserializeObject<Rootobject>(response);

                if (responseWeather != null)
                {
                   Temperature = responseWeather.main.temp;
                    Humidity = responseWeather.main.humidity;
                    Wind = responseWeather.wind.speed;
                    Pressure = responseWeather.main.pressure;
                    Clouds = responseWeather.clouds.all;

                 }

            }

       

        private async void GetCityAndWeather(double latitude, double longitude)
        {
            try
            {
                // Get the city name using reverse geocoding
                var placemarks = await Geocoding.GetPlacemarksAsync(latitude, longitude);
                var placemark = placemarks?.FirstOrDefault();
                if (placemark != null)
                {
                    City = placemark.Locality;
                }

              
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error fetching weather data: {ex}");
            }
        }



        async void OnGetWeatherButtonClicked(object sender, EventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(_CityEntry.Text))
            {
                GetWeatherInformation(_CityEntry.Text);

            }
        }


      
    }
}


