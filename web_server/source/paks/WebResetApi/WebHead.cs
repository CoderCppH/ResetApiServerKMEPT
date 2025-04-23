using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using web_server.source.paks.DataBaseORM.SQLiteType;
namespace web_server{
    class WebHead{
        private DataBaseConnection db = new DataBaseConnection();
        private HttpListener server = new HttpListener();
        public WebHead(){   
            server.Prefixes.Add(Values.HTTP_HEAD_URL);
            server.Start();
            InitDataBase();
        }
        private async void WriteHttp(HttpListenerResponse response, string text){
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            response.ContentLength64 = buffer.Length;
            using(var stream = response.OutputStream){
                await stream.WriteAsync(buffer, 0, buffer.Length);
                await stream.FlushAsync();
            }
        }
        private void InitDataBase(){
            db = new DataBaseConnection(db_name:Values.DB_NAME);
            db.OpenConnection();
            //create table user-data
            
            SQLiteUser.database = db;
            SQLiteUser.CreateTable();

            SQLiteAesKeyIv.database = db;
            SQLiteAesKeyIv.CreateTable();
        
            SQLiteGroup.dataBase = db;
            SQLiteGroup.CreateTable();

            SQLiteGroupMembers.dataBase = db;
            SQLiteGroupMembers.CreateTable();

            SQLiteMessanger.dataBase = db;
            SQLiteMessanger.CreateTable();

            SQLiteChat.dataBase = db; 
            SQLiteChat.CreateTable();

            SQLiteGroupsFriend.database = db;
            SQLiteGroupsFriend.CreateTable();
        }
        public async Task Loop(ThreadKeyBoard tkh){
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
                        string text = Encoding.UTF8.GetString(buffer);
                        try{
                            JsonUser jsonUser = JsonConvert.DeserializeObject<JsonUser>(text);
                            string hashPasswordSHA256 = WebSha256.CalcSha256($"{0}#{jsonUser.fullname}@{jsonUser.email}?{jsonUser.password}");
                            SQLiteUser sqlUser = new SQLiteUser(){ Id = jsonUser.id, FullName = jsonUser.fullname, Email = jsonUser.email, Password = hashPasswordSHA256};
                            if(sqlUser.Create()){
                                JsonAesKIV kiv = new JsonAesKIV();
                                kiv.Key = WebAse.GenerateKey();
                                kiv.IV = WebAse.GenerateVecotr();
                                SQLiteAesKeyIv sqlAes = new SQLiteAesKeyIv() { Id = 0, IV = kiv.IV, Key = kiv.Key, IdUser = SQLiteUser.GetByIdOrEmailUser(sqlUser.Id, sqlUser.Email).Id };
                                sqlAes.Create();
                                var json_otvet = new { code_status = true };
                                output_text = JsonConvert.SerializeObject(json_otvet);
                            }
                            else{
                                var json_otvet = new { code_status = false };
                                output_text = JsonConvert.SerializeObject(json_otvet);
                            }
                        }
                        catch{}
                    }
                               
                } break;
                case "/login_user":{
                    using(var input = request.InputStream)
                    {
                        byte[] buffer = new byte[request.ContentLength64];
                        await input.ReadAsync(buffer, 0, buffer.Length);
                        string json_text = Encoding.UTF8.GetString(buffer);
                        JsonUser jsonUser = new JsonUser();
                        if(json_text != string.Empty){
                            try{
                                jsonUser = JsonConvert.DeserializeObject<JsonUser>(json_text);
                            }catch{}
                            SQLiteUser sqlUser = new SQLiteUser(){ Id = jsonUser.id, FullName = jsonUser.fullname, Email = jsonUser.email, Password = jsonUser.password};
                            if(sqlUser.IsNotNull()){
                                if(sqlUser.IsObject()){      
                                    var json_otvet = new { code_status = sqlUser.Login(jsonUser.password) };
                                    output_text = JsonConvert.SerializeObject(json_otvet);
                                }
                                else
                                {
                                    var json_otvet = new { code_status = false};
                                    output_text = JsonConvert.SerializeObject(json_otvet);
                                }
                            }
                            else
                            {
                                var json_otvet = new { code_status = false};
                                output_text = JsonConvert.SerializeObject(json_otvet);
                            }
                        }
                    }
                }break;
                case "/make_cri":{
                    using(Aes aes = Aes.Create()){
                        using(var input = request.InputStream){
                            byte [] buffer = new byte[request.ContentLength64];
                            await input.ReadAsync(buffer, 0, buffer.Length);
                            string text = Encoding.UTF8.GetString(buffer);
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
                case "/get_cri":{
                    using (var input = request.InputStream)
                    {
                        byte[] buffer = new byte[request.ContentLength64];
                        await input.ReadAsync(buffer, 0, buffer.Length);
                        string json = Encoding.UTF8.GetString(buffer);
                        JsonUser user = JsonConvert.DeserializeObject<JsonUser>(json);
                        var sql_user = SQLiteUser.GetByIdOrEmailUser(user.id, user.email);
                        if(sql_user.IsObject())
                        {
                            string hashPasswordSHA256 = WebSha256.CalcSha256($"{0}#{user.fullname}@{user.email}?{user.password}");
                            if(sql_user.CheckPassword(hashPasswordSHA256))
                            {
                                SQLiteAesKeyIv sql_aes = SQLiteAesKeyIv.GetAesByIdUser(sql_user.Id);
                                JsonAesKIV json_aes = new JsonAesKIV();
                                json_aes.Key = sql_aes.Key;
                                json_aes.IV = sql_aes.IV;
                                output_text = JsonConvert.SerializeObject(json_aes);
                            }
                        }
                    }
                }break;
                case "/make_group":
                {
                   using(var input = request.InputStream)
                   {
                        byte[] buffer = new byte[request.ContentLength64];
                        await input.ReadAsync(buffer, 0, buffer.Length);
                        string json = Encoding.UTF8.GetString(buffer);
                        JsonGroup json_group = JsonConvert.DeserializeObject<JsonGroup>(json);
                        SQLiteGroup sql_group = new SQLiteGroup(){ Id = json_group.id, GroupName = json_group.group_name, Description = json_group.description };
                        if(sql_group.IsNotNull()){
                            if(sql_group.Create())
                            {
                                var json_otvet = new { code_status = true };
                                output_text = JsonConvert.SerializeObject(json_otvet);
                            }
                            else
                            {
                                var json_otvet = new { code_status = false };
                                output_text = JsonConvert.SerializeObject(json_otvet);
                            }
                        }  
                   }
                }break;
                case "/get_list_user": {
                        string list_user_json = JsonConvert.SerializeObject(SQLiteUser.GetAllUser());
                        output_text = list_user_json;
                    } break;
                case "/get_all_my_friends": {
                        using (var input = request.InputStream)
                        {
                            Console.WriteLine("input");
                            byte[] buffer = new byte[request.ContentLength64];
                            await input.ReadAsync(buffer, 0, buffer.Length);
                            string json = Encoding.UTF8.GetString(buffer);
                            JsonUser user = JsonConvert.DeserializeObject<JsonUser>(json);
                            var list_user = SQLiteGroupsFriend.GetAllReturnAllMyFriendsById(user.id);
                            output_text = JsonConvert.SerializeObject(list_user);
                        }
                    } break;
                case "/add_firend":
                    {
                        using (var input = request.InputStream)
                        {
                            byte[] buffer = new byte[request.ContentLength64];
                            await input.ReadAsync(buffer, 0, buffer.Length);
                            string json = Encoding.UTF8.GetString(buffer);
                            p_gorups_friends group = JsonConvert.DeserializeObject<p_gorups_friends>(json);
                            SQLiteGroupsFriend sQLiteGroupsFriend = new SQLiteGroupsFriend();
                            sQLiteGroupsFriend.Id = group.id;
                            sQLiteGroupsFriend.IdUser1 = group.id_user_1;
                            sQLiteGroupsFriend.IdUser2 = group.id_user_2;
                            if (sQLiteGroupsFriend.Create())
                            {
                                var json_otvet = new { code_status = true };
                                output_text = JsonConvert.SerializeObject(json_otvet);
                            }
                            else
                            {
                                var json_otvet = new { code_status = false };
                                output_text = JsonConvert.SerializeObject(json_otvet);
                            }

                        }
                    }
                    break;
                default:    
                {  
                    output_text = "__def_text_com__: " + tkh.GetArgumentCommand();   
                }break;
            }
            if(response != null)
                WriteHttp(response, output_text);
        }
        public void Close()
        {
            server.Stop();
            server.Close();
        }
    }
}