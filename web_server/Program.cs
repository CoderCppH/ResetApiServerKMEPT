using System;
using Newtonsoft.Json;
namespace web_server{
    class Program{
        static async Task Main(){

            Console.WriteLine("Server start");
            WebHead head = new WebHead();
            while(true)
            {
                await head.Loop();
            }
        }
    }
}
//main file
