using System.Text.Json;
using System.Net.Http;
using System.Text;
using System.IO.Compression;
namespace MainThread
{
    public class Lenta
    {
        public int id { get; set; }
        public string name_post { get; set; } = "";
        public string description_post { get; set; } = "";
        public string image_post { get; set; } = "";
        public int id_user { get; set; }
    }

    class MainClass
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        static async Task Main()
        {
            System.Console.WriteLine("Start client");
            await Get_Post_1();
            System.Console.WriteLine("Finshed client");
        }
        static async Task Get_Post_1()
        {
            string url = "http://localhost:5226/api/lenta/1";

            // GET-запрос
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                
                // Десериализация JSON в класс (System.Text.Json)
                Lenta lenta = JsonSerializer.Deserialize<List<Lenta>>(json)[0];

                Console.WriteLine($"name_post: {lenta.name_post}, description_post: {lenta.description_post}");

                var image = System.Convert.FromBase64String(lenta.image_post);
                System.Console.WriteLine(lenta.image_post);
                File.WriteAllBytes("img.jpg", image);
            }
            else
            {
                Console.WriteLine($"Ошибка: {response.StatusCode}");
            }
        }
    }
}