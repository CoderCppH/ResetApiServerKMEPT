using System.Configuration;
using System.Data.Entity.Validation;
using System.Data.SQLite;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using Newtonsoft.Json;

namespace web_server{
    class SQLiteUser: OrmSqlTable{
        //name
        public static string @nameTable = "user_data";
        public static string @id = "@id";
        public static string @fullname = "@fullname";
        public static string @email = "@email";
        public static string @password  = "@password";
        //value
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set;} = string.Empty;

        public static DataBaseConnection database = new DataBaseConnection();
        public static void CreateTable(){
            string sqlCommand = $"CREATE TABLE IF NOT EXISTS {@nameTable} ( id INTEGER PRIMARY KEY AUTOINCREMENT, fullname TEXT NOT NULL, email TEXT NOT NULL, password TEXT NOT NULL )";
            using(SQLiteCommand command = new SQLiteCommand(sqlCommand, database.GetConnection()))
                command.ExecuteNonQuery();
        }
        public override bool Create()
        {
            if(!IsObject()){
                string sqlCommand = $"INSERT INTO {@nameTable} (fullname, email, password) VALUES ({@fullname}, {@email}, {@password})";
                using(SQLiteCommand  command = new SQLiteCommand(sqlCommand, database.GetConnection())){
                    command.Parameters.AddWithValue(@fullname, FullName);
                    command.Parameters.AddWithValue(@email, Email);
                    command.Parameters.AddWithValue(@password, Password);
                    int codeResult = command.ExecuteNonQuery();
                    if(codeResult == 1)
                        return true;
                }
            }
            return false;
        }
        public static List<SQLiteUser> GetAllUser()
        {
            List<SQLiteUser> users = new List<SQLiteUser>();
            string sqlCommand = $"SELECT * FROM {@nameTable}";
            using(SQLiteCommand command = new SQLiteCommand(sqlCommand, database.GetConnection()))
            {
                var read = command.ExecuteReader();
                while(read.Read())
                    users.Add(new SQLiteUser(){Id = read.GetInt32(0), FullName = read.GetString(1), Email = read.GetString(2), Password = read.GetString(3)});
            }
            return users;
        }
        public override bool IsNotNull()
        {
            if(Email != string.Empty && Password != string.Empty && FullName != string.Empty)
                return true;
            return false;
        }
        public static SQLiteUser GetByIdOrEmailUser(int id_user, string email_user){
            foreach(var Iuser in GetAllUser())
                if(Iuser.Email == email_user || Iuser.Id == id_user)
                    return Iuser;
            return null;
        }
        public bool IsObject()
        {
            foreach(var Iuser in GetAllUser())
                if(Iuser.Email == Email || Iuser.Id == Id)
                    return true;
            return false;
        }
        public bool Login(string original_password)
        {
            string hashPasswordSHA256 = WebSha256.CalcSha256($"{0}#{FullName}@{Email}?{original_password}");
            foreach(var user in GetAllUser()){
                if(user.Email == Email && user.Password == hashPasswordSHA256){
                    return true;
                }
            }
            return false;
        }
        public bool CheckPassword(string hashPasswordSHA256)
        {
            foreach(var i_user in GetAllUser())
                if(i_user.Password.Equals(hashPasswordSHA256))
                    return true;
            return false;
        }
    }
}