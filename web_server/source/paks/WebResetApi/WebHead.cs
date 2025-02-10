using System.Net;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
namespace web_server{
    class WebHead{
        private WebBody Body = new WebBody();
        private DataBaseConnection db = new DataBaseConnection();
        private HttpListener server = new HttpListener();
        public WebHead(WebBody body){   
            server.Prefixes.Add(Values.HTTP_HEAD_URL);
            server.Start();
            this.Body = body;
            InitDataBase();
        }
        private async void WriteHttp(HttpListenerResponse response, string text){
            byte[] buffer = Encoding.ASCII.GetBytes(text);
            response.ContentLength64 = buffer.Length;
            using(var stream = response.OutputStream){
                await stream.WriteAsync(buffer, 0, buffer.Length);
                await stream.FlushAsync();
            }
        }
        private string Read(HttpListenerRequest request){
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
            SQLiteAesKeyIv.database = db;
            SQLiteAesKeyIv.CreateTable();
        }
        public async Task Loop(){
            string output_text = "";
            HttpListenerContext context = null;
            HttpListenerRequest request = null;
            HttpListenerResponse response = null;
            try{
                context = await server.GetContextAsync();
                request = context.Request;
                response = context.Response;
            }   catch {  }
            switch(request?.RawUrl){
                case "/make_user":{
                    using(var input = request.InputStream){
                        byte [] buffer = new byte[request.ContentLength64];
                        await input.ReadAsync(buffer, 0, buffer.Length);
                        string text = Encoding.ASCII.GetString(buffer);
                        JsonUser jsonUser = JsonConvert.DeserializeObject<JsonUser>(text);
                        string hashPasswordSHA256 = WebSha256.CalcSha256($"{jsonUser.id}#{jsonUser.fullname}@{jsonUser.email}?{jsonUser.password}");
                        SQLiteUser sqlUser = new SQLiteUser(){ Id = jsonUser.id, FullName = jsonUser.fullname, Email = jsonUser.email, Password = hashPasswordSHA256};
                        if(sqlUser.Create()){
                            JsonAesKIV kiv = new JsonAesKIV();
                            kiv.Key = WebAse.GenerateKey();
                            kiv.IV = WebAse.GenerateVecotr();
                            SQLiteAesKeyIv sqlAes = new SQLiteAesKeyIv() { Id = 0, IV = kiv.IV, Key = kiv.Key, IdUser = SQLiteUser.GetByIdOrEmailUser(sqlUser).Id };
                            sqlAes.Create();
                            output_text = JsonConvert.SerializeObject(kiv);
                        }
                    }                  
                } break;
                case "/make_cri":{
                    using(Aes aes = Aes.Create()){
                        using(var input = request.InputStream){
                            byte [] buffer = new byte[request.ContentLength64];
                            await input.ReadAsync(buffer, 0, buffer.Length);
                            string text = Encoding.ASCII.GetString(buffer);
                            JsonAesKIV kiv = new JsonAesKIV();
                            kiv.Key = WebAse.GenerateKey();
                            kiv.IV = WebAse.GenerateVecotr();
                            aes.Key = kiv.Key;
                            aes.IV = kiv.IV;
                            byte [] encrypted = WebAse.EncryptStringToBytes_Aes(text, aes.Key, aes.IV);        
                            Console.WriteLine($"Зашифрованные данные: {Convert.ToBase64String(encrypted)}");
                            System.Console.WriteLine($"разхешифрованные данные: {WebAse.DecryptStringFromBytes_Aes(encrypted, aes.Key, aes.IV)}");
                            output_text = JsonConvert.SerializeObject(kiv);
                        }
                    }
                }break;
                default:    {   output_text = "__def_text__";   }break;
            }
            if(response != null)
                WriteHttp(response, output_text);
        }
    }
}