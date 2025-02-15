using Newtonsoft.Json;

namespace web_client
{
    class Program
    {
        static HttpClient client;
        static string host = "http://127.0.0.1:8888/";
        static async Task Main()
        {
            System.Console.WriteLine("start client ");
            using(client = new HttpClient())
            {
                
                string url = $"{host}make_user";
                /*
                {
                    "id": 0,
                    "fullname": "Pargev",
                    "email": "coder.cpp.h@gmail.com",
                    "password": "DotnetCore2005@"
                }
                */
                await PostRequest(url, "{\"id\":0, \"fullname\":\"Pargev\", \"email\":\"pargev20002607@gmail.com\", \"password\":\"DotnetCore2005@\" }");
            }
        }
        private static async Task GetRequest(string url)
        {
            try
            {   
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode(); // выбросит исключение, если код ответа не 2xx
                string responseBody = await response.Content.ReadAsStringAsync();
                switch(url){
                    case "http://127.0.0.1:8888/image":{
                        File.WriteAllText("img.jpg", responseBody);
                    }break;
                }
                Console.WriteLine("GET Response:");
                //Console.WriteLine(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Ошибка при GET-запросе: {e.Message}");
            }
        }      //сделай файл в формате json и отправь 
        private static async Task PostRequest(string url, string data)
        {
            try
            {
                var json = data;
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                switch(url){
                    case "http://127.0.0.1:8888/make_user":{
                        var keyOrIv = JsonConvert.DeserializeObject<JsonAesKIV>(responseBody);
                        System.Console.WriteLine(responseBody);
                    }break;
                }
                
                
                Console.WriteLine("POST Response:");
                Console.WriteLine(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Ошибка при POST-запросе: {e.Message}");
            }
        }
    }
}