using System.Data.SQLite;
using System.Security.Cryptography.X509Certificates;

namespace web_server
{
    class SQLiteAesKeyIv
    {
        public static string @nameTable = "aes_key_iv";
        public static string @id = "@id";
        public static string @key = "@key";
        public static string @iv = "@iv";
        public static string @id_user = "@id_user";
        public int Id { get; set; }
        public byte[] Key { get; set; } = new byte[0];
        public byte[] IV { get; set; } = new byte[0];
        public int IdUser { get; set; }
        public static DataBaseConnection database = new DataBaseConnection();
        public static void CreateTable()
        {
            string sqlCommand = $"CREATE TABLE IF NOT EXISTS {@nameTable}(id INTEGER PRIMARY KEY AUTOINCREMENT, key BLOB NOT NULL, iv BLOB NOT NULL, id_user INTEGER NOT NULL, FOREIGN KEY (id_user) REFERENCES user_data(id))";
            using(SQLiteCommand command = new SQLiteCommand(sqlCommand, database.GetConnection()))
                command.ExecuteNonQuery();
        }
        public bool Create()
        {
            string sqlCommand = $"INSERT INTO {@nameTable} (key, iv, id_user ) VALUES ({@key}, {@iv}, {@id_user} ) ";
            using(SQLiteCommand command = new SQLiteCommand(sqlCommand, database.GetConnection()))
            {
                command.Parameters.AddWithValue(@key, Key);
                command.Parameters.AddWithValue(@iv, IV);
                command.Parameters.AddWithValue(@id_user, IdUser);
                int codeResult = command.ExecuteNonQuery();
                if(codeResult == 1)
                    return true;
            }
            return false;
        }
    }
}