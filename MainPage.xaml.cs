using System;
using System.Net.Http;
using Microsoft.Maui.Controls;

namespace RecuperacionLeonardoAndradeProgreso
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        private const string DogApiBaseUrl = "https://dog.ceo/api";
        private HttpClient _httpClient;

        public MainPage()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
        }

        

        private async void consumo_API(object sender, EventArgs e)
        {
            try
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://dog.ceo/api/breeds/image/random")
                };

                using (var response = await _httpClient.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();

                    // Utiliza JsonSerializer para deserializar JSON en .NET MAUI
                    var dogResponse = System.Text.Json.JsonSerializer.Deserialize<DogApiResponse>(body);

                    if (dogResponse.status == "success")
                    {
                        // Actualiza la imagen usando ImageSource.FromUri en .NET MAUI
                        imgRandom.Source = ImageSource.FromUri(new Uri(dogResponse.message));
                    }
                    else
                    {
                        // Utiliza DisplayAlert para mostrar un mensaje en caso de error
                        await DisplayAlert("Error", "Failed to retrieve dog image.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                // Utiliza DisplayAlert para mostrar un mensaje en caso de excepción
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }

    // Modelo para la respuesta de la API de perros
    public class DogApiResponse
    {
        public string message { get; set; }
        public string status { get; set; }
    }
}

