using System;
using Newtonsoft.Json;
namespace web_server{
    class Program{
        static async Task Main(){

            Console.WriteLine("Server start");
            WebBody body = new WebBody();
            WebHead head = new WebHead(body);
            while(true)
            {
                await head.Loop();
            }

        }
    }
}
//main file
