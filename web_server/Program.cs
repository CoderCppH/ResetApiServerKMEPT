using System;
using Newtonsoft.Json;
namespace web_server{
    class Program{
        static async Task Main(){
            ThreadKeyBoard tkh = new ThreadKeyBoard();
            Console.WriteLine("Server start");
            WebHead head = new WebHead();
            while(!tkh.GetExitMainThread())
            {
                await head.Loop(tkh);
            }
            head.Close();
            tkh.Close();
        }
    }
}
//main file
