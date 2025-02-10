using System.Data.SQLite;

namespace web_server{
    class SQLiteUser{
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
        public bool Create()
        {
            if(!IsUser()){
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
        public static List<SQLiteUser> GetUsers()
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
        public static SQLiteUser GetByIdOrEmailUser(SQLiteUser user){
            foreach(var Iuser in GetUsers())
                if(Iuser.Email == user.Email || Iuser.Id == user.Id)
                    return Iuser;
            return null;
        }
        public bool IsUser()
        {
            foreach(var Iuser in GetUsers())
                if(Iuser.Email == Email || Iuser.Id == Id)
                    return true;
            return false;
        }
    }
}