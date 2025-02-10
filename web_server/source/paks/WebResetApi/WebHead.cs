using System.Net;
using System.Text;
namespace web_server
{
    class WebHead
    {
        private WebBody Body = new WebBody();
        private DataBaseConnection db = new DataBaseConnection();
        private HttpListener server = new HttpListener();
        public WebHead(WebBody body)
        {   
            server.Prefixes.Add(Values.HTTP_HEAD_URL);
            server.Start();
            this.Body = body;
            InitDataBase();
        }
        private async void WriteHttp(HttpListenerResponse response, string text)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(text);
            response.ContentLength64 = buffer.Length;
            using(var stream = response.OutputStream){
                await stream.WriteAsync(buffer, 0, buffer.Length);
                await stream.FlushAsync();
            }
        }
        private string Read(HttpListenerRequest request)
        {
            byte [] buffer = new byte[request.ContentLength64];
            using(var stream = request.InputStream)
                stream.Read(buffer, 0, buffer.Length);
            
            return Encoding.ASCII.GetString(buffer);
        }
        private void InitDataBase(){
            db = new DataBaseConnection(db_name:Values.DB_NAME);
            db.OpenConnection();
            //create table user-data
            SQLiteUser.database = db;
            SQLiteUser.CreateTable();
        }
        public async Task Loop()
        {
            string output_text = "";
            HttpListenerContext context = null;
            HttpListenerRequest request = null;
            HttpListenerResponse response = null;
            try
            {
                context = await server.GetContextAsync();
                request = context.Request;
                response = context.Response;
            }
            catch{}
            string rawUrl = request?.RawUrl;
            switch(rawUrl)
            {
                case "/make_user":
                {
                    
                    using(var input = request.InputStream)
                    {
                        byte [] buffer = new byte[request.ContentLength64];
                        input.ReadAsync(buffer, 0, buffer.Length);
                        string text = Encoding.ASCII.GetString(buffer);
                        Console.WriteLine(text);
                    }
                    output_text = "okk_create_user";
                } break;
                default:
                {
                    output_text = "__def_text__";
                }
                break;
            }
            if(response != null){
                WriteHttp(response, output_text);
            }
        }
    }
}