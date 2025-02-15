using System.Runtime.InteropServices;

namespace web_server
{
    class ThreadKeyBoard
    {
        private Thread ThKey;
        private bool ExitMainThread = false;
        private string InputArgumentMD = string.Empty;
        public ThreadKeyBoard()
        {
            ThKey = new Thread(new ThreadStart(Run));
            ThKey.Start();
        }
        private void Run()
        {
            while(!ExitMainThread)
            {
                System.Console.WriteLine("Input ommand:: ");
                string commmand = Console.ReadLine();
                if(commmand.Equals("close") || commmand.Equals("exit"))
                {
                    ExitMainThread = true;
                }
                else
                {
                    InputArgumentMD = commmand;
                }
            }
        }
        public bool GetExitMainThread()=> ExitMainThread;
        public string GetArgumentCommand()=> InputArgumentMD;
        public void Close()
        {
            ThKey.Join();
        }
    }
}